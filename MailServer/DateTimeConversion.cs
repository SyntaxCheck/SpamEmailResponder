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
    public static bool IsStringSomeTypeOfDateTime(string s)
    {
        s = s.Trim();

        if (s.ToUpper().StartsWith("SENT:"))
            s = s.Substring("SENT:".Length - 1).Trim();
        if (s.StartsWith(":") || s.StartsWith(",") || s.StartsWith(";"))
            s = s.Substring(1).Trim();
        if (s.Contains("UTC+"))
            s = s.Substring(0, s.IndexOf("UTC+")).Trim();
        if (s.Contains("GMT+"))
            s = s.Substring(0, s.IndexOf("GMT+")).Trim();
        if (s.EndsWith(",") || s.EndsWith(":") || s.EndsWith(";"))
            s = s.Substring(0, s.Length - 1).Trim();

        string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
            "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss", "yyyy-MM-dd HH:mm", "yyyy-MM-dd H:mm",
            "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt", "yyyy-MM-dd HH:m", "yyyy-MM-dd H:m",
            "M/d/yyyy h:mm", "M/d/yyyy h:mm", "d/M/yy",
            "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm", "dddd, MMMM dd, yyyy 'at' hh:mm tt",
            "ddd d/MM/yy", "ddd d/M/yy", "ddd M/dd/yy", "ddd M/d/yy",
            "MMM d/MM/yy", "MMM d/M/yy", "MMM M/dd/yy", "MMM M/d/yy",
            "'On' ddd, MM/d/yy",
            "'On' ddd, MMM d, yyyy 'at' hh:mm",
            "'On' ddd, MMM d, yyyy 'at' hh:mm tt",
            "'On' ddd, MMM d, yyyy 'at' H:mm",
            "'On' ddd, MMM d, yyyy 'at' h:mm tt",
            "'On' ddd, MMM d, yyyy 'at' h:mm",
            "'On' dddd, MMMM d, yyyy, H:mm:ss tt",
            "'On' MM/d/yy",
        };
        DateTime dateValue;

        if (DateTime.TryParseExact(s, formats, new System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, out dateValue))
            return true;

        //Sometimes they include an invalid Month text when there is no part of the date that matches the text for the given month text. Remove the month text and try again
        s = s.Replace("January", "").Replace("February", "").Replace("March", "").Replace("April", "").Replace("May", "").Replace("June", "").Replace("July", "").Replace("August", "").Replace("September", "").Replace("October", "").Replace("November", "").Replace("December", "").Trim();
        s = s.Replace("Jan", "").Replace("Feb", "").Replace("Mar", "").Replace("Apr", "").Replace("May", "").Replace("Jun", "").Replace("Jul", "").Replace("Aug", "").Replace("Sep", "").Replace("Oct", "").Replace("Nov", "").Replace("Dec", "").Trim();

        while (s.Contains("  "))
            s = s.Replace("  ", " ");

        if (DateTime.TryParseExact(s, formats, new System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, out dateValue))
            return true;

        return false;
    } //Just verify the string is some type of DateTime, we do not care about getting an accurate date from this
}