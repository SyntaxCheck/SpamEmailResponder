using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Script.Serialization;

public class MailServerFunctions
{
    private const string settingFileLocation = "Settings.json";
    public const string ProcessFolderName = "AutoProcessedMail";
    public const int Timeout = 5000;
    public string UserName;
    public string Password;
    public string MyName;

    public Settings settings;

    public enum EmailType {
        Unknown = 0,
        Test = 1,
        BlankWithAttachment = 2,
        Inheritance = 3,
        Lottery = 4,
        OilAndGas = 5,
        Illuminati = 6,
        ConsignmentBox = 7,
        Beneficiary = 8,
        DeathOrDying = 9,
        LoanOffer = 10,
        MoneyStorage = 11,
        AtmCard = 12,
        Police = 13,
        GenericPayment = 14,
        SellingServices = 15,
        OnlineMarketingConsult = 16,
        BuildTrust = 17,
        Investor = 18,
        MoneyHack = 19,
        JobOffer = 20,
        SellingProducts = 21
    };

    public MailServerFunctions()
    {
        settings = new Settings();
        if (File.Exists(settingFileLocation))
        {
            string json = File.ReadAllText(settingFileLocation);

            settings = new JavaScriptSerializer().Deserialize<Settings>(json);
        }
        else
        {
            //Build the Settings file with some template data
            //TODO Add some documentation on how these fields will be used
            settings.EmailAddress = "PutYourEmailAddressHere@domain.com";
            settings.Password = "PutYouPasswordForTheEmailAddressHere";
            settings.MyName = "YourNameHere";
            settings.Acquaintance = new List<string>() { "Bob", "Steve", "Bill" };
            settings.Products = new List<string>() { "Cars", "Boats", "Lava Lamps", "Blinker Fluid" };
            settings.PaymentMethods = new List<string>() { "Cash", "Wire Transfer", "Bank Transfer", "Personal Check", "Bitcoin", "USD", "Euros", "Rupels" };
            settings.FollowupLine = new List<string>() { "Let me know what you need from me.", "What info do you need from me?", "Tell me exactly what you need from me." };
            settings.Greeting = new List<string>() { "Hello", "Howdy", "Hi" };
            settings.Locations = new List<string>() { "Florida", "Mexico", "Europe", "China" };
            settings.Introduction = new List<string>() { "I do not use the internet much so please be very specific.", "I was born and raised in the midwest. |Environment.NewLine| Herding cattle is what I did day in and day out.", "I have two brothers and a sister, I play professional basket baseball for the Minnesota Wild Twins." };
            settings.IntroductionClosing = new List<string>() { "Maybe this information is useful, maybe it is not", "That was a little it about who I am, maybe you want to share a little about yourself since I shared. I hate being the only one to share, it is always better if we all share. Note: I hate when people use the word 'Share' too often.", "thanks for taking the time to listen to me." };
            settings.IntroductionOpening = new List<string>() { "Before I respond to your email I want to give you a background on who I am.", "Let me tell you about myself before I answer.", "Here is a quick summary of me." };
            settings.Memory = new List<string>() { "I remember back in 2003 when times were simple. I am pretty sure we didn't have internet or eletricity back then. Something about a goose and a chicken but I forget.", "Today these kids have life too easy. I remember back when I went to school we had to walk up hill both ways everyday.", "I remember this one time wen Janice from accounting had a posted note stuck to her back for the entire day. One of the funniest things I have ever seen in my entire life. You know Janice though, always a hoot." };
            settings.Names = new List<string>() { "Margret", "Jane", "Jack" };
            settings.PersonDescriptionMale = new List<string>() { "He was a good person, he wouldn't hurt a fly.", "About 5 foot 8 inches in height. Maybe 170 pounds but it is tough to say. Green eyes but one eye was slightly larger than the other. Brown hair on the right side with red hair on the left side. When it comes to the hair the left side was black and the right side was green. Hair color was a pure white." };
            settings.PersonDescriptionFemale = new List<string>() { "You know her, always laughing. She had a laugh that would light up the entire room.", "About 5 foot 8 inches in height. Maybe 170 pounds but it is tough to say. Green eyes but one eye was slightly larger than the other. Brown hair on the right side with red hair on the left side. When it comes to the hair the left side was black and the right side was green. Hair color was a pure white." };
            settings.Signoff = new List<string>() { "Farewell my friend", "Bye", "Later gator, after awhile crocodile", "Cya", "ByeBye", "Bye now" };

            //Opening responses
            settings.ResponseOpeningAtmCard = new List<string>() { "" };
            settings.ResponseOpeningBlankEmailWithAttachment = new List<string>() { "I am not able to open the |attachmentType| attachment you sent, please send the email without an attachment!" };
            settings.ResponseOpeningConsignmentBox = new List<string>() { "" };
            settings.ResponseOpeningDeathOrDying = new List<string>() { "" };
            settings.ResponseOpeningGenericPayment = new List<string>() { "" };
            settings.ResponseOpeningIlluminati = new List<string>() { "" };
            settings.ResponseOpeningInvestor = new List<string>() { "" };
            settings.ResponseOpeningLoanOffer = new List<string>() { "" };
            settings.ResponseOpeningLottery = new List<string>() { "I am so excited for winning! What do you need from me?", "I need to tell all my friends I won! I need to tell: |GetListOfAcquaintance|" };
            settings.ResponseOpeningMoneyHack = new List<string>() { "" };
            settings.ResponseOpeningMoneyStorage = new List<string>() { "" };
            settings.ResponseOpeningOilAndGas = new List<string>() { "" };
            settings.ResponseOpeningOilAndGasQuestionList = new List<string> { "" };
            settings.ResponseOpeningPolice = new List<string>() { "" };
            settings.ResponseOpeningJobOffer = new List<string>() { "" };
            settings.ResponseOpeningSellingProducts = new List<string>() { "" };

            //Continued responses
            settings.ResponseContinuedAtmCard = new List<string>() { "" };
            settings.ResponseContinuedBeneficiary = new List<string>() { "" };
            settings.ResponseContinuedBlankEmailWithAttachment = new List<string>() { "" };
            settings.ResponseContinuedBuildTrust = new List<string>() { "" };
            settings.ResponseContinuedConsignmentBox = new List<string>() { "" };
            settings.ResponseContinuedDeathOrDying = new List<string>() { "" };
            settings.ResponseContinuedGenericPayment = new List<string>() { "" };
            settings.ResponseContinuedIlluminati = new List<string>() { "" };
            settings.ResponseContinuedInheritance = new List<string>() { "" };
            settings.ResponseContinuedInvestor = new List<string>() { "" };
            settings.ResponseContinuedLoanOffer = new List<string>() { "" };
            settings.ResponseContinuedLottery = new List<string>() { "" };
            settings.ResponseContinuedMoneyHack = new List<string>() { "" };
            settings.ResponseContinuedMoneyStorage = new List<string>() { "" };
            settings.ResponseContinuedOilAndGas = new List<string>() { "" };
            settings.ResponseContinuedPolice = new List<string>() { "" };
            settings.ResponseContinuedJobOffer = new List<string>() { "" };
            settings.ResponseContinuedSellingProducts = new List<string>() { "" };

            string json = new JavaScriptSerializer().Serialize(settings);
            File.WriteAllText(settingFileLocation, JsonHelper.FormatJson(json));
        }

        UserName = settings.EmailAddress;
        Password = settings.Password;
        MyName = settings.MyName;
    }

