using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckMoneyHack : EmailTypeBase
{
    public CheckMoneyHack()
    {
        Type = EmailType.MoneyHack;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (preProcessedBody.Trim().ToUpper().Contains("ATM BLANK CARD") ||
            (preProcessedBody.Trim().ToUpper().Contains("YOU PAY $") && preProcessedBody.Trim().ToUpper().Contains("AND GET $")) ||
            preProcessedBody.Trim().ToUpper().Contains("BUYING THE SAME PRODUCT FOR"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}