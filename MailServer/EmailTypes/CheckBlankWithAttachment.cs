using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckBlankWithAttachment : EmailTypeBase
{
    public CheckBlankWithAttachment()
    {
        Type = EmailType.BlankWithAttachment;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (PassNumber <= 1)
        {
            if (((preProcessedBody.Trim() == String.Empty || ((preProcessedBody.Length - currentMessage.SubjectLine.Length) < 40 &&
                (preProcessedBody.ToUpper().Contains("ATTACHMENT") || preProcessedBody.ToUpper().Contains("FILE") || preProcessedBody.ToUpper().Contains("ATTACHED") || preProcessedBody.ToUpper().Contains("DOCUMENT")))) && currentMessage.NumberOfAttachments > 0) ||
                ((preProcessedBody.Length - currentMessage.SubjectLine.Length) <= 3 && currentMessage.NumberOfAttachments > 0))
            {
                base.ParseResponse.IsMatch = true;
                base.ParseResponse.TotalHits++;
            }
        }
        else if (PassNumber == 2)
        {
            if (((preProcessedBody.Length - currentMessage.SubjectLine.Length) < 40 && (preProcessedBody.Trim().ToUpper().Contains("INLINE IMAGE"))) ||
            ((preProcessedBody.Length - currentMessage.SubjectLine.Length) < 50 && preProcessedBody.Trim().ToUpper().Contains("THIS MESSAGE IS FROM THE WORLD BANK") && currentMessage.NumberOfAttachments > 0) ||
            ((preProcessedBody.Length - currentMessage.SubjectLine.Length) < 110 && preProcessedBody.Trim().ToUpper().Contains("KINDLY SEND US YOUR PRICE LIST") && currentMessage.NumberOfAttachments > 0) ||
            ((preProcessedBody.Length - currentMessage.SubjectLine.Length) < 20 && preProcessedBody.Trim().ToUpper().Contains("OPEN") && currentMessage.NumberOfAttachments > 0) ||
            preProcessedBody.Trim().ToUpper().Contains("ATTACHED LETTER FOR DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("BELOW ATTACH") ||
            preProcessedBody.Trim().ToUpper().Contains("BELLOW ATTACH") ||
            preProcessedBody.Trim().ToUpper().Contains("DETAILS IN THE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("MY MASSAGE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("MY MASSAGE INCLUDED") ||
            preProcessedBody.Trim().ToUpper().Contains("MY MESSAGE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("MY MESSAGE INCLUDED") ||
            preProcessedBody.Trim().ToUpper().Contains("MY LETTER ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("MY LETTER INCLUDED") ||
            preProcessedBody.Trim().ToUpper().Contains("KINDLY OPEN THE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("KINDLY READ THE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("KINDLY SEE THE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE FIND ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE FIND AND DOWNLOAD ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("LETTER ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("LETTER ATTACHMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("OPEN ATTACHED FILE") ||
            preProcessedBody.Trim().ToUpper().Contains("OPEN ATTACHED LETTER") ||
            preProcessedBody.Trim().ToUpper().Contains("OPEN ATTACHED.") ||
            preProcessedBody.Trim().ToUpper().Contains("OPEN THE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("OPEN THE ATTACHMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("READ ATTACHED FILE") ||
            preProcessedBody.Trim().ToUpper().Contains("READ ATTACHED LETTER") ||
            preProcessedBody.Trim().ToUpper().Contains("READ ATTACHED.") ||
            preProcessedBody.Trim().ToUpper().Contains("READ THE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("READ THE ATTACHMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("READ THE DOCUMENTS") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE ATTACHED FILE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE ATTACHED LETTER") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE ATTACHED.") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE THE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE THE ATTACHMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("THROUGH THE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("VIEW ATTACHED FILE") ||
            preProcessedBody.Trim().ToUpper().Contains("VIEW ATTACHED LETTER") ||
            preProcessedBody.Trim().ToUpper().Contains("VIEW ATTACHED.") ||
            preProcessedBody.Trim().ToUpper().Contains("VIEW THE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("VIEW THE ATTACHMENT") ||
            (preProcessedBody.Trim().ToUpper().Contains("OPEN THE") && preProcessedBody.Trim().ToUpper().Contains("ATTACHED")))
            {
                base.ParseResponse.IsMatch = true;
                base.ParseResponse.TotalHits++;
            }
        }

        return base.ParseResponse;
    }
}