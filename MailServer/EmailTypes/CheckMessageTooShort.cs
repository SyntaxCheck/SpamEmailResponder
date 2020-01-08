using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckMessageTooShort : EmailTypeBase
{
    public CheckMessageTooShort()
    {
        Type = EmailType.MessageTooShort;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        return base.ParseResponse;
    }
}