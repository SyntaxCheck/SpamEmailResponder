using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

public class TextProcessing
{
    public static string PreProcessEmailText(Settings settings, string subjectLine, string emailBody)
    {
        return RemoveUselessText(MakeEmailEasierToRead(FixMessageTypos(subjectLine + " " + TextProcessing.RemoveReplyTextFromMessage(settings, emailBody).Replace("\r\n", " ").Replace("'", ""))));
    }
    public static string MakeEmailEasierToRead(string message)
    {
        string[] lineSplit = message.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

        for (int i = 0; i < lineSplit.Count(); i++)
        {
            //lineSplit[i] = lineSplit[i].TrimEnd(new char[] { ' ', '\t', '\r', '\n', '\u0009' });
            lineSplit[i] = lineSplit[i].Trim(); //Just remove front/back spacing. If they indent a paragraph we will lose that indenting which is fine since they do not exactly have good grammer or proper sentence structure anyways.
        }

        message = String.Empty;
        for (int i = 0; i < lineSplit.Count(); i++)
        {
            message += lineSplit[i] + Environment.NewLine;
        }

        while (message.Contains(Environment.NewLine + Environment.NewLine + Environment.NewLine))
        {
            message = message.Replace(Environment.NewLine + Environment.NewLine + Environment.NewLine, Environment.NewLine + Environment.NewLine);
        }
        while (message.Contains(">" + Environment.NewLine + ">" + Environment.NewLine))
        {
            message = message.Replace(">" + Environment.NewLine + ">" + Environment.NewLine, ">" + Environment.NewLine);
        }
        while (message.Contains(">>" + Environment.NewLine + ">>" + Environment.NewLine))
        {
            message = message.Replace(">>" + Environment.NewLine + ">>" + Environment.NewLine, ">>" + Environment.NewLine);
        }
        while (message.Contains(">>>" + Environment.NewLine + ">>>" + Environment.NewLine))
        {
            message = message.Replace(">>>" + Environment.NewLine + ">>>" + Environment.NewLine, ">>>" + Environment.NewLine);
        }
        char tab = '\u0009';
        while (message.Contains(tab.ToString() + tab.ToString()))
        {
            message = message.Replace(tab.ToString() + tab.ToString(), tab.ToString());
        }
        while (message.Contains("\t\t"))
        {
            message = message.Replace("\t\t", "\t");
        }
        while (message.Contains("		"))
        {
            message = message.Replace("		", "	");
        }
        while (message.Contains(" "))
        {
            message = message.Replace(" ", " ");
        }
        while (message.Contains("  "))
        {
            message = message.Replace("  ", " ");
        }

        return message;
    }
    public static string RemoveUselessText(string message)
    {
        if (message.ToUpper().Contains("[CID:") && message.Contains("]"))
        {
            int startIndex = message.ToUpper().IndexOf("[CID:");
            int endIndex = message.IndexOf(']', startIndex);
            if (endIndex > 0)
            {
                string removeText = message.Substring(startIndex, (endIndex + 1) - startIndex);

                if (!String.IsNullOrEmpty(removeText))
                {
                    message = message.Replace(removeText, " ");
                }
            }
        }

        while (message.Contains("&gt;"))
        {
            message = message.Replace("&gt;", " ");
        }
        while (message.Contains("&lt;"))
        {
            message = message.Replace("&lt;", " ");
        }
        while (message.Contains("\t"))
        {
            message = message.Replace("\t", " ");
        }
        while (message.Contains("________"))
        {
            message = message.Replace("________", " ");
        }
        while (message.Contains("  "))
        {
            message = message.Replace("  ", " ");
        }
        if (message.Trim().StartsWith("*") && message.Trim().EndsWith("*"))
        {
            if (message.Length > 2)
                message = message.Substring(1, message.Length - 2);
        }

        int pos = message.ToUpper().IndexOf("<https://www.avast.com/sig-email".ToUpper());
        if (pos > 0)
        {
            message = message.Substring(0, pos);
        }

        pos = message.ToUpper().IndexOf(("---" + Environment.NewLine + "This email has been checked for viruses by Avast").ToUpper());
        if (pos > 0)
        {
            message = message.Substring(0, pos);
        }

        pos = message.ToUpper().IndexOf("This email has been checked for viruses by Avast".ToUpper());
        if (pos > 0)
        {
            message = message.Substring(0, pos);
        }

        while (message.EndsWith(" "))
        {
            message = message.Substring(0, message.Length - 1);
        }
        while (message.EndsWith(Environment.NewLine))
        {
            message = message.Substring(0, message.Length - Environment.NewLine.Length);
        }

        return message;
    }
    public static string RemoveReplyTextFromMessage(Settings settings, string text) //Purpose of this function is to strip off the reply text sometimes included. This is the text of the email I sent that they are replying to
    {
        string compare = String.Empty;
        int pos = 0;

        compare = "-----ORIGINAL MESSAGE-----" + Environment.NewLine;
        if (text.ToUpper().Contains(compare))
        {
            pos = text.ToUpper().IndexOf(compare);
            if (pos > 0)
                text = text.Substring(0, pos);
        }

        compare = "________________________________" + Environment.NewLine;
        if (text.ToUpper().Contains(compare))
        {
            pos = text.ToUpper().IndexOf(compare);
            if (pos > 0)
                text = text.Substring(0, pos);
        }

        compare = "--------------------------------------------" + Environment.NewLine;
        if (text.ToUpper().Contains(compare))
        {
            pos = text.ToUpper().IndexOf(compare);
            if (pos > 0)
                text = text.Substring(0, pos);
        }

        compare = "FROM: " + settings.EmailAddress.ToUpper();
        if (text.ToUpper().Contains(compare))
        {
            if (!RemoveReplyTextFromMessageStartingAtDate(ref text, compare))
            {
                pos = text.ToUpper().IndexOf(compare);
                if (pos > 0)
                    text = text.Substring(0, pos);
            }
        }

        compare = "FROM:" + settings.EmailAddress.ToUpper();
        if (text.ToUpper().Contains(compare))
        {
            if (!RemoveReplyTextFromMessageStartingAtDate(ref text, compare))
            {
                pos = text.ToUpper().IndexOf(compare);
                if (pos > 0)
                    text = text.Substring(0, pos);
            }
        }

        compare = "FROM: \"" + settings.EmailAddress.ToUpper();
        if (text.ToUpper().Contains(compare))
        {
            if (!RemoveReplyTextFromMessageStartingAtDate(ref text, compare))
            {
                pos = text.ToUpper().IndexOf(compare);
                if (pos > 0)
                    text = text.Substring(0, pos);
            }
        }

        compare = "FROM:\"" + settings.EmailAddress.ToUpper();
        if (text.ToUpper().Contains(compare))
        {
            if (!RemoveReplyTextFromMessageStartingAtDate(ref text, compare))
            {
                pos = text.ToUpper().IndexOf(compare);
                if (pos > 0)
                    text = text.Substring(0, pos);
            }
        }

        compare = "AM, " + settings.EmailAddress.ToUpper();
        if (text.ToUpper().Contains(compare))
        {
            if (!RemoveReplyTextFromMessageStartingAtDate(ref text, compare))
            {
                pos = text.ToUpper().IndexOf(compare);
                if (pos > 0)
                    text = text.Substring(0, pos);
            }
        }

        compare = "AM," + settings.EmailAddress.ToUpper();
        if (text.ToUpper().Contains(compare))
        {
            if (!RemoveReplyTextFromMessageStartingAtDate(ref text, compare))
            {
                pos = text.ToUpper().IndexOf(compare);
                if (pos > 0)
                    text = text.Substring(0, pos);
            }
        }

        compare = "AM " + settings.EmailAddress.ToUpper() + " <";
        if (text.ToUpper().Contains(compare))
        {
            if (!RemoveReplyTextFromMessageStartingAtDate(ref text, compare))
            {
                pos = text.ToUpper().IndexOf(compare);
                if (pos > 0)
                    text = text.Substring(0, pos);
            }
        }

        compare = "PM, " + settings.EmailAddress.ToUpper();
        if (text.ToUpper().Contains(compare))
        {
            if (!RemoveReplyTextFromMessageStartingAtDate(ref text, compare))
            {
                pos = text.ToUpper().IndexOf(compare);
                if (pos > 0)
                    text = text.Substring(0, pos);
            }
        }

        compare = "PM," + settings.EmailAddress.ToUpper();
        if (text.ToUpper().Contains(compare))
        {
            if (!RemoveReplyTextFromMessageStartingAtDate(ref text, compare))
            {
                pos = text.ToUpper().IndexOf(compare);
                if (pos > 0)
                    text = text.Substring(0, pos);
            }
        }

        compare = "PM " + settings.EmailAddress.ToUpper() + " <";
        if (text.ToUpper().Contains(compare))
        {
            if (!RemoveReplyTextFromMessageStartingAtDate(ref text, compare))
            {
                pos = text.ToUpper().IndexOf(compare);
                if (pos > 0)
                    text = text.Substring(0, pos);
            }
        }

        compare = settings.EmailAddress.ToUpper() + " <" + settings.EmailAddress.ToUpper() + ">";
        if (text.ToUpper().Contains(compare))
        {
            if (!RemoveReplyTextFromMessageStartingAtDate(ref text, compare))
            {
                pos = text.ToUpper().IndexOf(compare);
                if (pos > 0)
                    text = text.Substring(0, pos);
            }
        }

        compare = settings.EmailAddress.ToUpper() + "<" + settings.EmailAddress.ToUpper() + ">";
        if (text.ToUpper().Contains(compare))
        {
            if (!RemoveReplyTextFromMessageStartingAtDate(ref text, compare))
            {
                pos = text.ToUpper().IndexOf(compare);
                if (pos > 0)
                    text = text.Substring(0, pos);
            }
        }

        return text;
    }
    public static bool RemoveReplyTextFromMessageStartingAtDate(ref string text, string compare)
    {
        bool replaceDone = false;

        int pos = text.ToUpper().IndexOf(compare);
        if (pos > 0)
        {
            int lineStart = text.Substring(0, pos).LastIndexOf(Environment.NewLine);
            string dateTimeStr = String.Empty;

            if (lineStart == -1)
            {
                lineStart = 0;
            }
            else
            {
                lineStart += Environment.NewLine.Length;
            }

            if ((lineStart - pos) < 50)
            {
                dateTimeStr = text.Substring(lineStart, pos - lineStart);
            }

            if (!String.IsNullOrEmpty(dateTimeStr))
            {
                if (DateTimeConversion.IsStringSomeTypeOfDateTime(dateTimeStr))
                {
                    text = text.Substring(0, lineStart);
                    replaceDone = true;
                }
            }

            if (!replaceDone)
            {
                if (lineStart > 0) //Attempt to check the next line up for the Sent timestamp. Sometimes the line before is formatted like this: "Sent: Sunday, November 04, 2018 at 12:34 PM"
                {
                    pos = lineStart - Environment.NewLine.Length;
                    lineStart = text.Substring(0, pos).LastIndexOf(Environment.NewLine);

                    if (lineStart == -1)
                    {
                        lineStart = 0;
                    }
                    else
                    {
                        lineStart += Environment.NewLine.Length;
                    }

                    if ((lineStart - pos) < 50)
                    {
                        dateTimeStr = text.Substring(lineStart, pos - lineStart);
                    }

                    if (DateTimeConversion.IsStringSomeTypeOfDateTime(dateTimeStr))
                    {
                        text = text.Substring(0, lineStart);
                        replaceDone = true;
                    }
                }
            }
        }

        return replaceDone;
    } //Attempt to find and remove the date in front on the reply text like so: "On Fri, Feb 8, 2019 at 10:42 PM emailAddress@domain.com <emailAddress@domain.com> wrote:"
    public static string AttemptToFindPersonName(string body)
    {
        string rtn = String.Empty;
        string regards = "Regards;Yours Faithfully;Yours Truely;Best,;Yours in Services;My Best,;My best to you;All best,;All the best;Best wishes;Bests,;Best Regards;Rgds;Warm Regards;Warmest Regards;Warmly,;Take care;Looking forward,;Rushing,;In haste,;Be well,;Peace,;Yours Truly;Very truely yours;Sincerely;Sincerely yours;See you around;With love,;Lots of love,;Warm wishes,;Take care;Remain Blessed;Many thanks,;Thanks,;Your beloved sister;God Bless,;Yours;God bless you;";

        //Get rid of all extra line breaks for the parsing
        while (body.Contains(Environment.NewLine + Environment.NewLine))
        {
            body = body.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
        }

        string[] lineSplit = body.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        string[] regardsSplit = regards.ToUpper().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = lineSplit.Count() - 1; i >= 0; i--)
        {
            lineSplit[i] = lineSplit[i].Trim().Trim('\r').Trim('\n');
            for (int j = 0; j < regardsSplit.Count(); j++)
            {
                if (lineSplit[i].ToUpper() == regardsSplit[j] || (lineSplit[i].ToUpper() + ",") == regardsSplit[j] || lineSplit[i].ToUpper() == (regardsSplit[j] + ",")) //Then name should be on next line
                {
                    if (lineSplit.Count() - 1 > i)
                    {
                        if (lineSplit[i + 1].Length < 30 && lineSplit[i + 1].Count(f => f == ' ') <= 4) //Do not take the entire next sentence if there is a sentence that ends with "Thanks,"
                        {
                            rtn = lineSplit[i + 1];
                            break;
                        }
                    }
                }
                if (regardsSplit[j].EndsWith(","))
                {
                    if (lineSplit[i].ToUpper().StartsWith(regardsSplit[j])) //Name might be following the regards
                    {
                        string[] sentenceSplit = lineSplit[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        string[] tempRegardsSplit = regardsSplit[j].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                        if (sentenceSplit.Count() - tempRegardsSplit.Count() < 5)
                        {
                            rtn = lineSplit[i].Replace(regardsSplit[j], "");
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(rtn))
                break;
        }

        if (rtn.StartsWith(",") || rtn.StartsWith("."))
            rtn = rtn.Substring(1);
        if (rtn.EndsWith(",") || rtn.EndsWith(".") || rtn.EndsWith("!"))
            rtn = rtn.Substring(0, rtn.Length - 1);

        if (rtn.Length > 30 && rtn.Count(f => f == ' ') > 4)
            rtn = String.Empty;

        return rtn;
    }
    public static string AttemptToFindReplyToEmailAddress(Settings settings, string body)
    {
        string replyToEmailAddress = String.Empty;
        string lineKeywordList = "EMAIL;EMAIL ADDRESS;EMAILADDRESS;MAILBOX;MAIL BOX;GMAIL;YAHOO;MSN;OUTLOOK;HOTMAIL;MY EMAIL;MY EMAIL ADDRESS;MAIL;MY MAIL;MY MAIL ADDRESS;CONTACT BANK VIA;MAILTO;EMAIL US;E-MAIL;CONTACT EMAIL;REPLY ME BACK;";

        //Need to do some body cleanup that causes us issues. Sometimes they put their email address like this: EMAIL...SomeEmail@domain.com
        for (int i = 25; i > 1; i--) //Do not remove all "." though since this is valid in an email address
        {
            string dot = ".";
            string replaceStr = String.Empty;

            for (int j = 0; j < i; j++)
            {
                replaceStr += dot;
            }
            body = body.Replace(replaceStr, " ");
        }

        //Sometimes they mistype ">" with "?", we don't need punctuation when parsing so just replace with space
        body = body.Replace("?", " ").Replace("&", " ").Replace("'", " ").Replace("*", " ").Replace("_____", " ").Replace(".COM__", ".COM");

        string[] lineSplit = body.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        string[] lineKeywordSplit = lineKeywordList.ToUpper().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

        char[] trimChars = new char[] { '\r', '\n', '\t' };
        for (int i = lineSplit.Count() - 1; i >= 0; i--)
        {
            lineSplit[i] = lineSplit[i].Trim(trimChars).Trim().ToUpper();

            //First check if there is mailto: text like the following: <mailto:somthing@gmail.com>
            if (lineSplit[i].Contains("MAILTO:"))
            {
                string lineScrubbed = lineSplit[i].Replace("<", " ").Replace(">", " ").Replace(Environment.NewLine, " ").Replace("\t", " ").Replace("\r", " ").Replace("\n", " ").Replace(" ", " ");
                string[] tempLineSplit = lineScrubbed.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in tempLineSplit)
                {
                    if (s.Contains("MAILTO:"))
                    {
                        string[] innerSplit = s.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string innerStr in innerSplit)
                        {
                            if (innerStr.Contains("@"))
                            {
                                if (TextProcessing.IsValidEmail(settings, innerStr))
                                {
                                    replyToEmailAddress = innerStr;
                                    return replyToEmailAddress;
                                }
                            }
                        }
                    }
                }
            }

            for (int j = 0; j < lineKeywordSplit.Count(); j++)
            {
                if (lineSplit[i].Contains(lineKeywordSplit[j].Trim().ToUpper()))
                {
                    //Scrub the line for special characters first
                    lineSplit[i] = lineSplit[i].Replace("{", " ").Replace("}", " ").Replace("]", " ").Replace("]", " ").Replace(",", " ").Replace("<", " ").Replace(">", " ").Replace("^", " ").Replace("(", " ").Replace(")", " ").Replace("\t", " ").Replace("\r", " ").Replace("\n", " ").Replace(" ", " ");

                    //Check the same line for something like "Email: somethin@gmail.com"
                    //Start by checking for ":" as the seperator
                    string[] tempSplit = lineSplit[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int k = 0; k < tempSplit.Count(); k++)
                    {
                        if (tempSplit[k].Contains("@"))
                        {
                            if (TextProcessing.IsValidEmail(settings, tempSplit[k]))
                            {
                                replyToEmailAddress = tempSplit[k];
                                return replyToEmailAddress;
                            }
                        }
                    }
                    tempSplit = lineSplit[i].Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int k = 0; k < tempSplit.Count(); k++)
                    {
                        if (tempSplit[k].Contains("@"))
                        {
                            if (TextProcessing.IsValidEmail(settings, tempSplit[k]))
                            {
                                replyToEmailAddress = tempSplit[k];
                                return replyToEmailAddress;
                            }
                        }
                    }
                    tempSplit = lineSplit[i].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int k = 0; k < tempSplit.Count(); k++)
                    {
                        if (tempSplit[k].Contains("@"))
                        {
                            if (TextProcessing.IsValidEmail(settings, tempSplit[k]))
                            {
                                replyToEmailAddress = tempSplit[k];
                                return replyToEmailAddress;
                            }
                        }
                    }
                    tempSplit = lineSplit[i].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int k = 0; k < tempSplit.Count(); k++)
                    {
                        if (tempSplit[k].Contains("@"))
                        {
                            if (TextProcessing.IsValidEmail(settings, tempSplit[k]))
                            {
                                replyToEmailAddress = tempSplit[k];
                                return replyToEmailAddress;
                            }
                        }
                    }
                    tempSplit = lineSplit[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int k = 0; k < tempSplit.Count(); k++)
                    {
                        if (tempSplit[k].Contains("@"))
                        {
                            if (TextProcessing.IsValidEmail(settings, tempSplit[k]))
                            {
                                replyToEmailAddress = tempSplit[k];
                                return replyToEmailAddress;
                            }
                        }
                    }
                    //Check the next line for the email address
                    if (lineSplit.Count() > i + 1)
                    {
                        //Split the next line on spaces and look for any words that have an @ symbol in it
                        tempSplit = lineSplit[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int k = 0; k < tempSplit.Count(); k++)
                        {
                            if (tempSplit[k].Contains("@"))
                            {
                                if (TextProcessing.IsValidEmail(settings, tempSplit[k]))
                                {
                                    replyToEmailAddress = tempSplit[k];
                                    return replyToEmailAddress;
                                }
                            }
                        }
                    }
                }
            }
        }

        return replyToEmailAddress;
    }
    public static string AttemptToFindPaymentType(string body)
    {
        string paymentRtn = String.Empty;

        if (body.ToUpper().Contains("MONEY GRAM") ||
            body.ToUpper().Contains("MONEYGRAM"))
        {
            paymentRtn = "money gram";
        }
        else if (body.ToUpper().Contains("WESTERN UNION") ||
            body.ToUpper().Contains("WESTERNUNION"))
        {
            paymentRtn = "western union";
        }
        else if (body.ToUpper().Contains("ATM CARD") ||
            body.ToUpper().Contains("ATM BANK CARD") ||
            body.ToUpper().Contains("ATMCARD") ||
            body.ToUpper().Contains("ATM VISA CARD"))
        {
            paymentRtn = "atm card";
        }
        else if (body.ToUpper().Contains("WIRE TRANSFER") ||
            body.ToUpper().Contains("WIRETRANSFER") ||
            body.ToUpper().Contains("BANK TRANSFER") ||
            body.ToUpper().Contains("BANKTRANSFER"))
        {
            paymentRtn = "wire transfer";
        }
        else if (body.ToUpper().Contains("ITUNE"))
        {
            paymentRtn = "iTunes card";
        }

        return paymentRtn;
    }
    public static string AttemptManualParseOfEmailAddress(string email)
    {
        string rtn = email;
        string[] split = email.Split(new char[] { ' ' });

        foreach (string s in split)
        {
            if (s.Contains('@'))
            {
                rtn = s;
                break;
            }
        }

        rtn = rtn.Replace("<", "").Replace(">", "").Replace("(", "").Replace(")", "").Replace("\"", "").Replace("'", "").Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "").Replace("|", "");

        return rtn;
    }
    public static string ScrubText(string text)
    {
        Regex rgx = new Regex("[^a-zA-Z0-9 -]");
        text = rgx.Replace(text, "");

        return text;
    }
    public static string SynonymReplacement(Random rand, string textToReplace)
    {
        //This is a very primative function to replace some common words
        string replaceList = "good,acceptable,excellent,great,marvelous,wonderful,aswesome,fruitful|";
        replaceList += "bad,awful,poor,lousy,crummy,horrible|";
        replaceList += "progress,advance,breakthrough,growth,momentum,movement|";
        replaceList += "trouble,concern,pain,difficulty,problem,unrest|";
        replaceList += "hope,wish,desire|";
        replaceList += "confused,baffled,befuddled,puzzled,perplexed|";
        replaceList += "information,intelligence,knowledge,material|";
        replaceList += "proof,validation,verification|";
        replaceList += "group,club,faction,society,flock|";
        replaceList += "interesting,compelling,engaging,though-provoking|";
        replaceList += "mention,acknowledgment,comment,indication,remark|";
        replaceList += "safe,secure,protected|";
        replaceList += "money,cash,fund,pay,wealth,coin|";
        replaceList += "small,tiny,little,miniature|";
        replaceList += "remember,relive,recall,commemorate|";
        replaceList += "different,contrasting,diverse,various|";
        replaceList += "charm,amuse,entertain,satisfy,tickle|";
        replaceList += "start,begin,kickoff|";
        replaceList += "quickly,hastily,hurriedly,immediately,instantly,promptly,rapidly,swiftly|";
        replaceList += "help,aide,assist|";
        replaceList += "hard,difficult,tough,burdensome,complicated|";
        replaceList += "easy,effortless,painless,simple,straightforward,uncomplicated|";
        replaceList += "willing,eager,prepared,ready|";
        replaceList += "admit,confess,acknowledge|";
        replaceList += "joy,delight,great pleasure,glee,jubilation,happiness,elation|";
        replaceList += "red,blue,green,pink,brown,black,white,yellow,orange,purple|";

        string[] groupSplit = replaceList.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        List<string[]> synonymList = new List<string[]>();

        foreach (string s in groupSplit)
        {
            synonymList.Add(s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
        }

        string[] textToReplaceWords = textToReplace.Split(new char[] { ' ' }, StringSplitOptions.None);

        //Get number of words that could be replaced
        int count = 0;
        List<int> indexToRemove = new List<int>();
        for (int i = 0; i < synonymList.Count(); i++)
        {
            int groupPreCount = count;
            for (int k = 0; k < synonymList[i].Length; k++)
            {
                for (int j = 0; j < textToReplaceWords.Length; j++)
                {
                    if (synonymList[i][k] == textToReplaceWords[j])
                    {
                        count++;
                    }
                }
            }

            if (count == groupPreCount)
            {
                indexToRemove.Add(i);
            }
        }

        if (count > 0)
        {
            //Remove all of the word groups that we found no hits in to help with performance
            if (indexToRemove.Count() > 0)
            {
                for (int i = indexToRemove.Count() - 1; i >= 0; i--)
                {
                    synonymList.RemoveAt(indexToRemove[i]);
                }
            }

            int percentChanceToReplace = 50;
            if (count <= 3)
            {
                percentChanceToReplace = 100;
            }

            for (int i = 0; i < synonymList.Count(); i++)
            {
                int groupPreCount = count;
                for (int k = 0; k < synonymList[i].Length; k++)
                {
                    for (int j = 0; j < textToReplaceWords.Length; j++)
                    {
                        if (synonymList[i][k] == textToReplaceWords[j])
                        {
                            if (rand.Next(0, 100) >= (100 - percentChanceToReplace))
                            {
                                textToReplaceWords[j] = synonymList[i][rand.Next(0, synonymList[i].Count() - 1)];
                            }
                        }
                    }
                }
            }

            textToReplace = String.Empty;
            for (int j = 0; j < textToReplaceWords.Length; j++)
            {
                textToReplace += textToReplaceWords[j] + ' ';
            }
        }

        return textToReplace.Trim();
    }
    public static string TextToHtml(string text)
    {
        text = "<pre>" + HttpUtility.HtmlEncode(text) + "</pre>";
        return text;
    }
    public static bool IsEnglish(string text)
    {
        Regex regex = new Regex(@"[A-Za-z0-9 .,-=+(){}\[\]\\]");
        MatchCollection matches = regex.Matches(text);

        //If over 90% of the characters match the above english characters then assume it is an english message
        if (matches.Count > Math.Round(text.Length * 0.9))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool IsValidEmail(Settings settings, string email)
    {
        try
        {
            email = email.Replace("<", "").Replace(">", "").Replace("\"", " ").Trim();

            if (!email.Contains('@') || !email.Contains('.') || email.Trim().Contains(' ') || email.Trim().Contains('?') ||
                email.Trim().ToUpper() == settings.EmailAddress.Trim().ToUpper() || email.Trim().Contains('/') ||
                email.Trim().Contains('\\') || email.Trim().Contains("...") || email.Trim().Contains('*'))
                return false;

            int atLocation = email.Trim().IndexOf('@');
            int domainComPortion = email.Trim().LastIndexOf('.');
            string name = email.Trim().Substring(0, atLocation);
            string domain = email.Trim().Substring(atLocation + 1);
            string domainCom = email.Trim().Substring(domainComPortion + 1);

            if (name.Trim().Length == 0 || domain.Trim().Length == 0 || domainCom.Trim().Length == 0)
                return false;

            var addr = new System.Net.Mail.MailAddress(email.Trim());
            return addr.Address.Trim().ToUpper() == email.Trim().ToUpper();
        }
        catch
        {
            return false;
        }
    }
    public static string FixMessageTypos(string text)
    {
        string newText = text;

        newText = Regex.Replace(newText, "recieve", "RECEIVE", RegexOptions.IgnoreCase);
        newText = Regex.Replace(newText, "notthe", "NOT THE", RegexOptions.IgnoreCase);
        newText = Regex.Replace(newText, "tome", "TO ME", RegexOptions.IgnoreCase);
        newText = Regex.Replace(newText, "spacial", "SPECIAL", RegexOptions.IgnoreCase);
        newText = Regex.Replace(newText, "hallo", "HELLO", RegexOptions.IgnoreCase);
        newText = Regex.Replace(newText, "wathsapp", "WHATSAPP", RegexOptions.IgnoreCase);
        newText = Regex.Replace(newText, "wathspp", "WHATSAPP", RegexOptions.IgnoreCase);
        newText = Regex.Replace(newText, "massage", "MESSAGE", RegexOptions.IgnoreCase);
        newText = Regex.Replace(newText, "blankmaybe", "BLANK MAYBE", RegexOptions.IgnoreCase);
        newText = Regex.Replace(newText, "wantsto", "WANTS TO", RegexOptions.IgnoreCase);
        newText = Regex.Replace(newText, "provid ", "PROVIDED ", RegexOptions.IgnoreCase);
        newText = Regex.Replace(newText, "sanding", "SENDING", RegexOptions.IgnoreCase);
        newText = Regex.Replace(newText, "yuo", "YOU", RegexOptions.IgnoreCase);
        newText = Regex.Replace(newText, "noward", "ONWARD", RegexOptions.IgnoreCase);

        return newText;
    }
}