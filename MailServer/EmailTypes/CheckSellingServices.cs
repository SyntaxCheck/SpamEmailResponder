using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckSellingServices : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckSellingServices(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.SellingServices;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("1ST PAGE RANK") ||
            preProcessedBody.Trim().ToUpper().Contains("WEB DESIGN") ||
            preProcessedBody.Trim().ToUpper().Contains("GENERATE HIGHER VISITOR TRAFFIC TO YOUR WEBSITE") ||
            preProcessedBody.Trim().ToUpper().Contains("HELP YOU OPTIMIZE YOUR WEBSITE") ||
            preProcessedBody.Trim().ToUpper().Contains("WEBSITE DESIGN") ||
            preProcessedBody.Trim().ToUpper().Contains("WEB SITE DESIGN") ||
            preProcessedBody.Trim().ToUpper().Contains("WANT YOUR CREDIT SCORE INCREASED") ||
            preProcessedBody.Trim().ToUpper().Contains("SEO, SEM, PPC") ||
            preProcessedBody.Trim().ToUpper().Contains("SEO COMPANY") ||
            preProcessedBody.Trim().ToUpper().Contains("SEO PACKAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEO PARAMETER") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU WANT YOUR WEBSITE TO BE RANKED") ||
            preProcessedBody.Trim().ToUpper().Contains("ONLINE MARKETING MANAGER") ||
            preProcessedBody.Trim().ToUpper().Contains("LET US KNOW YOUR WEBSITE") ||
            preProcessedBody.Trim().ToUpper().Contains("FRESH TOOLS & UNLIMITED SENDERS") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTIFIED SEVERAL SEO") ||
            preProcessedBody.Trim().ToUpper().Contains("MAILERSTORE.EU") ||
            preProcessedBody.Trim().ToUpper().Contains("I SELL TOOLS") ||
            preProcessedBody.Trim().ToUpper().Contains("I BUILD ANY KIND OF PAGE OR LINK OR SCRIPT") ||
            preProcessedBody.Trim().ToUpper().Contains("I SELL GOOD TOOLS") ||
            preProcessedBody.Trim().ToUpper().Contains("I WAS GOING THROUGH YOUR WEBSITE") ||
            preProcessedBody.Trim().ToUpper().Contains("VISIBILITY ON THE SEARCH ENGINE") ||
            preProcessedBody.Trim().ToUpper().Contains("TALENTED WEB DEVELOPER") ||
            preProcessedBody.Trim().ToUpper().Contains("DEVELOPMENT FIRM"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}