using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckBuildTrust : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckBuildTrust(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.BuildTrust;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("ALWAYS WITH STORY") ||
            preProcessedBody.Trim().ToUpper().Contains("AS LONG AS YOU REMAIN HONEST") ||
            preProcessedBody.Trim().ToUpper().Contains("AWARE OF YOUR BACKGROUND") ||
            preProcessedBody.Trim().ToUpper().Contains("BECOME A GOOD FRIEND") ||
            preProcessedBody.Trim().ToUpper().Contains("BECOME YOU GOOD FRIEND") ||
            preProcessedBody.Trim().ToUpper().Contains("BECOME YOUR GOOD FRIEND") ||
            preProcessedBody.Trim().ToUpper().Contains("BUILD TRUST") ||
            preProcessedBody.Trim().ToUpper().Contains("BUILD A GOOD RELATION") ||
            preProcessedBody.Trim().ToUpper().Contains("BUILD A FRUITFUL RELATION") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN I TRUST YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("CONVINCED THAT I AM COMMUNICATING WITH THE RIGHT PERSON") ||
            preProcessedBody.Trim().ToUpper().Contains("CONVINCED THAT I AM TALKING WITH THE RIGHT PERSON") ||
            preProcessedBody.Trim().ToUpper().Contains("CURRENTLY SINGLE GIRL") ||
            preProcessedBody.Trim().ToUpper().Contains("CURRENTLY SINGLE WOMAN") ||
            preProcessedBody.Trim().ToUpper().Contains("CREATING A SUCCESSFUL RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("DATING SITE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSSION ABOUT FRIENDSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("FOR LASTING RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("FRIEND TO TALK TO") ||
            preProcessedBody.Trim().ToUpper().Contains("GET TO KNOW EACH OTHER") ||
            preProcessedBody.Trim().ToUpper().Contains("GET TO KNOW EACHOTHER") ||
            preProcessedBody.Trim().ToUpper().Contains("GET TO KNOW YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("GOOD PARTNER") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE GOOD RELATIONSHIP WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HEAR MORE ABOUT YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HERE MORE ABOUT YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HOPE FOR FRIENDS") ||
            preProcessedBody.Trim().ToUpper().Contains("I AM A SINGLE GIRL") ||
            preProcessedBody.Trim().ToUpper().Contains("I AM A SINGLE WOMAN") ||
            preProcessedBody.Trim().ToUpper().Contains("I AM A SINGLE YOUNG LADY") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED YOUR RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("I LIKE TO HAVE MANY FRIEND") ||
            preProcessedBody.Trim().ToUpper().Contains("I LIKE TO KNOW YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I MISS SPENDING MORE TIME") ||
            preProcessedBody.Trim().ToUpper().Contains("I PICK INTEREST ON YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I SAW YOUR PROFILE TODAY") ||
            preProcessedBody.Trim().ToUpper().Contains("I SEE YOU AS SOMEONE I CAN WORK WITH") ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT TO MAKE A NEW AND SPECIAL FRIEND") ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT TO BE YOUR BEST FRIEND") ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT TO BE YOUR FRIEND") ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT US TO BE FRIEND") ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT US TO BECOME FRIEND") ||
            preProcessedBody.Trim().ToUpper().Contains("I WILL LIKE TO NO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I WILL TELL YOU MORE ABOUT MYSELF") ||
            preProcessedBody.Trim().ToUpper().Contains("I WISH TO TELL YOU ABOUT") ||
            preProcessedBody.Trim().ToUpper().Contains("I WOULD LIKE TO KNOW YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("IF YOU CAN TRUST") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERESTED IN KNOWING YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERESTED TO KNOW YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERESTED TO GET TO KNOW") ||
            preProcessedBody.Trim().ToUpper().Contains("IS THIS EMAIL PRIVATE") ||
            preProcessedBody.Trim().ToUpper().Contains("KISS AND CUDDLE") ||
            preProcessedBody.Trim().ToUpper().Contains("KNOW YOU BETTER") ||
            preProcessedBody.Trim().ToUpper().Contains("LIKE TO KNOW YOU MORE") ||
            preProcessedBody.Trim().ToUpper().Contains("LONG TERM RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("LONGTERM RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("LOOKING FOR YOUR FRIENDSH") ||
            preProcessedBody.Trim().ToUpper().Contains("LOOKING UP FOR MY GIRLFRIEND") ||
            preProcessedBody.Trim().ToUpper().Contains("LOVE RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("MAN WITH GOOD SENSE OF HUMOR") ||
            preProcessedBody.Trim().ToUpper().Contains("MEANINGFUL RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("MORE DETAIL ABOUT MYSELF") ||
            preProcessedBody.Trim().ToUpper().Contains("MORE DETAIL ABOUT YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("NEED YOUR ADVICE") ||
            preProcessedBody.Trim().ToUpper().Contains("OPEN UP TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE EXPRESS YOURSELF TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("PRESENTLY SINGLE GIRL") ||
            preProcessedBody.Trim().ToUpper().Contains("PRESENTLY SINGLE WOMAN") ||
            preProcessedBody.Trim().ToUpper().Contains("RELIABLE AND HONEST") ||
            preProcessedBody.Trim().ToUpper().Contains("REALLY LOVE TO KNOW YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("REALLY LOVE TO GET TO KNOW YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("REALLY WANT TO KNOW YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("REALLY WANT TO GET TO KNOW YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("SEARCHING FOR FRIENDSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("SEEKING YOUR ASSISTANCE") ||
            preProcessedBody.Trim().ToUpper().Contains("SERIOUS RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("SHARE PICTURE") ||
            preProcessedBody.Trim().ToUpper().Contains("SHARE MY PICTURE") ||
            preProcessedBody.Trim().ToUpper().Contains("SINGLE NEVER MARRIED") ||
            preProcessedBody.Trim().ToUpper().Contains("SINGLE LOOKING FOR") ||
            preProcessedBody.Trim().ToUpper().Contains("SOMEONE I CAN TRUST") ||
            preProcessedBody.Trim().ToUpper().Contains("TELL YOU MORE ABOUT MYSELF") ||
            preProcessedBody.Trim().ToUpper().Contains("THE FUND IS SAFE") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS 100% SAFE") ||
            preProcessedBody.Trim().ToUpper().Contains("UNTIL YOU RESPOND BACK") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT ABOUT YOU?") ||
            preProcessedBody.Trim().ToUpper().Contains("WE CAN LEARN MORE ABOUT EACH OTHER") ||
            preProcessedBody.Trim().ToUpper().Contains("VENTURE OF TRUST") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU SHOULD KEEP ME UPDATE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR GOOD FRIEND") ||
            (preProcessedBody.Trim().ToUpper().Contains("TRUST") && preProcessedBody.Trim().ToUpper().Contains("FRIENDSHIP")) ||
            (preProcessedBody.Trim().ToUpper().Contains("I AM WOMAN OF") && preProcessedBody.Trim().ToUpper().Contains("YEARS OLD FROM")))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}
