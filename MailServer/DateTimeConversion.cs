using System;

public class DateTimeConversion
{
    public static string DateTimeToStringTimestamp(DateTime? inputDateTime)
    {
        // formerly methodname "GetDB2TimeStamp"

        // convert DateTime value to String ("2011 09 06 6 3 4")
        string strDateNow = string.Format("{0:u}", inputDateTime).Replace("-", " ").Replace(":", " ").Replace("Z", " ");

        try
        {
            // convert single digit values (0 thru 9) to two digit values (00 thru 09)
            string[] strSplit = strDateNow.Trim().Split(' ');
            if (strSplit[3].Length == 1 || strSplit[4].Length == 1 || strSplit[5].Length == 1)
            {
                // HOURS
                if (strSplit[4].Length == 1)
                {
                    strSplit[3] = "0" + strSplit[3];
                }

                // MINUTES
                if (strSplit[4].Length == 1)
                {
                    strSplit[4] = "0" + strSplit[4];
                }

                // SECONDS
                if (strSplit[5].Length == 1)
                {
                    strSplit[5] = "0" + strSplit[5];
                }
            }

            // put back everything into the variable "yyyy-mm-dd hh:mm:ss.xxx"
            strDateNow = strSplit[0] + "-" + strSplit[1] + "-" + strSplit[2] + " " + strSplit[3] + ":" + strSplit[4] + ":" + strSplit[5] + ".001";
        }
        catch (Exception ex)
        {
            throw new Exception("DateTimeConversion.DateTimeToStringTimestamp() error: " + ex.Message);
        }

        // return string 
        return strDateNow;
    }

    //This Method Deals with a date format expected from a CDA document and specified by the CDA architecture 
    public static DateTime DateTimeFromString(string strDate)
    {
        // formerly methodname "convertStringToDateFormat"

        // inputs: "20120523085619.035-0500" representing ccyymmddhhmmss.millisec and timezone 5 HrsBeforeGMT
        //         "20120523085619-0200" representing ccyymmddhhmmss and timezone 2 HrsBeforeGMT
        //         "20120523085619" representing ccyymmddhhmmss
        //         "20120523" representing ccyymmdd 
        // Output: 2012-05-23 00:00:00.001000	

        DateTime? newDateTime = (DateTime?)null;
        string year = " ", month = "", day = "", hour = "", minute = "", second = "";

        try
        {
            // strip timezone off end of string if present
            if (strDate.Contains("-"))
            {
                strDate = strDate.Substring(0, strDate.IndexOf('-'));
            }

            // substring off year,month,day values
            if (strDate.Length >= 8)
            {
                year = strDate.Substring(0, 4);
                month = strDate.Substring(4, 2);
                day = strDate.Substring(6, 2);
            }

            // substring off hours
            if (strDate.Length > 8)
            {
                if (strDate.Length == 9)
                {
                    hour = strDate.Substring(8, 1);
                }
                else
                {
                    hour = strDate.Substring(8, 2);
                }
            }

            // substring off minutes
            if (strDate.Length > 10)
            {
                if (strDate.Length == 11)
                {
                    minute = strDate.Substring(10, 1);
                }
                else
                {
                    minute = strDate.Substring(10, 2);
                }
            }

            // substring off seconds
            if (strDate.Length > 12)
            {
                if (strDate.Length == 13)
                {
                    second = strDate.Substring(12, 1);
                }
                else
                {
                    second = strDate.Substring(12, 2);
                }
            }

            // YYYYMMDDHHMMSS
            if (strDate.Length >= 14)
            {
                newDateTime = new DateTime(
                                    Convert.ToInt32(year),
                                    Convert.ToInt32(month),
                                    Convert.ToInt32(day),
                                    Convert.ToInt32(hour),
                                    Convert.ToInt32(minute),
                                    Convert.ToInt32(second));
            }

            // YYYYMMDD
            else if (strDate.Length >= 8)
            {
                newDateTime = new DateTime(
                                    Convert.ToInt32(year),
                                    Convert.ToInt32(month),
                                    Convert.ToInt32(day));
            }

            // YYYYMM (default to 1st day)
            else if (strDate.Length == 6)
            {
                newDateTime = new DateTime(
                                    Convert.ToInt32(strDate.Substring(0, 4)),
                                    Convert.ToInt32(strDate.Substring(4, 2)),
                                    01);
            }

            // YYYY (default to 1stMonth & 1st Day)
            else if (strDate.Length == 4)
            {
                newDateTime = new DateTime(
                                    Convert.ToInt32(strDate.Substring(0, 4)),
                                    01,
                                    01);
            }
        }
        catch (Exception ex)
        {
            if (strDate == null)
                strDate = String.Empty;

            //JDW Note that the text: DateTimeFromString is searched for in PHCDARequestOut.ParseNodeET.Get() so changing this text would require changing that code in the catch block
            throw new Exception("DateTimeFromString(" + strDate + ") " + ex.Message, ex);
        }

        // cast nullable DateTime variable back to normal DateTime object before returning value
        return (DateTime)newDateTime;
    }

    public static DateTime DateTimeFromDB2DateString(string strDate)
    {
        // formerly methodname "convertStringToDateFormat"/"DateTimeFromString"

        // output: "2012-09-09-00.00.00.01"
        // inputs: "20120909"

        DateTime? newDateTime = (DateTime?)null;
        string year = " ", month = "", day = "";

        try
        {
            // substring off year,month,day values
            if (strDate.Length >= 8)
            {
                year = strDate.Substring(0, 4);//YYYY
                //-
                month = strDate.Substring(5, 2);//MM
                //-
                day = strDate.Substring(8, 2);//DD

                newDateTime = new DateTime(
                                    Convert.ToInt32(year),
                                    Convert.ToInt32(month),
                                    Convert.ToInt32(day));
            }
            // cast nullable DateTime variable back to normal DateTime object before returning value
            return (DateTime)newDateTime;
        }
        catch (Exception ex)
        {
            throw new Exception("DateTimeFromDB2DateString() " + ex.Message);
        }
    }
}