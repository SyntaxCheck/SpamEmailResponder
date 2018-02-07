using System;
using System.Xml;
using System.Xml.Serialization;

public class StandardResponse
{
    /***********************************************************************************
        * Acts as primary communication object between public Methods of various Classes.
        * Passes back results status (Code) and two levels of messaging (Message & Data), 
        * and optionally an object (ReturnObject) for use by the calling classes method.
        * 
        * CODE:      1=Success, -1=Error, 0=Warning
        * MESSAGE:   Primarily used to display first line of an error message but can also be used for success
        * DATA:      Only used for exceptions.  Identifies the call level at which the error ocurred and the exception message.
        * RETURNOBJ: For situation where an Object needs to be passed as the result of a public class method. The object
        *            may be a simple object like a boolean, char, string, or int, or it may be a more complex object
        *            such as a string array or other object.
        *  
        * ********************************************************************************/
    private int code;
    private string message;
    private string data;
    private string logData;
    private object returnObject;
    private Exception exception;

    // CONSTRUCTORS
    public StandardResponse()
    {
        code = -2;
        message = String.Empty;
        data = String.Empty;
        logData = String.Empty;
        returnObject = null;
        exception = null;
    }
    public StandardResponse(int code, string message)
    {
        this.code = code;
        this.message = message;
        data = String.Empty;
        logData = String.Empty;
        returnObject = null;
        exception = null;
    }
    public StandardResponse(int code, string message, string data)
    {
        this.code = code;
        this.message = message;
        this.data = data;
        logData = String.Empty;
        returnObject = null;
        exception = null;
    }
    public StandardResponse(int code, string message, string data, object returnObject)
    {
        this.code = code;
        this.message = message;
        this.data = data;
        logData = String.Empty;
        this.returnObject = returnObject;
        exception = null;
    }
    public StandardResponse(int code, string message, string data, object returnObject, Exception exception)
    {
        this.code = code;
        this.message = message;
        this.data = data;
        logData = String.Empty;
        this.returnObject = returnObject;
        this.exception = exception;
    }

    // ACCESSORS
    public int Code
    {
        get { return code; }
        set { code = value; }
    }
    public string Message
    {
        get { return message; }
        set { message = value; }
    }
    public string Data
    {
        get { return data; }
        set { data = value; }
    }
    public string LogData
    {
        get { return logData; }
        set { logData = value; }
    }
    public object ReturnObject
    {
        get { return returnObject; }
        set { returnObject = value; }
    }
    [XmlIgnore] //Cannot Serialize an Exception
    public Exception Exception
    {
        get { return exception; }
        set 
        {
            exception = value;
            if (exception != null)
            {
                if (data != String.Empty)
                    if (data.Length > 2 && data.Substring(data.Length - 2) != ". ")
                        data += ". ";

                if (!String.IsNullOrEmpty(exception.Message))
                    data += "Message: " + exception.Message + ". ";
                if (exception.InnerException != null && !String.IsNullOrEmpty(exception.InnerException.ToString()))
                    data += "Inner Exception: " + exception.InnerException.ToString() + ". ";
                if (!String.IsNullOrEmpty(exception.StackTrace))
                    data += "Stack Trace: " + exception.StackTrace + ". ";

                data = data.TrimEnd();
            }
        }
    }
 
    //METHODS
    public string AsString()
    {
        // Reconstruct StandardResponse as xml string.  
        // Use simple stringing (rather than XElement) to provide lower .NET 2.0 compatability with PowerBuilder.
        string stdRespString = "<Response>";
        stdRespString += "<Code>" + this.code.ToString() + "</Code>";
        stdRespString += "<Message>" + this.message + "</Message>";
        stdRespString += "<Data>" + this.data + "</Data>";
        if (!String.IsNullOrEmpty(logData))
        {
            stdRespString += "<LogData>" + this.logData + "</LogData>";
        }
        stdRespString += "</Response>";
        return stdRespString;
    }
    public override string ToString()
    {
        return AsString();
    }
    /// <summary>
    /// Pass in StandardResponse XML to be loaded into the appropriate properties
    /// </summary>
    /// <param name="responseXml"></param>
    /// <returns>Returns Error message if it fails to parse the string</returns>
    public string LoadString(string responseXml)
    {
        XmlDocument xmlDoc = new XmlDocument();
        int tempOut;

        try
        {
            xmlDoc.LoadXml(responseXml);
            XmlNode childNode = xmlDoc.FirstChild;

            if (childNode != null && childNode.Name.Equals("Response"))
            {
                if (childNode.CreateNavigator().SelectSingleNode("descendant::Code") != null)
                {
                    XmlNode codeNode = childNode.SelectSingleNode("descendant::Code");
                    if (int.TryParse(codeNode.InnerText.Trim(), out tempOut))
                    {
                        code = int.Parse(codeNode.InnerText.Trim());
                    }
                    else
                    {
                        return "Invalid StandardResponse XML Code node is not numeric. Value: " + codeNode.InnerText.Trim();
                    }
                }
                if (childNode.CreateNavigator().SelectSingleNode("descendant::Code") != null)
                {
                    XmlNode messageNode = childNode.SelectSingleNode("descendant::Message");
                    message = messageNode.InnerText.Trim();
                }
                if (childNode.CreateNavigator().SelectSingleNode("descendant::Data") != null)
                {
                    XmlNode dataNode = childNode.SelectSingleNode("descendant::Data");
                    data = dataNode.InnerText.Trim();
                }
                if (childNode.CreateNavigator().SelectSingleNode("descendant::LogData") != null)
                {
                    XmlNode logDataNode = childNode.SelectSingleNode("descendant::LogData");
                    logData = logDataNode.InnerText.Trim();
                }
            }
            else
            {
                if (childNode == null)
                    return "Invalid StandardResponse XML";
                else
                    return "Invalid StandardResponse XML string. Starting node: " + childNode.Name;
            }
        }
        catch (Exception ex)
        {
            return "Failed to load response. " + ex.Message;
        }

        return "";
    }
}