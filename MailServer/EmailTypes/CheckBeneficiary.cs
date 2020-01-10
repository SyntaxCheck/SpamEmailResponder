using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckBeneficiary : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckBeneficiary(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.Beneficiary;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("BENEFICIARY") ||
            preProcessedBody.Trim().ToUpper().Contains("ESTATE OF YOUR DECEASED") ||
            preProcessedBody.Trim().ToUpper().Contains("NEXT OF KIN"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}