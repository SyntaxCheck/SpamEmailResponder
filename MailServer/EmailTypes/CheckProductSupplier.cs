using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckProductSupplier : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckProductSupplier(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.ProductSupplier;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("LOOKING FOR A SUPPLIER") ||
            preProcessedBody.Trim().ToUpper().Contains("LOOKING FOR SUPPLIER") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND CURRENT CATALOG") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME CATALOG") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR CATALOG") ||
            preProcessedBody.Trim().ToUpper().Contains("SUPPLY OF YOUR PRODUCTS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PRODUCTS"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}