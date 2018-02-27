using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;

public class MailServerFunctions
{
    private const string settingFileLocation = "Settings.json";
    public const string ProcessFolderName = "AutoProcessedMail";
    public const int Timeout = 50000; //TODO turn this into a setting
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
        SellingProducts = 21,
        FreeMoney = 22,
        InformationGathering = 23,
        Phishing = 24,
        ScamVictim = 25
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
            settings.MyFakeAddress = "FakeAddressHere";
            settings.MyFakePhoneNumber = "(000) 000-0000";
            settings.MyFakeBirthdate = "01-01-1900";
            settings.MyFakeCountry = "USA";
            settings.MyFakeGender = "Male";
            settings.MyFakeMaritalStatus = "Single";
            settings.MyFakeOccupation = "Plumber";
            settings.PathToMyFakeID = @"c:\FakeID\PathHere.png";
            settings.Acquaintance = new List<string>() { "Bob", "Steve", "Bill", "Chad", "Mary", "Margret", "Joe", "Frank", "Cathy" };
            settings.Products = new List<string>() { "Cars", "Boats", "Lava Lamps", "Blinker Fluid" };
            settings.PaymentMethods = new List<string>() { "Cash", "Wire Transfer", "Bank Transfer", "Personal Check", "Bitcoin", "USD", "Euros", "Rupels" };
            settings.FollowupLine = new List<string>() { "Let me know what you need from me.", "What info do you need from me?", "Tell me exactly what you need from me." };
            settings.Greeting = new List<string>() { "Hello", "Howdy", "Hi", "How are you" };
            settings.Locations = new List<string>() { "Florida", "Mexico", "Europe", "China", "Montana", "New York" };
            settings.Introduction = new List<string>() { "I do not use the internet much so please be very specific.", "I was born and raised in the midwest. |Environment.NewLine| Herding cattle is what I did day in and day out.", "I have two brothers and a sister, I play professional basket baseball for the Minnesota Wild Twins." };
            settings.IntroductionClosing = new List<string>() { "Maybe this information is useful, maybe it is not", "That was a little it about who I am, maybe you want to share a little about yourself since I shared. I hate being the only one to share, it is always better if we all share. Note: I hate when people use the word 'Share' too often.", "thanks for taking the time to listen to me." };
            settings.IntroductionOpening = new List<string>() { "Before I respond to your email I want to give you a background on who I am.", "Let me tell you about myself before I answer.", "Here is a quick summary of me." };
            settings.Memory = new List<string>() { "I remember back in 2003 when times were simple. I am pretty sure we didn't have internet or eletricity back then. Something about a goose and a chicken but I forget.", "Today these kids have life too easy. I remember back when I went to school we had to walk up hill both ways everyday.", "I remember this one time wen Janice from accounting had a posted note stuck to her back for the entire day. One of the funniest things I have ever seen in my entire life. You know Janice though, always a hoot." };
            settings.Names = new List<string>() { "Margret", "Jane", "Jack" };
            settings.PersonDescriptionMale = new List<string>() { "He was a good person, he wouldn't hurt a fly.", "About 5 foot 8 inches in height. Maybe 170 pounds but it is tough to say. Green eyes but one eye was slightly larger than the other. Brown hair on the right side with red hair on the left side. When it comes to the hair the left side was black and the right side was green. Hair color was a pure white." };
            settings.PersonDescriptionFemale = new List<string>() { "You know her, always laughing. She had a laugh that would light up the entire room.", "About 5 foot 8 inches in height. Maybe 170 pounds but it is tough to say. Green eyes but one eye was slightly larger than the other. Brown hair on the right side with red hair on the left side. When it comes to the hair the left side was black and the right side was green. Hair color was a pure white." };
            settings.Signoff = new List<string>() { "Farewell my friend", "Bye", "Later gator, after awhile crocodile", "Cya", "ByeBye", "Bye now" };
            settings.RandomThoughts = new List<string>() { "Life is much too short to get caught up in the details." };

            //Question responses
            settings.QuestionsHowAreYou = new List<string>() { "I am doing well, thanks for asking." };
            settings.QuestionsJokingAround = new List<string>() { "I am taking this very serious." };
            settings.QuestionsNotAnswering = new List<string>() { "I am a very busy person and must have missed your question." };
            settings.QuestionsNotListening = new List<string>() { "I am trying to follow instructions but they are very confusing." };
            settings.QuestionsNotUnderstanding = new List<string>() { "I am very confused, can you try to explain what you want in an easier way?" };
            settings.QuestionsPermission = new List<string>() { "You have my permission." };
            settings.QuestionsSpokenLanguage = new List<string>() { "I speak english." };
            settings.QuestionsTrust = new List<string>() { "You can trust me." };
            settings.QuestionsAddress = new List<string>() { "My Address: |Address|." };
            settings.QuestionsBirthdate = new List<string>() { "MyName birthdate: |Birthdate|." };
            settings.QuestionsID = new List<string>() { "I included the ID." };
            settings.QuestionsPhoneNumber = new List<string>() { "My Phone Number: |PhoneNumber|." };
            settings.QuestionsNotAnswering = new List<string>() { "I must have missed your call." };
            settings.QuestionsCannotOpenAttachment = new List<string>() { "The file opens for me." };
            settings.QuestionsAlreadyIncludedID = new List<string>() { "I have already sent you the ID." };
            settings.QuestionsOccupation = new List<string>() { "I work as a |Occupation|" };
            settings.QuestionsGender = new List<string>() { "I am a |Gender|" };
            settings.QuestionsMaritalStatus = new List<string>() { "I am |MaritalStatus|" };
            settings.QuestionsCountry = new List<string>() { "I live in |Country|" };
            settings.QuestionsName = new List<string>() { "My name is |Name|" };

            //Opening responses
            settings.ResponseOpeningAtmCard = new List<string>() { "I am glad to see progress with my ATM card, please send card numbers over email." };
            settings.ResponseOpeningBlankEmailWithAttachment = new List<string>() { "I am not able to open the |attachmentType| attachment you sent, please send the email without an attachment!" };
            settings.ResponseOpeningConsignmentBox = new List<string>() { "I am happy to see progress with my delivery. Please attach a picture in the email so I know you have my box." };
            settings.ResponseOpeningDeathOrDying = new List<string>() { "This is a very sad situation we have found outselves in. How may I help?" };
            settings.ResponseOpeningGenericPayment = new List<string>() { "I see you are seeking my payment. Please provide your photo ID or employee badge to show proof you are who you say you are." };
            settings.ResponseOpeningIlluminati = new List<string>() { "I am glad you have contacted me. I want to join. Please describe to me the exact benefits I will receive." };
            settings.ResponseOpeningInvestor = new List<string>() { "I am very interested in your opportunity. Tell me more about you or your company." };
            settings.ResponseOpeningLoanOffer = new List<string>() { "I am interested in the loan offer." };
            settings.ResponseOpeningLottery = new List<string>() { "I am so excited for winning! What do you need from me?", "I need to tell all my friends I won! I need to tell: |GetListOfAcquaintance|" };
            settings.ResponseOpeningMoneyHack = new List<string>() { "I am interested in knowing more about this money trick." };
            settings.ResponseOpeningMoneyStorage = new List<string>() { "I can complete the task you have set for me. How promptly will I receive payment?" };
            settings.ResponseOpeningOilAndGas = new List<string>() { "I am interested in doing business with you." };
            settings.ResponseOpeningOilAndGasQuestionList = new List<string> { "How many people are in on the operation?", "How will the money be split?", "Who is the leader/boss of the operation?", "Can I get |GetRandomAcquaintance| in on the deal?", "Why have you chosen me for this honor?", "I am worried about the chinese, how will we deal with them?", "Have you done this sort of thing before?" };
            settings.ResponseOpeningPolice = new List<string>() { "I swear I didn't do it! Your looking for |GetRandomAcquaintance| I think they are heading to |GetRandomLocation| with a bunch of stolen |GetRandomProduct| that they bought with fake |GetRandomPaymentMethod|. |GetRandomFollowupLine|" };
            settings.ResponseOpeningJobOffer = new List<string>() { "I am interested in a job." };
            settings.ResponseOpeningSellingProducts = new List<string>() { "I am interested in your product. Can you tell me more about it?" };
            settings.ResponseOpeningInformationGathering = new List<string> { "Glad to receive your email. Can you tell me about yourself?" };
            settings.ResponseOpeningPhishing = new List<string>() { "The link you sent did not work. I get a 404 error, please help." };

