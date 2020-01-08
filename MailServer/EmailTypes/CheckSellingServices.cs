using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckSellingServices : EmailTypeBase
{
    public CheckSellingServices()
    {
        Type = EmailType.SellingServices;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (preProcessedBody.Trim().ToUpper().Contains("1ST PAGE RANK") ||
            preProcessedBody.Trim().ToUpper().Contains("WEB DESIGN") ||
            preProcessedBody.Trim().ToUpper().Contains("GENERATE HIGHER VISITOR TRAFFIC TO YOUR WEBSITE") ||
            preProcessedBody.Trim().ToUpper().Contains("WEBSITE DESIGN") ||
            preProcessedBody.Trim().ToUpper().Contains("WEB SITE DESIGN") ||
            preProcessedBody.Trim().ToUpper().Contains("WANT YOUR CREDIT SCORE INCREASED") ||
            preProcessedBody.Trim().ToUpper().Contains("SEO COMPANY") ||
            preProcessedBody.Trim().ToUpper().Contains("SEO PACKAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("I SELL GOOD TOOLS") ||
            preProcessedBody.Trim().ToUpper().Contains("DEVELOPMENT FIRM"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}