using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckRefugee : EmailTypeBase
{
    public CheckRefugee()
    {
        Type = EmailType.Refugee;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (preProcessedBody.Trim().ToUpper().Contains("AS A RESULT OF MY PRESENT SIT") ||
            preProcessedBody.Trim().ToUpper().Contains("HELP US COME OVER TO YOUR PLACE") ||
            preProcessedBody.Trim().ToUpper().Contains("HELP ME AND ME SISTER") ||
            preProcessedBody.Trim().ToUpper().Contains("REFUGEE") ||
            preProcessedBody.Trim().ToUpper().Contains("WANTED TO KILL ME") ||
            preProcessedBody.Trim().ToUpper().Contains("WE ARE LUCK TO RUN") ||
            preProcessedBody.Trim().ToUpper().Contains("WE ARE IN LUCK TO RUN") ||
            preProcessedBody.Trim().ToUpper().Contains("WILLING TO ASSIST ME") ||
            (preProcessedBody.Trim().ToUpper().Contains("PARENTS WAS AMONG THE") && preProcessedBody.Trim().ToUpper().Contains("CRASH")))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}