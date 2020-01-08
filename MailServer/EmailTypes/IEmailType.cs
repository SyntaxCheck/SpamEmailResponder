using System.Collections.Generic;
using static ResponseProcessing;

public interface IEmailType
{
    EmailType Type { get; set; }
    TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody);
}