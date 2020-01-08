using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckGenericAdvertisement : EmailTypeBase
{
    public CheckGenericAdvertisement()
    {
        Type = EmailType.GenericAdvertisement;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (preProcessedBody.Trim().ToUpper().Contains("CONSIDER TRADING WITH") ||
            preProcessedBody.Trim().ToUpper().Contains("CREDIT CARD DEBT CLEARANCE"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}