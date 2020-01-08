using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ResponseProcessing;

public abstract class EmailTypeBase : IEmailType
{
    public EmailTypeBase()
    {
        PassNumber = 1;
        ParseResponse = new TypeParseResponse() { IsMatch = false, MatchStrength = 0, TotalHits = 0 };
    }

    public EmailType Type { get; set; }
    public TypeParseResponse ParseResponse { get; set; }
    public int PassNumber { get; set; }

    public abstract TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody);
}