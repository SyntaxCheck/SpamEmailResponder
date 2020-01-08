using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckPolice : EmailTypeBase
{
    public CheckPolice()
    {
        Type = EmailType.Police;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (preProcessedBody.Trim().ToUpper().Contains("POLICE") ||
            preProcessedBody.Trim().ToUpper().Contains("CONVICTED TERRORIST") ||
            preProcessedBody.Trim().ToUpper().Contains("ENFORCEMENT OFFICER") ||
            preProcessedBody.Trim().ToUpper().Contains(" FBI ") ||
            preProcessedBody.Trim().ToUpper().Contains("WANTED TERRORIST"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}