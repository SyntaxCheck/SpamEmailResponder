using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckIlluminati : EmailTypeBase
{
    public CheckIlluminati()
    {
        Type = EmailType.Illuminati;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (preProcessedBody.Trim().ToUpper().Contains("ILLUMINATI") ||
            preProcessedBody.Trim().ToUpper().Contains("ILUMINATI"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}