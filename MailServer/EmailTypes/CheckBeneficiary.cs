using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckBeneficiary : EmailTypeBase
{
    public CheckBeneficiary()
    {
        Type = EmailType.Beneficiary;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (preProcessedBody.Trim().ToUpper().Contains("BENEFICIARY") ||
            preProcessedBody.Trim().ToUpper().Contains("ESTATE OF YOUR DECEASED") ||
            preProcessedBody.Trim().ToUpper().Contains("NEXT OF KIN"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}