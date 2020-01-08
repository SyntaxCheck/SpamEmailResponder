using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckSellingProducts : EmailTypeBase
{
    public CheckSellingProducts()
    {
        Type = EmailType.SellingProducts;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (preProcessedBody.Trim().ToUpper().Contains("MANUFACTURER OF LED") ||
            preProcessedBody.Trim().ToUpper().Contains("GOLD FOR SALE") ||
            preProcessedBody.Trim().ToUpper().Contains("ASPIRIN CREAM") ||
            preProcessedBody.Trim().ToUpper().Contains("OUR PRODUCT LINE") ||
            preProcessedBody.Trim().ToUpper().Contains("DIGIT GURU") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU WANT YOUR WEBSITE TO BE RANKED") ||
            preProcessedBody.Trim().ToUpper().Contains("LET US KNOW YOUR WEBSITE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEO, SEM, PPC") ||
            preProcessedBody.Trim().ToUpper().Contains("I SELL TOOLS") ||
            preProcessedBody.Trim().ToUpper().Contains("I BUILD ANY KIND OF PAGE OR LINK OR SCRIPT") ||
            (preProcessedBody.Trim().ToUpper().Contains("GOLD DUST") && preProcessedBody.Trim().ToUpper().Contains("BUYER")) ||
            (preProcessedBody.Trim().ToUpper().Contains("GOLD BARS") && preProcessedBody.Trim().ToUpper().Contains("BUYER")) ||
            (preProcessedBody.Trim().ToUpper().Contains("GOLD DUST") && preProcessedBody.Trim().ToUpper().Contains("FOR SALE")) ||
            (preProcessedBody.Trim().ToUpper().Contains("GOLD BARS") && preProcessedBody.Trim().ToUpper().Contains("FOR SALE")) ||
            (preProcessedBody.Trim().ToUpper().Contains("GOLD DUST") && preProcessedBody.Trim().ToUpper().Contains("COST FOR")) ||
            (preProcessedBody.Trim().ToUpper().Contains("GOLD BARS") && preProcessedBody.Trim().ToUpper().Contains("COST FOR")) ||
            preProcessedBody.Trim().ToUpper().Contains("LED DISPLAY SUPPLIER"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}