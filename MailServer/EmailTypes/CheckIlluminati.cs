using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckIlluminati : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckIlluminati(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.Illuminati;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("ILLUMINATI") ||
            preProcessedBody.Trim().ToUpper().Contains("ILUMINATI"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}