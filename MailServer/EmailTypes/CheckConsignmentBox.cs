using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckConsignmentBox : EmailTypeBase
{
    public CheckConsignmentBox()
    {
        Type = EmailType.ConsignmentBox;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (PassNumber <= 1)
        {
            if ((preProcessedBody.Trim().ToUpper().Contains("CONSIGNMENT") ||
                preProcessedBody.Trim().ToUpper().Contains("TRUNK BOX") ||
                preProcessedBody.Trim().ToUpper().Contains("PACKAGE BOX") ||
                preProcessedBody.Trim().ToUpper().Contains("SPECIAL PACKAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("PACKAGE DELIVER") ||
                preProcessedBody.Trim().ToUpper().Contains("YOUR PARCEL") ||
                preProcessedBody.Trim().ToUpper().Contains("DELIVER YOUR PACKAGE")) &&
                (!preProcessedBody.Trim().ToUpper().Contains("NOT A CONSIGNMENT") || //If we misclasified the type they might tell us we are not receiving a consignment box
                !preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVING A CONSIGNMENT") ||
                !preProcessedBody.Trim().ToUpper().Contains("NOT CONSIGNMENT")))
            {
                base.ParseResponse.IsMatch = true;
                base.ParseResponse.TotalHits++;
            }
        }
        else if (PassNumber == 2)
        {
            if (preProcessedBody.Trim().ToUpper().Contains("BOX")) //If no other hits then just look for the word BOX
            {
                base.ParseResponse.IsMatch = true;
                base.ParseResponse.TotalHits++;
            }
        }

        return base.ParseResponse;
    }
}