            //Continued responses
            settings.ResponseContinuedAtmCard = new List<string>() { "I understand what you are saying but can you just provide the card numbers?", "Thank you again for your patience, I think it would be easiest if you just provided the card numbers over email." };
            settings.ResponseContinuedBeneficiary = new List<string>() { "You have provided a lot of information, what is it you need from me exactly?", "How can we move forward with this?" };
            settings.ResponseContinuedBlankEmailWithAttachment = new List<string>() { "My email client cannot support attachment, please paste the contents of the attachment into the email." };
            settings.ResponseContinuedBuildTrust = new List<string>() { "Tell me what hobbies you have.", "Tell me more about what you do for work." };
            settings.ResponseContinuedConsignmentBox = new List<string>() { "I really need to see a picture of the consignment box you speak of.", "We cannot proceed until I see a picture of the box.", "I am not sure if you attached the picture but we cannot proceed until I see it." };
            settings.ResponseContinuedDeathOrDying = new List<string>() { "I wish to help you, how can I help.", "What do you need from me?", "Let me know what you need from me so that I can help." };
            settings.ResponseContinuedGenericPayment = new List<string>() { "The information you provided does not seem right, can you double check it?" };
            settings.ResponseContinuedIlluminati = new List<string>() { "Do I get to speak with the grandmaster?", "Can I get |GetRandomAcquaintance| in the club?" };
            settings.ResponseContinuedInheritance = new List<string>() { "|GetRandomFollowupLine|" };
            settings.ResponseContinuedInvestor = new List<string>() { "I see the information your provided but how will I make money?", "I get what your saying but what is in it for me?" };
            settings.ResponseContinuedLoanOffer = new List<string>() { "How low of a rate can you get me?", "Can I get a lower rate?" };
            settings.ResponseContinuedLottery = new List<string>() { "I am so excited that I won, please send me payment with an ATM card. Just send me the card number over email." };
            settings.ResponseContinuedMoneyHack = new List<string>() { "This trick seems to good to be true. How do I know this is not a trick on me?", "How can I know what you say is true?" };
            settings.ResponseContinuedMoneyStorage = new List<string>() { "I will hold onto your money.", "I am willing to help you with this.", "I think we will make a good team." };
            settings.ResponseContinuedOilAndGas = new List<string>() { "How much volume can we move?", "Will we get in trouble with the government?", "Can I see proof that what you say is true?" };
            settings.ResponseContinuedPolice = new List<string>() { "I can prove that I am innocent." };
            settings.ResponseContinuedJobOffer = new List<string>() { "Can I work remotely?", "Is there a promotion path?", "Do I get on the job training?" };
            settings.ResponseContinuedSellingProducts = new List<string>() { "How much product do you have?", "Can you accept payment in cryptocurrencies?" };
            settings.ResponseContinuedInformationGathering = new List<string>() { "Do you have a family?", "Have you ever been to |GetRandomLocation|?" };
            settings.ResponseContinuedPhishing = new List<string>() { "The link you sent isn't working and I really want to resolve this issue today.", "That isn't working for me. Is there another way to fix it?" };

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
                    int failedCount = 0;
                    while (failedCount < 5)
                    {
                        try
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
                                if (messages[0].ProcessingNotes.ToUpper().Contains("FAILED"))
                                {
                                    response.Code = -1;
                                    response.Message = messages[0].ProcessingNotes;
                                    return response;
                                }

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
                            break;
                        }
                        catch (Exception iex)
                        {
                            if (iex.Message.Contains("established connection failed because connected host has failed to respond"))
                            {
                                failedCount++;
                                Thread.Sleep(10000); //Sleep for 10 seconds and wait for the connection issue to clear up
                            }
                            else
                            {
                                throw iex;
                            }
                        }
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
    public StandardResponse SendSMTP(LoggerInfo loggerInfo, string toAddress, string subject, string bodyText, bool includeID)
    {
        return SendSMTP(loggerInfo, UserName, Password, UserName, UserName, toAddress, toAddress, subject, bodyText, Timeout, includeID);
    }
    public StandardResponse SendSMTP(LoggerInfo loggerInfo, string username, string password, string fromAddress, string fromAddressReadable, string toAddress, string toAddressReadable, string subject, string bodyText, int timeout, bool includeID)
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
                        //friendly = tmpSplit[0];
                        realAddress = tmpSplit[1];

                        for (int i = tmpSplit.Count() - 1; i >= 0; i--)
                        {
                            if (tmpSplit[i].Contains("@"))
                            {
                                realAddress = tmpSplit[i];
                                break;
                            }
                        }

