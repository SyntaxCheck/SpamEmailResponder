using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckPhishing : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckPhishing(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.Phishing;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("ACCOUNT HAS BEEN CREATED") ||
            preProcessedBody.Trim().ToUpper().Contains("ACCOUNT STATUS HAS BEEN CHANGED") ||
            preProcessedBody.Trim().ToUpper().Contains("BLOCKED ACCESS TO YOUR PAYPAL ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("CANCEL YOUR PAYPAL") ||
            preProcessedBody.Trim().ToUpper().Contains("CLICK HERE TO ACCESS MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("CLICK THE LINK BELOW") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPLETE YOUR PURCHASE") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRME YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("ESCROW COMMERCIAL BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("FAILURE TO DO SO PERMITS ACCOUNT SUSP") ||
            preProcessedBody.Trim().ToUpper().Contains("FAILURE TO SO PERMITS ACCOUNT SUSP") ||
            preProcessedBody.Trim().ToUpper().Contains("FIX MY ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("GOOGLE MANAGEMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("ISSUE WITH YOUR PAYPAL") ||
            preProcessedBody.Trim().ToUpper().Contains("LEFT THE FOLLOWING ITEM") ||
            preProcessedBody.Trim().ToUpper().Contains("LINK BELOW TO RESOLVE YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("MAILBOX HAS BEEN PROGRAMMED TO SHUT DOWN") ||
            preProcessedBody.Trim().ToUpper().Contains("PACKAGE OUT FOR DELIVER") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE CLICK BELOW TO STOP ACTION") ||
            preProcessedBody.Trim().ToUpper().Contains("POSSIBLE UNAUTHORISED ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("POSSIBLE UNAUTHORIZED ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("RESTRICTED YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("SECURE KEY APP") ||
            preProcessedBody.Trim().ToUpper().Contains("SOMEONE ACCESS TO YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("SOMEONE ACCESS YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("SOMEONE ACCESSED TO YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("SOMEONE ACCESSED YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("SOMEONE LOGGED TO YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("SOUTHWEST AIRLINES") ||
            preProcessedBody.Trim().ToUpper().Contains("SPAM ACTIVITIES") ||
            preProcessedBody.Trim().ToUpper().Contains("TEMPORARILY LOCKED") ||
            preProcessedBody.Trim().ToUpper().Contains("UNAUTHORIZED BY THE ACCOUNT OWNER") ||
            preProcessedBody.Trim().ToUpper().Contains("UNITED PARCEL SERVICE") ||
            preProcessedBody.Trim().ToUpper().Contains("UPDATE TO SECURE YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("USERGATE MAIL SERVICE") ||
            preProcessedBody.Trim().ToUpper().Contains("VISITED FROM AN UNUSUAL PLACE") ||
            preProcessedBody.Trim().ToUpper().Contains("WE DETECTED SOMETHING UNUSUAL") ||
            preProcessedBody.Trim().ToUpper().Contains("WE SUGGEST YOU SIGN IN WITH YOUR E-MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("WE SUGGEST YOU SIGN IN WITH YOUR EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("WE SUGGEST YOU SIGNIN WITH YOUR E-MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("WE SUGGEST YOU SIGNIN WITH YOUR EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU CAN SIGN IN ALIBABA") ||
            preProcessedBody.Trim().ToUpper().Contains("SITE UNDER MAINTENANCE") ||
            preProcessedBody.Trim().ToUpper().Contains("SITE IS UNDER MAINTENANCE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE A NOTIFICATION FROM") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ACCOUNT HAS BEEN LIMITED") ||
            preProcessedBody.Trim().ToUpper().Contains("WWW.HSBC.COM") ||
            (preProcessedBody.Trim().ToUpper().Contains("YOU HAVE (") && preProcessedBody.Trim().ToUpper().Contains(") NEW SECURITY MESSAGE")) ||
            (preProcessedBody.Trim().ToUpper().Contains("YOUR EMAIL ACCOUNT WILL BE PERM") && preProcessedBody.Trim().ToUpper().Contains("DISABLE")) ||
            (preProcessedBody.Trim().ToUpper().Contains("YOU HAVE (") && preProcessedBody.Trim().ToUpper().Contains(") NEW SECURITY MESSAGE")))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}