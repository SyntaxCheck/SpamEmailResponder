using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckDeathOrDying : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckDeathOrDying(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.DeathOrDying;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("AS A RESULT OF MY HEALTH") ||
            preProcessedBody.Trim().ToUpper().Contains("BEFORE I DIE I HAVE AN IMPORTANT") ||
            preProcessedBody.Trim().ToUpper().Contains("BEFORE HE DIED") ||
            preProcessedBody.Trim().ToUpper().Contains("BEFORE SHE DIED") ||
            preProcessedBody.Trim().ToUpper().Contains("CANCER DIAG") ||
            preProcessedBody.Trim().ToUpper().Contains("CANCER DISEASE") ||
            preProcessedBody.Trim().ToUpper().Contains("CANCER PATIENT") ||
            preProcessedBody.Trim().ToUpper().Contains("CHILD DIED") ||
            preProcessedBody.Trim().ToUpper().Contains("DAUGHTER DIED") ||
            preProcessedBody.Trim().ToUpper().Contains("DAY TO LIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("DAYS TO LIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("DIAGNOSED CANCER") ||
            preProcessedBody.Trim().ToUpper().Contains("DIAGNOSED FOR CANCER") ||
            preProcessedBody.Trim().ToUpper().Contains("DIED AFTER") ||
            preProcessedBody.Trim().ToUpper().Contains("DIED AS A RESULT") ||
            preProcessedBody.Trim().ToUpper().Contains("DIED AS THE RESULT") ||
            preProcessedBody.Trim().ToUpper().Contains("DIED IN") ||
            preProcessedBody.Trim().ToUpper().Contains("DIED LEAVING BEHIND") ||
            preProcessedBody.Trim().ToUpper().Contains("DIED OF CANCER") ||
            preProcessedBody.Trim().ToUpper().Contains("DIFFERENT SURGICAL OPERATIONS") ||
            preProcessedBody.Trim().ToUpper().Contains("FATHER DIED") ||
            preProcessedBody.Trim().ToUpper().Contains("HUSBAND DIED") ||
            preProcessedBody.Trim().ToUpper().Contains("FATHER DEATH") ||
            preProcessedBody.Trim().ToUpper().Contains("HUSBAND DEATH") ||
            preProcessedBody.Trim().ToUpper().Contains("ILL-HEALTH") ||
            preProcessedBody.Trim().ToUpper().Contains("ILLHEALTH") ||
            preProcessedBody.Trim().ToUpper().Contains("ILL HEALTH") ||
            preProcessedBody.Trim().ToUpper().Contains("IM SICK") ||
            preProcessedBody.Trim().ToUpper().Contains("LONG TIME ILLNESS") ||
            preProcessedBody.Trim().ToUpper().Contains("MONTH TO LIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("MONTHS TO LIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("MOTHER DIED") ||
            preProcessedBody.Trim().ToUpper().Contains("MY CANCER") ||
            preProcessedBody.Trim().ToUpper().Contains("OFFER WITH REGARDS TO MY LATE CLIENT") ||
            preProcessedBody.Trim().ToUpper().Contains("SICK AND DIEING") ||
            preProcessedBody.Trim().ToUpper().Contains("SON DIED") ||
            preProcessedBody.Trim().ToUpper().Contains("SUFFERING FROM CANCER") ||
            preProcessedBody.Trim().ToUpper().Contains("TERMINAL CANCER") ||
            preProcessedBody.Trim().ToUpper().Contains("UNDERGONE SEVERAL SURGICAL") ||
            preProcessedBody.Trim().ToUpper().Contains("WHO DIED A COUPLE") ||
            preProcessedBody.Trim().ToUpper().Contains("WHO DIED A FEW") ||
            preProcessedBody.Trim().ToUpper().Contains("WHO DIED COUPLE") ||
            preProcessedBody.Trim().ToUpper().Contains("WHO DIED SOME") ||
            preProcessedBody.Trim().ToUpper().Contains("WIFE DIED") ||
            preProcessedBody.Trim().ToUpper().Contains("WITH CANCER") ||
            preProcessedBody.Trim().ToUpper().Contains("YEAR TO LIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("YEARS TO LIVE") ||
            ((preProcessedBody.Trim().ToUpper().Contains("DIAGNOSED") || preProcessedBody.Trim().ToUpper().Contains("SUFFERING FROM")) && preProcessedBody.Trim().ToUpper().Contains("CANCER")))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }
        else
        {
            List<string> deathWords = new List<string>() { "DIE", "DEATH", "DYING", "DIEING", "KILL", "MURDER", "SICK" };
            List<string> secodaryWord = new List<string>() { "CANCER", "ILLNESS", "LEAVING BEHIND", "LEAVE BEHIND", "DIAGNO", "TREATMENT", "SURGERY", "SUFFERING", "TERMINAL", "ILLHEALTH", "HOSPITAL" };

            foreach (string s in deathWords)
            {
                foreach (string s2 in secodaryWord)
                {
                    if (preProcessedBody.Trim().ToUpper().Contains(s) && preProcessedBody.Trim().ToUpper().Contains(s2))
                    {
                        base.ParseResponse.IsMatch = true;
                        base.ParseResponse.TotalHits++;

                        break;
                    }
                }
            }
        }

        return base.ParseResponse;
    }
}