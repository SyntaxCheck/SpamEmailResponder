using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckAccountProblem : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckAccountProblem(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.AccountProblem;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("ACCOUNT SUSPENDED") ||
            preProcessedBody.Trim().ToUpper().Contains("ACCOUNT SUSPENSION") ||
            preProcessedBody.Trim().ToUpper().Contains("ACCOUNT HAS BEEN BLACKLISTED") ||
            preProcessedBody.Trim().ToUpper().Contains("VERIFICATION FAILURE ON YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("WARNING: VIRUS ALERT") ||
            preProcessedBody.Trim().ToUpper().Contains("WARNING VIRUS ALERT"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}