using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckMoneyStorage : EmailTypeBase
{
    public CheckMoneyStorage()
    {
        Type = EmailType.MoneyStorage;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (preProcessedBody.Trim().ToUpper().Contains("ABANDONED SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF MONEY IN YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF MONEY TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF MONEY YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("ASSISTANCE IN ORDER TO MOVE THE BALANCE") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOUR ACCOUNT RECEIVE $") ||
            preProcessedBody.Trim().ToUpper().Contains("CANNOT BE ABLE TO MOVE THIS HUGE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPENSATION FOR YOUR ASSISTANCE") ||
            preProcessedBody.Trim().ToUpper().Contains("DEPOSIT DOCUMENTS WHICH CONTAIN") ||
            preProcessedBody.Trim().ToUpper().Contains("DISTRIBUTE FUND WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("DISTRIBUTE FUNDS WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("DISTRIBUTE MY FUND WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("DISTRIBUTE MY FUNDS WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("EVACUATE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("EVACUATE THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS ALLOCATION") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND WAS MOVED") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND WERE MOVED") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS WAS MOVED") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS WERE MOVED") ||
            preProcessedBody.Trim().ToUpper().Contains("HANDLE TRANSACTION WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("HANDLING THIS TRANSACTION") ||
            preProcessedBody.Trim().ToUpper().Contains("HIGHLY PROFITABLE PROJECT") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEEDED A TRUSTED PARTNER") ||
            preProcessedBody.Trim().ToUpper().Contains("I RATHER SEND IT TO SOMEONE I DONT KNOW ON A MUTUAL AGREEMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT TO MOVE THIS MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("IS YOUR ACCOUNT ABLE RECEIVE $") ||
            preProcessedBody.Trim().ToUpper().Contains("IS YOUR ACCOUNT ABLE TO RECEIVE $") ||
            preProcessedBody.Trim().ToUpper().Contains("KEEP MY MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("KEEP THE FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("KEEP THE MONEY SAFE") ||
            preProcessedBody.Trim().ToUpper().Contains("KEEP THE MONEY SAVE") ||
            preProcessedBody.Trim().ToUpper().Contains("MOVE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("MOVE THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("PROPOSAL REGARDING MY FAMILY ESTATE") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THE DELIVERY ON MY BEHALF") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THE FUND AND KEEP IT") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THE MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SAFE KEEPING MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SAFE KEEPING OF MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SAFE KEEPING OF THE MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("STORE MY MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("VENTURE THAT WILL BENEFIT BOTH PART") ||
            preProcessedBody.Trim().ToUpper().Contains("VENTURE WHICH I WILL LIKE TO HANDLE WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK WITH ME AND CLAIM IT") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK WITH YOU IN SECURING THESE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK WITH YOU IN SECURING THIS FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK WITH YOU ON SECURING THESE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK WITH YOU ON SECURING THIS FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ACCOUNT TO RECEIVE $") ||
            (preProcessedBody.Trim().ToUpper().Contains("MOVE OUT OF THE COUNTRY") && preProcessedBody.Trim().ToUpper().Contains("FUNDS")))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}