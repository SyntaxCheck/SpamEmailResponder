using System;
using System.IO;
using System.Data.Odbc;
using System.Data.SqlClient;

public class Logger
{
    /***************************************************************************
        * This class requires use with a data-structure class object, and contains no data
        * properties (public or otherwise) of its own.  As such the methods of this class
        * may be used without instantiating the Logger() class.  Just use the methods
        * passing the LoggerInfo class object as the first parameter.
        * 
        * NOTE: the ValidatePath() method is called with the LoggerInfo class parameter
        *       passed as a "reference" variable so that the original LoggerInfo class
        *       object of the calling class will reflect any error or status flags.
        *       
        * ************************************************************************/
    public static void ValidatePath(ref LoggerInfo loggerInfo)
    {
        loggerInfo.ErrorFlag = false;

        if (loggerInfo.RootPath == "" || loggerInfo.FolderName == "")
        {
            loggerInfo.ErrorMsg = "Logger ValidatePath: RootPath and/or FolderName not provided";
            loggerInfo.ErrorFlag = true;
        }
        else
        {
            try
            {
                // check if ROOT path is valid
                if (!Directory.Exists(loggerInfo.RootPath))
                {
                    loggerInfo.ErrorMsg = "Logger ValidatePath: RootPath (" + loggerInfo.RootPath.Trim() + ") not found";
                    loggerInfo.ErrorFlag = true;
                }
                else
                {
                    // unconditionally attempt to create log FOLDER
                    // (CreateSubdirectory does nothing if it already exists)
                    DirectoryInfo appDI = new DirectoryInfo(loggerInfo.RootPath);
                    appDI.CreateSubdirectory(loggerInfo.FolderName);
                }

                loggerInfo.FullPathChecked = true;
            }
            catch (Exception ex)
            {
                loggerInfo.ErrorMsg = "Logger ValidatePath: failed. " + ex.Message;
                loggerInfo.ErrorFlag = true;
            }
        }

        return;
    }
    public static void Write(LoggerInfo loggerInfo, string logMsg)
    {
        // if we havent yet done so, validate complete file path to log file
        if (!loggerInfo.FullPathChecked)
        {
            ValidatePath(ref loggerInfo);
        }

        // once logger issue develops, stop trying to write to it
        if (!loggerInfo.ErrorFlag)
        {
            try
            {
                // if log file not currently present, create it with passed message
                // otherwise append message to existing log file
                if (!File.Exists(loggerInfo.FullPath))
                {
                    using (StreamWriter sw = File.CreateText(loggerInfo.FullPath))
                    {
                        sw.WriteLine(DateTime.Now.ToString() + ": " + logMsg);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(loggerInfo.FullPath))
                    {
                        sw.WriteLine(DateTime.Now.ToString() + ": " + logMsg);
                    }
                }

                loggerInfo.ErrorMsg = "written";
            }
            catch (Exception ex)
            {
                loggerInfo.ErrorMsg = "Logger Write(): failed. " + ex.Message;
            }
        }

        return;
    }
    public static void Write(LoggerInfo loggerInfo, string queryString, OdbcCommand cmd)
    {
        string loggingString = queryString;
        foreach (OdbcParameter p in cmd.Parameters)
        {
            loggingString = loggingString.Replace(p.ParameterName, p.Value == null ? "-NULL-" : p.Value.ToString());
        }

        if (String.IsNullOrEmpty(loggingString)) loggingString = "--Query/Parameters Log String is blank, Exact query passed in: " + queryString + "--";

        Write(loggerInfo, loggingString);
    }
    public static void Write(LoggerInfo loggerInfo, string queryString, SqlCommand cmd)
    {
        string loggingString = queryString;
        foreach (SqlParameter p in cmd.Parameters)
        {
            loggingString = loggingString.Replace(p.ParameterName, p.Value == null ? "-NULL-" : p.Value.ToString());
        }

        if (String.IsNullOrEmpty(loggingString)) loggingString = "--Query/Parameters Log String is blank, Exact query passed in: " + queryString + "--";

        Write(loggerInfo, loggingString);
    }
    public static void Write(LoggerInfo loggerInfo, string methodName, Exception thisException)
    {
        // if we havent yet done so, validate complete file path to log file
        if (!loggerInfo.FullPathChecked)
        {
            ValidatePath(ref loggerInfo);
        }

        // once logger issue develops, stop trying to write to it
        if (!loggerInfo.ErrorFlag)
        {
            if (thisException is System.Data.Odbc.OdbcException)
            {
                var ex = (System.Data.Odbc.OdbcException)thisException;
                try
                {
                    Write(loggerInfo, methodName + " ODBC Exception Message: " + ex.Message);
                    Write(loggerInfo, methodName + " ODBC Exception Source: " + ex.Source);
                    Write(loggerInfo, methodName + " ODBC Exception TargetSite: " + ex.TargetSite);
                    Write(loggerInfo, methodName + " ODBC Exception Data: " + ex.Data);
                    Write(loggerInfo, methodName + " ODBC Exception Error Code: " + ex.ErrorCode);
                    Write(loggerInfo, methodName + " ODBC Exception Errors: " + ex.Errors);
                    Write(loggerInfo, methodName + " ODBC Exception StackTrace: " + ex.StackTrace);
                    if (ex.InnerException != null)
                    {
                        Write(loggerInfo, methodName + " ODBC Inner Exception Message: " + ex.InnerException.Message);
                        Write(loggerInfo, methodName + " ODBC Inner Exception Source: " + ex.InnerException.Source);
                        Write(loggerInfo, methodName + " ODBC Inner Exception TargetSite: " + ex.InnerException.TargetSite);
                        Write(loggerInfo, methodName + " ODBC Inner Exception Data: " + ex.InnerException.Data);
                        Write(loggerInfo, methodName + " ODBC Inner Exception StackTrace: " + ex.InnerException.StackTrace);
                    }
                }
                catch (Exception) { }
            }
            else
            {
                var ex = thisException;
                try
                {
                    Write(loggerInfo, methodName + " Exception Message: " + ex.Message);
                    Write(loggerInfo, methodName + " Exception Source: " + ex.Source);
                    Write(loggerInfo, methodName + " Exception TargetSite: " + ex.TargetSite);
                    Write(loggerInfo, methodName + " Exception Data: " + ex.Data);
                    Write(loggerInfo, methodName + " Exception StackTrace: " + ex.StackTrace);
                    if (ex.InnerException != null)
                    {
                        Write(loggerInfo, methodName + " Inner Exception Message: " + ex.InnerException.Message);
                        Write(loggerInfo, methodName + " Inner Exception Source: " + ex.InnerException.Source);
                        Write(loggerInfo, methodName + " Inner Exception TargetSite: " + ex.InnerException.TargetSite);
                        Write(loggerInfo, methodName + " Inner Exception Data: " + ex.InnerException.Data);
                        Write(loggerInfo, methodName + " Inner Exception StackTrace: " + ex.InnerException.StackTrace);
                    }
                }
                catch (Exception) { }
            }
        }

        return;
    }
    public static void Write(LoggerInfo loggerInfo, string methodName, StandardResponse stdResponse)
    {
        // if we havent yet done so, validate complete file path to log file
        if (!loggerInfo.FullPathChecked)
        {
            ValidatePath(ref loggerInfo);
        }

        // once logger issue develops, stop trying to write to it
        if (!loggerInfo.ErrorFlag)
        {
            methodName = methodName.TrimEnd();
            Write(loggerInfo, methodName + " Standard Response Code: " + stdResponse.Code);
            Write(loggerInfo, methodName + " Standard Response Message: " + stdResponse.Message);

            if (stdResponse.Data != String.Empty)
            {
                Write(loggerInfo, methodName + "\r\nStandard Response Data: " + stdResponse.Data);
            }
        }
    }
    public static void WriteDbg(LoggerInfo loggerInfo, string queryString, OdbcCommand cmd)
    {
        string loggingString = queryString;
        foreach (OdbcParameter p in cmd.Parameters)
        {
            loggingString = loggingString.Replace(p.ParameterName, p.Value == null ? "-NULL-" : p.Value.ToString());
        }

        if (String.IsNullOrEmpty(loggingString)) loggingString = "--Query/Parameters Log String is blank, Exact query passed in: " + queryString + "--";

        WriteDbg(loggerInfo, loggingString);
    }
    public static void WriteDbg(LoggerInfo loggerInfo, string queryString, SqlCommand cmd)
    {
        string loggingString = queryString;
        foreach (SqlParameter p in cmd.Parameters)
        {
            loggingString = loggingString.Replace(p.ParameterName, p.Value == null ? "-NULL-" : p.Value.ToString());
        }

        if (String.IsNullOrEmpty(loggingString)) loggingString = "--Query/Parameters Log String is blank, Exact query passed in: " + queryString + "--";

        WriteDbg(loggerInfo, loggingString);
    }
    public static void WriteDbg(LoggerInfo loggerInfo, string logMsg)
    {
        // if we havent yet done so, validate complete file path to log file
        if (!loggerInfo.FullPathChecked)
        {
            ValidatePath(ref loggerInfo);
        }

        // once logger issue develops, stop trying to write to it
        if (!loggerInfo.ErrorFlag)
        {
            // assuming DebugLogging has been activated
            if (loggerInfo.DebugActive)
            {
                try
                {
                    // if log file not currently present, create it with passed message
                    // otherwise append message to existing log file
                    if (!File.Exists(loggerInfo.FullPath))
                    {
                        using (StreamWriter sw = File.CreateText(loggerInfo.FullPath))
                        {
                            sw.WriteLine(DateTime.Now.ToString() + ": " + logMsg);
                        }
                    }
                    else
                    {
                        using (StreamWriter sw = File.AppendText(loggerInfo.FullPath))
                        {
                            sw.WriteLine(DateTime.Now.ToString() + ": " + logMsg);
                        }
                    }

                    loggerInfo.ErrorMsg = "dbg written";
                }
                catch (Exception ex)
                {
                    loggerInfo.ErrorMsg = "Logger WriteDbg(): failed. " + ex.Message;
                }
            }
        }

        return;
    }
}