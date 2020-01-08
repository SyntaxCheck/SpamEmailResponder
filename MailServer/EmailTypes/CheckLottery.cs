using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckLottery : EmailTypeBase
{
    public CheckLottery()
    {
        Type = EmailType.Lottery;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (preProcessedBody.Trim().ToUpper().Contains("CONGRATULATIONS! YOU WON") ||
            preProcessedBody.Trim().ToUpper().Contains("CONGRATULATIONS, YOU WON") ||
            preProcessedBody.Trim().ToUpper().Contains("CONGRATULATIONS. YOU WON") ||
            preProcessedBody.Trim().ToUpper().Contains("COPY OF YOUR WINNING") ||
            preProcessedBody.Trim().ToUpper().Contains("E-MAIL HAS WON") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL HAS WON") ||
            preProcessedBody.Trim().ToUpper().Contains("GET A FREE IPHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("GET FREE IPHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("INFORM YOU THAT YOU WERE SELECTED FOR THE") ||
            preProcessedBody.Trim().ToUpper().Contains("LOTTERY") ||
            preProcessedBody.Trim().ToUpper().Contains("LOTTO DRAW") ||
            preProcessedBody.Trim().ToUpper().Contains("MILLION LOTTO") ||
            preProcessedBody.Trim().ToUpper().Contains("POWER BALL") ||
            preProcessedBody.Trim().ToUpper().Contains("POWERBALL") ||
            preProcessedBody.Trim().ToUpper().Contains("WINNER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR E-MAIL ADDERESS HAS WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR E-MAIL ADDERESS HAVE WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR E-MAIL ADDRESS HAS WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR E-MAIL ADDRESS HAVE WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR E-MAIL HAS WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR E-MAIL HAVE WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR EMAIL ADDERESS HAS WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR EMAIL ADDERESS HAVE WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR EMAIL ADDRESS HAS WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR EMAIL ADDRESS HAVE WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR EMAIL HAS WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR EMAIL HAVE WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR WINNING PIN") ||
            (preProcessedBody.Trim().ToUpper().Contains("CONGRATULATIONS") && preProcessedBody.Trim().ToUpper().Contains("PROMO")) ||
            ((preProcessedBody.Trim().ToUpper().Contains("YOU HAVE BEEN CHOSEN") || preProcessedBody.Trim().ToUpper().Contains("YOU HAVE BEEN CHOOSEN")) && (preProcessedBody.Trim().ToUpper().Contains("AWARD") || preProcessedBody.Trim().ToUpper().Contains("PROMO"))))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}