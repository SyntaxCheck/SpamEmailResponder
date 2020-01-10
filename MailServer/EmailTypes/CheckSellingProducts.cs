using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckSellingProducts : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckSellingProducts(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.SellingProducts;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("MANUFACTURER OF LED") ||
            preProcessedBody.Trim().ToUpper().Contains("GOLD FOR SALE") ||
            preProcessedBody.Trim().ToUpper().Contains("ASPIRIN CREAM") ||
            preProcessedBody.Trim().ToUpper().Contains("OUR PRODUCT LINE") ||
            preProcessedBody.Trim().ToUpper().Contains("DIGIT GURU") ||
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