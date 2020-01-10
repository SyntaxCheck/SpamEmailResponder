using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckScamVictim : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckScamVictim(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.ScamVictim;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("SCAM VICTIM") ||
            preProcessedBody.Trim().ToUpper().Contains("VICTIM OF SCAM") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE BEEN SCAM") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW EXACTLY WERE YOU SCAMMED") ||
            preProcessedBody.Trim().ToUpper().Contains("SENT FEE TO SCAM") ||
            preProcessedBody.Trim().ToUpper().Contains("MONEY TO THOSE SCAM") ||
            preProcessedBody.Trim().ToUpper().Contains("MONEY THAT YOU LOST TO SCAM") ||
            preProcessedBody.Trim().ToUpper().Contains("TO SCAM INNOCENT") ||
            preProcessedBody.Trim().ToUpper().Contains(".TO .SCAMMER") ||
            preProcessedBody.Trim().ToUpper().Contains("LOST MONEY TO SCAMMER") ||
            preProcessedBody.Trim().ToUpper().Contains("LOST MONEY DURING INTERNATIONAL TRANS") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE BEEN ABLE TO TRACK DOWN OFFICER") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE BE ABLE TO TRACK DOWN OFFICER") ||
            (preProcessedBody.Trim().ToUpper().Contains("YOU HAVE LOST A LOT OF MONEY") && preProcessedBody.Trim().ToUpper().Contains("COMPENSATE YOU")) ||
            (preProcessedBody.Trim().ToUpper().Contains("COMPENSATE YOU") && preProcessedBody.Trim().ToUpper().Contains("SCAM")))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}