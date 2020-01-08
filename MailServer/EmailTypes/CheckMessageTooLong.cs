using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckMessageTooLong : EmailTypeBase
{
    public CheckMessageTooLong()
    {
        Type = EmailType.MessageTooLong;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        return base.ParseResponse;
    }
}