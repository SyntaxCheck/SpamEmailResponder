using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckOilAndGas : EmailTypeBase
{
    public CheckOilAndGas()
    {
        Type = EmailType.OilAndGas;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (preProcessedBody.Trim().ToUpper().Contains("OIL AND GAS") ||
            preProcessedBody.Trim().ToUpper().Contains("PETROLEUM COMMODITIES AVAILABLE") ||
            preProcessedBody.Trim().ToUpper().Contains("CRUDE OIL BUSINES") ||
            preProcessedBody.Trim().ToUpper().Contains("CRUDE OIL PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("CRUDE OIL SALES") ||
            preProcessedBody.Trim().ToUpper().Contains("GAS AND OIL"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}