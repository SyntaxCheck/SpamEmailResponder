using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckGenericPayment : EmailTypeBase
{
    public CheckGenericPayment()
    {
        Type = EmailType.GenericPayment;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (PassNumber <= 1)
        {
            if (preProcessedBody.Trim().ToUpper().Contains("PAYMENT") ||
                preProcessedBody.Trim().ToUpper().Contains("COST FOR THE") ||
                preProcessedBody.Trim().ToUpper().Contains("MONEYGRAM") ||
                preProcessedBody.Trim().ToUpper().Contains("MONEY GRAM") ||
                preProcessedBody.Trim().ToUpper().Contains("WESTERN UNION") ||
                preProcessedBody.Trim().ToUpper().Contains("TRANSFER THE FUND") ||
                preProcessedBody.Trim().ToUpper().Contains("TRANSFER THIS FUND") ||
                preProcessedBody.Trim().ToUpper().Contains("HELP ME WITH THE RENEW DUES") ||
                preProcessedBody.Trim().ToUpper().Contains("GENERAL INSURANCE FEE") ||
                preProcessedBody.Trim().ToUpper().Contains("I ADVISE YOU TO PUT $") ||
                preProcessedBody.Trim().ToUpper().Contains("PARTNERSHIP TO TRANSFER") ||
                preProcessedBody.Trim().ToUpper().Contains("PAY THE UP FRONT FEE") ||
                preProcessedBody.Trim().ToUpper().Contains("PAY THE UPFRONT FEE") ||
                preProcessedBody.Trim().ToUpper().Contains("PAY THE UP FRONT AMOUNT") ||
                preProcessedBody.Trim().ToUpper().Contains("PAY THE UPFRONT AMOUNT") ||
                preProcessedBody.Trim().ToUpper().Contains("SEND TO THE BANK") ||
                preProcessedBody.Trim().ToUpper().Contains("ASSIST ME WITH THE RENEW DUES"))
            {
                base.ParseResponse.IsMatch = true;
                base.ParseResponse.TotalHits++;
            }
        }
        else if (PassNumber == 2)
        {
            if (preProcessedBody.Trim().ToUpper().Contains("$")) //If no other hits then just look for the Dollar symbol
            {
                base.ParseResponse.IsMatch = true;
                base.ParseResponse.TotalHits++;
            }
        }

        return base.ParseResponse;
    }
}