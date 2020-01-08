using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckLoanOffer : EmailTypeBase
{
    public CheckLoanOffer()
    {
        Type = EmailType.LoanOffer;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (preProcessedBody.Trim().ToUpper().Contains("FINANCIAL ASSISTANCE") ||
            preProcessedBody.Trim().ToUpper().Contains("FINANCIAL HELP") ||
            preProcessedBody.Trim().ToUpper().Contains("FINANCIAL PACKAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("PROJECT FINANCING") ||
            preProcessedBody.Trim().ToUpper().Contains("CONCERNING FUNDING OF YOUR BUSINESS PROJECT") ||
            preProcessedBody.Trim().ToUpper().Contains("CREDIT OFFER") ||
            preProcessedBody.Trim().ToUpper().Contains("LOW INTEREST RATE") ||
            preProcessedBody.Trim().ToUpper().Contains("LOAN") ||
            preProcessedBody.Trim().ToUpper().Contains("L0AN") ||
            preProcessedBody.Trim().ToUpper().Contains("WE OFFER ALL KINDS OF FINANCE") ||
            preProcessedBody.Trim().ToUpper().Contains("WE OFFER FAST AND LEGIT CASH") ||
            preProcessedBody.Trim().ToUpper().Contains("OFFER YOU A FINANCE") ||
            preProcessedBody.Trim().ToUpper().Contains("APPLY FOR CASH"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}