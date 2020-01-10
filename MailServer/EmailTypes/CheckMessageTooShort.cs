using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckMessageTooShort : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckMessageTooShort(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.MessageTooShort;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        return base.ParseResponse;
    }
}