                        //friendly = friendly.Replace("\"", "").Trim();
                        realAddress = realAddress.Replace("<", "").Replace(">", "").Trim();
                    }
                    else
                    {
                        //friendly = s;
                        realAddress = s.Replace("<", "").Replace(">", "").Trim();
                    }
                }
                else
                {
                    //friendly = s;
                    realAddress = s;
                }

                if(IsValidEmail(realAddress))
                    message.To.Add(new MailboxAddress(ScrubText(friendly), realAddress));
            }

            if (!subject.StartsWith("RE:"))
                subject = "RE: " + subject;

            message.Subject = subject;

            string htmlBodyText = TextToHtml(bodyText);

            Multipart alternative = new Multipart("alternative");
            TextPart plainText = null;
            plainText = new TextPart("plain");
            plainText.Text = bodyText;
            alternative.Add(plainText);

            //No need, plain text should be fine. HTML formatted with <PRE> looks different and might tip them off on my program
            //TextPart htmlText = null;
            //htmlText = new TextPart("html");
            //htmlText.Text = htmlBodyText;
            //alternative.Add(htmlText);

            Multipart messageBodyMultiPart = new Multipart("mixed");
            messageBodyMultiPart.Add(alternative);

            if (includeID)
            {
                MimePart attach = new MimePart();
                attach.ContentObject = new ContentObject(new MemoryStream(File.ReadAllBytes(settings.PathToMyFakeID)), ContentEncoding.Default);
                attach.ContentDisposition = new ContentDisposition(ContentDisposition.Attachment);
                attach.ContentTransferEncoding = ContentEncoding.Default;
                attach.FileName = Path.GetFileName(settings.PathToMyFakeID);

                messageBodyMultiPart.Add(attach);
            }

            message.Body = messageBodyMultiPart;

            Logger.WriteDbg(loggerInfo, "Message Built");

            int failedCount = 0;
            while (failedCount < 5)
            {
                try
                {
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

                    break; //If no exception was thrown break the loop
                }
                catch (Exception iex)
                {
                    if (iex.Message.Contains("established connection failed because connected host has failed to respond"))
                    {
                        failedCount++;
                        Thread.Sleep(10000); //Sleep for 10 seconds and wait for the connection issue to clear up
                    }
                    else
                    {
                        throw iex;
                    }
                }
            }
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
                if (String.IsNullOrEmpty(storageObj.EmailBodyPlain) && !String.IsNullOrEmpty(storageObj.EmailBodyHtml))
                {
                    HtmlConvert convert = new HtmlConvert();
                    storageObj.EmailBodyPlain = convert.ConvertHtml(storageObj.EmailBodyHtml);
                }

                foreach (var v in msg.FromAddress)
                {
                    if(msg.FromAddress.Count() <= 1 || !v.ToString().Trim().ToLower().Contains(".ocn.ne.jp")) //Often times they include multiple email addresses, the ocn.ne.jp ones tend to get rejected so exclude that email in the reply if it is not the only address
                        storageObj.ToAddress += v.ToString() + ";";
                }
                foreach (var v in msg.ReplyTo)
                {
                    storageObj.ToAddress += v.ToString() + ";";
                }
                foreach(var v in msg.FileAttachments)
                {
                    storageObj.AtachmentTypes += v.FileExtension + ",";
                    storageObj.AttachmentNames += v.FileName + ",";
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

    //TODO This class is potentially too large, consider splitting into smaller classes and remove regions
    //Opening Responses
    #region Opening Responses
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
    private string GetRandomOpeningResponseForBeneficiary(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string response = String.Empty;
        string introduction = String.Empty;
        string followup = String.Empty;
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        response += greetings + " " + name + ", ";

        //Opening introduction of myself
        introduction = SettingPostProcessing(GetRandomInroduction(rand), rand);
        //Closing Followup line
        followup = GetRandomFollowupLine(rand);

        response += directResponse + introduction + Environment.NewLine + Environment.NewLine + followup;

        return response;
    }
    private string GetRandomOpeningResponseForBlankEmailWithAttachment(Random rand, string greetings, string attachmentType)
    {
        return greetings + ". " + SettingPostProcessing(settings.ResponseOpeningBlankEmailWithAttachment[rand.Next(0, settings.ResponseOpeningBlankEmailWithAttachment.Count())], new List<string> { "|attachmentType|" }, new List<string> { attachmentType }, rand);
    }
    private string GetRandomOpeningResponseForLottery(Random rand, string greetings, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningLottery[rand.Next(0, settings.ResponseOpeningLottery.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand,5) }, rand);
    }
    private string GetRandomOpeningResponseForOilAndGas(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string rtn = String.Empty;
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        rtn = greetings + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningOilAndGas[rand.Next(0, settings.ResponseOpeningOilAndGas.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 5) }, rand);

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
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningIlluminati[rand.Next(0, settings.ResponseOpeningIlluminati.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForConsignmentBox(Random rand, string greetings, string name, string attachmentType, MailStorage currentMessage)
    {
        string attachmentIncludedText = String.Empty;
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        if (attachmentType == "Image")
        {
            attachmentIncludedText = " I have noticed that an image file was attached, I am unable to view the image file from my current device but I must trust that the file is a picture of my consignment box. I will review the phota later when I have image viewing capabilities. If you could also send a few other angles of the box that would be appreciated. I have received fake pictures in the past so hopefully you understand my request.";
        }
        else
        {
            attachmentIncludedText = " I have noticed that no image file was included. Could you send me a picture of my consignment package so that I can trust you do in fact have it? I do not know what my package is supposed to look like but just to verify you have it.";
        }

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningConsignmentBox[rand.Next(0, settings.ResponseOpeningConsignmentBox.Count())], new List<string> { "|attachmentIncludedText|" }, new List<string> { attachmentIncludedText }, rand);
    }
    private string GetRandomOpeningResponseForDeathOrDying(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningDeathOrDying[rand.Next(0, settings.ResponseOpeningDeathOrDying.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForLoanOffer(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningLoanOffer[rand.Next(0, settings.ResponseOpeningLoanOffer.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForMoneyStorage(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningMoneyStorage[rand.Next(0, settings.ResponseOpeningMoneyStorage.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForAtmCard(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningAtmCard[rand.Next(0, settings.ResponseOpeningAtmCard.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForPolice(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningPolice[rand.Next(0, settings.ResponseOpeningPolice.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForGenericPayment(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningGenericPayment[rand.Next(0, settings.ResponseOpeningGenericPayment.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForInvestor(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningInvestor[rand.Next(0, settings.ResponseOpeningInvestor.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForMoneyHack(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningMoneyHack[rand.Next(0, settings.ResponseOpeningMoneyHack.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForInheritance(Random rand, string greetings, string name, string attamentType, MailStorage currentMessage, List<MailStorage> pastMessages)
    {
        string response = String.Empty;
        string introduction = String.Empty;
        string inheritorDescription = String.Empty;
        string memories = String.Empty;
        string followup = String.Empty;
        bool isMale = true;
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        response += greetings + " " + name + ", ";

        //Opening introduction of myself
        introduction = SettingPostProcessing(GetRandomInroduction(rand), rand);
        //Description of interitor
        inheritorDescription = GetRandomPersonDescription(rand, ref isMale);
        //Share memories
        memories = GetRandomMemory(rand, isMale);
        //Closing Followup line
        followup = GetRandomFollowupLine(rand);

        response += directResponse + introduction + Environment.NewLine + Environment.NewLine + inheritorDescription + Environment.NewLine + Environment.NewLine + memories + Environment.NewLine + Environment.NewLine + followup;

        return response;
    }
    private string GetRandomOpeningResponseForBuildTrust(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string introduction = SettingPostProcessing(GetRandomInroduction(rand), rand);
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        List<string> lst = new List<string>
        {
            greetings + " friend, " + directResponse + introduction,
            greetings + " my friend, " + directResponse + introduction
        };

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomOpeningResponseForJobOffer(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningJobOffer[rand.Next(0, settings.ResponseOpeningJobOffer.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForSellingProducts(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningSellingProducts[rand.Next(0, settings.ResponseOpeningSellingProducts.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomOpeningResponseForFreeMoney(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningFreeMoney[rand.Next(0, settings.ResponseOpeningFreeMoney.Count())], rand);
    }
    private string GetRandomOpeningResponseForInformationGathering(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningInformationGathering[rand.Next(0, settings.ResponseOpeningInformationGathering.Count())], rand);
    }
    private string GetRandomOpeningResponseForPhishing(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningPhishing[rand.Next(0, settings.ResponseOpeningPhishing.Count())], rand);
    }
    private string GetRandomOpeningResponseForScamVictims(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningScamVictim[rand.Next(0, settings.ResponseOpeningScamVictim.Count())], rand);
    }
    #endregion

    //Unsubscribe
    #region Unsubscribe
    private string GetRandomResponseForSellingServices(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        List<string> lst = new List<string>
        {
            "UNSUBSCRIBE",
            "Please remove me from your mailing list"
        };

        return lst[rand.Next(0, lst.Count())];
    }
    #endregion

    //Continued Responses
    #region Continued Responses
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
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedLottery[rand.Next(0, settings.ResponseContinuedLottery.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomContinuedResponseForOilAndGas(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string rtn = String.Empty;
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        rtn = greetings + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedOilAndGas[rand.Next(0, settings.ResponseContinuedOilAndGas.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 5) }, rand);

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
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedIlluminati[rand.Next(0, settings.ResponseContinuedIlluminati.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomContinuedResponseForConsignmentBox(Random rand, string greetings, string name, string attachmentType, MailStorage currentMessage)
    {
        string attachmentIncludedText = String.Empty;
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        if (attachmentType == "Image")
        {
            attachmentIncludedText = " I have noticed that an image file was attached, I am unable to view the image file from my current device but I must trust that the file is a picture of my consignment box. I will review the phota later when I have image viewing capabilities. If you could also send a few other angles of the box that would be appreciated. I have received fake pictures in the past so hopefully you understand my request.";
        }
        else
        {
            attachmentIncludedText = " I have noticed that no image file was included. Could you send me a picture of my consignment package so that I can trust you do in fact have it? I do not know what my package is supposed to look like but just to verify you have it.";
        }

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedConsignmentBox[rand.Next(0, settings.ResponseContinuedConsignmentBox.Count())], new List<string> { "|attachmentIncludedText|" }, new List<string> { attachmentIncludedText }, rand);
    }
    private string GetRandomContinuedResponseForDeathOrDying(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedDeathOrDying[rand.Next(0, settings.ResponseContinuedDeathOrDying.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomContinuedResponseForLoanOffer(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedLoanOffer[rand.Next(0, settings.ResponseContinuedLoanOffer.Count())], new List<string> {  }, new List<string> {  }, rand);
    }
    private string GetRandomContinuedResponseForMoneyStorage(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedMoneyStorage[rand.Next(0, settings.ResponseContinuedMoneyStorage.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForAtmCard(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedAtmCard[rand.Next(0, settings.ResponseContinuedAtmCard.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForPolice(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedPolice[rand.Next(0, settings.ResponseContinuedPolice.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForGenericPayment(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningGenericPayment[rand.Next(0, settings.ResponseOpeningGenericPayment.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForInvestor(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningInvestor[rand.Next(0, settings.ResponseOpeningInvestor.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForMoneyHack(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedMoneyHack[rand.Next(0, settings.ResponseContinuedMoneyHack.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForInheritance(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedInheritance[rand.Next(0, settings.ResponseContinuedInheritance.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForBeneficiary(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedBeneficiary[rand.Next(0, settings.ResponseContinuedBeneficiary.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForBuildTrust(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedBuildTrust[rand.Next(0, settings.ResponseContinuedBuildTrust.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForJobOffer(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedJobOffer[rand.Next(0, settings.ResponseContinuedJobOffer.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForSellingProducts(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedSellingProducts[rand.Next(0, settings.ResponseContinuedSellingProducts.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 2) }, rand);
    }
    private string GetRandomContinuedResponseForFreeMoney(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedFreeMoney[rand.Next(0, settings.ResponseContinuedFreeMoney.Count())], rand);
    }
    private string GetRandomContinuedResponseForInformationGathering(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedInformationGathering[rand.Next(0, settings.ResponseContinuedInformationGathering.Count())], rand);
    }
    private string GetRandomContinuedResponseForPhishing(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedPhishing[rand.Next(0, settings.ResponseContinuedPhishing.Count())], rand);
    }
    private string GetRandomContinuedResponseForScamVictims(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedScamVictim[rand.Next(0, settings.ResponseContinuedScamVictim.Count())], rand);
    }
    #endregion

    //Supporting Random lists
    #region Random Lists
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
        //Simply get these lines and let the SettingPostProcessing handle all the replacements
        string opening = GetRandomIntroductionOpeningLine(rand);
        string body = GetRandomIntroductionBodyLine(rand);
        string closing = GetRandomIntroductionClosingLine(rand);

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
    private string GetRandomThought(Random rand)
    {
        List<string> lst = settings.RandomThoughts;

        return lst[rand.Next(0, lst.Count())];
    }
    #endregion

    //Get Random Questions lists
    #region Random Question lists
    private string GetRandomQuestionsHowAreYou(Random rand)
    {
        List<string> lst = settings.QuestionsHowAreYou;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsJokingAround(Random rand)
    {
        List<string> lst = settings.QuestionsJokingAround;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsNotAnswering(Random rand)
    {
        List<string> lst = settings.QuestionsNotAnswering;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsNotListening(Random rand)
    {
        List<string> lst = settings.QuestionsNotListening;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsNotUnderstanding(Random rand)
    {
        List<string> lst = settings.QuestionsNotUnderstanding;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsPermission(Random rand)
    {
        List<string> lst = settings.QuestionsPermission;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsSpokenLanguage(Random rand)
    {
        List<string> lst = settings.QuestionsSpokenLanguage;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsTrust(Random rand)
    {
        List<string> lst = settings.QuestionsTrust;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsAddress(Random rand)
    {
        List<string> lst = settings.QuestionsAddress;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsID(Random rand)
    {
        List<string> lst = settings.QuestionsID;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsPhoneNumber(Random rand)
    {
        List<string> lst = settings.QuestionsPhoneNumber;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsWhyNoAnswer(Random rand)
    {
        List<string> lst = settings.QuestionsWhyNoAnswer;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsCannotOpenAttachment(Random rand)
    {
        List<string> lst = settings.QuestionsCannotOpenAttachment;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsAlreadyIncludedID(Random rand)
    {
        List<string> lst = settings.QuestionsAlreadyIncludedID;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsBirthdate(Random rand)
    {
        List<string> lst = settings.QuestionsBirthdate;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsBetterPhoto(Random rand)
    {
        List<string> lst = settings.QuestionsBetterPhoto;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsOccupation(Random rand)
    {
        List<string> lst = settings.QuestionsOccupation;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsGender(Random rand)
    {
        List<string> lst = settings.QuestionsGender;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsMaritalStatus(Random rand)
    {
        List<string> lst = settings.QuestionsMaritalStatus;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsCountry(Random rand)
    {
        List<string> lst = settings.QuestionsCountry;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsName(Random rand)
    {
        List<string> lst = settings.QuestionsName;

        return lst[rand.Next(0, lst.Count())];
    }
    #endregion

    //Helper functions
    #region Helper functions
    private string SettingPostProcessing(string text, Random rand)
    {
        return SettingPostProcessing(text, new List<string> { }, new List<string> { }, rand);
    }
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
        placeholder.Add("|GetRandomThought|");
        replacement.Add(GetRandomThought(rand));
        placeholder.Add("|PhoneNumber|");
        replacement.Add(settings.MyFakePhoneNumber);
        placeholder.Add("|Address|");
        replacement.Add(settings.MyFakeAddress);
        placeholder.Add("|Birthdate|");
        replacement.Add(settings.MyFakeBirthdate);
        placeholder.Add("|Occupation|");
        replacement.Add(settings.MyFakeOccupation);
        placeholder.Add("|Gender|");
        replacement.Add(settings.MyFakeGender);
        placeholder.Add("|MaritalStatus|");
        replacement.Add(settings.MyFakeMaritalStatus);
        placeholder.Add("|Country|");
        replacement.Add(settings.MyFakeCountry);
        placeholder.Add("|Name|");
        replacement.Add(settings.MyName);

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

        //Get rid of all extra line breaks for the parsing
        while (body.Contains(Environment.NewLine + Environment.NewLine))
        {
            body = body.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
        }

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
    private string AttemptToFindReplyToEmailAddress(string body)
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
        body.Replace("?", " ");

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
                                if (IsValidEmail(innerStr))
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
                    lineSplit[i] = lineSplit[i].Replace("{"," ").Replace("}", " ").Replace("]", " ").Replace("]", " ").Replace(",", " ").Replace("<", " ").Replace(">", " ").Replace("^", " ").Replace("(", " ").Replace(")", " ").Replace("\t", " ").Replace("\r", " ").Replace("\n", " ").Replace(" ", " ");

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
                    tempSplit = lineSplit[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
    private string HandleDirectQuestions(string body, ref MailStorage currentMessage, Random rand)
    {
        string response = String.Empty;
        string preProcessedBody = body.Replace("\r\n", " ");
        bool alreadyRepliedNotAnswering = false;

        if (preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU DOING") || 
            preProcessedBody.Trim().ToUpper().Contains("HOW YOU DOING") || 
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU OVER THERE") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU TODAY"))
        {
            response += GetRandomQuestionsHowAreYou(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("IS THIS A JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU MAKING FUN") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU THINK THIS IS JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU PLAYING WITH US") ||
            preProcessedBody.Trim().ToUpper().Contains("IF YOU KNOW YOU ARE NOT SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU TAKE US FOR A FOOL") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU THINK YOU ARE FUNNY"))
        {
            response += GetRandomQuestionsJokingAround(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("NO PICK UP WHEN") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU NOT PICK UP") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT PICKING UP") ||
            (
                (preProcessedBody.Trim().ToUpper().Contains("CALL") || 
                    preProcessedBody.Trim().ToUpper().Contains("PHONE") || 
                    preProcessedBody.Trim().ToUpper().Contains("RANG") || 
                    preProcessedBody.Trim().ToUpper().Contains("RING")
                ) &&
                (preProcessedBody.Trim().ToUpper().Contains("NOT ANSWERING ME") ||
                    preProcessedBody.Trim().ToUpper().Contains("IGNORING MY QUESTION") ||
                    preProcessedBody.Trim().ToUpper().Contains("NOT ANSWER MY QUESTION") ||
                    preProcessedBody.Trim().ToUpper().Contains("DID YOU NOT ANSWER") ||
                    preProcessedBody.Trim().ToUpper().Contains("YOU DID NOT ANSWER") ||
                    preProcessedBody.Trim().ToUpper().Contains("NO ANSWER WHEN") ||
                    preProcessedBody.Trim().ToUpper().Contains("YOU DID NO ANSWER") ||
                    preProcessedBody.Trim().ToUpper().Contains("FAILED TO CONNECT") ||
                    preProcessedBody.Trim().ToUpper().Contains("COULD NOT GET YOU ON THE PHONE") ||
                    preProcessedBody.Trim().ToUpper().Contains("NOT ANSWER ME")
                )
            ) ||
            preProcessedBody.Trim().ToUpper().Contains("YOU DID NOT PICK UP"))
        {
            response += GetRandomQuestionsWhyNoAnswer(rand) + " ";
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("NOT ANSWERING ME") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT ANSWER MY QUESTION") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU NOT ANSWER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU DID NOT ANSWER") ||
            preProcessedBody.Trim().ToUpper().Contains("NO ANSWER WHEN") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU DID NO ANSWER") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT ANSWER ME"))
        {
            alreadyRepliedNotAnswering = true;
            response += GetRandomQuestionsNotAnswering(rand) + " ";
        }
        if (!alreadyRepliedNotAnswering)
        {
            if (preProcessedBody.Trim().ToUpper().Contains("IGNORING MY QUESTION") ||
                preProcessedBody.Trim().ToUpper().Contains("NOT RESPONDING MY QUESTION") ||
                preProcessedBody.Trim().ToUpper().Contains("NO RESPONDING MY QUESTION") ||
                preProcessedBody.Trim().ToUpper().Contains("NOT RESPONDING QUESTION") ||
                preProcessedBody.Trim().ToUpper().Contains("NO RESPONDING QUESTION") ||
                preProcessedBody.Trim().ToUpper().Contains("IGNORING QUESTION"))
            {
                response += GetRandomQuestionsNotAnswering(rand) + " ";
            }
        }

        if (preProcessedBody.Trim().ToUpper().Contains("NOT LISTENING ME") ||
            preProcessedBody.Trim().ToUpper().Contains("LISTEN TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("PAY ATTENTION TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT PAYING ATTENTION"))
        {
            response += GetRandomQuestionsNotListening(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("NOT UNDERSTANDING MY") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT UNDERSTANDING ME") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFUSED BY MY") ||
            preProcessedBody.Trim().ToUpper().Contains("WHY SO CONFUSED"))
        {
            response += GetRandomQuestionsNotUnderstanding(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("HAVE YOUR PERMISSION") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN I HAVE PERMISSION") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED PERMISSION") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU GIVE PERMISSION"))
        {
            response += GetRandomQuestionsPermission(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("DO YOU SPEAK ENGLISH") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT LANGUAGE DO YOU SPEAK") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOU TALK ENGLISH") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOU SPEAK ENGLISH"))
        {
            response += GetRandomQuestionsSpokenLanguage(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("CAN I TRUST YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW CAN I TRUST YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOU BE TRUSTED") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN BE TRUSTED"))
        {
            response += GetRandomQuestionsTrust(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR LEGAL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR LEGAL FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR FIRST AND LAST NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME.....") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME___") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME :") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT NAME DO YOU GO BY"))
        {
            response += GetRandomQuestionsName(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("WHERE DO YOU LIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("PROVIDE YOUR ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("WHERE CAN I SEND") ||
            preProcessedBody.Trim().ToUpper().Contains("MAILING ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("POSTAL ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE YOUR ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("BILLING ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("RESIDENTIAL ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS.....") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS___") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS :") ||
            preProcessedBody.Trim().ToUpper().Contains("GIVE ME YOUR ADDRESS"))
        {
            response += GetRandomQuestionsAddress(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("COUNTRY OF ORIGIN") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY.....") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY :") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY___") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY NAME.....") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY NAME :") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY NAME___") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT COUNTRY DO YOU LIVE"))
        {
            response += GetRandomQuestionsCountry(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("YOUR OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION.....") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION :") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION___") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR JOB"))
        {
            response += GetRandomQuestionsOccupation(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("YOUR GENDER") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX.....") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX :") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX___") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR SEX"))
        {
            response += GetRandomQuestionsGender(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("YOUR MARITAL STATUS") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITAL STATUS.....") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITAL STATUS___") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITAL STATUS :") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU SINGLE"))
        {
            response += GetRandomQuestionsMaritalStatus(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("OLD ARE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTHDATE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR AGE") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH YEAR") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH DAY") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH MONTH") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH TIME") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE.....") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE :") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE___") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU AGE"))
        {
            response += GetRandomQuestionsBirthdate(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT YOUR PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE YOUR PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE YOUR NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED YOUR NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT NUMBER TO") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW CAN I REACH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PRIVATE PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("DIRECT PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE.....") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE.....") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER.....") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE___") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE___") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER___") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE :") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER :") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE :") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW CAN I CALL YOU"))
        {
            response += GetRandomQuestionsPhoneNumber(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("SEND YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ALONG YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("ATTACH YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("ATTACH THE ID") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE THE ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ID") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ID OR PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("UPLOAD YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("UPLOAD THE ID") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT OR ID CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD.....") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE.....") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE___") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD___") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE :") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD :") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL ME YOUR ID"))
        {
            if (!currentMessage.IncludedIDinPast)
            {
                response += GetRandomQuestionsID(rand) + " ";
                currentMessage.IncludeID = true; //The SendMessage will pull in the image file
            }
            else
            {
                response += GetRandomQuestionsAlreadyIncludedID(rand) + " ";
            }
        }
        else
        {
            if (preProcessedBody.Trim().ToUpper().Contains("CANNOT OPEN FILE") ||
                preProcessedBody.Trim().ToUpper().Contains("CAN NOT OPEN FILE") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE TO OPEN FILE") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE OPEN FILE") ||
                preProcessedBody.Trim().ToUpper().Contains("CANNOT OPEN ATTACH") ||
                preProcessedBody.Trim().ToUpper().Contains("CAN NOT OPEN ATTACH") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE TO OPEN ATTACH") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE OPEN ATTACH") ||
                preProcessedBody.Trim().ToUpper().Contains("CANNOT ACCESS FILE") ||
                preProcessedBody.Trim().ToUpper().Contains("CAN NOT ACCESS FILE") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE TO ACCESS FILE") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE ACCESS FILE") ||
                preProcessedBody.Trim().ToUpper().Contains("CANNOT ACCESS ATTACH") ||
                preProcessedBody.Trim().ToUpper().Contains("CAN NOT ACCESS ATTACH") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE TO ACCESS ATTACH") ||
                preProcessedBody.Trim().ToUpper().Contains("CANNOT OPEN IMAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("CANNOT ACCESS IMAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("CAN NOT OPEN IMAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("CAN NOT ACCESS IMAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE TO OPEN IMAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE OPEN IMAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE TO ACCESS IMAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE ACCESS IMAGE"))
            {
                response += GetRandomQuestionsCannotOpenAttachment(rand) + " ";
            }
        }
        if (preProcessedBody.Trim().ToUpper().Contains("BETTER PICTURE") ||
            preProcessedBody.Trim().ToUpper().Contains("BETTER PHOTO") ||
            preProcessedBody.Trim().ToUpper().Contains("BETTER IMAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("QUALITY PICUTRE") ||
            preProcessedBody.Trim().ToUpper().Contains("QUALITY PHOTO") ||
            preProcessedBody.Trim().ToUpper().Contains("QUALITY IMAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("CLOSER PICTURE") ||
            preProcessedBody.Trim().ToUpper().Contains("CLOSE PICTURE") ||
            preProcessedBody.Trim().ToUpper().Contains("CLOSER PHOTO") ||
            preProcessedBody.Trim().ToUpper().Contains("CLOSE PHOTO") ||
            preProcessedBody.Trim().ToUpper().Contains("CLOSER IMAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("CLOSE IMAGE"))
        {
            response += GetRandomQuestionsBetterPhoto(rand) + " ";
        }

        if (!String.IsNullOrEmpty(response))
        {
            response = Environment.NewLine + Environment.NewLine + SettingPostProcessing(response, rand).Trim() + Environment.NewLine + Environment.NewLine;
        }

        return response;
    }
    public bool IsValidEmail(string email)
    {
        try
        {
            email = email.Replace("<", "").Replace(">", "").Trim();

            if (!email.Contains('@') || email.Trim().Contains(' ') || email.Trim().Contains('?') || 
                email.Trim().ToUpper() == settings.EmailAddress.Trim().ToUpper() || email.Trim().Contains('/') || 
                email.Trim().Contains('\\') || email.Trim().Contains("..."))
                return false;

            int atLocation = email.Trim().IndexOf('@');
            string name = email.Trim().Substring(0, atLocation);
            string domain = email.Trim().Substring(atLocation + 1);

            if (name.Trim().Length == 0 || domain.Trim().Length == 0)
                return false;

            var addr = new System.Net.Mail.MailAddress(email.Trim());
            return addr.Address.Trim().ToUpper() == email.Trim().ToUpper();
        }
        catch
        {
            return false;
        }
    }
    public string MakeEmailEasierToRead(string message)
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
    public string TextToHtml(string text)
    {
        text = "<pre>" + HttpUtility.HtmlEncode(text) + "</pre>";
        return text;
    }
    public string SynonymReplacement(Random rand, string textToReplace)
    {
        //This is a very primative function to replace some common words
        string replaceList = "good,acceptable,excellent,great,marvelous,wonderful,aswesome|";
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
        replaceList += "money,cash,fund,pay,wealth|";
        replaceList += "small,tiny,little,miniature|";
        replaceList += "remember,relive,recall,commemorate|";
        replaceList += "different,contrasting,diverse,various|";
        replaceList += "charm,amuse,entertain,satisfy,tickle|";
        replaceList += "start,begin,kickoff|";
        replaceList += "quickly,hastily,hurriedly,immediately,instantly,promptly,rapidly,swiftly|";
        replaceList += "help,aide,assist|";

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
    public string SettingsFileValidate()
    {
        string rtn = String.Empty;

        if (settings.EmailAddress == "PutYourEmailAddressHere@domain.com")
        {
            if (!String.IsNullOrEmpty(rtn))
                rtn += Environment.NewLine;

            rtn += "The email address in the settings file is the default value.";
        }
        if (settings.Password == "PutYouPasswordForTheEmailAddressHere")
        {
            if (!String.IsNullOrEmpty(rtn))
                rtn += Environment.NewLine;

            rtn += "The email password in the settings file is the default value.";
        }
        if (settings.MyName == "YourNameHere")
        {
            if (!String.IsNullOrEmpty(rtn))
                rtn += Environment.NewLine;

            rtn += "The name in the settings file is the default value.";
        }
        if (settings.MyFakeAddress == "FakeAddressHere")
        {
            if (!String.IsNullOrEmpty(rtn))
                rtn += Environment.NewLine;

            rtn += "The address in the settings file is the default value.";
        }
        if (settings.MyFakePhoneNumber == "(000) 000-0000")
        {
            if (!String.IsNullOrEmpty(rtn))
                rtn += Environment.NewLine;

            rtn += "The phone number in the settings file is the default value. Use google voice or some other 3rd party app to get a fake phone number.";
        }
        if (settings.MyFakeBirthdate == "01-01-1900")
        {
            if (!String.IsNullOrEmpty(rtn))
                rtn += Environment.NewLine;

            rtn += "The birthdate in the settings file is the default value.";
        }
        if (settings.PathToMyFakeID == @"c:\FakeID\PathHere.png")
        {
            if (!String.IsNullOrEmpty(rtn))
                rtn += Environment.NewLine;

            rtn += "The fake ID path in the settings file is the default value.";
        }
        else
        {
            if(!File.Exists(settings.PathToMyFakeID))
                rtn += "The fake ID path in the settings file is not accessible. Please check that the path is correct or grant access to the file. If you do not want to make one then simply take any picture from online open it in notepad and delete half of the characters so that it is unopenable.";
        }

        return rtn;
    }
    public string ScrubText(string text)
    {
        Regex rgx = new Regex("[^a-zA-Z0-9 -]");
        text = rgx.Replace(text, "");

        return text;
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
        string preProcessedBody = currentMessage.SubjectLine + " " + currentMessage.EmailBodyPlain.Replace("\r\n", " ");

        preProcessedBody = MakeEmailEasierToRead(preProcessedBody);

        //Types of emails
        if (currentMessage.SubjectLine.Contains("Test ") || currentMessage.SubjectLine.Contains(" Test"))
        {
            type = EmailType.Test;
        }
        else if (((preProcessedBody.Trim() == String.Empty || ((preProcessedBody.Length - currentMessage.SubjectLine.Length) < 40 && 
            (preProcessedBody.ToUpper().Contains("ATTACHMENT") || preProcessedBody.ToUpper().Contains("FILE")))) && currentMessage.NumberOfAttachments > 0) ||
            ((preProcessedBody.Length - currentMessage.SubjectLine.Length) <= 3 && currentMessage.NumberOfAttachments > 0))
        {
            type = EmailType.BlankWithAttachment;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("ILLUMINATI") || 
            preProcessedBody.Trim().ToUpper().Contains("ILUMINATI"))
        {
            type = EmailType.Illuminati;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("DIED AFTER") || 
            preProcessedBody.Trim().ToUpper().Contains("CANCER DIAG") || 
            preProcessedBody.Trim().ToUpper().Contains("WITH CANCER") || 
            preProcessedBody.Trim().ToUpper().Contains("MY CANCER") || 
            preProcessedBody.Trim().ToUpper().Contains("HUSBAND DIED") || 
            preProcessedBody.Trim().ToUpper().Contains("WIFE DIED") ||
            preProcessedBody.Trim().ToUpper().Contains("DIED OF CANCER") ||
            preProcessedBody.Trim().ToUpper().Contains("MOTHER DIED") ||
            preProcessedBody.Trim().ToUpper().Contains("FATHER DIED") ||
            preProcessedBody.Trim().ToUpper().Contains("DIAGNOSED FOR CANCER") ||
            preProcessedBody.Trim().ToUpper().Contains("DIAGNOSED CANCER") ||
            preProcessedBody.Trim().ToUpper().Contains("CHILD DIED") || 
            preProcessedBody.Trim().ToUpper().Contains("SON DIED") ||
            preProcessedBody.Trim().ToUpper().Contains("DIED IN") ||
            preProcessedBody.Trim().ToUpper().Contains("MONTHS TO LIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("MONTH TO LIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("DAYS TO LIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("DAY TO LIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("YEARS TO LIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("YEAR TO LIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("LONG TIME ILLNESS") ||
            preProcessedBody.Trim().ToUpper().Contains("CANCER PATIENT") ||
            preProcessedBody.Trim().ToUpper().Contains("SUFFERING FROM CANCER") ||
            preProcessedBody.Trim().ToUpper().Contains("DIED LEAVING BEHIND") ||
            preProcessedBody.Trim().ToUpper().Contains("WHO DIED A COUPLE") ||
            preProcessedBody.Trim().ToUpper().Contains("WHO DIED A FEW") ||
            preProcessedBody.Trim().ToUpper().Contains("WHO DIED SOME") ||
            (preProcessedBody.Trim().ToUpper().Contains("DIAGNOSED") && preProcessedBody.Trim().ToUpper().Contains("CANCER")) ||
            preProcessedBody.Trim().ToUpper().Contains("DAUGHTER DIED"))
        {
            type = EmailType.DeathOrDying;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("SCAM VICTIM") ||
            preProcessedBody.Trim().ToUpper().Contains("VICTIM OF SCAM") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE BEEN SCAM") ||
            preProcessedBody.Trim().ToUpper().Contains("SENT FEE TO SCAM") ||
            preProcessedBody.Trim().ToUpper().Contains("MONEY TO THOSE SCAM") ||
            preProcessedBody.Trim().ToUpper().Contains("TO SCAM INNOCENT") ||
            preProcessedBody.Trim().ToUpper().Contains("LOST MONEY DURING INTERNATIONAL TRANS") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE BEEN ABLE TO TRACK DOWN OFFICER") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE BE ABLE TO TRACK DOWN OFFICER") ||
            (preProcessedBody.Trim().ToUpper().Contains("YOU HAVE LOST A LOT OF MONEY") && preProcessedBody.Trim().ToUpper().Contains("COMPENSATE YOU")) ||
            (preProcessedBody.Trim().ToUpper().Contains("COMPENSATE YOU") && preProcessedBody.Trim().ToUpper().Contains("SCAM")))
        {
            type = EmailType.ScamVictim;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("OIL AND GAS") ||
            preProcessedBody.Trim().ToUpper().Contains("PETROLEUM COMMODITIES AVAILABLE") ||
            preProcessedBody.Trim().ToUpper().Contains("CRUDE OIL BUSINES") ||
            preProcessedBody.Trim().ToUpper().Contains("GAS AND OIL"))
        {
            type = EmailType.OilAndGas;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("JOB OFFER") || 
            preProcessedBody.Trim().ToUpper().Contains("POSITION IN COMPANY") || 
            preProcessedBody.Trim().ToUpper().Contains("POSITION IN OUR COMPANY") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK FOR ME") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK HERE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR UPDATED RESUME") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR RESUME") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL US YOUR RESUME") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL US YOUR UPDATED RESUME") ||
            preProcessedBody.Trim().ToUpper().Contains("LOOKING ON CONTRACT") ||
            preProcessedBody.Trim().ToUpper().Contains("LOOKING TO CONTRACT") ||
            preProcessedBody.Trim().ToUpper().Contains("PART TIME JOB") ||
            preProcessedBody.Trim().ToUpper().Contains("FULL TIME JOB") ||
            preProcessedBody.Trim().ToUpper().Contains("JOINING OUR TEAM") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERESTED TO WORK IN") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERESTED TO WORK AT") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERESTED TO WORK FOR") ||
            preProcessedBody.Trim().ToUpper().Contains("CRUDE OIL LICENSE OPERATOR") ||
            preProcessedBody.Trim().ToUpper().Contains("ASSIST ME GO INTO INDUSTRIALIZATION") ||
            preProcessedBody.Trim().ToUpper().Contains("VACANT POST IN OUR COMPANY") ||
            preProcessedBody.Trim().ToUpper().Contains("VACANT POST IN MY COMPANY") ||
            preProcessedBody.Trim().ToUpper().Contains("VACANT POST FOR OUR COMPANY") ||
            preProcessedBody.Trim().ToUpper().Contains("VACANT POST FOR MY COMPANY") ||
            preProcessedBody.Trim().ToUpper().Contains("DOESN'T INTERFERE WITH YOUR REGULAR WORK") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK TOGETHER AND SHARE COMMISSION") ||
            (preProcessedBody.Trim().ToUpper().Contains("OUR COMPANY") && preProcessedBody.Trim().ToUpper().Contains("WORK")) ||
            preProcessedBody.Trim().ToUpper().Contains("JOB PLACEMENT"))
        {
            type = EmailType.JobOffer;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("WEB DESIGN") ||
            preProcessedBody.Trim().ToUpper().Contains("GENERATE HIGHER VISITOR TRAFFIC TO YOUR WEBSITE") ||
            preProcessedBody.Trim().ToUpper().Contains("WEBSITE DESIGN") ||
            preProcessedBody.Trim().ToUpper().Contains("WEB SITE DESIGN") ||
            preProcessedBody.Trim().ToUpper().Contains("DEVELOPMENT FIRM"))
        {
            type = EmailType.SellingServices;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("ONLINE MARKETING CONSULT"))
        {
            type = EmailType.OnlineMarketingConsult;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("FINANCIAL ASSISTANCE") || 
            preProcessedBody.Trim().ToUpper().Contains("FINANCIAL HELP") || 
            preProcessedBody.Trim().ToUpper().Contains("LOAN") || 
            preProcessedBody.Trim().ToUpper().Contains("APPLY FOR CASH"))
        {
            type = EmailType.LoanOffer;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("LOTTERY") || 
            preProcessedBody.Trim().ToUpper().Contains("POWER BALL") || 
            preProcessedBody.Trim().ToUpper().Contains("POWERBALL") ||
            preProcessedBody.Trim().ToUpper().Contains("E-MAIL HAS WON") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL HAS WON") ||
            preProcessedBody.Trim().ToUpper().Contains("LOTTO DRAW") ||
            preProcessedBody.Trim().ToUpper().Contains("MILLION LOTTO") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR WINNING PIN") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR E-MAIL HAVE WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR EMAIL HAVE WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR E-MAIL HAS WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR EMAIL HAS WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR E-MAIL ADDRESS HAVE WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR EMAIL ADDRESS HAVE WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR E-MAIL ADDRESS HAS WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR EMAIL ADDRESS HAS WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR E-MAIL ADDERESS HAVE WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR EMAIL ADDERESS HAVE WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR E-MAIL ADDERESS HAS WON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR EMAIL ADDERESS HAS WON") ||
            preProcessedBody.Trim().ToUpper().Contains("CONGRATULATIONS! YOU WON") ||
            preProcessedBody.Trim().ToUpper().Contains("CONGRATULATIONS. YOU WON") ||
            preProcessedBody.Trim().ToUpper().Contains("CONGRATULATIONS, YOU WON") ||
            preProcessedBody.Trim().ToUpper().Contains("GET FREE IPHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("GET A FREE IPHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("COPY OF YOUR WINNING") ||
            (preProcessedBody.Trim().ToUpper().Contains("CONGRATULATIONS") && preProcessedBody.Trim().ToUpper().Contains("PROMO")) ||
            ((preProcessedBody.Trim().ToUpper().Contains("YOU HAVE BEEN CHOSEN") || preProcessedBody.Trim().ToUpper().Contains("YOU HAVE BEEN CHOOSEN")) && (preProcessedBody.Trim().ToUpper().Contains("AWARD") || preProcessedBody.Trim().ToUpper().Contains("PROMO"))) ||
            preProcessedBody.Trim().ToUpper().Contains("WINNER"))
        {
            type = EmailType.Lottery;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("CONSIGNMENT") || 
            preProcessedBody.Trim().ToUpper().Contains("TRUNK BOX") ||
            preProcessedBody.Trim().ToUpper().Contains("PACKAGE BOX") ||
            preProcessedBody.Trim().ToUpper().Contains("PACKAGE DELIVER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PARCEL") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVER YOUR PACKAGE"))
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
        else if (preProcessedBody.Trim().ToUpper().Contains("ATM CARD") || 
            preProcessedBody.Trim().ToUpper().Contains("ATMCARD") || 
            preProcessedBody.Trim().ToUpper().Contains("ATM CREDIT CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("VISA CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("ATM MASTER CREDIT CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("ATM MASTER CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("MASTER CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("ATM VISA CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS A CREDIT CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ATM WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("BANK CHEQUE"))
        {
            type = EmailType.AtmCard;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("INHERITENCE") ||
            preProcessedBody.Trim().ToUpper().Contains("INHERIT"))
        {
            type = EmailType.Inheritance;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("BENEFICIARY") ||
            preProcessedBody.Trim().ToUpper().Contains("ESTATE OF YOUR DECEASED") ||
            preProcessedBody.Trim().ToUpper().Contains("NEXT OF KIN"))
        {
            type = EmailType.Beneficiary;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("KEEP MY MONEY") || 
            preProcessedBody.Trim().ToUpper().Contains("ABANDONED SUM") || 
            preProcessedBody.Trim().ToUpper().Contains("MOVE THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("MOVE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("STORE MY MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("EVACUATE THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("EVACUATE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("KEEP THE MONEY SAFE") || 
            preProcessedBody.Trim().ToUpper().Contains("KEEP THE MONEY SAVE") ||
            preProcessedBody.Trim().ToUpper().Contains("SAFE KEEPING OF THE MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SAFE KEEPING OF MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SAFE KEEPING MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("KEEP THE FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS WAS MOVED") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND WAS MOVED") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS WERE MOVED") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND WERE MOVED") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPENSATION FOR YOUR ASSISTANCE") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF MONEY IN YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF MONEY TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF MONEY YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THE DELIVERY ON MY BEHALF") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THE FUND AND KEEP IT") ||
            preProcessedBody.Trim().ToUpper().Contains("VENTURE WHICH I WILL LIKE TO HANDLE WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("PROPOSAL REGARDING MY FAMILY ESTATE") ||
            preProcessedBody.Trim().ToUpper().Contains("HIGHLY PROFITABLE PROJECT") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOUR ACCOUNT RECEIVE $") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ACCOUNT TO RECEIVE $") ||
            preProcessedBody.Trim().ToUpper().Contains("IS YOUR ACCOUNT ABLE RECEIVE $") ||
            preProcessedBody.Trim().ToUpper().Contains("IS YOUR ACCOUNT ABLE TO RECEIVE $") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK WITH ME AND CLAIM IT") ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT TO MOVE THIS MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("DISTRIBUTE MY FUNDS WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("DISTRIBUTE MY FUND WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("DISTRIBUTE FUNDS WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("DISTRIBUTE FUND WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK WITH YOU IN SECURING THIS FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK WITH YOU IN SECURING THESE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK WITH YOU ON SECURING THIS FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK WITH YOU ON SECURING THESE FUND") ||
            (preProcessedBody.Trim().ToUpper().Contains("MOVE OUT OF THE COUNTRY") && preProcessedBody.Trim().ToUpper().Contains("FUNDS")) ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THE MONEY"))
        {
            type = EmailType.MoneyStorage;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("INVESTOR") || 
            preProcessedBody.Trim().ToUpper().Contains("PROFIT SHARING") || 
            preProcessedBody.Trim().ToUpper().Contains("INVESTMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS DEAL") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS DISCUSSION") ||
            preProcessedBody.Trim().ToUpper().Contains("PROFITABLE PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS TALK") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS CONTRACT") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS OFFER") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERESTED IN A BUSINESS") ||
            preProcessedBody.Trim().ToUpper().Contains("MIDDLEMAN BETWEEN OUR COMPANY") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS PARTNER") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOU WORK WITH ME") ||
            preProcessedBody.Trim().ToUpper().Contains("PARTNER WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("IF WE WORK TOGETHER") ||
            preProcessedBody.Trim().ToUpper().Contains("LUCRATIVE PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("LUCRATIVE/CONFIDENTIAL DEAL") ||
            preProcessedBody.Trim().ToUpper().Contains("LUCRATIVE/CONFIDENTIAL OPPORTUNITY") ||
            preProcessedBody.Trim().ToUpper().Contains("LUCRATIVE/CONFIDENTIAL PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("LUCRATIVE/CONFIDENTIAL BUSINES") ||
            preProcessedBody.Trim().ToUpper().Contains("WE CAN WORK TOGETHER") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE A PROJECT FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE PROJECT FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HAS A PROJECT FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("REGARDING A PROJECT") ||
            preProcessedBody.Trim().ToUpper().Contains("PROPOSAL THAT MIGHT INTEREST") ||
            preProcessedBody.Trim().ToUpper().Contains("PROPOSAL FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSING THIS OPPORTUNITY TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("PICKED YOU FOR HUMANITARIAN GRANT") ||
            preProcessedBody.Trim().ToUpper().Contains("PICKED YOU FOR A HUMANITARIAN GRANT") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS THAT WILL BENEFIT BOTH OF US") ||
            preProcessedBody.Trim().ToUpper().Contains("WE CAN WORK OUT THIS FOR OUR BENEFIT") ||
            preProcessedBody.Trim().ToUpper().Contains("WE CAN WORK THIS OUT FOR OUR BENEFIT") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERESTING DEAL WORTH") ||
            (preProcessedBody.Trim().ToUpper().Contains("PROJECT") && preProcessedBody.Trim().ToUpper().Contains("BENEFIT TO YOU")) ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS THAT COULD BE BROUGHT YOUR WAY"))
        {
            type = EmailType.Investor;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("PAYMENT") || 
            preProcessedBody.Trim().ToUpper().Contains("MONEYGRAM") ||
            preProcessedBody.Trim().ToUpper().Contains("MONEY GRAM") ||
            preProcessedBody.Trim().ToUpper().Contains("WESTERN UNION") || 
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER THE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("HELP ME WITH THE RENEW DUES") ||
            preProcessedBody.Trim().ToUpper().Contains("ASSIST ME WITH THE RENEW DUES"))
        {
            type = EmailType.GenericPayment;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("LIKE TO KNOW YOU MORE") || 
            preProcessedBody.Trim().ToUpper().Contains("GET TO KNOW YOU") || 
            preProcessedBody.Trim().ToUpper().Contains("BUILD TRUST") || 
            preProcessedBody.Trim().ToUpper().Contains("I SEE YOU AS SOMEONE I CAN WORK WITH") || 
            preProcessedBody.Trim().ToUpper().Contains("I WILL TELL YOU MORE ABOUT MYSELF") || 
            preProcessedBody.Trim().ToUpper().Contains("GET TO KNOW EACHOTHER") || 
            preProcessedBody.Trim().ToUpper().Contains("GET TO KNOW EACH OTHER") || 
            preProcessedBody.Trim().ToUpper().Contains("LONGTERM RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("UNTIL YOU RESPOND BACK") ||
            preProcessedBody.Trim().ToUpper().Contains("LONG TERM RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("SEEKING YOUR ASSISTANCE") ||
            preProcessedBody.Trim().ToUpper().Contains("SERIOUS RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("I AM A SINGLE YOUNG LADY") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSSION ABOUT FRIENDSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("MAN WITH GOOD SENSE OF HUMOR") ||
            preProcessedBody.Trim().ToUpper().Contains("FOR LASTING RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("DATING SITE") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN I TRUST YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED YOUR RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE GOOD RELATIONSHIP WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I WILL LIKE TO NO YOU") || //This honestly is the exact text from an email...
            preProcessedBody.Trim().ToUpper().Contains("I WOULD LIKE TO KNOW YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("SINGLE NEVER MARRIED") ||
            preProcessedBody.Trim().ToUpper().Contains("PRESENTLY SINGLE GIRL") ||
            preProcessedBody.Trim().ToUpper().Contains("CURRENTLY SINGLE GIRL") ||
            preProcessedBody.Trim().ToUpper().Contains("PRESENTLY SINGLE WOMAN") ||
            preProcessedBody.Trim().ToUpper().Contains("CURRENTLY SINGLE WOMAN") ||
            preProcessedBody.Trim().ToUpper().Contains("BECOME YOUR GOOD FRIEND") ||
            preProcessedBody.Trim().ToUpper().Contains("BECOME YOU GOOD FRIEND") ||
            preProcessedBody.Trim().ToUpper().Contains("BECOME A GOOD FRIEND") ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT US TO BE FRIENDS") ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT US TO BECOME FRIENDS") ||
            preProcessedBody.Trim().ToUpper().Contains("AWARE OF YOUR BACKGROUND") ||
            preProcessedBody.Trim().ToUpper().Contains("AS LONG AS YOU REMAIN HONEST") ||
            preProcessedBody.Trim().ToUpper().Contains("RELIABLE AND HONEST") ||
            (preProcessedBody.Trim().ToUpper().Contains("TRUST") && preProcessedBody.Trim().ToUpper().Contains("FRIENDSHIP")) ||
            (preProcessedBody.Trim().ToUpper().Contains("I AM WOMAN OF") && preProcessedBody.Trim().ToUpper().Contains("YEARS OLD FROM")) ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT TO MAKE A NEW AND SPECIAL FRIEND"))
        {
            type = EmailType.BuildTrust;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("MANUFACTURER OF LED") ||
            preProcessedBody.Trim().ToUpper().Contains("GOLD FOR SALE") ||
            preProcessedBody.Trim().ToUpper().Contains("ASPIRIN CREAM") ||
            preProcessedBody.Trim().ToUpper().Contains("OUR PRODUCT LINE") ||
            preProcessedBody.Trim().ToUpper().Contains("LED DISPLAY SUPPLIER"))
        {
            type = EmailType.SellingProducts;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("UPDATE TO SECURE YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ACCOUNT HAS BEEN LIMITED") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRME YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("ACCOUNT HAS BEEN CREATED") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPLETE YOUR PURCHASE") ||
            preProcessedBody.Trim().ToUpper().Contains("LEFT THE FOLLOWING ITEM") ||
            preProcessedBody.Trim().ToUpper().Contains("RESTRICTED YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("GOOGLE MANAGEMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("CANCEL YOUR PAYPAL") ||
            preProcessedBody.Trim().ToUpper().Contains("UNAUTHORIZED BY THE ACCOUNT OWNER") ||
            preProcessedBody.Trim().ToUpper().Contains("ISSUE WITH YOUR PAYPAL") ||
            preProcessedBody.Trim().ToUpper().Contains("BLOCKED ACCESS TO YOUR PAYPAL ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("TEMPORARILY LOCKED") ||
            preProcessedBody.Trim().ToUpper().Contains("SOMEONE LOGGED TO YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("SOMEONE ACCESSED TO YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("SOMEONE ACCESSED YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("SOMEONE ACCESS YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("SOMEONE ACCESS TO YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("SPAM ACTIVITIES") ||
            preProcessedBody.Trim().ToUpper().Contains("MAILBOX HAS BEEN PROGRAMMED TO SHUT DOWN") ||
            preProcessedBody.Trim().ToUpper().Contains("FIX MY ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("USERGATE MAIL SERVICE") ||
            preProcessedBody.Trim().ToUpper().Contains("CLICK THE LINK BELOW") ||
            preProcessedBody.Trim().ToUpper().Contains("ACCOUNT STATUS HAS BEEN CHANGED") ||
            preProcessedBody.Trim().ToUpper().Contains("WE DETECTED SOMETHING UNUSUAL") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU CAN SIGN IN ALIBABA") ||
            preProcessedBody.Trim().ToUpper().Contains("WE SUGGEST YOU SIGNIN WITH YOUR EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("WE SUGGEST YOU SIGN IN WITH YOUR EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("WE SUGGEST YOU SIGNIN WITH YOUR E-MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("WE SUGGEST YOU SIGN IN WITH YOUR E-MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("PACKAGE OUT FOR DELIVER") ||
            preProcessedBody.Trim().ToUpper().Contains("VISITED FROM AN UNUSUAL PLACE"))
        {
            type = EmailType.Phishing;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("DONATING") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATION") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATED") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASE OF THE FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("DON ATION") ||
            preProcessedBody.Trim().ToUpper().Contains("EXPECTING TO RECEIVE IS A CASH") ||
            preProcessedBody.Trim().ToUpper().Contains("TO BE COMPENSATED") ||
            preProcessedBody.Trim().ToUpper().Contains("THE TRANSMISSION OF THE FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER TO YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("TO YOUR BANK ACCOUNT") ||
            (preProcessedBody.Trim().ToUpper().Contains("FUND") && preProcessedBody.Trim().ToUpper().Contains("URGENT DELIVERY")) ||
            (preProcessedBody.Trim().ToUpper().Contains("ASSIGNED TO BE DELIVERED") && preProcessedBody.Trim().ToUpper().Contains("$")) ||
            preProcessedBody.Trim().ToUpper().Contains("COMPENSATION FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("CLAIM THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("PROMISE TO PAY THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("PROMISE TO PAY THE SOM") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER OF THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVER YOUR OWN COMPENSATION") ||
            preProcessedBody.Trim().ToUpper().Contains("INCOMING TRANSFER NOTIFICATION") ||
            preProcessedBody.Trim().ToUpper().Contains("KEPT THE CHEQUE WITH THEM") ||
            preProcessedBody.Trim().ToUpper().Contains("KEPT THE CHECK WITH THEM") ||
            preProcessedBody.Trim().ToUpper().Contains("OFFERED YOU WITH $") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSACTION WE WERE PURSING TOGETHER") ||
            preProcessedBody.Trim().ToUpper().Contains("CREDIT YOUR MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PACKAGE WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("ASSIST IN RECEIVING") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU WILL BE RECEIVING THE FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF YOUR CHECK") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF YOUR SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF YOUR MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF YOUR WEALTH") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFERRED TO YOUR OWN PERSONAL ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND THE SUM OF") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOU THE REST OF MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("WITH THE SUM AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("TOTAL SUM OF") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE AN UNCLAIMED FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER OUR CASH") ||
            preProcessedBody.Trim().ToUpper().Contains("STILL WANT YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR CASHIER'S CHECK") ||
            preProcessedBody.Trim().ToUpper().Contains("CLAIM YOUR BANK DRAFT") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTAINS THE SUM OF") ||
            preProcessedBody.Trim().ToUpper().Contains("UNCLAIMED FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("INVOLVING THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("EFFECT THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFERRED THE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPENSATION AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("INSTANT RICH SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("INVEST THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUND WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("UNPAID FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS HAS BEEN ORDERED") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUNDS VALUED") ||
            preProcessedBody.Trim().ToUpper().Contains("REGARDS TO YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATE A HUGE AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS TO YOUR CONTACT") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS TO YOU CONTACT") ||
            preProcessedBody.Trim().ToUpper().Contains("THOSE FUNDS TRANSFERRED") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE A PACKAGE OF $") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASE SOME FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR DELIVERY WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATE WHAT I HAVE TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("MAPPED OUT A COMPENSATION") ||
            preProcessedBody.Trim().ToUpper().Contains("ON YOUR FAVOR A DRAFT WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("SO HE CAN RELEASE YOUR DRAFT TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("ASK HIM TO SEND YOU THE TOTAL") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASED TO YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUND AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUNDS WILL BE PAID") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUNDS TO BE PAID") ||
            preProcessedBody.Trim().ToUpper().Contains("BANK CHECK DRAFT") ||
            (preProcessedBody.Trim().ToUpper().Contains("FUND") && preProcessedBody.Trim().ToUpper().Contains("UNCLAIMED") && preProcessedBody.Trim().ToUpper().Contains("DEPOSITED")) ||
            (preProcessedBody.Trim().ToUpper().Contains("OF THIS MONEY") && preProcessedBody.Trim().ToUpper().Contains("OFFER YOU")) ||
            preProcessedBody.Trim().ToUpper().Contains("I WISH TO BEQUEATH YOU IN SPECIES THIS SU M") || //How can we possibly predict emails with wording/grammer like this?
            (preProcessedBody.Trim().ToUpper().Contains("DONATE $") && preProcessedBody.Trim().ToUpper().Contains("TO YOU")) ||
            preProcessedBody.Trim().ToUpper().Contains("OFFERED YOU $"))
        {
            type = EmailType.FreeMoney;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("EMPLOYMENT")) //If no other hits and the email contains employment assume its a job offer
        {
            type = EmailType.JobOffer;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("I HAVE SPECIAL PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN I ASK YOU A FAVOR") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL I DISCUSS WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("GIVE ME A CALL") ||
            preProcessedBody.Trim().ToUpper().Contains("GIVE ME CALL") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL ME") ||
            preProcessedBody.Trim().ToUpper().Contains("ANY PROBLEM IN LIFE") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR AGE") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW OLD ARE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU DOING") ||
            preProcessedBody.Trim().ToUpper().Contains("DEAR HOW ARE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU, I AM VERY GOOD") ||
            preProcessedBody.Trim().ToUpper().Contains("WHERE ARE YOU FROM") ||
            preProcessedBody.Trim().ToUpper().Contains("CHARITY PROPOSAL FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT ME") ||
            (preProcessedBody.Trim().ToUpper().Contains("SEND AN EMAIL") && preProcessedBody.Trim().ToUpper().Contains("FOR MORE INFO")) ||
            preProcessedBody.Trim().ToUpper().Contains("RANDOMLY SELECTED INDIVID") ||
            preProcessedBody.Trim().ToUpper().Contains("TALK WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("TALK TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I SHALL PROVIDE YOU WITH DETAILS UPON YOUR RESPONSE") ||
            preProcessedBody.Trim().ToUpper().Contains("I SHALL PROVIDE YOU WITH DETAILS ON YOUR RESPONSE") ||
            preProcessedBody.Trim().ToUpper().Contains("I SHALL GIVE YOU DETAILS UPON YOUR RESPONSE") ||
            preProcessedBody.Trim().ToUpper().Contains("I SHALL GIVE YOU DETAILS ON YOUR RESPONSE") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN I DISCUSS WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("PERSONAL DISCUSSION") ||
            preProcessedBody.Trim().ToUpper().Contains("HELP NEEDED") ||
            preProcessedBody.Trim().ToUpper().Contains("HELP ME") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE HELP ME") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE WRITE ME") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU SPEAK ENGLISH") ||
            preProcessedBody.Trim().ToUpper().Contains("HELP POWER PROGRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE MY PREVIOUS EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("PERMISSION TO EMAIL YOU MY PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("IMPORTANT TO DISCUSS WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE A VERY LUCRATIVE DEAL") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIDENTIAL BRIEF") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS A VERY IMPORTANT ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS AN VERY IMPORTANT ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS IMPORTANT ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS AN IMPORTANT ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS A IMPORTANT ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS A ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS AN ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("BEEN TRYING TO REACH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("KNOW IF YOU RECEIVED MY PREVIOUS MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("IMPORTANT INFORMATION FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HEY DEAR") ||
            preProcessedBody.Trim().ToUpper().Contains("HELLO DEAR") ||
            preProcessedBody.Trim().ToUpper().Contains("HI DEAR") ||
            preProcessedBody.Trim().ToUpper().Contains("COCA-COLA AWARD") ||
            preProcessedBody.Trim().ToUpper().Contains("SOMETHING URGENT") ||
            preProcessedBody.Trim().ToUpper().Contains("IMPORTANT I LIKE TO SHARE") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED YOU URGENTLY") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE CONTACT MY SON") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS TO INFORM YOU THAT YOU HAVE BEEN PICKED") ||
            preProcessedBody.Trim().ToUpper().Contains("SOMETHING IMPORTANT FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("SOME THING IMPORTANT FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE GOOD PROFILE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE A GOOD PROFILE") ||
            preProcessedBody.Trim().ToUpper().Contains("ORDERS FROM MR. PRESIDENT") ||
            preProcessedBody.Trim().ToUpper().Contains("NEED TO TALK ITS VERY IMPORTANT") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS PURE BUSINESS") ||
            preProcessedBody.Trim().ToUpper().Contains("HW ARE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("MISS SHARON RIVAS") ||
            preProcessedBody.Trim().ToUpper().Contains("INFO WHY THIS EMAIL IS COMING TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("INFO ON WHY THIS EMAIL IS COMING TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("INFO ON WHY THE EMAIL IS COMING TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("INFO ON WHY THE EMAIL IS COMING FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("VERY IMPORTANT THING TO TELL YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I URGENTLY NEED YOUR ASSISTANCE") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN WE TALK") ||
            (preProcessedBody.Trim().ToUpper().Contains("GOOD MORNING AND HOW ARE YOU") && preProcessedBody.Trim().ToUpper().Contains("MY NAME IS") && (preProcessedBody.Length - currentMessage.SubjectLine.Length) < 100) ||
            (preProcessedBody.Trim().ToUpper().Contains("HI") || preProcessedBody.Trim().ToUpper().Contains("HELLO") || preProcessedBody.Trim().ToUpper().Contains("DEAR FRIEND")) && (preProcessedBody.Length - currentMessage.SubjectLine.Length) <= 10)
        {
            type = EmailType.InformationGathering;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("BOX")) //If no other hits then just look for the word BOX
        {
            type = EmailType.ConsignmentBox;
        }
        else if (((preProcessedBody.Length - currentMessage.SubjectLine.Length) < 40 && (preProcessedBody.Trim().ToUpper().Contains("INLINE IMAGE"))) || 
            preProcessedBody.Trim().ToUpper().Contains("KINDLY OPEN THE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("KINDLY SEE THE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("ATTACHED LETTER FOR DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DETAILS IN THE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("READ THE ATTACHED LETTER") ||
            preProcessedBody.Trim().ToUpper().Contains("READ THE ATTACHED FILE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE THE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("OPEN THE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("VIEW THE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("READ THE ATTACHMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE THE ATTACHMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("OPEN THE ATTACHMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("VIEW THE ATTACHMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("VIEW ATTACHED.") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE ATTACHED FILE") ||
            preProcessedBody.Trim().ToUpper().Contains("READ THE ATTACHED FILE") ||
            preProcessedBody.Trim().ToUpper().Contains("READ ATTACHED FILE") ||
            preProcessedBody.Trim().ToUpper().Contains("LETTER ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("LETTER ATTACHMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("KINDLY READ THE ATTACHED"))
        {
            type = EmailType.BlankWithAttachment;
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
        else if (currentMessage.AtachmentTypes.ToUpper().Contains("PNG") || 
            currentMessage.AtachmentTypes.ToUpper().Contains("JPG") || 
            currentMessage.AtachmentTypes.ToUpper().Contains("JPEG") || 
            currentMessage.AtachmentTypes.ToUpper().Contains("BMP") || 
            currentMessage.AtachmentTypes.ToUpper().Contains("GIF") || 
            currentMessage.AtachmentTypes.ToUpper().Contains("TIFF"))
        {
            attachmentType = "Image";
        }

        //Look to see if we have already sent our ID to them, so that we do not send it again
        for (int i = pastMessages.Count() - 1; i > 0; i--)
        {
            if (pastMessages[i].IncludeID || pastMessages[i].IncludedIDinPast)
            {
                currentMessage.IncludedIDinPast = true;
                break;
            }
        }

        //Try to find their name from the past messages
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
            //Parse the body looking for their name, if a name is not found we generate a random one.
            //TODO Add more name searching methods
            string tmpName = AttemptToFindPersonName(currentMessage.EmailBodyPlain);
            if (!String.IsNullOrEmpty(tmpName))
                name = tmpName;
        }
        if (String.IsNullOrEmpty(name))
        {
            name = GetRandomName(rand);
        }

        //Some of the scammers like to use an invalid email to send the message then instruct you to email them with the email in the message (Maybe to avoid getting the mailbox shutdown) so try to find the email address they want you to send to
        string newEmailAddress = AttemptToFindReplyToEmailAddress(currentMessage.SubjectLine + " " + currentMessage.EmailBodyPlain).Trim();
        if (!String.IsNullOrEmpty(newEmailAddress))
        {
            //Remove trailing "." from email address if it was displayed at the end of a sentence
            while (newEmailAddress.EndsWith("."))
            {
                newEmailAddress = newEmailAddress.Substring(0, newEmailAddress.Length - 1);
            }
            //Remove trailing "_" from email address. Sometimes their messages start and end every line with "_" and not sure why
            while (newEmailAddress.EndsWith("_"))
            {
                newEmailAddress = newEmailAddress.Substring(0, newEmailAddress.Length - 1);
            }
            //Remove trailing "|" from email address. Sometimes they throw random symbols after the email address
            while (newEmailAddress.EndsWith("|"))
            {
                newEmailAddress = newEmailAddress.Substring(0, newEmailAddress.Length - 1);
            }

            if (!currentMessage.ToAddress.Trim().ToUpper().Contains(newEmailAddress.ToUpper()) && !settings.EmailAddress.ToUpper().Contains(newEmailAddress.ToUpper()))
            {
                currentMessage.ToAddress += newEmailAddress + ";";
            }
        }

        //If after all the checks we still do not know who to send it to then search every word to see if any are a valid email address
        if (String.IsNullOrEmpty(currentMessage.ToAddress))
        {
            string[] tempSplit = (currentMessage.SubjectLine + " " + currentMessage.EmailBodyHtml).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int k = 0; k < tempSplit.Count(); k++)
            {
                if (tempSplit[k].Contains("@"))
                {
                    if (IsValidEmail(tempSplit[k]))
                    {
                        currentMessage.ToAddress += tempSplit[k] + ";";
                    }
                }
            }
        }

        currentMessage.PersonName = name;
        greeting = GetRandomGreeting(rand);
        signOff = GetRandomSignOff(rand);

        switch (type)
        {
            case EmailType.Test:
                if (pastMessages.Count() > 0)
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
                    rtnResponse = GetRandomOpeningResponseForBeneficiary(rand, greeting, name, currentMessage);
                break;
            case EmailType.Lottery:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForLottery(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForLottery(rand, greeting, currentMessage);
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
                    rtnResponse = GetRandomContinuedResponseForConsignmentBox(rand, greeting, name, attachmentType, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForConsignmentBox(rand, greeting, name, attachmentType, currentMessage);
                break;
            case EmailType.DeathOrDying:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForDeathOrDying(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForDeathOrDying(rand, greeting, name, currentMessage);
                break;
            case EmailType.LoanOffer:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForLoanOffer(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForLoanOffer(rand, greeting, name, currentMessage);
                break;
            case EmailType.MoneyStorage:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForMoneyStorage(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForMoneyStorage(rand, greeting, name, currentMessage);
                break;
            case EmailType.AtmCard:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForAtmCard(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForAtmCard(rand, greeting, name, currentMessage);
                break;
            case EmailType.Police:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForPolice(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForPolice(rand, greeting, name, currentMessage);
                break;
            case EmailType.GenericPayment:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForGenericPayment(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForGenericPayment(rand, greeting, name, currentMessage);
                break;
            case EmailType.SellingServices:
                rtnResponse = GetRandomResponseForSellingServices(rand, greeting, name, currentMessage);
                break;
            case EmailType.OnlineMarketingConsult:
                rtnResponse = "No";
                break;
            case EmailType.BuildTrust:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForBuildTrust(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForBuildTrust(rand, greeting, name, currentMessage);
                break;
            case EmailType.Investor:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForInvestor(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForInvestor(rand, greeting, name, currentMessage);
                break;
            case EmailType.MoneyHack:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForMoneyHack(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForMoneyHack(rand, greeting, name, currentMessage);
                break;
            case EmailType.JobOffer:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForJobOffer(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForJobOffer(rand, greeting, name, currentMessage);
                break;
            case EmailType.SellingProducts:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForSellingProducts(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForSellingProducts(rand, greeting, name, currentMessage);
                break;
            case EmailType.FreeMoney:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForFreeMoney(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForFreeMoney(rand, greeting, name, currentMessage);
                break;
            case EmailType.InformationGathering:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForInformationGathering(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForInformationGathering(rand, greeting, name, currentMessage);
                break;
            case EmailType.Phishing:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForPhishing(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForPhishing(rand, greeting, name, currentMessage);
                break;
            case EmailType.ScamVictim:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForScamVictims(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForScamVictims(rand, greeting, name, currentMessage);
                break;
        }

        if (!String.IsNullOrEmpty(rtnResponse))
            rtnResponse += Environment.NewLine + Environment.NewLine + signOff + ", " + MyName;

        //TODO Make enabling this a setting
        //Replace synonyms
        rtnResponse = SynonymReplacement(rand, rtnResponse);

        return rtnResponse;
    }
    #endregion
}