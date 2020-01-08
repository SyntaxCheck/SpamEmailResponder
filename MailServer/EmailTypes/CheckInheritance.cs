using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckInheritance : EmailTypeBase
{
    public CheckInheritance()
    {
        Type = EmailType.Inheritance;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (preProcessedBody.Trim().ToUpper().Contains("INHERITENCE") ||
            preProcessedBody.Trim().ToUpper().Contains("INHERIT"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}