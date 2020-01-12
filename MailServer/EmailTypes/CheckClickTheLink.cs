using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckClickTheLink : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckClickTheLink(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.ClickTheLink;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("CLICK LINK") ||
            preProcessedBody.Trim().ToUpper().Contains("CLICK ABOVE LINK") ||
            preProcessedBody.Trim().ToUpper().Contains("CLICK BELOW LINK") ||
            preProcessedBody.Trim().ToUpper().Contains("CLICK ON LINK") ||
            preProcessedBody.Trim().ToUpper().Contains("CLICK ON THE LINK") ||
            preProcessedBody.Trim().ToUpper().Contains("CLICK THE ABOVE LINK") ||
            preProcessedBody.Trim().ToUpper().Contains("CLICK THE BELOW LINK") ||
            preProcessedBody.Trim().ToUpper().Contains("CLICK THE LINK") ||
            preProcessedBody.Trim().ToUpper().Contains("CLICK THIS ABOVE LINK") ||
            preProcessedBody.Trim().ToUpper().Contains("CLICK THIS BELOW LINK") ||
            preProcessedBody.Trim().ToUpper().Contains("CLICK THIS LINK") ||
            preProcessedBody.Trim().ToUpper().Contains("TO VIEW IT ONLINE, PLEASE GO HERE:"))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}