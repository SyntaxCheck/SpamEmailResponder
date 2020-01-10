using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckMessageTooLong : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckMessageTooLong(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.MessageTooLong;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        return base.ParseResponse;
    }
}