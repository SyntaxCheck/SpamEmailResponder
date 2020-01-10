using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckMoneyHack : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckMoneyHack(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.MoneyHack;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("ATM BLANK CARD") ||
            (preProcessedBody.Trim().ToUpper().Contains("YOU PAY $") && preProcessedBody.Trim().ToUpper().Contains("AND GET $")) ||
            preProcessedBody.Trim().ToUpper().Contains("BUYING THE SAME PRODUCT FOR"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}