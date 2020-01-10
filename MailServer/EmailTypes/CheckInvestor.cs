using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckInvestor : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckInvestor(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.Investor;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("ASSIST ME TO RECEIVE AND INVEST THIS") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS CONTRACT") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS COOPERATION") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS DEAL") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS DISCUSSION") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS JOINT PARTNER") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS OFFER") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS PARTNER") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS TALK") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS THAT COULD BE BROUGHT YOUR WAY") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS THAT WILL BENEFIT BOTH OF US") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS TO DISCUSS WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOU WORK WITH ME") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIDENTIAL DEAL") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND YOU BUSINESS") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND YOUR BUSINESS") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDING YOU BUSINESS") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDING YOUR BUSINESS") ||
            preProcessedBody.Trim().ToUpper().Contains("HAS A PROJECT FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE A DEAL FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE A PROJECT FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE PROJECT FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HELP ME INVEST") ||
            preProcessedBody.Trim().ToUpper().Contains("HELPING ME INVEST") ||
            preProcessedBody.Trim().ToUpper().Contains("IF WE WORK TOGETHER") ||
            preProcessedBody.Trim().ToUpper().Contains("IMPORTANT PARTNERSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERESTED IN A BUSINESS") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERESTING DEAL WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("INVESTMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("INVESTOR") ||
            preProcessedBody.Trim().ToUpper().Contains("LIKE TO KNOW IF YOU CAN BE OUR DISTRIBUTOR") ||
            preProcessedBody.Trim().ToUpper().Contains("LUCRATIVE PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("LUCRATIVE/CONFIDENTIAL BUSINES") ||
            preProcessedBody.Trim().ToUpper().Contains("LUCRATIVE/CONFIDENTIAL DEAL") ||
            preProcessedBody.Trim().ToUpper().Contains("LUCRATIVE/CONFIDENTIAL OPPORTUNITY") ||
            preProcessedBody.Trim().ToUpper().Contains("LUCRATIVE/CONFIDENTIAL PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("MIDDLEMAN BETWEEN OUR COMPANY") ||
            preProcessedBody.Trim().ToUpper().Contains("PARTNER WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSING THIS OPPORTUNITY TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("PICKED YOU FOR A HUMANITARIAN GRANT") ||
            preProcessedBody.Trim().ToUpper().Contains("PICKED YOU FOR HUMANITARIAN GRANT") ||
            preProcessedBody.Trim().ToUpper().Contains("PRIVATE OFFER WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("PROFIT SHARING") ||
            preProcessedBody.Trim().ToUpper().Contains("PROFITABLE PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("PROPOSAL FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("PROPOSAL THAT MIGHT INTEREST") ||
            preProcessedBody.Trim().ToUpper().Contains("REGARDING A PROJECT") ||
            preProcessedBody.Trim().ToUpper().Contains("SPONSOR A PROJECT") ||
            preProcessedBody.Trim().ToUpper().Contains("SPONSOR THE PROJECT") ||
            preProcessedBody.Trim().ToUpper().Contains("WE CAN WORK OUT THIS FOR OUR BENEFIT") ||
            preProcessedBody.Trim().ToUpper().Contains("WE CAN WORK THIS OUT FOR OUR BENEFIT") ||
            preProcessedBody.Trim().ToUpper().Contains("WE CAN WORK TOGETHER") ||
            preProcessedBody.Trim().ToUpper().Contains("WISH TO INVEST") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK WITH YOU") ||
            (preProcessedBody.Trim().ToUpper().Contains("LETS SPLIT") && (preProcessedBody.Trim().ToUpper().Contains("IN THIS DEAL") || preProcessedBody.Trim().ToUpper().Contains("ON THIS DEAL"))) ||
            (preProcessedBody.Trim().ToUpper().Contains("PROJECT") && preProcessedBody.Trim().ToUpper().Contains("BENEFIT TO YOU")))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}