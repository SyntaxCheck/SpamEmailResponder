using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckJobOffer : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckJobOffer(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.JobOffer;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (PassNumber <= 1)
        {
            if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
                preProcessedBody.Trim().ToUpper().Contains("ASSIST ME GO INTO INDUSTRIALIZATION") ||
                preProcessedBody.Trim().ToUpper().Contains("CRUDE OIL LICENSE OPERATOR") ||
                preProcessedBody.Trim().ToUpper().Contains("DEVELOPMENT FOR SMALL TO LARGE") ||
                preProcessedBody.Trim().ToUpper().Contains("DOESNT INTERFERE WITH YOUR REGULAR WORK") ||
                preProcessedBody.Trim().ToUpper().Contains("EMAIL US YOUR RESUME") ||
                preProcessedBody.Trim().ToUpper().Contains("EMAIL US YOUR UPDATED RESUME") ||
                preProcessedBody.Trim().ToUpper().Contains("FULL TIME JOB") ||
                preProcessedBody.Trim().ToUpper().Contains("INTERESTED TO WORK AT") ||
                preProcessedBody.Trim().ToUpper().Contains("INTERESTED TO WORK FOR") ||
                preProcessedBody.Trim().ToUpper().Contains("INTERESTED TO WORK IN") ||
                preProcessedBody.Trim().ToUpper().Contains("JOB OFFER") ||
                preProcessedBody.Trim().ToUpper().Contains("JOB OPENING") ||
                preProcessedBody.Trim().ToUpper().Contains("JOB OPPORTUNITY") ||
                preProcessedBody.Trim().ToUpper().Contains("JOB PLACEMENT") ||
                preProcessedBody.Trim().ToUpper().Contains("JOB VACANCY") ||
                preProcessedBody.Trim().ToUpper().Contains("JOINING OUR TEAM") ||
                preProcessedBody.Trim().ToUpper().Contains("LOOKING ON CONTRACT") ||
                preProcessedBody.Trim().ToUpper().Contains("LOOKING TO CONTRACT") ||
                preProcessedBody.Trim().ToUpper().Contains("MOBILE APPLICATION DEVELOPMENT") ||
                preProcessedBody.Trim().ToUpper().Contains("PART TIME JOB") ||
                preProcessedBody.Trim().ToUpper().Contains("POSITION IN COMPANY") ||
                preProcessedBody.Trim().ToUpper().Contains("POSITION IN OUR COMPANY") ||
                preProcessedBody.Trim().ToUpper().Contains("RAW MATERIALS EXPORTERS") ||
                preProcessedBody.Trim().ToUpper().Contains("SEEKING A BROAD VARIETY OF INDIVIDUALS") ||
                preProcessedBody.Trim().ToUpper().Contains("SEEKING CERTIFIED BUSINESS INDIVIDUALS") ||
                preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR RESUME") ||
                preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR UPDATED RESUME") ||
                preProcessedBody.Trim().ToUpper().Contains("SUBMIT YOUR RESUME") ||
                preProcessedBody.Trim().ToUpper().Contains("THE JOB POSITION") ||
                preProcessedBody.Trim().ToUpper().Contains("THIS JOB IS APPLICABLE FOR") ||
                preProcessedBody.Trim().ToUpper().Contains("VACANT POST FOR MY COMPANY") ||
                preProcessedBody.Trim().ToUpper().Contains("VACANT POST FOR OUR COMPANY") ||
                preProcessedBody.Trim().ToUpper().Contains("VACANT POST IN MY COMPANY") ||
                preProcessedBody.Trim().ToUpper().Contains("VACANT POST IN OUR COMPANY") ||
                preProcessedBody.Trim().ToUpper().Contains("WORK FOR ME") ||
                preProcessedBody.Trim().ToUpper().Contains("WORK HERE") ||
                preProcessedBody.Trim().ToUpper().Contains("WORK TOGETHER AND SHARE COMMISSION") ||
                preProcessedBody.Trim().ToUpper().Contains("WORK WITH OUR HOTEL") ||
                preProcessedBody.Trim().ToUpper().Contains("WORKING AS PACKAGE RECEIVER") ||
                (preProcessedBody.Trim().ToUpper().Contains("I WANT YOU TO") && preProcessedBody.Trim().ToUpper().Contains("MANAGE THIS PROJECT")) ||
                (preProcessedBody.Trim().ToUpper().Contains("INTERESTED IN TAKING UP A ") && preProcessedBody.Trim().ToUpper().Contains("POSITION")) ||
                (preProcessedBody.Trim().ToUpper().Contains("EARN $") && preProcessedBody.Trim().ToUpper().Contains("WEEKLY REPLY FOR MORE")) ||
                (preProcessedBody.Trim().ToUpper().Contains("EARN US") && preProcessedBody.Trim().ToUpper().Contains("WEEKLY REPLY FOR MORE")) ||
                (preProcessedBody.Trim().ToUpper().Contains("OUR COMPANY") && preProcessedBody.Trim().ToUpper().Contains("WORK")))
            {
                base.ParseResponse.IsMatch = true;
                base.ParseResponse.TotalHits++;
            }
        }
        else if (PassNumber == 2)
        {
            if (preProcessedBody.Trim().ToUpper().Contains("EMPLOYMENT")) //If no other hits and the email contains employment assume its a job offer
            {
                base.ParseResponse.IsMatch = true;
                base.ParseResponse.TotalHits++;
            }
        }

        return base.ParseResponse;
    }
}