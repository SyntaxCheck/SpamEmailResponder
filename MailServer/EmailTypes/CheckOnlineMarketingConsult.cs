using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckOnlineMarketingConsult : EmailTypeBase
{
    public CheckOnlineMarketingConsult()
    {
        Type = EmailType.OnlineMarketingConsult;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (preProcessedBody.Trim().ToUpper().Contains("ONLINE MARKETING CONSULT"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}