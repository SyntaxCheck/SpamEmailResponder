using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckInheritance : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckInheritance(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.Inheritance;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("INHERITENCE") ||
            preProcessedBody.Trim().ToUpper().Contains("INHERIT"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}