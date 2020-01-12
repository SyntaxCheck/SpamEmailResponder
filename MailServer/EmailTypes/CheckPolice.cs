using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckPolice : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckPolice(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.Police;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("POLICE") ||
            preProcessedBody.Trim().ToUpper().Contains("CONVICTED TERRORIST") ||
            preProcessedBody.Trim().ToUpper().Contains("ENFORCEMENT OFFICER") ||
            preProcessedBody.Trim().ToUpper().Contains(" FBI ") ||
            preProcessedBody.Trim().ToUpper().Contains("REPORTED YOUR MONEY LAUNDER") ||
            preProcessedBody.Trim().ToUpper().Contains("WANTED TERRORIST"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}