    //This function will process a single message
    public StandardResponse GetMessages(LoggerInfo loggerInfo, ref List<MailStorage> storage)
    {
        #region References
        //https://github.com/jstedfast/MimeKit/blob/master/component/GettingStarted.md
        //https://github.com/jstedfast/MailKit
        #endregion
        StandardResponse response = new StandardResponse { Code = 1, Message = "", Data = "" };
        List<EmailMessage> messages = new List<EmailMessage>();

        try
        {
            using (var client = new ImapClient())
            {
                NetworkCredential credentials = new NetworkCredential(UserName, Password);

                using (var cancel = new CancellationTokenSource())
                {
                    client.Timeout = Timeout;

                    Logger.WriteDbg(loggerInfo, "Imap Pre-Connect");
                    client.Connect("imap.gmail.com", 993, true, cancel.Token);

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH");

                    Logger.WriteDbg(loggerInfo, "Imap Pre-Authenticate");
                    client.Authenticate(credentials, cancel.Token);

                    //All IMAP servers have an inbox, consume all messages in inbox
                    var inbox = client.Inbox;
                    Logger.WriteDbg(loggerInfo, "Imap Pre-Inbox Open");
                    inbox.Open(FolderAccess.ReadWrite, cancel.Token);

                    int processAmount = 0;

                    //Hard code to process a single message at a time, this is due to some memory issues we received from a series of emails with large attachments
                    //TODO Add threading
                    if (inbox.Count > 0) processAmount = 1;

                    for (int i = 0; i < processAmount; i++)
                    {
                        //Consume Message into EmailMessage Object
                        EmailMessage msg = new EmailMessage(loggerInfo, inbox.GetMessage(i, cancel.Token));
                        messages.Add(msg);
                        Logger.WriteDbg(loggerInfo, "Imap Get Message " + i.ToString() + ". Email subject: " + msg.Subject);
                        msg = null;
                    }

                    //Return if nothing to process
                    if (messages.Count() > 0)
                    {
                        //Pass the message to the handler function to generate a reply
                        response = HandleMessage(messages[0], ref storage);
                        if (response.Code < 0)
                        {
                            return response;
                        }

                        //Post Process if we processed successfully
                        //Create the Process folder if it does not exist
                        IMailFolder personal = client.GetFolder(client.PersonalNamespaces[0]);
                        try
                        {
                            Boolean foundFolder = false;
                            foreach (var folder in personal.GetSubfolders(false))
                            {
                                if (folder.Name == ProcessFolderName)
                                {
                                    foundFolder = true;
                                    break;
                                }
                            }

                            if (!foundFolder)
                            {
                                personal.Create(ProcessFolderName, true, cancel.Token);
                            }
                        }
                        catch (Exception ex)
                        {
                            //Log that we failed to (most likely) create the folder, this is not a hard error as it just causes more processing to be executed during the IMAP polling
                            Logger.Write(loggerInfo, "Failed to read folders, or create read folder.", ex);
                        }

                        //After the mail has been processed then move the mail to the processed folder. This way if we get disconnected the mail would still be in the inbox folder
                        try
                        {
                            inbox.MoveTo(0, personal.GetSubfolder(ProcessFolderName, cancel.Token));
                        }
                        catch (Exception ex2)
                        {
                            response.Code = -1;
                            response.Message = "Exception during IMAP Post Processing";
                            response.Data = "IIS.SendMessage.POST.HandlerDirect(). Exception - " + ex2.Message + Environment.NewLine + ex2.StackTrace;
                        }

                        Logger.WriteDbg(loggerInfo, "Imap Close Inbox and disconnect from the mailbox");

                        //Close with Expunge = True since we are moving emails in this function
                        inbox.Close(true, cancel.Token);
                        client.Disconnect(true, cancel.Token);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            response.Code = -1;
            response.Message = "Exception Getting Messages via IMAP4";
            response.Data = "MailServerFunctions.GetMessages(). Exception - " + ex.Message + Environment.NewLine + ex.InnerException;
        }

        return response;
    }
    public StandardResponse SendSMTP(LoggerInfo loggerInfo, string toAddress, string subject, string bodyText)
    {
        return SendSMTP(loggerInfo, UserName, Password, UserName, UserName, toAddress, toAddress, subject, bodyText, Timeout);
    }
    public StandardResponse SendSMTP(LoggerInfo loggerInfo, string username, string password, string fromAddress, string fromAddressReadable, string toAddress, string toAddressReadable, string subject, string bodyText, int timeout)
    {
        string hostName = "smtp.gmail.com";
        int port = 465;
        StandardResponse response = new StandardResponse() { Code = 0 };
        SmtpClient client = new SmtpClient();
        MimeMessage message = new MimeMessage();

        try
        {
            if (String.IsNullOrEmpty(fromAddressReadable)) fromAddressReadable = fromAddress;
            if (String.IsNullOrEmpty(toAddressReadable)) toAddressReadable = toAddress;

            string[] toSplit = toAddress.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            message.From.Add(new MailboxAddress(fromAddress, fromAddress));
            foreach (string s in toSplit)
            {
                string friendly = String.Empty, realAddress = String.Empty;

                string[] tmpSplit = s.Split(new string[] { "<" }, StringSplitOptions.RemoveEmptyEntries);
                if (tmpSplit.Count() > 0)
                {
                    if (tmpSplit.Count() > 1)
                    {
                        friendly = tmpSplit[0];
                        realAddress = tmpSplit[1];

                        friendly = friendly.Replace("\"", "").Trim();
                        realAddress = realAddress.Replace("<", "").Replace(">", "").Trim();
                    }
                    else
                    {
                        friendly = s;
                        realAddress = s;
                    }
                }
                else
                {
                    friendly = s;
                    realAddress = s;
                }

                message.To.Add(new MailboxAddress(friendly, realAddress));
            }

            if (!subject.StartsWith("RE:"))
                subject = "RE: " + subject;

            message.Subject = subject;

            if (!String.IsNullOrEmpty(bodyText))
            {
                message.Body = new TextPart("plain")
                {
                    Text = bodyText
                };
            }

            Logger.WriteDbg(loggerInfo, "Message Built");

            if (timeout > 0)
            {
                client.Timeout = timeout;
            }
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            client.Connect(hostName, port, true);
            Logger.WriteDbg(loggerInfo, "Connected to Host");

            // Note: since we don't have an OAuth2 token, disable
            // the XOAUTH2 authentication mechanism.
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            client.Authenticate(username, password);
            Logger.WriteDbg(loggerInfo, "Authenticated User");
            client.Send(message);
            Logger.WriteDbg(loggerInfo, "Sent Message");
            client.Disconnect(true);
            Logger.WriteDbg(loggerInfo, "Disconnect");
        }
        catch (Exception ex)
        {
            response.Code = -1;
            response.Message = "Failed to Send SMTP Message";
            response.Data = ex.Message;
            Logger.Write(loggerInfo, "Failed to Send SMTP Message: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace);
        }

        return response;
    }
    public StandardResponse HandleMessage(EmailMessage msg, ref List<MailStorage> storage)
    {
        StandardResponse response = new StandardResponse { Code = 0, Message = String.Empty, Data = String.Empty };

        try
        {
            //Ignore No-Reply emails, no reason to process an email that is not monitored or that was a real email sent to our mailbox.
            if (msg.FromAddress.Count() > 0 && !msg.FromAddress[0].ToString().ToUpper().Contains("NO-REPLY@") && !msg.FromAddress[0].ToString().ToUpper().Contains("NOREPLY@") && !msg.FromAddress[0].ToString().ToUpper().Contains("MAILER-DAEMON@") && !msg.FromAddress[0].ToString().ToUpper().Contains("POSTMASTER@"))
            {
                MailStorage storageObj = new MailStorage();

                storageObj.MsgId = msg.MessageId;
                storageObj.DateReceived = msg.MessageDttm;
                storageObj.NumberOfAttachments = msg.FileAttachments.Count();
                storageObj.SubjectLine = msg.Subject.Replace("RE: ","").Replace("FW: ","");

                //Extract the Body information
                foreach (TextPart txtPart in msg.TextParts)
                {
                    string msgType = txtPart.ContentType.MediaType + "/" + txtPart.ContentType.MediaSubtype;
                    if (msgType.Trim().ToLower() == "text/plain")
                    {
                        storageObj.EmailBodyPlain = txtPart.Text;
                    }
                    else if (msgType.Trim().ToLower() == "text/html")
                    {
                        storageObj.EmailBodyHtml = txtPart.Text;
                    }
                    else if (msgType.Trim().ToLower() == "text/rtf")
                    {
                        storageObj.EmailBodyRich = txtPart.Text;
                    }
                }

                //If no Plain text was included convert the HTML to plain text
                if (String.IsNullOrEmpty(storageObj.EmailBodyPlain) && String.IsNullOrEmpty(storageObj.EmailBodyPlain))
                {
                    HtmlConvert convert = new HtmlConvert();
                    storageObj.EmailBodyPlain = convert.ConvertHtml(storageObj.EmailBodyHtml);
                }

                string name = AttemptToFindPersonName(storageObj.EmailBodyPlain);
                if (!String.IsNullOrEmpty(name))
                    storageObj.PersonName = name;

                foreach (var v in msg.FromAddress)
                {
                    if(msg.FromAddress.Count() > 1 && !v.ToString().Trim().ToLower().EndsWith(".ocn.ne.jp")) //Often times they include multiple email addresses, the ocn.ne.jp ones tend to get rejected so exclude that email in the reply if it is not the only address
                        storageObj.ToAddress += v.ToString() + ";";
                }
                foreach (var v in msg.ReplyTo)
                {
                    storageObj.ToAddress += v.ToString() + ";";
                }
                foreach(var v in msg.FileAttachments)
                {
                    storageObj.AtachmentTypes += v.FileExtension + ",";
                }

                string newEmailAddress = AttemptToFindReplyToEmailAddress(storageObj.EmailBodyPlain).Trim();
                if (!String.IsNullOrEmpty(newEmailAddress))
                {
                    if (!storageObj.ToAddress.Trim().ToUpper().Contains(newEmailAddress.ToUpper()) && !settings.EmailAddress.ToUpper().Contains(newEmailAddress.ToUpper()))
                    {
                        storageObj.ToAddress += newEmailAddress + ";";
                    }
                }

                //Get list of previous messages in the thread
                List<MailStorage> previousMessagesInThread = new List<MailStorage>();
                foreach (MailStorage ms in storage)
                {
                    if (ms.SubjectLine == storageObj.SubjectLine)
                    {
                        int foundCount = 0;
                        foreach (var v in msg.FromAddress)
                        {
                            if (ms.ToAddress.Contains(v.ToString()))
                            {
                                foundCount++;
                                break;
                            }
                        }
                        foreach (var v in msg.ReplyTo)
                        {
                            if (ms.ToAddress.Contains(v.ToString()))
                            {
                                foundCount++;
                                break;
                            }
                        }

                        if (foundCount > 0)
                        {
                            previousMessagesInThread.Add(ms);
                        }
                    }
                }

                //Determine response
                storageObj.DeterminedReply = GetResponseForType(ref storageObj, previousMessagesInThread.OrderBy(t => t.DateReceived).ToList());

                storage.Add(storageObj);
            }
        }
        catch (Exception ex)
        {
            response.Code = -1;
            response.Message = "Failed to HandleMessage. Exception: " + ex.Message;
            response.Exception = ex;
        }

        return response;
    }

    //Helper functions
    private string SettingPostProcessing(string text, List<string> placeholder, List<string> replacement, Random rand)
    {
        if (placeholder.Count() != replacement.Count())
            throw new Exception("Setting Post Process counts do not match");

        placeholder.Add("|introduction|");
        replacement.Add(GetRandomInroduction(rand));
        placeholder.Add("|Environment.NewLine|");
        replacement.Add(Environment.NewLine);
        placeholder.Add("|GetRandomAcquaintance|");
        replacement.Add(GetRandomAcquaintance(rand));
        placeholder.Add("|GetRandomFollowupLine|");
        replacement.Add(GetRandomFollowupLine(rand));
        placeholder.Add("|GetRandomLocation|");
        replacement.Add(GetRandomLocation(rand));
        placeholder.Add("|GetRandomProduct|");
        replacement.Add(GetRandomProduct(rand));
        placeholder.Add("|GetRandomPaymentMethod|");
        replacement.Add(GetRandomPaymentMethod(rand));

        for (int i = 0; i < placeholder.Count(); i++)
        {
            text = text.Replace(placeholder[i], replacement[i]);
        }

        return text;
    }
    private string AttemptToFindPersonName(string body)
    {
        string rtn = String.Empty;
        string regards = "Regards;Yours Faithfully;Yours Truely;Best,;Yours in Services;My Best,;My best to you;All best,;All the best;Best wishes;Bests,;Best Regards;Rgds;Warm Regards;Warmest Regards;Warmly,;Take care;Looking forward,;Rushing,;In haste,;Be well,;Peace,;Yours Truly;Very truely yours;Sincerely;Sincerely yours;See you around;With love,;Lots of love,;Warm wishes,;Take care;Remain Blessed;Many thanks,;Thanks,;Your beloved sister;";

        string[] lineSplit = body.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        string[] regardsSplit = regards.ToUpper().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = lineSplit.Count() - 1; i >= 0; i--)
        {
            lineSplit[i] = lineSplit[i].Trim();
            for (int j = 0; j < regardsSplit.Count(); j++)
            {
                if (lineSplit[i].ToUpper() == regardsSplit[j] || lineSplit[i].ToUpper() + "," == regardsSplit[j]) //Then name should be on next line
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
                        string[] tempRegardsSplit = regardsSplit[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                        if (sentenceSplit.Count() - tempRegardsSplit.Count() < 5)
                        {
                            rtn = lineSplit[i].Replace(regardsSplit[i],"");
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
    private string AttemptToFindReplyToEmailAddress(string body)
    {
        string replyToEmailAddress = String.Empty;
        string lineKeywordList = "EMAIL;EMAIL ADDRESS;EMAILADDRESS;MAILBOX;MAIL BOX;GMAIL;YAHOO;MSN;OUTLOOK;HOTMAIL;MY EMAIL;MY EMAIL ADDRESS;MAIL;MY MAIL;MY MAIL ADDRESS;";

        string[] lineSplit = body.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        string[] lineKeywordSplit = lineKeywordList.ToUpper().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = lineSplit.Count() - 1; i >= 0; i--)
        {
            lineSplit[i] = lineSplit[i].Trim().ToUpper();
            
            for (int j = 0; j < lineKeywordSplit.Count(); j++)
            {
                if (lineSplit[i].StartsWith(lineKeywordSplit[j].Trim().ToUpper()))
                {
                    //Check the same line for something like "Email: somethin@gmail.com"
                    //Start by checking for ":" as the seperator
                    string[] tempSplit = lineSplit[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int k = 0; k < tempSplit.Count(); k++)
                    {
                        if (tempSplit[k].Contains("@"))
                        {
                            if (IsValidEmail(tempSplit[k]))
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
                            if (IsValidEmail(tempSplit[k]))
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
                            if (IsValidEmail(tempSplit[k]))
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
                            if (IsValidEmail(tempSplit[k]))
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
                                if (IsValidEmail(tempSplit[k]))
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
    public bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email.Trim());
            return addr.Address.Trim().ToUpper() == email.Trim().ToUpper();
        }
        catch
        {
            return false;
        }
    }
    public string GetResponseForType(ref MailStorage currentMessage, List<MailStorage> pastMessages)
    {
        Random rand = new Random();
        EmailType type = EmailType.Unknown;
        string rtnResponse = String.Empty;
        string attachmentType = "File";
        string name = String.Empty;
        string greeting = String.Empty;
        string signOff = String.Empty;
        string preProcessedBody = currentMessage.SubjectLine + " " + currentMessage.EmailBodyPlain.Replace("\r\n"," ");

        //Types of emails
        if (currentMessage.SubjectLine.Contains("Test ") || currentMessage.SubjectLine.Contains(" Test"))
        {
            type = EmailType.Test;
        }
        else if ((preProcessedBody.Trim() == String.Empty || (preProcessedBody.Length < 40 && (preProcessedBody.ToUpper().Contains("ATTACHMENT") || preProcessedBody.ToUpper().Contains("FILE")))) && currentMessage.NumberOfAttachments > 0)
        {
            type = EmailType.BlankWithAttachment;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("ILLUMINATI") || preProcessedBody.Trim().ToUpper().Contains("ILUMINATI"))
        {
            type = EmailType.Illuminati;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("DIED AFTER") || preProcessedBody.Trim().ToUpper().Contains("CANCER DIAG") || preProcessedBody.Trim().ToUpper().Contains("WITH CANCER") || preProcessedBody.Trim().ToUpper().Contains("MY CANCER") || preProcessedBody.Trim().ToUpper().Contains("HUSBAND DIED") || preProcessedBody.Trim().ToUpper().Contains("WIFE DIED") || preProcessedBody.Trim().ToUpper().Contains("CHILD DIED") || preProcessedBody.Trim().ToUpper().Contains("SON DIED") || preProcessedBody.Trim().ToUpper().Contains("DAUGHTER DIED"))
        {
            type = EmailType.DeathOrDying;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("OIL AND GAS") || preProcessedBody.Trim().ToUpper().Contains("GAS AND OIL"))
        {
            type = EmailType.OilAndGas;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("KEEP MY MONEY") || preProcessedBody.Trim().ToUpper().Contains("ABANDONED SUM") || preProcessedBody.Trim().ToUpper().Contains("MOVE THE SUM") || preProcessedBody.Trim().ToUpper().Contains("STORE MY MONEY") || preProcessedBody.Trim().ToUpper().Contains("KEEP THE MONEY SAFE") || preProcessedBody.Trim().ToUpper().Contains("KEEP THE MONEY SAVE") || preProcessedBody.Trim().ToUpper().Contains("RECEIVE THE FUND AND KEEP IT") || preProcessedBody.Trim().ToUpper().Contains("RECEIVE THE MONEY"))
        {
            type = EmailType.MoneyStorage;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("JOB OFFER") || preProcessedBody.Trim().ToUpper().Contains("POSITION IN COMPANY") || preProcessedBody.Trim().ToUpper().Contains("POSITION IN OUR COMPANY"))
        {
            type = EmailType.JobOffer;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("WEB DESIGN") || preProcessedBody.Trim().ToUpper().Contains("DEVELOPMENT FIRM"))
        {
            type = EmailType.SellingServices;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("ONLINE MARKETING CONSULT"))
        {
            type = EmailType.OnlineMarketingConsult;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("FINANCIAL ASSISTANCE") || preProcessedBody.Trim().ToUpper().Contains("FINANCIAL HELP") || preProcessedBody.Trim().ToUpper().Contains("LOAN") || preProcessedBody.Trim().ToUpper().Contains("APPLY FOR CASH"))
        {
            type = EmailType.LoanOffer;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("LOTTERY") || preProcessedBody.Trim().ToUpper().Contains("POWER BALL") || preProcessedBody.Trim().ToUpper().Contains("POWERBALL") || preProcessedBody.Trim().ToUpper().Contains("WINNER"))
        {
            type = EmailType.Lottery;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("CONSIGNMENT") || preProcessedBody.Trim().ToUpper().Contains("TRUNK BOX"))
        {
            type = EmailType.ConsignmentBox;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("POLICE"))
        {
            type = EmailType.Police;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("ATM BLANK CARD"))
        {
            type = EmailType.MoneyHack;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("ATM CARD") || preProcessedBody.Trim().ToUpper().Contains("ATMCARD") || preProcessedBody.Trim().ToUpper().Contains("ATM CREDIT CARD") || preProcessedBody.Trim().ToUpper().Contains("BANK CHEQUE"))
        {
            type = EmailType.AtmCard;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("INHERITENCE") || preProcessedBody.Trim().ToUpper().Contains("INHERIT"))
        {
            type = EmailType.Inheritance;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("BENEFICIARY") || preProcessedBody.Trim().ToUpper().Contains("NEXT OF KIN") || preProcessedBody.Trim().ToUpper().Contains("DONATING") || preProcessedBody.Trim().ToUpper().Contains("DONATION") || preProcessedBody.Trim().ToUpper().Contains("DON ATION") || preProcessedBody.Trim().ToUpper().Contains("TRANSFER TO YOUR ACCOUNT") || preProcessedBody.Trim().ToUpper().Contains("TO YOUR BANK ACCOUNT"))
        {
            type = EmailType.Beneficiary;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("INVESTOR") || preProcessedBody.Trim().ToUpper().Contains("PROFIT SHARING") || preProcessedBody.Trim().ToUpper().Contains("INVESTMENT"))
        {
            type = EmailType.Investor;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("PAYMENT") || preProcessedBody.Trim().ToUpper().Contains("MONEYGRAM") || preProcessedBody.Trim().ToUpper().Contains("WESTERN UNION") || preProcessedBody.Trim().ToUpper().Contains("TRANSFER THE FUND"))
        {
            type = EmailType.GenericPayment;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("LIKE TO KNOW YOU MORE") || preProcessedBody.Trim().ToUpper().Contains("GET TO KNOW YOU") || preProcessedBody.Trim().ToUpper().Contains("BUILD TRUST") || preProcessedBody.Trim().ToUpper().Contains("I SEE YOU AS SOMEONE I CAN WORK WITH") || preProcessedBody.Trim().ToUpper().Contains("I WILL TELL YOU MORE ABOUT MYSELF") || preProcessedBody.Trim().ToUpper().Contains("GET TO KNOW EACHOTHER") || preProcessedBody.Trim().ToUpper().Contains("GET TO KNOW EACH OTHER") || preProcessedBody.Trim().ToUpper().Contains("LONGTERM RELATIONSHIP") || preProcessedBody.Trim().ToUpper().Contains("LONG TERM RELATIONSHIP"))
        {
            type = EmailType.BuildTrust;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("MANUFACTURER OF LED"))
        {
            type = EmailType.SellingProducts;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("BOX")) //If no other hits then just look for the word BOX
        {
            type = EmailType.ConsignmentBox;
        }

        currentMessage.MessageType = (int)type;

        //Determine attachment type
        if (currentMessage.AtachmentTypes.ToUpper().Contains("PDF"))
        {
            attachmentType = "PDF";
        }
        else if (currentMessage.AtachmentTypes.ToUpper().Contains("DOC") || currentMessage.AtachmentTypes.ToUpper().Contains("RTF"))
        {
            attachmentType = "Word";
        }
        else if (currentMessage.AtachmentTypes.ToUpper().Contains("PNG") || currentMessage.AtachmentTypes.ToUpper().Contains("JPG") || currentMessage.AtachmentTypes.ToUpper().Contains("JPEG") || currentMessage.AtachmentTypes.ToUpper().Contains("BMP") || currentMessage.AtachmentTypes.ToUpper().Contains("GIF") || currentMessage.AtachmentTypes.ToUpper().Contains("TIFF"))
        {
            attachmentType = "Image";
        }
        if (String.IsNullOrEmpty(currentMessage.PersonName))
        {
            for (int i = pastMessages.Count() - 1; i > 0; i--)
            {
                if (!String.IsNullOrEmpty(pastMessages[i].PersonName))
                {
                    name = pastMessages[i].PersonName;
                    break;
                }
            }
        }
        else
        {
            name = currentMessage.PersonName;
        }
        if (String.IsNullOrEmpty(name))
        {
            name = GetRandomName(rand);
        }

        currentMessage.PersonName = name;
        greeting = GetRandomGreeting(rand);
        signOff = GetRandomSignOff(rand);

        switch (type)
        {
            case EmailType.Test:
                if(pastMessages.Count() > 0)
                    rtnResponse = GetRandomOpeningResponseTest(rand);
                else
                    rtnResponse = GetRandomContinuedResponseTest(rand);
                break;
            case EmailType.BlankWithAttachment:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomOpeningResponseForBlankEmailWithAttachment(rand, greeting, attachmentType);
                else
                    rtnResponse = GetRandomContinuedResponseForBlankEmailWithAttachment(rand, greeting, attachmentType);
                break;
            case EmailType.Inheritance:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForInheritance(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForInheritance(rand, greeting, name, attachmentType, currentMessage, pastMessages);
                break;
            case EmailType.Beneficiary:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForBeneficiary(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForBeneficiary(rand, greeting, name);
                break;
            case EmailType.Lottery:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForLottery(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForLottery(rand, greeting);
                break;
            case EmailType.OilAndGas:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForOilAndGas(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForOilAndGas(rand, greeting, name, currentMessage);
                break;
            case EmailType.Illuminati:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForIlluminati(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForIlluminati(rand, greeting, name, currentMessage);
                break;
            case EmailType.ConsignmentBox:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForConsignmentBox(rand, greeting, name, attachmentType);
                else
                    rtnResponse = GetRandomOpeningResponseForConsignmentBox(rand, greeting, name, attachmentType);
                break;
            case EmailType.DeathOrDying:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForDeathOrDying(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForDeathOrDying(rand, greeting, name);
                break;
            case EmailType.LoanOffer:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForLoanOffer(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForLoanOffer(rand, greeting, name);
                break;
            case EmailType.MoneyStorage:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForMoneyStorage(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForMoneyStorage(rand, greeting, name);
                break;
            case EmailType.AtmCard:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForAtmCard(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForAtmCard(rand, greeting, name);
                break;
            case EmailType.Police:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForPolice(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForPolice(rand, greeting, name);
                break;
            case EmailType.GenericPayment:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForGenericPayment(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForGenericPayment(rand, greeting, name);
                break;
            case EmailType.SellingServices:
                rtnResponse = GetRandomResponseForSellingServices(rand, greeting, name);
                break;
            case EmailType.OnlineMarketingConsult:
                rtnResponse = "No";
                break;
            case EmailType.BuildTrust:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForBuildTrust(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForBuildTrust(rand, greeting, name);
                break;
            case EmailType.Investor:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForInvestor(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForInvestor(rand, greeting, name);
                break;
            case EmailType.MoneyHack:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForMoneyHack(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForMoneyHack(rand, greeting, name);
                break;
            case EmailType.JobOffer:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForJobOffer(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForJobOffer(rand, greeting, name);
                break;
        }

        if(!String.IsNullOrEmpty(rtnResponse))
            rtnResponse += Environment.NewLine + Environment.NewLine + signOff + ", " + MyName;

        return rtnResponse;
    }
    //Opening Responses
    private string GetRandomOpeningResponseTest(Random rand)
    {
        List<string> lst = new List<string>
        {
            "Your Test message has been received.",
            "Thank you for the Test message.",
            "The Test email worked successfully."
        };

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomOpeningResponseForBeneficiary(Random rand, string greetings, string name)
    {
        string response = String.Empty;
        string introduction = String.Empty;
        string followup = String.Empty;
        bool isMale = true;

        response += greetings + " " + name + ", ";

        //Opening introduction of myself
        introduction = GetRandomInroduction(rand);
        //Closing Followup line
        followup = GetRandomFollowupLine(rand);

        response += introduction + Environment.NewLine + Environment.NewLine + followup;

        return response;
    }
    private string GetRandomOpeningResponseForBlankEmailWithAttachment(Random rand, string greetings, string attachmentType)
    {
        return greetings + ". " + SettingPostProcessing(settings.ResponseOpeningBlankEmailWithAttachment[rand.Next(0, settings.ResponseOpeningBlankEmailWithAttachment.Count())], new List<string> { "|attachmentType|" }, new List<string> { attachmentType }, rand);
    }
    private string GetRandomOpeningResponseForLottery(Random rand, string greetings)
    {
        return greetings + ". " + SettingPostProcessing(settings.ResponseOpeningLottery[rand.Next(0, settings.ResponseOpeningLottery.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand,5) }, rand);
    }
    private string GetRandomOpeningResponseForOilAndGas(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string rtn = String.Empty;

        rtn = greetings + ". " + SettingPostProcessing(settings.ResponseOpeningOilAndGas[rand.Next(0, settings.ResponseOpeningOilAndGas.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 5) }, rand);

        if (currentMessage.EmailBodyPlain.ToUpper().Contains("LOW PRICE") || currentMessage.EmailBodyPlain.ToUpper().Contains("LOW RATE") || currentMessage.EmailBodyPlain.ToUpper().Contains("CHEAP") || currentMessage.EmailBodyPlain.ToUpper().Contains("DISCOUNT") || currentMessage.EmailBodyPlain.ToUpper().Contains("A GOOD PRICE"))
        {
            rtn += Environment.NewLine + Environment.NewLine + "You mention that I would get the product at a good rate so I have a few questions:" + Environment.NewLine;
            rtn += GetRandomListOfOilAndGasQuestions(rand, 2);
            rtn += "These are the initial questions I have.";
        }

        return rtn;
    }
    private string GetRandomOpeningResponseForIlluminati(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseOpeningIlluminati[rand.Next(0, settings.ResponseOpeningIlluminati.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForConsignmentBox(Random rand, string greetings, string name, string attachmentType)
    {
        string attachmentIncludedText = String.Empty;

        if (attachmentType == "Image")
        {
            attachmentIncludedText = " I have noticed that an image file was attached, I am unable to view the image file from my current device but I must trust that the file is a picture of my consignment box. I will review the phota later when I have image viewing capabilities. If you could also send a few other angles of the box that would be appreciated. I have received fake pictures in the past so hopefully you understand my request.";
        }
        else
        {
            attachmentIncludedText = " I have noticed that no image file was included. Could you send me a picture of my consignment package so that I can trust you do in fact have it? I do not know what my package is supposed to look like but just to verify you have it.";
        }

        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseOpeningConsignmentBox[rand.Next(0, settings.ResponseOpeningConsignmentBox.Count())], new List<string> { "|attachmentIncludedText|" }, new List<string> { attachmentIncludedText }, rand);
    }
    private string GetRandomOpeningResponseForDeathOrDying(Random rand, string greetings, string name)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseOpeningDeathOrDying[rand.Next(0, settings.ResponseOpeningDeathOrDying.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForLoanOffer(Random rand, string greetings, string name)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseOpeningLoanOffer[rand.Next(0, settings.ResponseOpeningLoanOffer.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForMoneyStorage(Random rand, string greetings, string name)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseOpeningMoneyStorage[rand.Next(0, settings.ResponseOpeningMoneyStorage.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForAtmCard(Random rand, string greetings, string name)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseOpeningAtmCard[rand.Next(0, settings.ResponseOpeningAtmCard.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForPolice(Random rand, string greetings, string name)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseOpeningPolice[rand.Next(0, settings.ResponseOpeningPolice.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForGenericPayment(Random rand, string greetings, string name)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseOpeningGenericPayment[rand.Next(0, settings.ResponseOpeningGenericPayment.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForInvestor(Random rand, string greetings, string name)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseOpeningInvestor[rand.Next(0, settings.ResponseOpeningInvestor.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForMoneyHack(Random rand, string greetings, string name)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseOpeningMoneyHack[rand.Next(0, settings.ResponseOpeningMoneyHack.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForInheritance(Random rand, string greetings, string name, string attamentType, MailStorage currentMessage, List<MailStorage> pastMessages)
    {
        string response = String.Empty;
        string introduction = String.Empty;
        string inheritorDescription = String.Empty;
        string memories = String.Empty;
        string followup = String.Empty;
        bool isMale = true;

        response += greetings + " " + name + ", ";

        //Opening introduction of myself
        introduction = GetRandomInroduction(rand);
        //Description of interitor
        inheritorDescription = GetRandomPersonDescription(rand, ref isMale);
        //Share memories
        memories = GetRandomMemory(rand, isMale);
        //Closing Followup line
        followup = GetRandomFollowupLine(rand);

        response += introduction + Environment.NewLine + Environment.NewLine + inheritorDescription + Environment.NewLine + Environment.NewLine + memories + Environment.NewLine + Environment.NewLine + followup;

        return response;
    }
    private string GetRandomResponseForSellingServices(Random rand, string greetings, string name)
    {
        List<string> lst = new List<string>
        {
            "UNSUBSCRIBE",
            "Please remove me from mailing list"
        };

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomOpeningResponseForBuildTrust(Random rand, string greetings, string name)
    {
        string introduction = GetRandomInroduction(rand);
        List<string> lst = new List<string>
        {
            greetings + " friend, " + introduction,
            greetings + " my friend, " + introduction
        };

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomOpeningResponseForJobOffer(Random rand, string greetings, string name)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseOpeningJobOffer[rand.Next(0, settings.ResponseOpeningJobOffer.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForSellingProducts(Random rand, string greetings, string name)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseOpeningSellingProducts[rand.Next(0, settings.ResponseOpeningSellingProducts.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    //Continued Responses
    private string GetRandomContinuedResponseTest(Random rand)
    {
        List<string> lst = new List<string>
        {
            "Your continued Test message has been received.",
            "Thank you for the continued Test message.",
            "The Test email continues to work successfully."
        };

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomContinuedResponseForBlankEmailWithAttachment(Random rand, string greetings, string attachmentType)
    {
        return greetings + ". " + SettingPostProcessing(settings.ResponseContinuedBlankEmailWithAttachment[rand.Next(0, settings.ResponseContinuedBlankEmailWithAttachment.Count())], new List<string> { "|attachmentType|" }, new List<string> { attachmentType }, rand);
    }
    private string GetRandomContinuedResponseForLottery(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseContinuedLottery[rand.Next(0, settings.ResponseContinuedLottery.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomContinuedResponseForOilAndGas(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string rtn = String.Empty;

        rtn = greetings + ". " + SettingPostProcessing(settings.ResponseContinuedOilAndGas[rand.Next(0, settings.ResponseContinuedOilAndGas.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 5) }, rand);

        if (currentMessage.EmailBodyPlain.ToUpper().Contains("LOW PRICE") || currentMessage.EmailBodyPlain.ToUpper().Contains("LOW RATE") || currentMessage.EmailBodyPlain.ToUpper().Contains("CHEAP") || currentMessage.EmailBodyPlain.ToUpper().Contains("DISCOUNT") || currentMessage.EmailBodyPlain.ToUpper().Contains("A GOOD PRICE"))
        {
            rtn += Environment.NewLine + Environment.NewLine + "I have a few more questions:" + Environment.NewLine;
            rtn += GetRandomListOfOilAndGasQuestions(rand, 2);
            rtn += "These are the additional questions I have.";
        }

        return rtn;
    }
    private string GetRandomContinuedResponseForIlluminati(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseContinuedIlluminati[rand.Next(0, settings.ResponseContinuedIlluminati.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomContinuedResponseForConsignmentBox(Random rand, string greetings, string name, string attachmentType)
    {
        string attachmentIncludedText = String.Empty;

        if (attachmentType == "Image")
        {
            attachmentIncludedText = " I have noticed that an image file was attached, I am unable to view the image file from my current device but I must trust that the file is a picture of my consignment box. I will review the phota later when I have image viewing capabilities. If you could also send a few other angles of the box that would be appreciated. I have received fake pictures in the past so hopefully you understand my request.";
        }
        else
        {
            attachmentIncludedText = " I have noticed that no image file was included. Could you send me a picture of my consignment package so that I can trust you do in fact have it? I do not know what my package is supposed to look like but just to verify you have it.";
        }

        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseContinuedConsignmentBox[rand.Next(0, settings.ResponseContinuedConsignmentBox.Count())], new List<string> { "|attachmentIncludedText|" }, new List<string> { attachmentIncludedText }, rand);
    }
    private string GetRandomContinuedResponseForDeathOrDying(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseContinuedDeathOrDying[rand.Next(0, settings.ResponseContinuedDeathOrDying.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomContinuedResponseForLoanOffer(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseContinuedLoanOffer[rand.Next(0, settings.ResponseContinuedLoanOffer.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomContinuedResponseForMoneyStorage(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseContinuedMoneyStorage[rand.Next(0, settings.ResponseContinuedMoneyStorage.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForAtmCard(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseContinuedAtmCard[rand.Next(0, settings.ResponseContinuedAtmCard.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForPolice(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseContinuedPolice[rand.Next(0, settings.ResponseContinuedPolice.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForGenericPayment(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseOpeningGenericPayment[rand.Next(0, settings.ResponseOpeningGenericPayment.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForInvestor(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseOpeningInvestor[rand.Next(0, settings.ResponseOpeningInvestor.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForMoneyHack(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseContinuedMoneyHack[rand.Next(0, settings.ResponseContinuedMoneyHack.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForInheritance(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseContinuedInheritance[rand.Next(0, settings.ResponseContinuedInheritance.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForBeneficiary(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseContinuedBeneficiary[rand.Next(0, settings.ResponseContinuedBeneficiary.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForBuildTrust(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseContinuedBuildTrust[rand.Next(0, settings.ResponseContinuedBuildTrust.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForJobOffer(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseContinuedJobOffer[rand.Next(0, settings.ResponseContinuedJobOffer.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForSellingProducts(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        return greetings + " " + name + ". " + SettingPostProcessing(settings.ResponseContinuedSellingProducts[rand.Next(0, settings.ResponseContinuedSellingProducts.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }

    //Supporting Random lists
    private string GetRandomName(Random rand)
    {
        List<string> lst = settings.Names;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomGreeting(Random rand)
    {
        List<string> lst = settings.Greeting;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomSignOff(Random rand)
    {
        List<string> lst = settings.Signoff;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomInroduction(Random rand)
    {
        string opening = SettingPostProcessing(GetRandomIntroductionOpeningLine(rand), new List<string>() {  }, new List<string>() { }, rand);
        string body = SettingPostProcessing(GetRandomIntroductionBodyLine(rand), new List<string>() {  }, new List<string>() { }, rand);
        string closing = SettingPostProcessing(GetRandomIntroductionClosingLine(rand), new List<string>() {  }, new List<string>() { }, rand);

        return opening + " " + body + " " + closing;
    }
    private string GetRandomIntroductionOpeningLine(Random rand)
    {
        List<string> lst = settings.IntroductionOpening;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomIntroductionBodyLine(Random rand)
    {
        List<string> lst = settings.Introduction;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomIntroductionClosingLine(Random rand)
    {
        List<string> lst = settings.IntroductionClosing;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomPersonDescription(Random rand, ref bool descriptionIsMale)
    {
        List<string> lstMale = settings.PersonDescriptionMale;
        List<string> lstFemale = settings.PersonDescriptionFemale;

        if (rand.Next(0, 100) > 50)
        {
            descriptionIsMale = true;
            return lstMale[rand.Next(0, lstMale.Count())];
        }
        else
        {
            descriptionIsMale = false;
            return lstFemale[rand.Next(0, lstFemale.Count())];
        }
    }
    private string GetRandomMemory(Random rand, bool isMale)
    {
        string rtnValue = String.Empty;
        List<string> lst = settings.Memory;

        rtnValue = lst[rand.Next(0, lst.Count())];
        if (!isMale)
        {
            rtnValue = rtnValue.Replace(" him ", " her ").Replace(" he ", " she ").Replace(" his ", " her ").Replace(" Him ", " Her ").Replace(" He ", " She ").Replace(" His ", " Her ").Replace(" him.", " her.").Replace(" he.", " she.").Replace(" his.", " her.");
        }

        return rtnValue;
    }
    private string GetRandomFollowupLine(Random rand)
    {
        List<string> lst = settings.FollowupLine;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomAcquaintance(Random rand)
    {
        List<string> lst = settings.Acquaintance;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetListOfAcquaintance(Random rand, int count)
    {
        string rtn = String.Empty;

        for (int i = 0; i < count; i++)
        {
            if (i != 0)
                rtn += ", ";

            rtn += GetRandomAcquaintance(rand);
        }

        return rtn;
    }
    private string GetRandomListOfOilAndGasQuestions(Random rand, int count)
    {
        int[] indexes = new int[count];
        int indexCtr = 0;
        string rtn = String.Empty;

        if (settings.ResponseOpeningOilAndGasQuestionList.Count() < count)
        {
            while (indexes.Count() < count)
            {
                int tmp = rand.Next(0, count);

                bool found = false;
                foreach (int i in indexes)
                {
                    if (tmp == i)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    indexes[indexCtr] = tmp;
                    indexCtr++;
                }
            }

            for (int i = 0; i < indexes.Count(); i++)
            {
                rtn += (i + 1).ToString() + ") " + SettingPostProcessing(settings.ResponseOpeningOilAndGasQuestionList[indexes[i]], new List<string> {  }, new List<string> {  }, rand) + Environment.NewLine;
            }
        }
        else
        {
            throw new Exception("More Oil and Gas questions requested than setup. Please add more ResponseOpeningOilAndGasQuestionList to the settings.json file.");
        }

        return rtn;
    }
    private string GetRandomLocation(Random rand)
    {
        List<string> lst = settings.Locations;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomProduct(Random rand)
    {
        List<string> lst = settings.Products;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomPaymentMethod(Random rand)
    {
        List<string> lst = settings.PaymentMethods;

        return lst[rand.Next(0, lst.Count())];
    }

    private string GetRandomOpeningResponseForXXXXXX(Random rand, string greetings, string name, string attamentType, MailStorage currentMessage, List<MailStorage> pastMessages)
    {
        List<string> lst = new List<string>
        {
            greetings + " " + name + ", Example",
            greetings + " " + name + ", Test"
        };

        return lst[rand.Next(0, lst.Count())];
    }
}