using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckOnlineMarketingConsult : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckOnlineMarketingConsult(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.OnlineMarketingConsult;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("ONLINE MARKETING CONSULT"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}