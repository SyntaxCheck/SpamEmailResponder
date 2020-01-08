using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckShipping : EmailTypeBase
{
    public CheckShipping()
    {
        Type = EmailType.Shipping;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (preProcessedBody.Trim().ToUpper().Contains("TRUSTING US WITH YOUR SHIPMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY TRACKING") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY UPDATE") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY PACKAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("USPS REF") ||
            preProcessedBody.Trim().ToUpper().Contains("USPS TRACK") ||
            preProcessedBody.Trim().ToUpper().Contains("FEDEX REF") ||
            preProcessedBody.Trim().ToUpper().Contains("FEDEX TRACK") ||
            preProcessedBody.Trim().ToUpper().Contains("UPS REF") ||
            preProcessedBody.Trim().ToUpper().Contains("UPS TRACK") ||
            preProcessedBody.Trim().ToUpper().Contains("DHL REF") ||
            preProcessedBody.Trim().ToUpper().Contains("DHL TRACK") ||
            preProcessedBody.Trim().ToUpper().Contains("FIRSTFRONT EXPRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR DELIVERY") ||
            preProcessedBody.Trim().ToUpper().Contains("REGARDS TO YOUR PACKAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("TRACKING NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("STATUS OF YOUR PACKAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("WILL BE DELIVER TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("WILL BE DELIVERED TO YOU"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}