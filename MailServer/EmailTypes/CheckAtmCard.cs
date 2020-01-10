using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckAtmCard : EmailTypeBase
{
    private ResponseSettings Settings {get; set;}

    public CheckAtmCard(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.AtmCard;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("ATM CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("ATMCARD") ||
            preProcessedBody.Trim().ToUpper().Contains("ATM CREDIT CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("VISA CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("ATM MASTER CREDIT CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("ATM MASTER CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("MASTER CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("ATM VISA CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS A CREDIT CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ATM WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("BANK CHEQUE"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}