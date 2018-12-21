using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows.Forms;

public class MailServerFunctions
{
    private const string settingFileLocation = "Settings.json";
    public const string ProcessFolderName = "AutoProcessedMail";
    public const int Timeout = 50000; //TODO turn this into a setting
    public int LastInboxCount;
    public List<int> InboxCountHistory;
    public string UserName;
    public string Password;
    public string MyName;
    private int messagesSendSinceRandomize;

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
        ScamVictim = 25,
        ForeignLanguage = 26,
        GenericAdvertisement = 27,
        MessageTooLong = 28,
        MessageTooShort = 29
    };

    public MailServerFunctions()
    {
        LastInboxCount = 0;
        messagesSendSinceRandomize = 0;
        InboxCountHistory = new List<int>();
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
            settings.PathToMyFakeID = @"c:\FakeID\PathHereCanBeAnyTypeOfFileAndDoesNotNeedToBeAnIdCanEvenBeACorruptImageFile.png";
            settings.OutgoingMessageIdDomainName = "mail.gmail.com";
            settings.MinutesDelayBeforeAnsweringAnEmail = "240";
            settings.EnableLongMessageTypeReplies = "FALSE";
            settings.EnableShortMessageTypeReplies = "FALSE";
            settings.EnableSendingTransferReceiptsImageFiles = "FALSE";
            settings.LongMessageUpperLimit = 2000;
            settings.ShortMessageLowerLimit = 75;
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
            settings.ConsignmentBoxImageIncluded = new List<string>() { "I have noticed an image file was included. Please include more angles." };
            settings.ConsignmentBoxImageNotIncluded = new List<string>() { "I have noticed no image file was included. Please reply back with a picture." };
            settings.ResponseLongMessageType = new List<string>() { "The message is too long for me to read." };
            settings.ResponseShortMessageType = new List<string>() { "The message is lacking details." };
            settings.PathToTransferReceipts = new List<string>() {  };

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
            settings.QuestionsWeAreCaught = new List<string>() { "I am not sure what you mean." };
            settings.QuestionsProvideDetails = new List<string>() { "What information do you need?" };
            settings.QuestionsAreYouReady = new List<string>() { "I am ready." };
            settings.QuestionsChangeContactMethod = new List<string>() { "I prefer to talk through email." };
            settings.QuestionsContactMeLater = new List<string>() { "I look forward to hearing back from you." };
            settings.QuestionsAreYouOnboard = new List<string>() { "Yes I am." };
            settings.QuestionsPayTheFee = new List<string>() { "How do you want me to pay the fee?", "I don't know how to do |GetPaymentTypeFromMessage| payment." };
            settings.QuestionsTheyConfused = new List<string>() { "Why are you confused?", "Which part are you confused about?" };
            settings.QuestionsHowBigOfLoan = new List<string>() { "Maybe we start with a $|GetRandomNumber10000| loan? How much will my payments be for that?", "I might want to start with a $|GetRandomNumber10000| loan if that works? Do you know what my payments will be?" };
            settings.QuestionsMustPayBefore = new List<string>() { "You want me to do as you ask but you do not do as I ask.", "How can we trust eachother if you do not ever do as I ask?" };
            settings.QuestionsMeetUs = new List<string>() { "I will not be able to meet you today, I am out of town for business to |GetRandomLocation|", "I cannot meet right now since I am out of town." };
            settings.QuestionsFillOutForm = new List<string>() { "If you included the form as an attachment I cannot view it with my mail client. Please include the contents in the body of the email.", "I cannot view attachments, could you copy the attachment into the email?" };
            settings.QuestionsGetBackToUs = new List<string>() { "What should I get back to you about?", "I think I am confused as to what you want me to do." };
            settings.QuestionsNeedBankDetails = new List<string>() { "How do I find my bank details?", "I am still not sure how to find my bank details." };
            settings.QuestionsWhatTypeOfProof = new List<string>() { "I am not sure the best type of proof. Maybe a certificate or a letter from someone?", "You are the expert in this, tell me what is the best type of proof?" };
            settings.QuestionsHowDoYouWantFundsReleased = new List<string>() { "Please pay me with |GetRandomPaymentMethod|." };
            settings.QuestionsWeCantDoThat = new List<string>() { "Explain to me why you can't do that?" };
            settings.QuestionsContactTheBank = new List<string>() { "I tried to contact the bank about this but they are confused.", "Could you send me the bank contact information?" };
            settings.QuestionsAreYouMember = new List<string>() { "I want to be a member." };
            settings.QuestionsDidYouSeeOurMessage = new List<string>() { "I did not see it." };
            settings.QuestionsInvalidAddress = new List<string>() { "My house is new and it does not show up on some places.", "I recently built my house so the address might not showup." };
            settings.QuestionsTellUsWhatTheyAskedYouToDo = new List<string>() { "They asked me to leave when I read them the emails. Maybe I said something wrong?", "They told me to stop wasting their time with all these emails I bring to them. Maybe you need to tell me exactly what to say?", "They told me they would not read the emails from my phone.", "They told me to stop coming in there." };
            settings.QuestionsTellUsAboutYourself = new List<string>() { "|introduction|",  };
            settings.QuestionsAutomatedProgram = new List<string>() { "I am a real person just trying to respond to your emails.", "I am not sure why you think I am not a real person. Don't I reply to your messages?" };
            settings.QuestionsUseWalmartToPay = new List<string>() { "I do not live near a walmart that has those payment options.", "I called the local walmart and they do not offer those payment options." };
            settings.QuestionsHowMuchMoneyDoIHave = new List<string>() { "I have $|GetRandomNumber10000|." };
            settings.QuestionsSendTransferReceipt = new List<string>() { "I have attached the receipt." };

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
            settings.ResponseContinuedForeignLanguage = new List<string>() { "I only speak english.", "Can you resend the email in english?" };
            settings.ResponseContinuedGenericAdvertisement = new List<string>() { "I am very interested. Can you tell me why I would go with you over your competitor?", "Can you tell me more about it?" };

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
            settings.ResponseContinuedForeignLanguage = new List<string>() { "Please send the english version.", "I only speak english." };
            settings.ResponseContinuedGenericAdvertisement = new List<string>() { "I would be happy to hear any additional information you have.", "Interesting, do you have more information?" };

            string json = new JavaScriptSerializer().Serialize(settings);
            File.WriteAllText(settingFileLocation, JsonHelper.FormatJson(json));

            MessageBox.Show("Default settings file has been created. Please modify the settings file before starting the program. The settings file has been filled with example responses, see the readme for the full list of commands that can be put into the settings.", "Please fill in settings");
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
                            LastInboxCount = inbox.Count;
                            InboxCountHistory.Add(LastInboxCount);
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

                                response = HandleMessage(loggerInfo, messages[0], ref storage);
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
                                Thread.Sleep(90000); //Sleep for 90 seconds and wait for the connection issue to clear up
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
    public StandardResponse SendSMTP(LoggerInfo loggerInfo, string toAddress, string subject, string bodyText, bool includeID, bool includePaymentReceipt, string inReplyTo, ref string myMessageId)
    {
        return SendSMTP(loggerInfo, UserName, Password, UserName, UserName, toAddress, toAddress, subject, bodyText, Timeout, includeID, includePaymentReceipt, inReplyTo, ref myMessageId);
    }
    public StandardResponse SendSMTP(LoggerInfo loggerInfo, string username, string password, string fromAddress, string fromAddressReadable, string toAddress, string toAddressReadable, string subject, string bodyText, int timeout, bool includeID, bool includePaymentReceipt, string inReplyTo, ref string myMessageId)
    {
        string hostName = "smtp.gmail.com";
        int port = 465;
        StandardResponse response = new StandardResponse() { Code = 0 };
        SmtpClient client = new SmtpClient();
        MimeMessage message = new MimeMessage();
        Random rand = new Random();

        if (messagesSendSinceRandomize > 25)
        {
            //After sending 25 messages randomize the value give or take 20% so that the program never sends at a consistant rate
            int minutesOut = 0;
            Int32.TryParse(settings.MinutesDelayBeforeAnsweringAnEmail, out minutesOut);
            settings.MinutesDelayBeforeAnsweringAnEmail = rand.Next((int)(minutesOut * 0.8), (int)(minutesOut * 1.2)).ToString();
        }

        try
        {
            if (String.IsNullOrEmpty(fromAddressReadable)) fromAddressReadable = fromAddress;
            if (String.IsNullOrEmpty(toAddressReadable)) toAddressReadable = toAddress;

            if (toAddress.ToUpper().Contains(".COM*"))
            {
                toAddress = toAddress.ToUpper().Replace(".COM*", ".COM");
            }

            string[] toSplit = toAddress.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            //If we are trying to send to more than 10 people there might be a problem.
            if (toSplit.Count() > 15)
            {
                response.Code = -1;
                response.Message = "Email has more than 15 people on the email. Do not send the message.";

                return response;
            }

            message.InReplyTo = inReplyTo;
            string inReplyToPartial = String.Empty;
            if (inReplyTo.Length >= 5)
            {
                inReplyToPartial = ScrubText(inReplyTo.Substring(0, 5));
            }
            message.MessageId = CalculateMD5Hash(DateTime.Now.ToString()) + inReplyToPartial + GetRandomCharacters(rand, 5) + "@" + settings.OutgoingMessageIdDomainName; //Default is to Imitate the gmail message ID
            myMessageId = message.MessageId;

            Logger.WriteDbg(loggerInfo, "Message ID: " + myMessageId);

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

                if (realAddress.Contains('+'))
                {
                    string[] addrSplit = realAddress.Split(new char[] { '+' });
                    foreach (string addrS in addrSplit)
                    {
                        if (addrS.Contains('@'))
                        {
                            realAddress = addrS;
                            break;
                        }
                    }
                }

                if (IsValidEmail(realAddress))
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
            if (includePaymentReceipt)
            {
                string randomPath = GetRandomPaymentReceiptPath(rand);
                if (!String.IsNullOrEmpty(randomPath) || File.Exists(randomPath))
                {
                    MimePart attach = new MimePart();
                    attach.ContentObject = new ContentObject(new MemoryStream(File.ReadAllBytes(randomPath)), ContentEncoding.Default);
                    attach.ContentDisposition = new ContentDisposition(ContentDisposition.Attachment);
                    attach.ContentTransferEncoding = ContentEncoding.Default;
                    attach.FileName = Path.GetFileName(randomPath);

                    messageBodyMultiPart.Add(attach);
                }
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
                    Logger.WriteDbg(loggerInfo, "Sent Message. Subject: " + message.Subject);
                    client.Disconnect(true);
                    Logger.WriteDbg(loggerInfo, "Disconnect");

                    messagesSendSinceRandomize++;

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
            Logger.Write(loggerInfo, "Message Subject: " + message.Subject);
            Logger.Write(loggerInfo, "Message Subject: " + message.MessageId);
        }

        return response;
    }
    public StandardResponse HandleMessage(LoggerInfo loggerInfo, EmailMessage msg, ref List<MailStorage> storage)
    {
        StandardResponse response = new StandardResponse { Code = 0, Message = String.Empty, Data = String.Empty };

        try
        {
            //Ignore No-Reply emails, no reason to process an email that is not monitored or that was a real email sent to our mailbox.
            if (msg.FromAddress.Count() > 0 && !msg.FromAddress[0].ToString().ToUpper().Contains("NO-REPLY@") && 
                !msg.FromAddress[0].ToString().ToUpper().Contains("NOREPLY@") && 
                !msg.FromAddress[0].ToString().ToUpper().Contains("MAILER-DAEMON@") && 
                !msg.FromAddress[0].ToString().ToUpper().Contains("POSTMASTER@") &&
                !msg.FromAddress[0].ToString().ToUpper().Contains("@NIANTICLABS") &&
                !msg.FromAddress[0].ToString().ToUpper().Contains("NOTIFICATIONS@LIVE.NEXT"))
            {
                MailStorage storageObj = new MailStorage();

                storageObj.MsgId = msg.MessageId;
                storageObj.DateReceived = msg.MessageDttm;
                storageObj.DateProcessed = DateTime.Now;
                storageObj.NumberOfAttachments = msg.FileAttachments.Count();
                storageObj.SubjectLine = msg.Subject.Replace("RE: ", "").Replace("FW: ", "").Replace("re: ", "").Replace("fw: ", "").Replace("Re: ", "").Replace("Fw: ", "");
                storageObj.InReplyToMsgId = msg.InReplyTo;

                //Extract the Body information
                foreach (TextPart txtPart in msg.TextParts)
                {
                    string msgType = txtPart.ContentType.MediaType + "/" + txtPart.ContentType.MediaSubtype;
                    if (msgType.Trim().ToLower() == "text/plain")
                    {
                        storageObj.EmailBodyPlain = MakeEmailEasierToRead(txtPart.Text);
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

                //Received extremely large email with nothing but lines like this: 345345345345436636263gmail-m_-
                //We do not want to store that or have to compare against it for performance reasons
                if (storageObj.EmailBodyPlain.Contains("gmail-m_-"))
                {
                    List<string> lines = storageObj.EmailBodyPlain.ToLower().Split(new char[] { '\r' }).ToList();
                    if (lines.Count(t => t.Contains("gmail-m_-")) > 50)
                    {
                        Logger.Write(loggerInfo, "Detected large email with no purpose. Blanking out the email.");
                        storageObj.EmailBodyPlain = String.Empty;
                        storageObj.EmailBodyHtml = String.Empty;
                        storageObj.EmailBodyRich = String.Empty;
                    }
                }

                //If no Plain text was included convert the HTML to plain text
                if (String.IsNullOrEmpty(storageObj.EmailBodyPlain) && !String.IsNullOrEmpty(storageObj.EmailBodyHtml))
                {
                    HtmlConvert convert = new HtmlConvert();

                    try
                    {
                        storageObj.EmailBodyPlain = convert.ConvertHtml(storageObj.EmailBodyHtml);
                    }
                    catch (Exception ex)
                    {
                        Logger.Write(loggerInfo, "Failed to convert to plain text from HTML. HTML Message:" + Environment.NewLine + Environment.NewLine + storageObj.EmailBodyHtml + Environment.NewLine + Environment.NewLine + "Error Message: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace);
                    }
                }

                foreach (var v in msg.FromAddress)
                {
                    if (msg.FromAddress.Count() <= 1 || !v.ToString().Trim().ToLower().Contains(".ocn.ne.jp")) //Often times they include multiple email addresses, the ocn.ne.jp ones tend to get rejected so exclude that email in the reply if it is not the only address
                        storageObj.ToAddress += v.ToString() + ";";
                }
                foreach (var v in msg.ReplyTo)
                {
                    storageObj.ToAddress += v.ToString() + ";";
                }
                foreach (var v in msg.FileAttachments)
                {
                    storageObj.AtachmentTypes += v.FileExtension + ",";
                    storageObj.AttachmentNames += v.FileName + ",";
                }

                //Get list of previous messages in the thread
                List<MailStorage> previousMessagesInThread = GetPreviousMessagesInThread(storage, storageObj);
                
                //Determine response
                storageObj.DeterminedReply = GetResponseForType(loggerInfo, ref storageObj, previousMessagesInThread.OrderBy(t => t.DateReceived).ToList());

                storage.Add(storageObj);
            }
            else
            {
                response.Code = 50;
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

    //Special Types
    #region SpecialTypes
    private string GetRandomOpeningResponseLongMessageType(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseLongMessageType[rand.Next(0, settings.ResponseLongMessageType.Count())], rand);
    }
    private string GetRandomOpeningResponseShortMessageType(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseShortMessageType[rand.Next(0, settings.ResponseShortMessageType.Count())], rand);
    }
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

        return greetings + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningLottery[rand.Next(0, settings.ResponseOpeningLottery.Count())], rand);
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

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningIlluminati[rand.Next(0, settings.ResponseOpeningIlluminati.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForConsignmentBox(Random rand, string greetings, string name, string attachmentType, MailStorage currentMessage)
    {
        string attachmentIncludedText = String.Empty;
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        if (attachmentType == "Image")
        {
            attachmentIncludedText = " " + GetRandomConsignmentBoxImageIncluded(rand);
        }
        else
        {
            attachmentIncludedText = " " + GetRandomConsignmentBoxImageNotIncluded(rand);
        }

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningConsignmentBox[rand.Next(0, settings.ResponseOpeningConsignmentBox.Count())], new List<string> { "|attachmentIncludedText|" }, new List<string> { attachmentIncludedText }, rand);
    }
    private string GetRandomOpeningResponseForDeathOrDying(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningDeathOrDying[rand.Next(0, settings.ResponseOpeningDeathOrDying.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForLoanOffer(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningLoanOffer[rand.Next(0, settings.ResponseOpeningLoanOffer.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForMoneyStorage(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningMoneyStorage[rand.Next(0, settings.ResponseOpeningMoneyStorage.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForAtmCard(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningAtmCard[rand.Next(0, settings.ResponseOpeningAtmCard.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForPolice(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningPolice[rand.Next(0, settings.ResponseOpeningPolice.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForGenericPayment(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningGenericPayment[rand.Next(0, settings.ResponseOpeningGenericPayment.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForInvestor(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningInvestor[rand.Next(0, settings.ResponseOpeningInvestor.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForMoneyHack(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningMoneyHack[rand.Next(0, settings.ResponseOpeningMoneyHack.Count())], new List<string> { }, new List<string> { }, rand);
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

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningJobOffer[rand.Next(0, settings.ResponseOpeningJobOffer.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForSellingProducts(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningSellingProducts[rand.Next(0, settings.ResponseOpeningSellingProducts.Count())], new List<string> { }, new List<string> { }, rand);
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
    private string GetRandomOpeningResponseForForeignLanguage(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningForeignLanguage[rand.Next(0, settings.ResponseOpeningForeignLanguage.Count())], rand);
    }
    private string GetRandomOpeningResponseForGenericAdvertisement(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningGenericAdvertisement[rand.Next(0, settings.ResponseOpeningGenericAdvertisement.Count())], rand);
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

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedLottery[rand.Next(0, settings.ResponseContinuedLottery.Count())], rand);
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

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedIlluminati[rand.Next(0, settings.ResponseContinuedIlluminati.Count())], rand);
    }
    private string GetRandomContinuedResponseForConsignmentBox(Random rand, string greetings, string name, string attachmentType, MailStorage currentMessage, List<MailStorage> pastMessages)
    {
        string attachmentIncludedText = String.Empty;
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        if (attachmentType == "Image")
        {
            attachmentIncludedText = " " + GetRandomConsignmentBoxImageIncluded(rand);
        }
        else
        {
            attachmentIncludedText = " " + GetRandomConsignmentBoxImageNotIncluded(rand);
        }

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedConsignmentBox[rand.Next(0, settings.ResponseContinuedConsignmentBox.Count())], new List<string> { "|attachmentIncludedText|" }, new List<string> { attachmentIncludedText }, rand);
    }
    private string GetRandomContinuedResponseForDeathOrDying(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedDeathOrDying[rand.Next(0, settings.ResponseContinuedDeathOrDying.Count())], rand);
    }
    private string GetRandomContinuedResponseForLoanOffer(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedLoanOffer[rand.Next(0, settings.ResponseContinuedLoanOffer.Count())], rand);
    }
    private string GetRandomContinuedResponseForMoneyStorage(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedMoneyStorage[rand.Next(0, settings.ResponseContinuedMoneyStorage.Count())], rand);
    }
    private string GetRandomContinuedResponseForAtmCard(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedAtmCard[rand.Next(0, settings.ResponseContinuedAtmCard.Count())], rand);
    }
    private string GetRandomContinuedResponseForPolice(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedPolice[rand.Next(0, settings.ResponseContinuedPolice.Count())], rand);
    }
    private string GetRandomContinuedResponseForGenericPayment(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningGenericPayment[rand.Next(0, settings.ResponseOpeningGenericPayment.Count())], rand);
    }
    private string GetRandomContinuedResponseForInvestor(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningInvestor[rand.Next(0, settings.ResponseOpeningInvestor.Count())], rand);
    }
    private string GetRandomContinuedResponseForMoneyHack(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedMoneyHack[rand.Next(0, settings.ResponseContinuedMoneyHack.Count())], rand);
    }
    private string GetRandomContinuedResponseForInheritance(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedInheritance[rand.Next(0, settings.ResponseContinuedInheritance.Count())], rand);
    }
    private string GetRandomContinuedResponseForBeneficiary(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedBeneficiary[rand.Next(0, settings.ResponseContinuedBeneficiary.Count())], rand);
    }
    private string GetRandomContinuedResponseForBuildTrust(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedBuildTrust[rand.Next(0, settings.ResponseContinuedBuildTrust.Count())], rand);
    }
    private string GetRandomContinuedResponseForJobOffer(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedJobOffer[rand.Next(0, settings.ResponseContinuedJobOffer.Count())], rand);
    }
    private string GetRandomContinuedResponseForSellingProducts(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedSellingProducts[rand.Next(0, settings.ResponseContinuedSellingProducts.Count())], rand);
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
    private string GetRandomContinuedResponseForForeignLanguage(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedForeignLanguage[rand.Next(0, settings.ResponseContinuedForeignLanguage.Count())], rand);
    }
    private string GetRandomContinuedResponseForGenericAdvertisement(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        string directResponse = HandleDirectQuestions(MakeEmailEasierToRead(currentMessage.EmailBodyPlain), ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedGenericAdvertisement[rand.Next(0, settings.ResponseContinuedGenericAdvertisement.Count())], rand);
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
    private string GetRandomMemory(Random rand)
    {
        return GetRandomMemory(rand, null);
    }
    private string GetRandomMemory(Random rand, bool? isMale)
    {
        string rtnValue = String.Empty;
        List<string> lst = settings.Memory;

        rtnValue = lst[rand.Next(0, lst.Count())];
        if (isMale != null && isMale == false)
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
        List<int> indexes = new List<int>();
        int indexCtr = 0;
        string rtn = String.Empty;

        if (settings.ResponseOpeningOilAndGasQuestionList.Count() > count)
        {
            while (indexes.Count() < count)
            {
                int tmp = rand.Next(0, settings.ResponseOpeningOilAndGasQuestionList.Count() - 1);

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
                    indexes.Add(tmp);
                    indexCtr++;
                }
            }

            for (int i = 0; i < indexes.Count(); i++)
            {
                rtn += (i + 1).ToString() + ") " + SettingPostProcessing(settings.ResponseOpeningOilAndGasQuestionList[indexes[i]], new List<string> { }, new List<string> { }, rand) + Environment.NewLine;
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
    private string GetRandomConsignmentBoxImageIncluded(Random rand)
    {
        List<string> lst = settings.ConsignmentBoxImageIncluded;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomConsignmentBoxImageNotIncluded(Random rand)
    {
        List<string> lst = settings.ConsignmentBoxImageNotIncluded;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomPaymentReceiptPath(Random rand)
    {
        List<string> lst = settings.PathToTransferReceipts;

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
    private string GetRandomQuestionsWeAreCaught(Random rand)
    {
        List<string> lst = settings.QuestionsWeAreCaught;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsProvideDetails(Random rand)
    {
        List<string> lst = settings.QuestionsProvideDetails;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsAreYouReady(Random rand)
    {
        List<string> lst = settings.QuestionsAreYouReady;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsChangeContactMethod(Random rand)
    {
        List<string> lst = settings.QuestionsChangeContactMethod;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsContactMeLater(Random rand)
    {
        List<string> lst = settings.QuestionsContactMeLater;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsAreYouOnboard(Random rand)
    {
        List<string> lst = settings.QuestionsAreYouOnboard;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsPayTheFee(Random rand)
    {
        List<string> lst = settings.QuestionsPayTheFee;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsTheyConfused(Random rand)
    {
        List<string> lst = settings.QuestionsTheyConfused;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsHowBigOfLoan(Random rand)
    {
        List<string> lst = settings.QuestionsHowBigOfLoan;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsMustPayBefore(Random rand)
    {
        List<string> lst = settings.QuestionsMustPayBefore;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsMeetUs(Random rand)
    {
        List<string> lst = settings.QuestionsMeetUs;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsFillOutForm(Random rand)
    {
        List<string> lst = settings.QuestionsFillOutForm;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsGetBackToUs(Random rand)
    {
        List<string> lst = settings.QuestionsGetBackToUs;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsNeedBankDetails(Random rand)
    {
        List<string> lst = settings.QuestionsNeedBankDetails;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsWhatTypeOfProof(Random rand)
    {
        List<string> lst = settings.QuestionsWhatTypeOfProof;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsHowDoYouWantFundsReleased(Random rand)
    {
        List<string> lst = settings.QuestionsHowDoYouWantFundsReleased;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsWeCantDoThat(Random rand)
    {
        List<string> lst = settings.QuestionsWeCantDoThat;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsContactTheBank(Random rand)
    {
        List<string> lst = settings.QuestionsContactTheBank;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsAreYouMember(Random rand)
    {
        List<string> lst = settings.QuestionsAreYouMember;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsDidYouSeeOurMessage(Random rand)
    {
        List<string> lst = settings.QuestionsDidYouSeeOurMessage;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsInvalidAddress(Random rand)
    {
        List<string> lst = settings.QuestionsInvalidAddress;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsTellUsWhatTheyAskedYouToDo(Random rand)
    {
        List<string> lst = settings.QuestionsTellUsWhatTheyAskedYouToDo;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsTellUsAboutYourself(Random rand)
    {
        List<string> lst = settings.QuestionsTellUsAboutYourself;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsAutomatedProgram(Random rand)
    {
        List<string> lst = settings.QuestionsAutomatedProgram;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsUseWalmartToPay(Random rand)
    {
        List<string> lst = settings.QuestionsUseWalmartToPay;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsHowMuchMoneyDoIHave(Random rand)
    {
        List<string> lst = settings.QuestionsHowMuchMoneyDoIHave;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsSendTransferReceipt(Random rand)
    {
        List<string> lst = settings.QuestionsSendTransferReceipt;

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
        string paymentType = String.Empty;
        if (placeholder.Count() != replacement.Count())
            throw new Exception("Setting Post Process counts do not match");

        //Attempt to find the payment method they are requesting and default to the generic term "transfer" if we cannot find it
        paymentType = AttemptToFindPaymentType(text);
        if (String.IsNullOrEmpty(paymentType))
            paymentType = "transfer";

        placeholder.Add("|introduction|");
        replacement.Add(GetRandomInroduction(rand));
        placeholder.Add("|GetRandomMemory|");
        replacement.Add(GetRandomMemory(rand));
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
        placeholder.Add("|GetRandomNumber10|");
        replacement.Add(GetRandomNumber(rand, 10).ToString());
        placeholder.Add("|GetRandomNumber100|");
        replacement.Add(GetRandomNumber(rand, 100).ToString());
        placeholder.Add("|GetRandomNumber1000|");
        replacement.Add(GetRandomNumber(rand, 1000).ToString());
        placeholder.Add("|GetRandomNumber10000|");
        replacement.Add(GetRandomNumber(rand, 10000).ToString());
        placeholder.Add("|GetRandomNumber100000|");
        replacement.Add(GetRandomNumber(rand, 100000).ToString());
        placeholder.Add("|GetRandomNumber1000000|");
        replacement.Add(GetRandomNumber(rand, 1000000).ToString());
        placeholder.Add("|GetRandomColor|");
        replacement.Add(GetRandomColor(rand));
        placeholder.Add("|GetRandomCharacters1|");
        replacement.Add(GetRandomCharacters(rand, 1));
        placeholder.Add("|GetRandomCharacters2|");
        replacement.Add(GetRandomCharacters(rand, 2));
        placeholder.Add("|GetRandomCharacters3|");
        replacement.Add(GetRandomCharacters(rand, 3));
        placeholder.Add("|GetRandomCharacters4|");
        replacement.Add(GetRandomCharacters(rand, 4));
        placeholder.Add("|GetRandomCharacters5|");
        replacement.Add(GetRandomCharacters(rand, 5));
        placeholder.Add("|GetRandomCharacters10|");
        replacement.Add(GetRandomCharacters(rand, 10));
        placeholder.Add("|GetRandomCharacters25|");
        replacement.Add(GetRandomCharacters(rand, 25));
        placeholder.Add("|GetRandomCharacters50|");
        replacement.Add(GetRandomCharacters(rand, 50));
        placeholder.Add("|GetRandomCharacters100|");
        replacement.Add(GetRandomCharacters(rand, 100));
        placeholder.Add("|GetRandomCharacters1000|");
        replacement.Add(GetRandomCharacters(rand, 1000));
        placeholder.Add("|GetListOfAcquaintance|");
        replacement.Add(GetListOfAcquaintance(rand, 2));
        placeholder.Add("|GetPaymentTypeFromMessage|");
        replacement.Add(paymentType);
        placeholder.Add("|Environment.NewLine|");
        replacement.Add(Environment.NewLine);

        int ctr = 0;
        while (text.Contains("|") && ctr < 25) //Sometimes replace values contain other replace values, keep looping until we have replaces them all
        {
            for (int i = 0; i < placeholder.Count(); i++)
            {
                text = text.Replace(placeholder[i], replacement[i]);
            }
            ctr++;
        }

        return text;
    }
    private string AttemptToFindPersonName(string body)
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
                    lineSplit[i] = lineSplit[i].Replace("{", " ").Replace("}", " ").Replace("]", " ").Replace("]", " ").Replace(",", " ").Replace("<", " ").Replace(">", " ").Replace("^", " ").Replace("(", " ").Replace(")", " ").Replace("\t", " ").Replace("\r", " ").Replace("\n", " ").Replace(" ", " ");

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
    private string AttemptToFindPaymentType(string body)
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
    private string AttemptManualParseOfEmailAddress(string email)
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
    public double GetHoursBetweenSending()
    {
        int tmpOut = 0;
        Int32.TryParse(settings.MinutesDelayBeforeAnsweringAnEmail, out tmpOut);

        return tmpOut / 60.0;
    }
    public bool IsValidEmail(string email)
    {
        try
        {
            email = email.Replace("<", "").Replace(">", "").Replace("\"", " ").Trim();

            if (!email.Contains('@') || !email.Contains('.') || email.Trim().Contains(' ') || email.Trim().Contains('?') ||
                email.Trim().ToUpper() == settings.EmailAddress.Trim().ToUpper() || email.Trim().Contains('/') ||
                email.Trim().Contains('\\') || email.Trim().Contains("..."))
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
    public string RemoveUselessText(string message)
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
        while (message.Contains("  "))
        {
            message = message.Replace("  ", " ");
        }
        if (message.Trim().StartsWith("*") && message.Trim().EndsWith("*"))
        {
            if(message.Length > 2)
                message = message.Substring(1, message.Length - 2);
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
        if (String.IsNullOrEmpty(settings.OutgoingMessageIdDomainName))
        {
            if (!String.IsNullOrEmpty(rtn))
                rtn += Environment.NewLine;

            rtn += "The Outgoing Message ID Domain Name cannot be blank. If you are unsure on what to put here then use googles domain name of: mail.gmail.com";
        }
        if (String.IsNullOrEmpty(settings.MinutesDelayBeforeAnsweringAnEmail))
        {
            if (!String.IsNullOrEmpty(rtn))
                rtn += Environment.NewLine;

            rtn += "You must choose how long to wait before the program replies to an email by filling in MinutesDelayBeforeAnsweringAnEmail. It might be unrealistic if the program instantly replied to every new email sent. That is why we have the delay.";
        }
        int outInt = 0;
        if (Int32.TryParse(settings.MinutesDelayBeforeAnsweringAnEmail, out outInt) == false)
        {
            if (!String.IsNullOrEmpty(rtn))
                rtn += Environment.NewLine;

            rtn += "The MinutesDelayBeforeAnsweringAnEmail must be an integer value.";
        }
        else if (outInt <= 0)
        {
            if (!String.IsNullOrEmpty(rtn))
                rtn += Environment.NewLine;

            rtn += "The MinutesDelayBeforeAnsweringAnEmail must be greater than 0.";
        }
        if (settings.PathToMyFakeID == @"c:\FakeID\PathHere.png")
        {
            if (!String.IsNullOrEmpty(rtn))
                rtn += Environment.NewLine;

            rtn += "The fake ID path in the settings file is the default value.";
        }
        else
        {
            if (!File.Exists(settings.PathToMyFakeID))
                rtn += "The fake ID path in the settings file is not accessible. Please check that the path is correct or grant access to the file. If you do not want to make one then simply take any picture from online open it in notepad and delete half of the characters so that it is unopenable.";
        }
        for (int i = 0; i < settings.PathToTransferReceipts.Count(); i++)
        {
            if (!String.IsNullOrEmpty(settings.PathToTransferReceipts[i].Trim()) && !File.Exists(settings.PathToTransferReceipts[i]))
            {
                if (!String.IsNullOrEmpty(rtn))
                    rtn += Environment.NewLine;

                rtn += "Could not find the PathToTransferReceipts file: " + settings.PathToTransferReceipts[i] + " .";
            }
        }

        return rtn;
    }
    public string ScrubText(string text)
    {
        Regex rgx = new Regex("[^a-zA-Z0-9 -]");
        text = rgx.Replace(text, "");

        return text;
    }
    public string GetRandomCharacters(Random rand, int numberOfChars)
    {
        string rtn = String.Empty;
        string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        char[] charArr = chars.ToCharArray();

        for (int i = 0; i < numberOfChars; i++)
        {
            rtn += charArr[rand.Next(0, charArr.Length - 1)];
        }

        return rtn;
    }
    public int GetRandomNumber(Random rand, int maxNumber)
    {
        return rand.Next(0, maxNumber);
    }
    public string GetRandomColor(Random rand)
    {
        List<string> colors = new List<string>() { "red", "yellow", "blue", "pink", "orange", "purple", "green", "brown", "white", "black", "grey" };

        return colors[rand.Next(0, colors.Count() - 1)];
    }
    public bool IsEnglish(string text)
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
    public bool ParseBooleanSetting(string str)
    {
        bool rtn = false;

        if (str.ToUpper().Trim() == "T" || str.ToUpper().Trim() == "TRUE" || str.ToUpper().Trim() == "Y" || str.ToUpper().Trim() == "YES")
            rtn = true;

        return rtn;
    }
    public string RemoveReplyTextFromMessage(string text) //Purpose of this function is to strip off the reply text sometimes included. This is the text of the email I sent that they are replying to
    {
        string compare = "FROM: " + settings.EmailAddress.ToUpper();
        if (text.ToUpper().Contains(compare))
        {
            int pos = text.IndexOf(compare);
            if(pos > 0)
                text = text.Substring(0, pos);
        }

        compare = "FROM:" + settings.EmailAddress.ToUpper();
        if (text.ToUpper().Contains(compare))
        {
            int pos = text.IndexOf(compare);
            if (pos > 0)
                text = text.Substring(0, pos);
        }

        compare = "AM, " + settings.EmailAddress.ToUpper();
        if (text.ToUpper().Contains(compare))
        {
            int pos = text.IndexOf(compare);
            if (pos > 0)
                text = text.Substring(0, pos);
        }

        compare = "AM," + settings.EmailAddress.ToUpper();
        if (text.ToUpper().Contains(compare))
        {
            int pos = text.IndexOf(compare);
            if (pos > 0)
                text = text.Substring(0, pos);
        }

        compare = "PM, " + settings.EmailAddress.ToUpper();
        if (text.ToUpper().Contains(compare))
        {
            int pos = text.IndexOf(compare);
            if (pos > 0)
                text = text.Substring(0, pos);
        }

        compare = "PM," + settings.EmailAddress.ToUpper();
        if (text.ToUpper().Contains(compare))
        {
            int pos = text.IndexOf(compare);
            if (pos > 0)
                text = text.Substring(0, pos);
        }

        compare = settings.EmailAddress.ToUpper() + " <" + settings.EmailAddress.ToUpper() + ">";
        if (text.ToUpper().Contains(compare))
        {
            int pos = text.IndexOf(compare);
            if (pos > 0)
                text = text.Substring(0, pos);
        }

        compare = settings.EmailAddress.ToUpper() + "<" + settings.EmailAddress.ToUpper() + ">";
        if (text.ToUpper().Contains(compare))
        {
            int pos = text.IndexOf(compare);
            if (pos > 0)
                text = text.Substring(0, pos);
        }

        return text;
    }
    public string CalculateMD5Hash(string input)
    {
        MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }

        return sb.ToString();
    }
    private string PreProcessEmailText(string subjectLine, string emailBody)
    {
        return RemoveUselessText(MakeEmailEasierToRead(RemoveReplyTextFromMessage(subjectLine + " " + emailBody.Replace("\r\n", " ").Replace("'", ""))));
    }
    public List<MailStorage> GetPreviousMessagesInThread(List<MailStorage> storage, MailStorage mail)
    {
        List<MailStorage> previousMessagesInThread = new List<MailStorage>();

        foreach (MailStorage ms in storage)
        {
            if (ms.Ignored) //Dont add ignored messages to stats since it most likely is duplicates
                continue;

            if (ms.MsgId != mail.MsgId) //Skip including the message we are working on
            {
                if (!String.IsNullOrEmpty(ms.MyReplyMsgId) && !String.IsNullOrEmpty(mail.InReplyToMsgId) && ms.MyReplyMsgId == mail.InReplyToMsgId)
                {
                    previousMessagesInThread.Add(ms);
                }
                else if (ms.SubjectLine.Replace("RE:", "").Replace("FW:", "").Trim() == mail.SubjectLine.Replace("RE:", "").Replace("FW:", "").Trim())
                {
                    int foundCount = 0;

                    foreach (string v in mail.ToAddressList)
                    {
                        string emailAddres  =String.Empty;

                        try
                        {
                            System.Net.Mail.MailAddress address = new System.Net.Mail.MailAddress(v.Replace("\"", ""));
                            emailAddres = address.Address;
                        }
                        catch (Exception)
                        {
                            emailAddres = AttemptManualParseOfEmailAddress(v);
                        }

                        if (ms.ToAddress.Contains(emailAddres))
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
        }

        return previousMessagesInThread;
    }
    private string HandleDirectQuestions(string body, ref MailStorage currentMessage, Random rand)
    {
        string response = String.Empty;
        string preProcessedBody = body.Replace("\r\n", " ").Replace("---", "...").Replace("'","");
        bool alreadyRepliedNotAnswering = false;
        bool askedForDetails = false;

        if (preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU DOING") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW YOU DOING") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU OVER THERE") ||
            preProcessedBody.Trim().ToUpper().Contains("HOPE ALL IS WELL WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HOPE YOU ARE VERY FINE TODAY") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU AND YOUR FAMILY TODAY") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT YOU ARE UP TO") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE DOING WONDERFULLY WELL TODAY") ||
            preProcessedBody.Trim().ToUpper().Contains("IM DOING GOOD AND YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("TOP OF THE DAY TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE A NICE DAY BROTHER") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU TODAY"))
        {
            response += GetRandomQuestionsHowAreYou(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("YOUR 419 FORMAT IS TOO MUCH") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR A SCAM BAITER") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP SENDING USELESS MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE VERY STUPID") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE A MAD MAN") ||
            preProcessedBody.Trim().ToUpper().Contains("SEEMS YOU JUST CAME BACK FROM A PSYCHIATRIC") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP CONTACTING ME PLEASE") ||
            preProcessedBody.Trim().ToUpper().Contains("I WILL GET YOU ARREST") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR A SCAM BETER"))
        {
            response += GetRandomQuestionsWeAreCaught(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("INDEED YOU ARE A HUMAN") ||
            preProcessedBody.Trim().ToUpper().Contains("NEED TO KNOW YOU ARE HUMAN") ||
            preProcessedBody.Trim().ToUpper().Contains("QUIT ACTING BOT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE A BOT") ||
            preProcessedBody.Trim().ToUpper().Contains("QUIT ACTING ROBOT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE A ROBOT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU A ROBOT") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU A ROBOT") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU ROBOT") ||
            preProcessedBody.Trim().ToUpper().Contains("AUTOMATED PROGRAM SENDING") ||
            preProcessedBody.Trim().ToUpper().Contains("AUTOMATED REPLY PROGRAM"))
        {
            response += GetRandomQuestionsAutomatedProgram(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("ALL KIND OF STUPID QUESTION") ||
            preProcessedBody.Trim().ToUpper().Contains("ANY KIND OF JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU A CLOWN") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU HERE FOR JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU MAKING A JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU MAKING FUN") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU MAKING JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU PLAYING WITH US") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU SURE YOU ARE MENTALLY") ||
            preProcessedBody.Trim().ToUpper().Contains("BE SERIOUS IF YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("CANNOT AFFORD TO JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("DO NOT CONTACT ME") ||
            preProcessedBody.Trim().ToUpper().Contains("DO NOT WRITE ANY FURTHER") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU TAKE US FOR A FOOL") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU THINK THIS IS JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU THINK YOU ARE FUNNY") ||
            preProcessedBody.Trim().ToUpper().Contains("DONT EMAIL TO ME AGAIN") ||
            preProcessedBody.Trim().ToUpper().Contains("DONT WRITE ME AGAIN") ||
            preProcessedBody.Trim().ToUpper().Contains("FINAL WARNING") ||
            preProcessedBody.Trim().ToUpper().Contains("FUCK YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("FUCK YOUR FAMILY") ||
            preProcessedBody.Trim().ToUpper().Contains("FUK YOUR FAMILY") ||
            preProcessedBody.Trim().ToUpper().Contains("GETTING ANGRY OR ANNOYED") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE GOTTEN SOMEONE THAT KNOWS THE VALUE OF MY MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("HOPE YOU ARE NOT A SERIOUS TYPE") ||
            preProcessedBody.Trim().ToUpper().Contains("HOPE YOU ARE NOT PLAYING GAME") ||
            preProcessedBody.Trim().ToUpper().Contains("I AM NOT HERE TO DO FAKE BUSINES") ||
            preProcessedBody.Trim().ToUpper().Contains("I DO NOT KNOW IF YOU ARE A JOKER") ||
            preProcessedBody.Trim().ToUpper().Contains("I DONT KNOW IF YOU ARE A JOKER") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE NO TIME TO WASTE") ||
            preProcessedBody.Trim().ToUpper().Contains("I PERSONALLY DO NOT HAVE TIME FOR") ||
            preProcessedBody.Trim().ToUpper().Contains("I WILL GET YOU ARREST") ||
            preProcessedBody.Trim().ToUpper().Contains("IF YOU KNOW YOU ARE NOT SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("IF YOU WRITE SUCH EMAIL TO US AGAIN, YOUR ACCOUNT WILL BE SUSPEND") ||
            preProcessedBody.Trim().ToUpper().Contains("IF YOUR SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("IS THIS A JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("IT SEEMS YOU ARE DRUNK") ||
            preProcessedBody.Trim().ToUpper().Contains("IT SEEMS YOURE DRUNK") ||
            preProcessedBody.Trim().ToUpper().Contains("JOKER PLEASE DONT") ||
            preProcessedBody.Trim().ToUpper().Contains("JOKING WITH") ||
            preProcessedBody.Trim().ToUpper().Contains("KIND OF GAME PLAY") ||
            preProcessedBody.Trim().ToUpper().Contains("KIND OF PLAY GAME") ||
            preProcessedBody.Trim().ToUpper().Contains("MAY YOUR FATHER DIE") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT HERE TO BE PLAYING ALL THE TIME") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT HERE TO BE PLAYING GAMES") ||
            preProcessedBody.Trim().ToUpper().Contains("SERIOUS ONES SCALE THROUGH") ||
            preProcessedBody.Trim().ToUpper().Contains("SOUND LIKE A JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP ACTING FUNNY") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP SCAMING YOURSELF") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP SCAMMING YOURSELF") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP SENDING MAILS THAT DO NOT HAVE ANY MEANING") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP SENDING MAILS THAT DO NOT HAVE MEANING") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP SENDING ME AN EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP SENDING ME EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP SENDING THIS NONSENSE") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP THIS YOUR MADNESS SPEECH") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP WRITING NONSENCE") ||
            preProcessedBody.Trim().ToUpper().Contains("TALKING SHIT") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS A SERIOUS TRANSACTION AND NOT A CHILDS PLAY") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS NOT A JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS NOT CHILD PLAY") ||
            preProcessedBody.Trim().ToUpper().Contains("USELEE FOOL FUCK YOU OFF") ||
            preProcessedBody.Trim().ToUpper().Contains("USELESS FOOL FUCK OFF") ||
            preProcessedBody.Trim().ToUpper().Contains("USELESS FOOL FUCK YOU OFF") ||
            preProcessedBody.Trim().ToUpper().Contains("WARN YOU TO STOP CONTACTING ME") ||
            preProcessedBody.Trim().ToUpper().Contains("WASTING MY TIME ANYMORE") ||
            preProcessedBody.Trim().ToUpper().Contains("WE ARE NOT HERE FOR PLAY") ||
            preProcessedBody.Trim().ToUpper().Contains("WE ARE NOT HERE TO PLAY") ||
            preProcessedBody.Trim().ToUpper().Contains("WE ARE NOT JOKING") ||
            preProcessedBody.Trim().ToUpper().Contains("WE ARE NOT PLAYING") ||
            preProcessedBody.Trim().ToUpper().Contains("WILL PUNISH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT EXACTLY IS YOU PROBLEM") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS WRONG WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT YOUR PROBLEM IS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE A DRUG ADDICT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE A FUCHING JOKER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE A FUCKING JOKER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE FUNNY") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE FRUSTRATING") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE NEVER READY OR SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE NOT IN THIS PLANET") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE JUST A JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE MAD MAN") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE NOT SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE SO NOT SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE SERIOUS ABOUT THIS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE VERY STUPID") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU DO NOT LOOK SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE TIME FOR RUBBISH I DONT HAVE YOUR TIME MY TIME IS MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU MUST BE A CRAZY") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU SEEM NOT SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU SEEMS NOT SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU SOUND LIKE A JOKER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU SHOULD STOP VISITING MY BOX") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU TALKS OUT OF SENSE AND POINT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOURE NOT SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ACCOUNT HAS ALREADY BEEN SUSPEND") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ARE NOT A SERIOUS PERSON"))
        {
            response += GetRandomQuestionsJokingAround(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("LETS TALK ON THE PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("LETS TALK OVER TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DONT FORGET TO TEXT ME") ||
            preProcessedBody.Trim().ToUpper().Contains("DON,T FORGET TO TEXT ME") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE IF POSSIBLE CALL ME") ||
            preProcessedBody.Trim().ToUpper().Contains("TEXT ME YOUR NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("TALK NEXT VIA PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("TALK WITH YOU ON THE PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("CONVERSATION WITH YOU ON PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("CONVERSATION WITH YOU ON THE PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR WHATSAPP NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE CALLED") ||
            preProcessedBody.Trim().ToUpper().Contains("TO CALL YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("LETS DEAL ON PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("KINDLY TEXT OR CALL US") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU CAN TEXT ME ON MY PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("COME OVER IN FACE BOOK"))
        {
            response += GetRandomQuestionsChangeContactMethod(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("ARE YOU READY?") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU READY TO WORK") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU READY TO PAY THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("LET ME KNOW YOUR READINESS") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM YOUR READINESS") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR DECISION FOR YOUR MONEY RECEPTION") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU READY TO MAKE THE PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU READY TO RECEIVE YOUR TRANSFER"))
        {
            response += GetRandomQuestionsAreYouReady(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("I WILL WRITE MORE TOMORROW") ||
            preProcessedBody.Trim().ToUpper().Contains("I WILL MESSAGE MORE TOMORROW") ||
            preProcessedBody.Trim().ToUpper().Contains("WILL GET BACK TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I WILL WRITE MORE LATER"))
        {
            response += GetRandomQuestionsContactMeLater(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("ARE YOU HELPING ME") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU WILLING") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU NO LONGER INTERESTED") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOU HANDLE THE DEAL") ||
            preProcessedBody.Trim().ToUpper().Contains("INTEREST TO WORK WITH ME") ||
            preProcessedBody.Trim().ToUpper().Contains("WILLINGNESS TO WORK WITH ME") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU INTERESTED TO RECEIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("NOTIFY US IF YOU ARE NOT INTERESTED") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU REALLY INTERESTED") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE NOT INTERESTED") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN WE DO BUSINES") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU WANT TO PROCEED"))
        {
            response += GetRandomQuestionsAreYouOnboard(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("A DELIVERY FEE OF $") ||
            preProcessedBody.Trim().ToUpper().Contains("ACTIVATION FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("BUY ITUNE") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPLETE THE TAX PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM THE REMAIN FES") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM THE REMAINING FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("FAST ABOUT GETTING THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("FEE YOU HAVE TO PAY") ||
            preProcessedBody.Trim().ToUpper().Contains("GET A ITUNE") ||
            preProcessedBody.Trim().ToUpper().Contains("GET AN ITUNE") ||
            preProcessedBody.Trim().ToUpper().Contains("GET ITUNE") ||
            preProcessedBody.Trim().ToUpper().Contains("GO AHEAD AND MAKE THE PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("GO AHEAD AND PAY THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("GO AHEAD AND SEND THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("GO AND PAY THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("GO AND SEND THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("HAS THE MONEY BEEN SENT") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE TO SEND $") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE TO SEND USD") ||
            preProcessedBody.Trim().ToUpper().Contains("IM WAITING FOR YOUR PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("MAKE THE DELIVERY FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("NEEDED REQUIRED $") ||
            preProcessedBody.Trim().ToUpper().Contains("ONLY THING DELAYING NOW IS THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("PAY FOR THE CHARGES") ||
            preProcessedBody.Trim().ToUpper().Contains("PAY THE DELIVERY FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("PAY THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("PAY THE MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("PURCHASE ITUNE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND IT THROUGH MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND IT THROUGH WESTERN") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME THE MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND REQUESTED FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND SHIPING FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND SHIPPING FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE ACCOUNT OPENING FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE CHARGE TO US") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE CLEARANCE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE DELIVERY FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE FEE SO WE CAN PROCEED") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE NEEDED FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE REQUESTED FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE REQUIRED FEE") |
            preProcessedBody.Trim().ToUpper().Contains("SEND THE SHIPING FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE SHIPPING FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THIS FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND TO MAKE THE TOTAL OF THE AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND TO US THE COURIER SERVICE CHARGE") ||
            preProcessedBody.Trim().ToUpper().Contains("SENDING THE MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SENT THE CLEARANCE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("TAKE CARE OF THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("THE COURIER FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("WE ONLY NEED THE FEE FROM YOU NOW") ||
            preProcessedBody.Trim().ToUpper().Contains("WHEN ARE YOU MAKING THE PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("WHEN YOU ARE SENDING THE REQUIRED AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE ADVISED THE REQUESTED FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE TO SEND THE FEE NOW") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE SENDING THE NEEDED AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU CAN MAKE USE OF MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE TO PAY FOR OWNERSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU NEED TO SEND HIM SOME MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU NEED TO SEND THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU SEND THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU TO SEND THE $") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU TO SEND THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU TO SEND THE MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU TO SEND THE US") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU WILL SEND THE FEE THROUGH") ||
            (preProcessedBody.Trim().ToUpper().Contains("JUST MAKE THE") && preProcessedBody.Trim().ToUpper().Contains("DELIVERY PAYMENT")))
        {
            response += GetRandomQuestionsPayTheFee(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("ARE YOU TALKING ABOUT") ||
            preProcessedBody.Trim().ToUpper().Contains("I AM CONFUSED") ||
            preProcessedBody.Trim().ToUpper().Contains("BUT CAN NOT COMPREHEND THE CONTENT") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT ARE YOU SAYING") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT DO YOU MEAN") ||
            preProcessedBody.Trim().ToUpper().Contains("I DID NOT UNDERSTAND YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I DID NOT UNDERSTAND WHAT YOU MEAN") ||
            preProcessedBody.Trim().ToUpper().Contains("I DID NOT UNDERSTAND WHAT YOU ARE") ||
            preProcessedBody.Trim().ToUpper().Contains("I DONT UNDERSTAND YOUR") ||
            preProcessedBody.Trim().ToUpper().Contains("I DONT UNDERSTAND WHAT YOU MEAN") ||
            preProcessedBody.Trim().ToUpper().Contains("I DONT UNDERSTAND WHAT YOU ARE") ||
            preProcessedBody.Trim().ToUpper().Contains("I DO NOT UNDERSTAND YOUR") ||
            preProcessedBody.Trim().ToUpper().Contains("I DO NOT UNDERSTAND WHAT YOU MEAN") ||
            preProcessedBody.Trim().ToUpper().Contains("I DO NOT UNDERSTAND WHAT YOU ARE") ||
            preProcessedBody.Trim().ToUpper().Contains("I REALLY DO NOT UNDERSTAND YOUR") ||
            preProcessedBody.Trim().ToUpper().Contains("I REALLY DO NOT UNDERSTAND WHAT YOU MEAN") ||
            preProcessedBody.Trim().ToUpper().Contains("I REALLY DO NOT UNDERSTAND WHAT YOU ARE") ||
            preProcessedBody.Trim().ToUpper().Contains("I CAN NOT UNDERSTAND YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT ARE YOU SAYING") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT ARE YOU JUST SAYING") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT ARE YOU TALKING ABOUT") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS GOING ON") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW DO YOU MEAN?") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT EVEN A WORD I CAN PICK") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOU EXPLAIN WHAT THIS MESSAGE MEANS") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOU EXPLAIN WHAT THIS MEANS") ||
            preProcessedBody.Trim().ToUpper().Contains("?????") ||
            preProcessedBody.Trim().ToUpper().Contains("I DONT UNDERSTAND WHAT YOU ARE SAYING"))
        {
            response += GetRandomQuestionsTheyConfused(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("AS SOON AS YOU MAKE THE PAYMENT WE WILL SEND YOU THE PICTURE") ||
            preProcessedBody.Trim().ToUpper().Contains("AS SOON AS YOU MAKE THE PAYMENT WE WILL SEND THE PICTURE") ||
            preProcessedBody.Trim().ToUpper().Contains("WE WILL SEND THE PICTURE WHEN THE PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("WE WILL SEND THE PICTURE WHEN YOU MAKE THE PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("THE PICTURE WILL BE SENT WHEN YOU MAKE THE PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU NEED TO PROVIDE THE INFORMATION FIRST") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU MUST PROVIDE THE INFORMATION FIRST") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU NEED TO PROVIDE FULL DETAILS FIRST") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU MUST PROVIDE FULL DETAILS FIRST") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU NEED TO PROVIDE THE DETAILS FIRST") ||
            preProcessedBody.Trim().ToUpper().Contains("ONCE YOU SEND THE FEE WE WILL DELIVER IT") ||
            preProcessedBody.Trim().ToUpper().Contains("WITHOUT SEND THE FEE YOU CANNOT") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT ALLOWED NOW TO SHOW YOU THE IMAGE AT THIS MOMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT ALLOWED TO SHOW YOU THE IMAGE AT THIS MOMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR PERSONAL INFORMATION FIRST BEFORE") ||
            preProcessedBody.Trim().ToUpper().Contains("AFTER YOUR PAYMENT IS CONFIRMED") ||
            (preProcessedBody.Trim().ToUpper().Contains("IF YOU DONT WANT TO SEND THE") && preProcessedBody.Trim().ToUpper().Contains("USD")) ||
            (preProcessedBody.Trim().ToUpper().Contains("IF YOU DONT WANT TO SEND THE") && preProcessedBody.Trim().ToUpper().Contains("FEE")) ||
            (preProcessedBody.Trim().ToUpper().Contains("IF YOU DONT WANT TO SEND THE") && preProcessedBody.Trim().ToUpper().Contains("PAYMENT")) ||
            preProcessedBody.Trim().ToUpper().Contains("YOU MUST PROVIDE THE DETAILS FIRST"))
        {
            response += GetRandomQuestionsMustPayBefore(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("POSSIBLE THAT YOU COME MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("WOULD YOU BE ABLE TO COME MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("COULD YOU MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("WOULD YOU BE WILLING TO COME MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("WOULD YOU HAVE TIME TO COME MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("POSSIBLE THAT YOU MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("WOULD YOU BE ABLE TO MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("WOULD YOU BE WILLING TO MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("APPEARING HERE IN PERSON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE TO COME DOWN TO NIGERIA") ||
            preProcessedBody.Trim().ToUpper().Contains("COME DOWN HERE IN BENIN REPUBLIC") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU WANT TO COME TO MY HOUSE") ||
            preProcessedBody.Trim().ToUpper().Contains("WANT TO COME TO DUBAI") ||
            preProcessedBody.Trim().ToUpper().Contains("WHEN ARE YOU COMING") ||
            preProcessedBody.Trim().ToUpper().Contains("INTEND TO MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOUR FLIGHT DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("WOULD YOU HAVE TIME TO MEET US"))
        {
            response += GetRandomQuestionsMeetUs(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("I HAVE ATTACHED A FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("DOCUMENT AND RETURN BACK TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("FILL OUT THE ATTACHED FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("FILL OUT THE INCLUDED FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("FILL OUT THE ATTACHED DOCUMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("HERE IS THE APPLICATION FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE INCLUDED THE APPLICATION FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE THE ATTACHED APPLICATION FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("THE APPLICATION FORM IS ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("FILL IN THE APPLICATION FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("FILL IN THE DOCUMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("FILL THE LOAN APPLICATION FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("ENDORSE ATTACHED COPY") ||
            preProcessedBody.Trim().ToUpper().Contains("SIGN THE FORM AS REQUESTED") ||
            preProcessedBody.Trim().ToUpper().Contains("IT IS REQUESTED OF YOU TO FILL OUT AND SIGN") ||
            preProcessedBody.Trim().ToUpper().Contains("THE APPLICATION FORM IS INCLUDED"))
        {
            response += GetRandomQuestionsFillOutForm(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("GET BACK TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("WAITING TO HEAR FROM YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("UPDATE ME BACK") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL ME BACK") ||
            preProcessedBody.Trim().ToUpper().Contains("NOTIFY US AS SOON AS YOU HEAR FROM"))
        {
            response += GetRandomQuestionsGetBackToUs(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("YOUR BANK DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR BANKING DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR BANK INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR BANKING INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FULL BANK DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FULL BANKING DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR BANK ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR BANK ACCOUNT"))
        {
            response += GetRandomQuestionsNeedBankDetails(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF DO YOU NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF DO YOU WANT") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF DO YOU SEEK") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF DO YOU ASK") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF DID YOU NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF DID YOU WANT") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF DID YOU SEEK") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF DID YOU ASK") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT PROOF DO YOU ASK OF") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT KIND OF PROOF DO YOU ASK") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT KIND OF PROOF DO YOU NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT KIND OF PROOF DO YOU WANT") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT KIND OF PROOF DO YOU SEEK") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT KIND OF PROOF?") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT CAN OF PROOF DID YOU WANT") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT MORE CONVICTION DO YOU NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF?"))
        {
            response += GetRandomQuestionsWhatTypeOfProof(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("HOW DO YOU WANT YOUR FUNDS TO BE RELEASED") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW DO YOU WANT US TO RELEASE THE FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW YOU WANT YOUR FUNDS PRESENTED") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW YOU WISH TO HAVE YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("OPTION YOU WISH TO RECEIVE THE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW DO WE SEND THE FUNDS TO YOU") ||
            preProcessedBody.Replace(" ","").Trim().ToUpper().Contains("ATMCARD,BANKTOBANKWIRETRANSFERORDIP") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW DO YOU WANT THE FUNDS RELEASED"))
        {
            response += GetRandomQuestionsHowDoYouWantFundsReleased(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("WE CANNOT DO THAT") ||
            preProcessedBody.Trim().ToUpper().Contains("WE CANT DO THAT") ||
            preProcessedBody.Trim().ToUpper().Contains("WE ARE UNABLE TO DO THAT") ||
            preProcessedBody.Trim().ToUpper().Contains("THAT IS NOT SOMETHING WE CAN DO") ||
            preProcessedBody.Trim().ToUpper().Contains("WE DONT TAKE PICTURES OF CUSTOMERS LUGGAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("IT IS NOT POSSIBLE FOR ME") ||
            preProcessedBody.Trim().ToUpper().Contains("ITS NOT POSSIBLE FOR ME") ||
            preProcessedBody.Trim().ToUpper().Contains("THE LOAN CANNOT BE PROCESSED") ||
            preProcessedBody.Trim().ToUpper().Contains("WE DONT TAKE PICTURES OF CUSTOMERS LUGGAGE"))
        {
            response += GetRandomQuestionsWeCantDoThat(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("CONTACT THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACTED THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("MESSAGE THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("MESSAGED THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT WITH THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS THE BANK EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("ABLE TO REACH THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("GO TO YOUR BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("TAKE IT TO YOUR BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("TALK TO THE BANK"))
        {
            response += GetRandomQuestionsContactTheBank(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("ARE YOU A MEMBER OF OUR") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU MEMBER OF OUR") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU PART OF OUR CLUB") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU PART OF OUR GROUP") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU HAVE MEMBERSHIP"))
        {
            response += GetRandomQuestionsAreYouMember(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("DID YOU GET OUR LAST MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET OUR MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU SEE OUR LAST MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU SEE OUR MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE OUR MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE OUR LAST MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECIEVE OUR MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECIEVE OUR LAST MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET OUR LAST TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET OUR TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU SEE OUR LAST TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU SEE OUR TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE OUR TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE OUR LAST TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECIEVE OUR TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECIEVE OUR LAST TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET OUR LAST PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET OUR PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU SEE OUR LAST PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU SEE OUR PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE OUR PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE OUR LAST PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECIEVE OUR PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECIEVE OUR LAST PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("I HOPE YOU SAW OUR MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("I HOPE YOU SAW OUR MASSAGE"))
        {
            response += GetRandomQuestionsDidYouSeeOurMessage(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("NOT THE RIGHT ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("INVALID ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("FAKE ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT REAL ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("PRETEND ADDRESS"))
        {
            response += GetRandomQuestionsInvalidAddress(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("WHAT DID THEY TELL YOU TO DO") ||
            preProcessedBody.Trim().ToUpper().Contains("ALL MESSAGE THAT YOU WILL RECEIVE FROM THEM, YOU MUST FORWARD TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("ALWAYS FORWARD IT TO ME TO MAKE SURE THAT YOU ARE PROTECTED") ||
            preProcessedBody.Trim().ToUpper().Contains("TELL US WHAT THEY TOLD YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("TELL US WHAT THEY SAID TO DO") ||
            preProcessedBody.Trim().ToUpper().Contains("LET ME KNOW WHAT THEY ASKED YOU TO DO") ||
            preProcessedBody.Trim().ToUpper().Contains("WHATEVER THEY SEND YOU FORWARD IT TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT EVER THEY SEND YOU FORWARD IT TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHATEVER THEY SENT YOU FORWARD IT TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT EVER THEY SENT YOU FORWARD IT TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT THE BANK ASK YOU TO DO") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT THE BANK NEEDS YOU TO DO") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT THE BANK HAS YOU DO") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT WAS THE RESPONSE FROM THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT THE BANK ASKS YOU TO DO"))
        {
            response += GetRandomQuestionsTellUsWhatTheyAskedYouToDo(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("USE WALMART TO PAY") ||
            preProcessedBody.Trim().ToUpper().Contains("WALMART TO SEND THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU CAN DO THIS AT WALMART") ||
            preProcessedBody.Trim().ToUpper().Contains("WALMART TO MAKE PAY") ||
            preProcessedBody.Trim().ToUpper().Contains("WALMART TO DO PAY") ||
            preProcessedBody.Trim().ToUpper().Contains("WALMART TO HANDLE") ||
            preProcessedBody.Trim().ToUpper().Contains("WALMART TO TAKE CARE OF") ||
            preProcessedBody.Trim().ToUpper().Contains("WALMART WITH THIS PAY") ||
            preProcessedBody.Trim().ToUpper().Contains("USE WALMART TO WALMART") ||
            preProcessedBody.Trim().ToUpper().Contains("VIA WALMART TO WALMART") ||
            preProcessedBody.Trim().ToUpper().Contains("WALMART STORE CLOSE") ||
            preProcessedBody.Trim().ToUpper().Contains("FEE AT WALMART"))
        {
            response += GetRandomQuestionsUseWalmartToPay(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("HOW MUCH MONEY DO YOU HAVE") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW MUCH FUNDS DO YOU HAVE") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW MANY FUNDS DO YOU HAVE") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW MUCH DO YOU HAVE RIGHT NOW") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW MANY MONEY DO YOU HAVE"))
        {
            response += GetRandomQuestionsHowMuchMoneyDoIHave(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("SEND THE TRANSFER RECIEPT") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE TRANSFER RECEIPT") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE THE TRANSFER RECIEPT") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE THE TRANSFER RECEIPT") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND TRANSFER RECIEPT") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND TRANSFER RECEIPT") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE TRANSFER RECIEPT") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE TRANSFER RECEIPT"))
        {
            currentMessage.IncludePaymentReceipt = true;
            response += GetRandomQuestionsSendTransferReceipt(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("NO PICK UP WHEN") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU NOT PICK UP") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU WAS NOT PICK UP YOU CALL WHY") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE NOT REACHABLE BY PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("COULD NOT GET YOU ON PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("COULD NOT GET YOU ON THE PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("TRIED TO CALL YOU NO RESPONSE") ||
            preProcessedBody.Trim().ToUpper().Contains("TRIED CALLING YOU BUT UNSUCCESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NUMBER IS NOT A WORKING") ||
            preProcessedBody.Trim().ToUpper().Contains("TRIED CALLING YOU ON THE PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE TRIED CALLING YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE CALL") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PHONE NUMBER IS ANSWER VOICE CALL ONLY") ||
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
                    preProcessedBody.Trim().ToUpper().Contains("DID NOT PICKUP") ||
                    preProcessedBody.Trim().ToUpper().Contains("DIDNT PICKUP") ||
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
                preProcessedBody.Trim().ToUpper().Contains("DIDT RESPOND TO MY MAIL") ||
                preProcessedBody.Trim().ToUpper().Contains("DIDNT RESPOND TO MY MAIL") ||
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
            preProcessedBody.Trim().ToUpper().Contains("NOT UNDERSTAND WHAT YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFUSED BY MY") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT ARE YOU REALLY SAYING") ||
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
            preProcessedBody.Trim().ToUpper().Contains("I NEED FROM YOU IS YOUR TRUST") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED FROM YOU IS YOU TRUST") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED TO HAVE YOUR TRUST") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED TO KNOW I CAN TRUST") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN BE TRUSTED"))
        {
            response += GetRandomQuestionsTrust(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR LEGAL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR LEGAL FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR FIRST AND LAST NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOU NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOUR NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("STATING YOUR NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("STATING YOUR FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("STATING YOUR LEGAL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("STATING YOUR LEGAL FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("STATING YOUR FIRST AND LAST NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME.....") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME .....") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME___") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME----") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME====") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME :") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME:") ||
            preProcessedBody.Trim().ToUpper().Contains("NAMES.....") ||
            preProcessedBody.Trim().ToUpper().Contains("NAMES .....") ||
            preProcessedBody.Trim().ToUpper().Contains("NAMES___") ||
            preProcessedBody.Trim().ToUpper().Contains("NAMES----") ||
            preProcessedBody.Trim().ToUpper().Contains("NAMES====") ||
            preProcessedBody.Trim().ToUpper().Contains("NAMES :") ||
            preProcessedBody.Trim().ToUpper().Contains("NAMES:") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME INFULL.....") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME INFULL .....") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT NAME DO YOU GO BY") ||
            preProcessedBody.Trim().ToUpper().Contains("WHO ARE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("RECONFIRM YOUR FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHO DO YOU REALLY ARE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NAME AND FIRST NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("1. FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("[FULL NAMES]") ||
            preProcessedBody.Trim().ToUpper().Contains("[FULL NAME]") ||
            preProcessedBody.Trim().ToUpper().Contains("FULL NAMES") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NAME AND ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR COMPLETE NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPLETE FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME\r\n") ||
            preProcessedBody.Replace(" ", "").ToUpper().Contains("NAME,ADDRESS,EMAIL,TELEPHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FULL NAME."))
        {
            askedForDetails = true;
            response += GetRandomQuestionsName(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("2:YOUR CURRENT RESIDENCE ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("WHERE DO YOU LIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("WHERE ARE YOU") || 
            preProcessedBody.Trim().ToUpper().Contains("PROVIDE YOUR ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("WHERE CAN I SEND") ||
            preProcessedBody.Trim().ToUpper().Contains("MAILING ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("POSTAL ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR EXACT ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR HOME AND OFFICE ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE YOUR ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("BILLING ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("RESIDENTIAL ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("HOME ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS.....") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS .....") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS___") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS----") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS====") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS :") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS:") ||
            preProcessedBody.Trim().ToUpper().Contains("OFFICE ADDRESS.") ||
            preProcessedBody.Trim().ToUpper().Contains(", ADDRESS,") ||
            preProcessedBody.Trim().ToUpper().Contains(", MAILING ADDRESS,") ||
            preProcessedBody.Trim().ToUpper().Contains(", POSTAL ADDRESS,") ||
            preProcessedBody.Trim().ToUpper().Contains(", FULL ADDRESS,") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS WHERE YOU WANT HIM TO SEND THE") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS WHERE YOU WANT THEM TO SEND THE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR FULL ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR DELIVERY INFORMATION") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOU DELIVERY INFORMATION") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR DELIVERY DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOU DELIVERY DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM YOUR RECEIVING ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM YOUR DELIVERY ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM WITH YOU DELIVERY INFORMATION") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM WITH YOUR DELIVERY INFORMATION") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM WITH YOU DELIVERY ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM WITH YOUR DELIVERY ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("RECONFIRM TO US DHL YOU FULL INFORMATION") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR COMPLETE ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR FULL INFORMATION TO AVOID WRONG DELIVER") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR OFFICE/RESIDENTIAL ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS AND PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER AND ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("PROVIDE THE RESIDENTIAL") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM US YOUR FULL RESIDENTIAL") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM YOUR HOME ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NAME AND ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("FULL DELIVERY ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS AND OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS AND YOUR OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("DIRECT CONTACT ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("WHICH CITY ARE YOU IN") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE YOUR ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR MAILING ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("LOCATION\r\n") ||
            preProcessedBody.Replace(" ", "").ToUpper().Contains("NAME,ADDRESS,EMAIL,TELEPHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("GIVE ME YOUR ADDRESS"))
        {
            askedForDetails = true;
            response += GetRandomQuestionsAddress(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("COUNTRY OF ORIGIN") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY.....") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY .....") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY :") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY:") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY___") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY----") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY====") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY NAME.....") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY NAME .....") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY NAME :") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY NAME:") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY NAME___") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY NAME----") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY NAME====") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY/CITY.....") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY/CITY .....") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY/CITY :") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY/CITY:") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY/CITY___") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY/CITY----") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY/CITY====") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR COUNTRY.") ||
            preProcessedBody.Trim().ToUpper().Contains(", COUNTRY,") ||
            preProcessedBody.Trim().ToUpper().Contains(", COUNTRY NAME,") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY DO YOU COME FROM") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY, OCCUPATION, AGE AND TELEPHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY AND AGE") ||
            preProcessedBody.Trim().ToUpper().Contains("TELL ME WHERE YOU FROM") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR COUNTRY OF ORIGIN") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY ARE YOU FROM") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT COUNTRY DO YOU LIVE"))
        {
            askedForDetails = true;
            response += GetRandomQuestionsCountry(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("YOUR OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION.....") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION .....") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION :") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION:") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION___") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION----") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION====") ||
            preProcessedBody.Trim().ToUpper().Contains("POSITION.....") ||
            preProcessedBody.Trim().ToUpper().Contains("POSITION .....") ||
            preProcessedBody.Trim().ToUpper().Contains("POSITION :") ||
            preProcessedBody.Trim().ToUpper().Contains("POSITION:") ||
            preProcessedBody.Trim().ToUpper().Contains("POSITION___") ||
            preProcessedBody.Trim().ToUpper().Contains("POSITION----") ||
            preProcessedBody.Trim().ToUpper().Contains("POSITION====") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR OCCUPATION.") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR JOB.") ||
            preProcessedBody.Trim().ToUpper().Contains("JOB___") ||
            preProcessedBody.Trim().ToUpper().Contains("JOB----") ||
            preProcessedBody.Trim().ToUpper().Contains("JOB====") ||
            preProcessedBody.Trim().ToUpper().Contains("JOB.....") ||
            preProcessedBody.Trim().ToUpper().Contains("JOB :") ||
            preProcessedBody.Trim().ToUpper().Contains("JOB:") ||
            preProcessedBody.Trim().ToUpper().Contains(", OCCUPATION,") ||
            preProcessedBody.Trim().ToUpper().Contains(", JOB,") ||
            preProcessedBody.Trim().ToUpper().Contains(", JOB TITLE,") ||
            preProcessedBody.Trim().ToUpper().Contains("WHERE DO YOU WORK") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR AGE & OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("[OCCUPATION]") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY, OCCUPATION, AGE AND TELEPHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE NUMBER AND OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS AND OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS AND YOUR OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PROFESSION") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("JOB\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR JOB"))
        {
            askedForDetails = true;
            response += GetRandomQuestionsOccupation(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("YOUR GENDER") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX.....") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX .....") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX :") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX:") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX___") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX----") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX====") ||
            preProcessedBody.Trim().ToUpper().Contains("GENDER.....") ||
            preProcessedBody.Trim().ToUpper().Contains("GENDER .....") ||
            preProcessedBody.Trim().ToUpper().Contains("GENDER :") ||
            preProcessedBody.Trim().ToUpper().Contains("GENDER:") ||
            preProcessedBody.Trim().ToUpper().Contains("GENDER___") ||
            preProcessedBody.Trim().ToUpper().Contains("GENDER----") ||
            preProcessedBody.Trim().ToUpper().Contains("GENDER====") ||
            preProcessedBody.Trim().ToUpper().Contains(", SEX,") ||
            preProcessedBody.Trim().ToUpper().Contains(", GENDER,") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE/SEX/MARITAL STATUS") ||
            preProcessedBody.Trim().ToUpper().Contains("[SEX]") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR SEX"))
        {
            askedForDetails = true;
            response += GetRandomQuestionsGender(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("YOUR MARITAL STATUS") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITAL STATUS.....") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITAL STATUS .....") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITAL STATUS___") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITAL STATUS----") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITAL STATUS====") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITAL STATUS :") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITAL STATUS:") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITALSTATUS.....") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITALSTATUS .....") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITALSTATUS___") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITALSTATUS----") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITALSTATUS====") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITALSTATUS :") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITALSTATUS:") ||
            preProcessedBody.Trim().ToUpper().Contains(", MARITAL STATUS,") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE/SEX/MARITAL STATUS") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU SINGLE"))
        {
            askedForDetails = true;
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
            preProcessedBody.Trim().ToUpper().Contains("AGE .....") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE :") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE:") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE___") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE----") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE====") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTHDATE.....") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTHDATE .....") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTHDATE :") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTHDATE:") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTHDATE___") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTHDATE----") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTHDATE====") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH DATE.....") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH DATE .....") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH DATE :") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH DATE:") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH DATE___") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH DATE----") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH DATE====") ||
            preProcessedBody.Trim().ToUpper().Contains("DATE OF BIRTH.....") ||
            preProcessedBody.Trim().ToUpper().Contains("DATE OF BIRTH .....") ||
            preProcessedBody.Trim().ToUpper().Contains("DATE OF BIRTH :") ||
            preProcessedBody.Trim().ToUpper().Contains("DATE OF BIRTH:") ||
            preProcessedBody.Trim().ToUpper().Contains("DATE OF BIRTH___") ||
            preProcessedBody.Trim().ToUpper().Contains("DATE OF BIRTH----") ||
            preProcessedBody.Trim().ToUpper().Contains("DATE OF BIRTH====") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR AGE.") ||
            preProcessedBody.Trim().ToUpper().Contains(", AGE,") ||
            preProcessedBody.Trim().ToUpper().Contains(", BIRTHDATE,") ||
            preProcessedBody.Trim().ToUpper().Contains(", BIRTH DATE,") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE/SEX/MARITAL STATUS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR AGE & OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY AND AGE") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY, OCCUPATION, AGE AND TELEPHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("[AGE]") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU AGE"))
        {
            askedForDetails = true;
            response += GetRandomQuestionsBirthdate(rand) + " ";
        }
        if (preProcessedBody.Replace(" ", "").ToUpper().Contains("NAME,ADDRESS,EMAIL,TELEPHONE") ||
            preProcessedBody.Trim().ToUpper().Contains(", PHONE NUMBER,") ||
            preProcessedBody.Trim().ToUpper().Contains(", PHONE,") ||
            preProcessedBody.Trim().ToUpper().Contains(", TELEPHONE NUMBER,") ||
            preProcessedBody.Trim().ToUpper().Contains(", TELEPHONE,") ||
            preProcessedBody.Trim().ToUpper().Contains("1:YOUR TELEPHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS AND PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("CELL PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY, OCCUPATION, AGE AND TELEPHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("DIRECT PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("DROP YOUR TELEPHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("DROP YOUR TELL, WOULD LIKE TO CALL") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT NUMBER DID YOU SAY THAT I SHOULD MESSAGE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT NUMBER DID YOU SAID THAT I SHOULD MESSAGE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HOME PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW CAN I CALL YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW CAN I REACH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED YOUR NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE YOUR NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE YOUR PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("MOBILE PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("NEED YOUR CELL PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("NEED YOUR DIRECT PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("NEED YOUR PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER AND ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE NUMBER AND OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("PRIVATE PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOU TELEPHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOUR TELEPHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NUMBER\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NUMBERS\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE/FAX NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT NUMBER TO") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT YOUR PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU MOBILE TELEPHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR CELL NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR CELL PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR CURRENT PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR DIRECT CELL PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR MOBILE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR MOBILE/CELL PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NUMBER.") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PERMANENT TELEPHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PRIVATE PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR TELEPHONE AND FAX NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR TELEPHONE NO.") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR TELEPHONE NUMBER.") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR TELEPHONE.") ||
            preProcessedBody.Trim().ToUpper().Contains("[TEL]") ||
            preProcessedBody.Trim().ToUpper().Contains("TEL.....") ||
            preProcessedBody.Trim().ToUpper().Contains("TEL .....") ||
            preProcessedBody.Trim().ToUpper().Contains("TEL___") ||
            preProcessedBody.Trim().ToUpper().Contains("TEL----") ||
            preProcessedBody.Trim().ToUpper().Contains("TEL====") ||
            preProcessedBody.Trim().ToUpper().Contains("TEL :") ||
            preProcessedBody.Trim().ToUpper().Contains("TEL:") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE.....") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE .....") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE___") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE----") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE====") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE :") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE:") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NO.....") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NO .....") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NO___") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NO----") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NO====") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NO :") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NO:") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE.....") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE .....") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE :") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE:") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE___") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE----") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE====") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER.....") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER .....") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER___") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER----") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER====") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER :") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER:") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL NO.....") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL NO .....") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL NO___") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL NO----") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL NO====") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL NO :") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL NO:"))
        {
            askedForDetails = true;
            response += GetRandomQuestionsPhoneNumber(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("HOW MUCH LOAN DO YOU NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW MUCH DO YOU NEED AS A LOAN") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW LARGE OF A LOAN DO YOU NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW LARGE OF A LOAN DO YOU SEEK") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW LARGE OF A LOAN DO YOU REQUEST") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF FUNDING YOU ARE LOOKING FOR") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF FUNDING YOU NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF FUNDING YOU ARE SEEKING") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF LOAN") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT REQUESTED:") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT REQUESTED :") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT REQUESTED.....") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT REQUESTED______") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT REQUESTED-----") ||
            preProcessedBody.Trim().ToUpper().Contains("LOAN AMOUNT:") ||
            preProcessedBody.Trim().ToUpper().Contains("LOAN AMOUNT :") ||
            preProcessedBody.Trim().ToUpper().Contains("LOAN AMOUNT.....") ||
            preProcessedBody.Trim().ToUpper().Contains("LOAN AMOUNT______") ||
            preProcessedBody.Trim().ToUpper().Contains("LOAN AMOUNT-----") ||
            preProcessedBody.Trim().ToUpper().Contains("LOAN AMOUNT NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("LOAN AMOUNT REQUEST") ||
            preProcessedBody.Trim().ToUpper().Contains("LET US KNOW THE AMOUNT YOU NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW MUCH LOAN YOU ARE IN NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("NOTE THE AMOUNT YOU WANT TO BORROW") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW MUCH OF A LOAN DO YOU NEED"))
        {
            response += GetRandomQuestionsHowBigOfLoan(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("SEND YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ALONG YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ID") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("ATTACH YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("ATTACH THE ID") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE THE ID") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ID OR PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR IDENTITY OR PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR IDENTIFICATION OR PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("UPLOAD YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("UPLOAD THE ID") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT OR ID CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITY CARD OR INTERNATIONAL PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("COPY OF YOUR I.D") ||
            preProcessedBody.Trim().ToUpper().Contains("COPY OF YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("COPY OF YOUR WORK ID") ||
            preProcessedBody.Trim().ToUpper().Contains("COPY OF YOUR PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains(" ID COPY") ||
            preProcessedBody.Trim().ToUpper().Contains("WITH OUT IDENTIFICATION") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD.....") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD .....") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD___") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD----") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD====") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD :") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD:") ||
            preProcessedBody.Trim().ToUpper().Contains("I.D CARD.....") ||
            preProcessedBody.Trim().ToUpper().Contains("I.D CARD .....") ||
            preProcessedBody.Trim().ToUpper().Contains("I.D CARD___") ||
            preProcessedBody.Trim().ToUpper().Contains("I.D CARD----") ||
            preProcessedBody.Trim().ToUpper().Contains("I.D CARD====") ||
            preProcessedBody.Trim().ToUpper().Contains("I.D CARD :") ||
            preProcessedBody.Trim().ToUpper().Contains("I.D CARD:") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITY CARD.....") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITY CARD .....") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITY CARD___") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITY CARD----") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITY CARD====") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITY CARD :") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITY CARD:") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE.....") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE .....") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE___") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE----") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE====") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE :") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE:") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT.....") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT .....") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT___") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT----") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT====") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT :") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT:") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITYCARD.....") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITYCARD .....") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITYCARD___") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITYCARD----") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITYCARD====") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITYCARD :") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITYCARD:") ||
            preProcessedBody.Trim().ToUpper().Contains(", ID,") ||
            preProcessedBody.Trim().ToUpper().Contains(", ID CARD,") ||
            preProcessedBody.Trim().ToUpper().Contains(", LICENSE,") ||
            preProcessedBody.Trim().ToUpper().Contains(", LICENSE CARD,") ||
            preProcessedBody.Trim().ToUpper().Contains("ANY FORM OF IDENTIFICATION") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR INTL.PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ID BY ATTACHMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR IDENTIFICATION") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR IDENTITY COPY") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERNATIONAL PASSPORT OR DRIVER LICENSE") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERNATIONAL PASSPORT OR DRIVERS LICENSE") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERNATIONAL APSSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTIFICATION DOCUMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("NO IDENTIFICATION SENT") ||
            preProcessedBody.Trim().ToUpper().Contains("WITH OUT INDETIFICATION") ||
            preProcessedBody.Trim().ToUpper().Contains("WITHOUT INDETIFICATION") ||
            preProcessedBody.Trim().ToUpper().Contains("WITH OUT ID CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("WITHOUT ID CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("WITH OUT I.D CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("WITHOUT I.D CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("PERSONAL ID CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("SCAN YOUR ID CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME ANY OF YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME ANY OF YOUR I.D") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME ANY OF YOUR IDENTITY") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME ANY OF YOUR IDENTIFICATION") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME ANY OF YOUR DRIVER") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVED YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVED YOUR I.D") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVED YOUR PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVED YOUR DRIVER") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVE YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVE YOUR I.D") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVE YOUR PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVE YOUR DRIVER") ||
            preProcessedBody.Trim().ToUpper().Contains("DRIVING ID CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("DRIVERS LICENSES") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERNATIONAL PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("VALID IDENTIFICATION") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL ME YOUR ID"))
        {
            askedForDetails = true;
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
        if (preProcessedBody.Trim().ToUpper().Contains("INCLUDE THIS CODE:") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE THIS CODE :") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE THE FOLLOWING CODE:") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE THE FOLLOWING CODE :") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE THIS FOLLOWING CODE:") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE THIS FOLLOWING CODE :") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THIS CODE :") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THIS CODE:") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE CODE:") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE CODE :") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE FOLLOWING CODE:") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE FOLLOWING CODE :"))
        {
            string[] split = preProcessedBody.ToUpper().Split(new char[] { ' ' });
            string code = String.Empty;
            bool codeNext = false;

            foreach (string s in split)
            {
                if (s == "CODE:" || s == "CODE")
                {
                    codeNext = true;
                    continue;
                }
                if (codeNext)
                {
                    if (s.Trim() == String.Empty || s.Trim().Length < 3)
                    {
                        continue;
                    }
                    else
                    {
                        code = s;
                        break;
                    }
                }
            }

            if (String.IsNullOrEmpty(code.Trim()))
            {
                //Generate a randome code since we couldn't parse it out. They probably will just ignore the fact that it is the wrong code.
                code = CalculateMD5Hash(DateTime.Now.ToString()).Substring(0, rand.Next(4, 10));
            }

            response += Environment.NewLine + "Here is the code: " + code + Environment.NewLine;
        }
        if (!askedForDetails &&
            (preProcessedBody.Trim().ToUpper().Contains("PROVIDE THE REQUIRED DETAILS") ||
            preProcessedBody.Trim().ToUpper().Contains("PROVIDE ALL THIS DETAILS") ||
            preProcessedBody.Trim().ToUpper().Contains("PROVIDE THE INFORMATION NEEDED") ||
            preProcessedBody.Trim().ToUpper().Contains("FORWARD ME YOUR FULL DATA") ||
            preProcessedBody.Trim().ToUpper().Contains("WITHOUT YOUR DETAILS") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR FULL INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR FULL INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOUR FULL INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOU FULL DETAILS") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR FULL DETAILS") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOUR INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOUR FULL INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE REQUIRED INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME THE CONTACT INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR PERSONAL INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR FULL DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR FULL INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("RECONFIRM YOUR DATAS") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED THOSE INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("I ASK YOU FOR YOUR INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("DETAILS NEEDED FROM YOU BEFORE WE CAN PROCEED") ||
            preProcessedBody.Trim().ToUpper().Contains("WAITING FOR YOUR INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU FULL INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR DETAILS INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("FORWARD THE DETAILS") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM YOUR DATAS") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND FULL INFORMATION")))
        {
            response += GetRandomQuestionsProvideDetails(rand) + " ";
        }
        response = response.Trim();
        if (!String.IsNullOrEmpty(response))
        {
            response = Environment.NewLine + Environment.NewLine + SettingPostProcessing(response, rand).Trim() + Environment.NewLine + Environment.NewLine;
        }

        return response;
    }
    public string GetResponseForType(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages)
    {
        Random rand = new Random();
        EmailType type = EmailType.Unknown;
        string rtnResponse = String.Empty;
        string attachmentType = "File";
        string name = String.Empty;
        string greeting = String.Empty;
        string signOff = String.Empty;
        string preProcessedBody = PreProcessEmailText(currentMessage.SubjectLine, currentMessage.EmailBodyPlain);
        //string preProcessedBody = currentMessage.SubjectLine + " " + currentMessage.EmailBodyPlain.Replace("\r\n", " ").Replace("'","");
        //preProcessedBody = RemoveUselessText(MakeEmailEasierToRead(RemoveReplyTextFromMessage(preProcessedBody)));

        bool foundSame = false;
        if (preProcessedBody.Length > 200) //Only check for duplacte long emails since some of the shorter emails could be the same between different email threads. Like "What do you mean?" as a reply to many different situations
        {
            DateTime determinedDateCutoff = currentMessage.DateReceived.AddDays(-2.0);

            foreach (MailStorage ms in pastMessages)
            {
                //string tmpPastMsg = RemoveUselessText(MakeEmailEasierToRead(RemoveReplyTextFromMessage(ms.SubjectLine + " " + ms.EmailBodyPlain.Replace("\r\n", " ").Replace("'", ""))));
                string tmpPastMsg = PreProcessEmailText(ms.SubjectLine, ms.EmailBodyPlain);

                //Only check recent messages for duplicates, if they resend the same email later we can reply to it again
                if (ms.DateReceived >= determinedDateCutoff)
                {
                    if (tmpPastMsg != preProcessedBody)
                    {
                        if (preProcessedBody.Length > 0 && tmpPastMsg.Length > 0)
                        {
                            int sizeDifference = Math.Abs(tmpPastMsg.Length - preProcessedBody.Length);
                            double percentChange = (double)sizeDifference / preProcessedBody.Length;
                            if ((percentChange * 100) < 20) //If we have less than a 20% change in size move to the next check
                            {
                                //I am going to take 3 word chunks and search for each set of 3 word pairs to attempt to see if the email is mostly the same
                                string[] words = preProcessedBody.Split(new char[] { ' ' });

                                int foundSuccessCount = 0;
                                int foundFailCount = 0;
                                for (int i = 0; i < words.Length - 2; i++)
                                {
                                    string tmpTri = words[i] + ' ' + words[i + 1] + ' ' + words[i + 2];
                                    if (tmpPastMsg.Contains(tmpTri))
                                    {
                                        foundSuccessCount++;
                                    }
                                    else
                                    {
                                        foundFailCount++;
                                    }

                                    if (foundFailCount > 20 && foundSuccessCount == 0)
                                    {
                                        break; //Early break if we find nothing but fails to start the compare to save on performance
                                    }
                                }

                                //If the amount of 3 word pairs successfully found is less than 90% return a fail
                                double triFoundPercent = (double)foundSuccessCount / (foundSuccessCount + foundFailCount);
                                if ((triFoundPercent * 100) > 90)
                                {
                                    foundSame = true;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        foundSame = true;
                        break;
                    }
                }
            }
        }

        if (foundSame)
        {
            currentMessage.Ignored = true;
            currentMessage.Replied = true;
            currentMessage.IsDuplicateMessage = true;

            Logger.Write(loggerInfo, "Message determined to be duplicate. Message Subject: " + currentMessage.SubjectLine + ", Message ID: " + currentMessage.MsgId);

            return String.Empty;
        }

        //Types of emails
        if (ParseBooleanSetting(settings.EnableLongMessageTypeReplies) && preProcessedBody.Length > settings.LongMessageUpperLimit)
        {
            type = EmailType.MessageTooLong;
        }
        else if (ParseBooleanSetting(settings.EnableShortMessageTypeReplies) && preProcessedBody.Length - RemoveUselessText(MakeEmailEasierToRead(currentMessage.SubjectLine)).Length < settings.ShortMessageLowerLimit)
        {
            type = EmailType.MessageTooShort;
        }
        else if (currentMessage.SubjectLine.Contains("Test ") || currentMessage.SubjectLine.Contains(" Test"))
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
        else if (preProcessedBody.Trim().ToUpper().Contains("BEFORE I DIE I HAVE AN IMPORTANT") ||
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
            preProcessedBody.Trim().ToUpper().Contains("FATHER DIED") ||
            preProcessedBody.Trim().ToUpper().Contains("HUSBAND DIED") ||
            preProcessedBody.Trim().ToUpper().Contains("FATHER DEATH") ||
            preProcessedBody.Trim().ToUpper().Contains("HUSBAND DEATH") ||
            preProcessedBody.Trim().ToUpper().Contains("LONG TIME ILLNESS") ||
            preProcessedBody.Trim().ToUpper().Contains("MONTH TO LIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("MONTHS TO LIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("MOTHER DIED") ||
            preProcessedBody.Trim().ToUpper().Contains("MY CANCER") ||
            preProcessedBody.Trim().ToUpper().Contains("SON DIED") ||
            preProcessedBody.Trim().ToUpper().Contains("SUFFERING FROM CANCER") ||
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
            type = EmailType.DeathOrDying;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("SCAM VICTIM") ||
            preProcessedBody.Trim().ToUpper().Contains("VICTIM OF SCAM") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE BEEN SCAM") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW EXACTLY WERE YOU SCAMMED") ||
            preProcessedBody.Trim().ToUpper().Contains("SENT FEE TO SCAM") ||
            preProcessedBody.Trim().ToUpper().Contains("MONEY TO THOSE SCAM") ||
            preProcessedBody.Trim().ToUpper().Contains("MONEY THAT YOU LOST TO SCAM") ||
            preProcessedBody.Trim().ToUpper().Contains("TO SCAM INNOCENT") ||
            preProcessedBody.Trim().ToUpper().Contains(".TO .SCAMMER") ||
            preProcessedBody.Trim().ToUpper().Contains("LOST MONEY TO SCAMMER") ||
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
            preProcessedBody.Trim().ToUpper().Contains("CRUDE OIL PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("CRUDE OIL SALES") ||
            preProcessedBody.Trim().ToUpper().Contains("GAS AND OIL"))
        {
            type = EmailType.OilAndGas;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("ASSIST ME GO INTO INDUSTRIALIZATION") ||
            preProcessedBody.Trim().ToUpper().Contains("CRUDE OIL LICENSE OPERATOR") ||
            preProcessedBody.Trim().ToUpper().Contains("DOESNT INTERFERE WITH YOUR REGULAR WORK") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL US YOUR RESUME") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL US YOUR UPDATED RESUME") ||
            preProcessedBody.Trim().ToUpper().Contains("FULL TIME JOB") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERESTED TO WORK AT") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERESTED TO WORK FOR") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERESTED TO WORK IN") ||
            preProcessedBody.Trim().ToUpper().Contains("JOB OFFER") ||
            preProcessedBody.Trim().ToUpper().Contains("JOB PLACEMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("JOINING OUR TEAM") ||
            preProcessedBody.Trim().ToUpper().Contains("LOOKING ON CONTRACT") ||
            preProcessedBody.Trim().ToUpper().Contains("LOOKING TO CONTRACT") ||
            preProcessedBody.Trim().ToUpper().Contains("PART TIME JOB") ||
            preProcessedBody.Trim().ToUpper().Contains("POSITION IN COMPANY") ||
            preProcessedBody.Trim().ToUpper().Contains("POSITION IN OUR COMPANY") ||
            preProcessedBody.Trim().ToUpper().Contains("SEEKING A BROAD VARIETY OF INDIVIDUALS") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR RESUME") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR UPDATED RESUME") ||
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
            type = EmailType.JobOffer;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("WEB DESIGN") ||
            preProcessedBody.Trim().ToUpper().Contains("GENERATE HIGHER VISITOR TRAFFIC TO YOUR WEBSITE") ||
            preProcessedBody.Trim().ToUpper().Contains("WEBSITE DESIGN") ||
            preProcessedBody.Trim().ToUpper().Contains("WEB SITE DESIGN") ||
            preProcessedBody.Trim().ToUpper().Contains("WANT YOUR CREDIT SCORE INCREASED") ||
            preProcessedBody.Trim().ToUpper().Contains("I SELL GOOD TOOLS") ||
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
            preProcessedBody.Trim().ToUpper().Contains("FINANCIAL PACKAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("PROJECT FINANCING") ||
            preProcessedBody.Trim().ToUpper().Contains("CONCERNING FUNDING OF YOUR BUSINESS PROJECT") ||
            preProcessedBody.Trim().ToUpper().Contains("CREDIT OFFER") ||
            preProcessedBody.Trim().ToUpper().Contains("LOW INTEREST RATE") ||
            preProcessedBody.Trim().ToUpper().Contains("LOAN") ||
            preProcessedBody.Trim().ToUpper().Contains("L0AN") ||
            preProcessedBody.Trim().ToUpper().Contains("APPLY FOR CASH"))
        {
            type = EmailType.LoanOffer;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("CONGRATULATIONS! YOU WON") ||
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
            type = EmailType.Lottery;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("POLICE") ||
            preProcessedBody.Trim().ToUpper().Contains("CONVICTED TERRORIST") ||
            preProcessedBody.Trim().ToUpper().Contains("ENFORCEMENT OFFICER") ||
            preProcessedBody.Trim().ToUpper().Contains(" FBI ") ||
            preProcessedBody.Trim().ToUpper().Contains("WANTED TERRORIST"))
        {
            type = EmailType.Police;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("ATM BLANK CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("BUYING THE SAME PRODUCT FOR"))
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
        else if (preProcessedBody.Trim().ToUpper().Contains("ABANDONED SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF MONEY IN YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF MONEY TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF MONEY YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("ASSIST ME TO RECEIVE AND INVEST THIS") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOUR ACCOUNT RECEIVE $") ||
            preProcessedBody.Trim().ToUpper().Contains("CANNOT BE ABLE TO MOVE THIS HUGE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPENSATION FOR YOUR ASSISTANCE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISTRIBUTE FUND WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("DISTRIBUTE FUNDS WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("DISTRIBUTE MY FUND WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("DISTRIBUTE MY FUNDS WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("EVACUATE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("EVACUATE THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND WAS MOVED") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND WERE MOVED") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS WAS MOVED") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS WERE MOVED") ||
            preProcessedBody.Trim().ToUpper().Contains("HIGHLY PROFITABLE PROJECT") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEEDED A TRUSTED PARTNER") ||
            preProcessedBody.Trim().ToUpper().Contains("I RATHER SEND IT TO SOMEONE I DONT KNOW ON A MUTUAL AGREEMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT TO MOVE THIS MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("IS YOUR ACCOUNT ABLE RECEIVE $") ||
            preProcessedBody.Trim().ToUpper().Contains("IS YOUR ACCOUNT ABLE TO RECEIVE $") ||
            preProcessedBody.Trim().ToUpper().Contains("KEEP MY MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("KEEP THE FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("KEEP THE MONEY SAFE") ||
            preProcessedBody.Trim().ToUpper().Contains("KEEP THE MONEY SAVE") ||
            preProcessedBody.Trim().ToUpper().Contains("MOVE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("MOVE THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("PROPOSAL REGARDING MY FAMILY ESTATE") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THE DELIVERY ON MY BEHALF") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THE FUND AND KEEP IT") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THE MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SAFE KEEPING MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SAFE KEEPING OF MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SAFE KEEPING OF THE MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("STORE MY MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("VENTURE THAT WILL BENEFIT BOTH PART") ||
            preProcessedBody.Trim().ToUpper().Contains("VENTURE WHICH I WILL LIKE TO HANDLE WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK WITH ME AND CLAIM IT") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK WITH YOU IN SECURING THESE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK WITH YOU IN SECURING THIS FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK WITH YOU ON SECURING THESE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK WITH YOU ON SECURING THIS FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ACCOUNT TO RECEIVE $") ||
            (preProcessedBody.Trim().ToUpper().Contains("MOVE OUT OF THE COUNTRY") && preProcessedBody.Trim().ToUpper().Contains("FUNDS")))
        {
            type = EmailType.MoneyStorage;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("BUSINESS CONTRACT") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS COOPERATION") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS DEAL") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS DISCUSSION") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS JOINT PARTNER") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS OFFER") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS PARTNER") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS TALK") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS THAT COULD BE BROUGHT YOUR WAY") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS THAT WILL BENEFIT BOTH OF US") ||
            preProcessedBody.Trim().ToUpper().Contains("BUSINESS TO DISCUSS WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOU WORK WITH ME") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIDENTIAL DEAL") ||
            preProcessedBody.Trim().ToUpper().Contains("HAS A PROJECT FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE A PROJECT FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE PROJECT FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("IF WE WORK TOGETHER") ||
            preProcessedBody.Trim().ToUpper().Contains("IMPORTANT PARTNERSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERESTED IN A BUSINESS") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERESTING DEAL WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("INVESTMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("INVESTOR") ||
            preProcessedBody.Trim().ToUpper().Contains("LIKE TO KNOW IF YOU CAN BE OUR DISTRIBUTOR") ||
            preProcessedBody.Trim().ToUpper().Contains("LUCRATIVE PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("LUCRATIVE/CONFIDENTIAL BUSINES") ||
            preProcessedBody.Trim().ToUpper().Contains("LUCRATIVE/CONFIDENTIAL DEAL") ||
            preProcessedBody.Trim().ToUpper().Contains("LUCRATIVE/CONFIDENTIAL OPPORTUNITY") ||
            preProcessedBody.Trim().ToUpper().Contains("LUCRATIVE/CONFIDENTIAL PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("MIDDLEMAN BETWEEN OUR COMPANY") ||
            preProcessedBody.Trim().ToUpper().Contains("PARTNER WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSING THIS OPPORTUNITY TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("PICKED YOU FOR A HUMANITARIAN GRANT") ||
            preProcessedBody.Trim().ToUpper().Contains("PICKED YOU FOR HUMANITARIAN GRANT") ||
            preProcessedBody.Trim().ToUpper().Contains("PRIVATE OFFER WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("PROFIT SHARING") ||
            preProcessedBody.Trim().ToUpper().Contains("PROFITABLE PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("PROPOSAL FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("PROPOSAL THAT MIGHT INTEREST") ||
            preProcessedBody.Trim().ToUpper().Contains("REGARDING A PROJECT") ||
            preProcessedBody.Trim().ToUpper().Contains("WE CAN WORK OUT THIS FOR OUR BENEFIT") ||
            preProcessedBody.Trim().ToUpper().Contains("WE CAN WORK THIS OUT FOR OUR BENEFIT") ||
            preProcessedBody.Trim().ToUpper().Contains("WE CAN WORK TOGETHER") ||
            (preProcessedBody.Trim().ToUpper().Contains("LETS SPLIT") && (preProcessedBody.Trim().ToUpper().Contains("IN THIS DEAL") || preProcessedBody.Trim().ToUpper().Contains("ON THIS DEAL"))) ||
            (preProcessedBody.Trim().ToUpper().Contains("PROJECT") && preProcessedBody.Trim().ToUpper().Contains("BENEFIT TO YOU")))
        {
            type = EmailType.Investor;
        }
        else if ((preProcessedBody.Trim().ToUpper().Contains("CONSIGNMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("TRUNK BOX") ||
            preProcessedBody.Trim().ToUpper().Contains("PACKAGE BOX") ||
            preProcessedBody.Trim().ToUpper().Contains("PACKAGE DELIVER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PARCEL") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVER YOUR PACKAGE")) &&
            (!preProcessedBody.Trim().ToUpper().Contains("NOT A CONSIGNMENT") || //If we misclasified the type they might tell us we are not receiving a consignment box
            !preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVING A CONSIGNMENT") ||
            !preProcessedBody.Trim().ToUpper().Contains("NOT CONSIGNMENT")))
        {
            type = EmailType.ConsignmentBox;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("MONEYGRAM") ||
            preProcessedBody.Trim().ToUpper().Contains("MONEY GRAM") ||
            preProcessedBody.Trim().ToUpper().Contains("WESTERN UNION") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER THE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("HELP ME WITH THE RENEW DUES") ||
            preProcessedBody.Trim().ToUpper().Contains("GENERAL INSURANCE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("ASSIST ME WITH THE RENEW DUES"))
        {
            type = EmailType.GenericPayment;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("AS LONG AS YOU REMAIN HONEST") ||
            preProcessedBody.Trim().ToUpper().Contains("AWARE OF YOUR BACKGROUND") ||
            preProcessedBody.Trim().ToUpper().Contains("BECOME A GOOD FRIEND") ||
            preProcessedBody.Trim().ToUpper().Contains("BECOME YOU GOOD FRIEND") ||
            preProcessedBody.Trim().ToUpper().Contains("BECOME YOUR GOOD FRIEND") ||
            preProcessedBody.Trim().ToUpper().Contains("BUILD TRUST") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN I TRUST YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("CONVINCED THAT I AM COMMUNICATING WITH THE RIGHT PERSON") ||
            preProcessedBody.Trim().ToUpper().Contains("CONVINCED THAT I AM TALKING WITH THE RIGHT PERSON") ||
            preProcessedBody.Trim().ToUpper().Contains("CURRENTLY SINGLE GIRL") ||
            preProcessedBody.Trim().ToUpper().Contains("CURRENTLY SINGLE WOMAN") ||
            preProcessedBody.Trim().ToUpper().Contains("DATING SITE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSSION ABOUT FRIENDSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("FOR LASTING RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("GET TO KNOW EACH OTHER") ||
            preProcessedBody.Trim().ToUpper().Contains("GET TO KNOW EACHOTHER") ||
            preProcessedBody.Trim().ToUpper().Contains("GET TO KNOW YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE GOOD RELATIONSHIP WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I AM A SINGLE YOUNG LADY") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED YOUR RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("I PICK INTEREST ON YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I SAW YOUR PROFILE TODAY") ||
            preProcessedBody.Trim().ToUpper().Contains("I SEE YOU AS SOMEONE I CAN WORK WITH") ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT TO MAKE A NEW AND SPECIAL FRIEND") ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT US TO BE FRIENDS") ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT US TO BECOME FRIENDS") ||
            preProcessedBody.Trim().ToUpper().Contains("I WILL LIKE TO NO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I WILL TELL YOU MORE ABOUT MYSELF") ||
            preProcessedBody.Trim().ToUpper().Contains("I WOULD LIKE TO KNOW YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("IF YOU CAN TRUST") ||
            preProcessedBody.Trim().ToUpper().Contains("LIKE TO KNOW YOU MORE") ||
            preProcessedBody.Trim().ToUpper().Contains("LONG TERM RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("LONGTERM RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("MAN WITH GOOD SENSE OF HUMOR") ||
            preProcessedBody.Trim().ToUpper().Contains("MEANINGFUL RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("MORE DETAIL ABOUT MYSELF") ||
            preProcessedBody.Trim().ToUpper().Contains("MORE DETAIL ABOUT YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("PRESENTLY SINGLE GIRL") ||
            preProcessedBody.Trim().ToUpper().Contains("PRESENTLY SINGLE WOMAN") ||
            preProcessedBody.Trim().ToUpper().Contains("RELIABLE AND HONEST") ||
            preProcessedBody.Trim().ToUpper().Contains("SEEKING YOUR ASSISTANCE") ||
            preProcessedBody.Trim().ToUpper().Contains("SERIOUS RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("SINGLE NEVER MARRIED") ||
            preProcessedBody.Trim().ToUpper().Contains("UNTIL YOU RESPOND BACK") ||
            (preProcessedBody.Trim().ToUpper().Contains("TRUST") && preProcessedBody.Trim().ToUpper().Contains("FRIENDSHIP")) ||
            (preProcessedBody.Trim().ToUpper().Contains("I AM WOMAN OF") && preProcessedBody.Trim().ToUpper().Contains("YEARS OLD FROM")))
        {
            type = EmailType.BuildTrust;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("MANUFACTURER OF LED") ||
            preProcessedBody.Trim().ToUpper().Contains("GOLD FOR SALE") ||
            preProcessedBody.Trim().ToUpper().Contains("ASPIRIN CREAM") ||
            preProcessedBody.Trim().ToUpper().Contains("OUR PRODUCT LINE") ||
            (preProcessedBody.Trim().ToUpper().Contains("GOLD DUST") && preProcessedBody.Trim().ToUpper().Contains("BUYER")) ||
            (preProcessedBody.Trim().ToUpper().Contains("GOLD BARS") && preProcessedBody.Trim().ToUpper().Contains("BUYER")) ||
            (preProcessedBody.Trim().ToUpper().Contains("GOLD DUST") && preProcessedBody.Trim().ToUpper().Contains("FOR SALE")) ||
            (preProcessedBody.Trim().ToUpper().Contains("GOLD BARS") && preProcessedBody.Trim().ToUpper().Contains("FOR SALE")) ||
            (preProcessedBody.Trim().ToUpper().Contains("GOLD DUST") && preProcessedBody.Trim().ToUpper().Contains("COST FOR")) ||
            (preProcessedBody.Trim().ToUpper().Contains("GOLD BARS") && preProcessedBody.Trim().ToUpper().Contains("COST FOR")) ||
            preProcessedBody.Trim().ToUpper().Contains("LED DISPLAY SUPPLIER"))
        {
            type = EmailType.SellingProducts;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("ACCOUNT HAS BEEN CREATED") ||
            preProcessedBody.Trim().ToUpper().Contains("ACCOUNT STATUS HAS BEEN CHANGED") ||
            preProcessedBody.Trim().ToUpper().Contains("BLOCKED ACCESS TO YOUR PAYPAL ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("CANCEL YOUR PAYPAL") ||
            preProcessedBody.Trim().ToUpper().Contains("CLICK HERE TO ACCESS MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("CLICK THE LINK BELOW") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPLETE YOUR PURCHASE") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRME YOUR ACCOUNT") ||
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
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE A NOTIFICATION FROM") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ACCOUNT HAS BEEN LIMITED") ||
            (preProcessedBody.Trim().ToUpper().Contains("YOU HAVE (") && preProcessedBody.Trim().ToUpper().Contains(") NEW SECURITY MESSAGE")) ||
            (preProcessedBody.Trim().ToUpper().Contains("YOUR EMAIL ACCOUNT WILL BE PERM") && preProcessedBody.Trim().ToUpper().Contains("DISABLE")) ||
            (preProcessedBody.Trim().ToUpper().Contains("YOU HAVE (") && preProcessedBody.Trim().ToUpper().Contains(") NEW SECURITY MESSAGE")))
        {
            type = EmailType.Phishing;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("ABANDONED FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("ASK HIM TO SEND YOU THE TOTAL") ||
            preProcessedBody.Trim().ToUpper().Contains("ASSIST IN RECEIVING") ||
            preProcessedBody.Trim().ToUpper().Contains("ASSISTANCE TO SET UP MY CHARITY FOUNDATION") ||
            preProcessedBody.Trim().ToUpper().Contains("AWARDED THE SUM OF") ||
            preProcessedBody.Trim().ToUpper().Contains("BANK CHECK DRAFT") ||
            preProcessedBody.Trim().ToUpper().Contains("CASH GRANT DONATION") ||
            preProcessedBody.Trim().ToUpper().Contains("CHOOSEN TO RECEIVE $") ||
            preProcessedBody.Trim().ToUpper().Contains("CHOOSING TO RECEIVE $") ||
            preProcessedBody.Trim().ToUpper().Contains("CHOSEN TO RECEIVE $") ||
            preProcessedBody.Trim().ToUpper().Contains("CLAIM HIS DEPOSITED FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("CLAIM THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("CLAIM YOUR BANK DRAFT") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPENSATION AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPENSATION FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPENSATION FOR YOUR EFFORTS") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT THE PAYING BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTAINS THE SUM OF") ||
            preProcessedBody.Trim().ToUpper().Contains("CREDIT YOUR MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("CRYPTOCURRENCY FREE") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVER YOUR OWN COMPENSATION") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF YOUR CHECK") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF YOUR MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF YOUR SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF YOUR WEALTH") ||
            preProcessedBody.Trim().ToUpper().Contains("DON ATION") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATE A HUGE AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATE WHAT I HAVE TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATE THE SUM OF") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATE TO CHARITY THROUGH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATED") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATING") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATION") ||
            preProcessedBody.Trim().ToUpper().Contains("EFFECT THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("EXPECTING TO RECEIVE IS A CASH") ||
            preProcessedBody.Trim().ToUpper().Contains("FREE CRYPTOCURRENCY") ||
            preProcessedBody.Trim().ToUpper().Contains("FREE GRANT") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND BELONGING TO MY DECEASED CLIENT") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND IN A CASHIER CHECK") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND IN A CASHIER CHEQUE") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND THE SUM OF") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS HAS BEEN ORDERED") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS IN A CASHIER CHECK") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS IN A CASHIER CHEQUE") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS TO YOU CONTACT") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS TO YOU, CONTACT") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS TO YOUR CONTACT") ||
            preProcessedBody.Trim().ToUpper().Contains("GREATLY IN NEED OF AN INDIVIDUAL WHO CAN HANDLE") ||
            preProcessedBody.Trim().ToUpper().Contains("GREATLY IN NEED OF INDIVIDUAL WHO CAN HANDLE") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE A PACKAGE OF $") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE WILLED £") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE WILLED $") ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT TO DISTRIBUTE MY $") ||
            preProcessedBody.Trim().ToUpper().Contains("I WISH TO BEQUEATH YOU IN SPECIES THIS SU M") || //How can we possibly predict emails with wording/grammer like this?
            preProcessedBody.Trim().ToUpper().Contains("I WISH TO WILL $") ||
            preProcessedBody.Trim().ToUpper().Contains("I WISH TO WILL US$") ||
            preProcessedBody.Trim().ToUpper().Contains("I WISH TO WILL USD$") ||
            preProcessedBody.Trim().ToUpper().Contains("IMMEDIATE WITHDRAWAL OF YOUR CASH FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("IN REGARDS TO YOUR DRAFT OF $") ||
            preProcessedBody.Trim().ToUpper().Contains("IN REGARDS TO YOUR DRAFT OF US") ||
            preProcessedBody.Trim().ToUpper().Contains("INCOMING TRANSFER NOTIFICATION") ||
            preProcessedBody.Trim().ToUpper().Contains("INSTANT RICH SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERIGENS FUND WHORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERNATIONAL CASHIER'S BANK DRAFT, TO THE TUNE") ||
            preProcessedBody.Trim().ToUpper().Contains("INVEST THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("INVOLVING THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("KEPT THE CHECK WITH THEM") ||
            preProcessedBody.Trim().ToUpper().Contains("KEPT THE CHEQUE WITH THEM") ||
            preProcessedBody.Trim().ToUpper().Contains("MAPPED OUT A COMPENSATION") ||
            preProcessedBody.Trim().ToUpper().Contains("MONETARY FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("OFFERED YOU $") ||
            preProcessedBody.Trim().ToUpper().Contains("OFFERED YOU WITH $") ||
            preProcessedBody.Trim().ToUpper().Contains("ON YOUR FAVOR A DRAFT WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("PROCESSING OF FUND TRANSFER") ||
            preProcessedBody.Trim().ToUpper().Contains("PROMISE TO PAY THE SOM") ||
            preProcessedBody.Trim().ToUpper().Contains("PROMISE TO PAY THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THIS FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THIS MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THIS SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THIS WEALTH") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE YOUR PROFIT") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVING THEIR MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVING YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVING YOUR MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVING YOUR SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVING YOUR WEALTH") ||
            preProcessedBody.Trim().ToUpper().Contains("REFLECT IN YOUR BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("REGARDS TO YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASE OF THE FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASE SOME FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASE YOUR DRAFT") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASED TO YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("SECURED SUM OF") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOU THE REST OF MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SO HE CAN RELEASE YOUR DRAFT TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("SOLUTION TO A MONEY TRANSFER") ||
            preProcessedBody.Trim().ToUpper().Contains("STILL WANT YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("THAT WAS AWARDED TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("THE TRANSMISSION OF THE FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("THOSE FUNDS TRANSFERRED") ||
            preProcessedBody.Trim().ToUpper().Contains("TO BE COMPENSATED") ||
            preProcessedBody.Trim().ToUpper().Contains("TO YOUR BANK ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("TOTAL SUM OF") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSACTION WE WERE PURSING TOGETHER") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER OF THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER OF YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER OF YOUR MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER OF YOUR SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER OF YOUR WEALTH") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER OUR CASH") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER SUM OF $") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER TO YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFERRED THE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFERRED TO YOUR OWN PERSONAL ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFERRING OF THIS FUND SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFERRING YOUR FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFERRING YOUR MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("UNCLAIMED ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("UNCLAIMED FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("UNPAID FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("USD WAS DANATED TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("USD WAS DONATED TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("WE HAVE DECIDED TO DONATE THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("WILL THIS FORTUNE TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("WITH THE SUM AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE ELIGIBLE TO RECEIVE YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE AN UNCLAIMED FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU WILL BE RECEIVING THE FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR CASHIER'S CHECK") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR DELIVERY WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUND AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUND RELEASE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUND WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUNDS TO BE PAID") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUNDS VALUED") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUNDS WILL BE PAID") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PACKAGE WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR SHARE/COMPENSATION") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU WILL RECEIVE YOUR FUND") ||
            (preProcessedBody.Trim().ToUpper().Contains("FUND") && preProcessedBody.Trim().ToUpper().Contains("URGENT DELIVERY")) ||
            (preProcessedBody.Trim().ToUpper().Contains("ASSIGNED TO BE DELIVERED") && preProcessedBody.Trim().ToUpper().Contains("$")) ||
            (preProcessedBody.Trim().ToUpper().Contains("FUND") && preProcessedBody.Trim().ToUpper().Contains("UNCLAIMED") && preProcessedBody.Trim().ToUpper().Contains("DEPOSITED")) ||
            (preProcessedBody.Trim().ToUpper().Contains("OF THIS MONEY") && preProcessedBody.Trim().ToUpper().Contains("OFFER YOU")) ||
            (preProcessedBody.Trim().ToUpper().Contains("DONATE $") && preProcessedBody.Trim().ToUpper().Contains("TO YOU")))
        {
            type = EmailType.FreeMoney;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("EMPLOYMENT")) //If no other hits and the email contains employment assume its a job offer
        {
            type = EmailType.JobOffer;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("AM NOW A SINGER") ||
            preProcessedBody.Trim().ToUpper().Contains("ANY PROBLEM IN LIFE") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU STILL INTERESTED") ||
            preProcessedBody.Trim().ToUpper().Contains("BEEN TRYING TO REACH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL I DISCUSS WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL ME") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN I ASK YOU A FAVOR") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN I DISCUSS WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN WE TALK") ||
            preProcessedBody.Trim().ToUpper().Contains("CHARITY PROPOSAL FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("COCA-COLA AWARD") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIDENTIAL BRIEF") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT ME") ||
            preProcessedBody.Trim().ToUpper().Contains("DEAR HOW ARE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET MY EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET MY E-MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET MY LAST EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET MY PREVIOUS EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE MY PREVIOUS EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET MY LAST E-MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET MY PREVIOUS E-MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE MY PREVIOUS E-MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS A IMPORTANT ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS A ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS A VERY IMPORTANT ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS AN IMPORTANT ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS AN ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS AN VERY IMPORTANT ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS IMPORTANT ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU SPEAK ENGLISH") ||
            preProcessedBody.Trim().ToUpper().Contains("GET BACK TO ME FOR MORE INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("GIVE ME A CALL") ||
            preProcessedBody.Trim().ToUpper().Contains("GIVE ME CALL") ||
            preProcessedBody.Trim().ToUpper().Contains("HELLO DEAR") ||
            preProcessedBody.Trim().ToUpper().Contains("HELP ME") ||
            preProcessedBody.Trim().ToUpper().Contains("HELP NEEDED") ||
            preProcessedBody.Trim().ToUpper().Contains("HELP POWER PROGRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("HEY DEAR") ||
            preProcessedBody.Trim().ToUpper().Contains("HI DEAR") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU DOING") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU, I AM VERY GOOD") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW OLD ARE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HUMANITARIAN CHARITY OFFER") ||
            preProcessedBody.Trim().ToUpper().Contains("HW ARE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I AM HAVING A MEETING WITH MY CLIENT BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE A VERY LUCRATIVE DEAL") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE SPECIAL PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE WAITED FOR YOU SO LONG") ||
            preProcessedBody.Trim().ToUpper().Contains("I LIKE US TO TALK") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED YOU URGENTLY") ||
            preProcessedBody.Trim().ToUpper().Contains("I REALLY NEED TO HEAR FROM YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I SHALL GIVE YOU DETAILS ON YOUR RESPONSE") ||
            preProcessedBody.Trim().ToUpper().Contains("I SHALL GIVE YOU DETAILS UPON YOUR RESPONSE") ||
            preProcessedBody.Trim().ToUpper().Contains("I SHALL PROVIDE YOU WITH DETAILS ON YOUR RESPONSE") ||
            preProcessedBody.Trim().ToUpper().Contains("I SHALL PROVIDE YOU WITH DETAILS UPON YOUR RESPONSE") ||
            preProcessedBody.Trim().ToUpper().Contains("I URGENTLY NEED YOUR ASSISTANCE") ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT TO MEET YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("IMPORTANT I LIKE TO SHARE") ||
            preProcessedBody.Trim().ToUpper().Contains("IMPORTANT INFORMATION FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("IMPORTANT TO DISCUSS WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("INFO ON WHY THE EMAIL IS COMING FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("INFO ON WHY THE EMAIL IS COMING TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("INFO ON WHY THIS EMAIL IS COMING TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("INFO WHY THIS EMAIL IS COMING TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("KNOW IF YOU RECEIVED MY PREVIOUS MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("MEET PEOPLE FOR DIFFERENT REASONS RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("MISS SHARON RIVAS") ||
            preProcessedBody.Trim().ToUpper().Contains("MUSICAL ARTIST NAME IS SENATOR") ||
            preProcessedBody.Trim().ToUpper().Contains("NEED TO TALK ITS VERY IMPORTANT") ||
            preProcessedBody.Trim().ToUpper().Contains("NO TIME TO WASTE") ||
            preProcessedBody.Trim().ToUpper().Contains("ORDERS FROM MR. PRESIDENT") ||
            preProcessedBody.Trim().ToUpper().Contains("PERMISSION TO EMAIL YOU MY PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("PERSONAL DISCUSSION") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE CONTACT MY SON") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE HELP ME") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE WRITE ME") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE I WANT YOU TO ASSIST") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE I WANT YOUR ASSIST") ||
            preProcessedBody.Trim().ToUpper().Contains("RANDOMLY SELECTED INDIVID") ||
            preProcessedBody.Trim().ToUpper().Contains("REALLY SURPRISE READING YOUR MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOU MY PICTURE") ||
            preProcessedBody.Trim().ToUpper().Contains("SOME THING IMPORTANT FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("SOMETHING IMPORTANT FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("SOMETHING URGENT") ||
            preProcessedBody.Trim().ToUpper().Contains("TALK TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("TALK WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("TELL HER TO SEND YOU THE MEMBERSHIP FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("THE EMAIL ADDRESS YOU USED TO CONTACT US IS NO LONGER VALID") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS PURE BUSINESS") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS TO INFORM YOU THAT YOU HAVE BEEN PICKED") ||
            preProcessedBody.Trim().ToUpper().Contains("VERY IMPORTANT THING TO TELL YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT ARE YOU REALLY SAYING") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR AGE") ||
            preProcessedBody.Trim().ToUpper().Contains("WHERE ARE YOU FROM") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE A GOOD PROFILE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE GOOD PROFILE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE URGENT CALL") ||
            (preProcessedBody.Trim().ToUpper().Contains("SEND AN EMAIL") && preProcessedBody.Trim().ToUpper().Contains("FOR MORE INFO")) ||
            (preProcessedBody.Trim().ToUpper().Contains("GOOD MORNING AND HOW ARE YOU") && preProcessedBody.Trim().ToUpper().Contains("MY NAME IS") && (preProcessedBody.Length - currentMessage.SubjectLine.Length) < 100) ||
            (preProcessedBody.Trim().ToUpper().Contains("HI") || preProcessedBody.Trim().ToUpper().Contains("HELLO") || preProcessedBody.Trim().ToUpper().Contains("GREETING") || preProcessedBody.Trim().ToUpper().Contains("DEAR FRIEND")) && (preProcessedBody.Length - currentMessage.SubjectLine.Length) <= 10)
        {
            type = EmailType.InformationGathering;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("CONSIDER TRADING WITH"))
        {
            type = EmailType.GenericAdvertisement;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("À ÉTÉ") ||
            preProcessedBody.Trim().ToUpper().Contains("ÊTES") ||
            preProcessedBody.Trim().Contains("ß") ||
            preProcessedBody.Trim().ToUpper().Contains("ı") ||
            preProcessedBody.Trim().ToUpper().Contains("Ő") ||
            preProcessedBody.Trim().Contains("ő") ||
            preProcessedBody.Trim().ToUpper().Contains("Ű") ||
            preProcessedBody.Trim().Contains("ű") ||
            preProcessedBody.Trim().Contains("Ӳ") ||
            preProcessedBody.Trim().Contains("ӳ") ||
            preProcessedBody.Trim().ToUpper().Contains("AFIN QUE VOS") ||
            preProcessedBody.Trim().ToUpper().Contains("AHN-YOUNG-HA-SE-YO") ||
            preProcessedBody.Trim().ToUpper().Contains("ANNAK BARMELY") ||
            preProcessedBody.Trim().ToUpper().Contains("BEHOEFTE") ||
            preProcessedBody.Trim().ToUpper().Contains("BONJOUR") ||
            preProcessedBody.Trim().ToUpper().Contains("BUENOS DÍAS") ||
            preProcessedBody.Trim().ToUpper().Contains("CAS OU VOTRE") ||
            preProcessedBody.Trim().ToUpper().Contains("CHARGÉ") ||
            preProcessedBody.Trim().ToUpper().Contains("CIAO") ||
            preProcessedBody.Trim().ToUpper().Contains("CRÉDIT") ||
            preProcessedBody.Trim().ToUpper().Contains("DE LUTTER") ||
            preProcessedBody.Trim().ToUpper().Contains("DE VOS RÊVES") ||
            preProcessedBody.Trim().ToUpper().Contains("DES PAYS DE") ||
            preProcessedBody.Trim().ToUpper().Contains("DSCH") ||
            preProcessedBody.Trim().ToUpper().Contains("EEUW") ||
            preProcessedBody.Trim().ToUpper().Contains("EL BANCO") ||
            preProcessedBody.Trim().ToUpper().Contains("EZ AZ ") ||
            preProcessedBody.Trim().ToUpper().Contains("FÜR") ||
            preProcessedBody.Trim().ToUpper().Contains("GUTEN TAG") ||
            preProcessedBody.Trim().ToUpper().Contains("HABARI") ||
            preProcessedBody.Trim().ToUpper().Contains("HALLO") ||
            preProcessedBody.Trim().ToUpper().Contains("HOLA") ||
            preProcessedBody.Trim().ToUpper().Contains("IEUW") ||
            preProcessedBody.Trim().ToUpper().Contains("JAMBO") ||
            preProcessedBody.Trim().ToUpper().Contains("KONBAN WA") ||
            preProcessedBody.Trim().ToUpper().Contains("KONNICHIWA") ||
            preProcessedBody.Trim().ToUpper().Contains("MARHABA") ||
            preProcessedBody.Trim().ToUpper().Contains("MERHABA") ||
            preProcessedBody.Trim().ToUpper().Contains("NAMASTE") ||
            preProcessedBody.Trim().ToUpper().Contains("NAY HOH") ||
            preProcessedBody.Trim().ToUpper().Contains("NEM AZ ") ||
            preProcessedBody.Trim().ToUpper().Contains("NI HAU") ||
            preProcessedBody.Trim().ToUpper().Contains("NO ME GUSTA") ||
            preProcessedBody.Trim().ToUpper().Contains("OHAYO") ||
            preProcessedBody.Trim().ToUpper().Contains("OLÀ") ||
            preProcessedBody.Trim().ToUpper().Contains("ONS DAN EEN") ||
            preProcessedBody.Trim().ToUpper().Contains("OU ZE5 OU") ||
            preProcessedBody.Trim().ToUpper().Contains("PARA TODAS") ||
            preProcessedBody.Trim().ToUpper().Contains("POR ESTE") ||
            preProcessedBody.Trim().ToUpper().Contains("POR FAVOR") ||
            preProcessedBody.Trim().ToUpper().Contains("PREGA DI") ||
            preProcessedBody.Trim().ToUpper().Contains("QUE JE") ||
            preProcessedBody.Trim().ToUpper().Contains("QUI NOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("SAIN BAINUU") ||
            preProcessedBody.Trim().ToUpper().Contains("SALAAM") ||
            preProcessedBody.Trim().ToUpper().Contains("SALAMA ALEIKUM") ||
            preProcessedBody.Trim().ToUpper().Contains("SALEMETSIZ BE") ||
            preProcessedBody.Trim().ToUpper().Contains("SANNU") ||
            preProcessedBody.Trim().ToUpper().Contains("SE JOUE") ||
            preProcessedBody.Trim().ToUpper().Contains("SOCIÉTÉS") ||
            preProcessedBody.Trim().ToUpper().Contains("STUUR ONS") ||
            preProcessedBody.Trim().ToUpper().Contains("SZIA") ||
            preProcessedBody.Trim().ToUpper().Contains("TSCH") ||
            preProcessedBody.Trim().ToUpper().Contains("UNO DE ") ||
            preProcessedBody.Trim().ToUpper().Contains("WENN JA") ||
            preProcessedBody.Trim().ToUpper().Contains("WIE IST DEINE") ||
            preProcessedBody.Trim().ToUpper().Contains("ZDRAS-TVUY-TE") ||
            !IsEnglish(preProcessedBody.Trim()))
        {
            type = EmailType.ForeignLanguage;
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("BOX")) //If no other hits then just look for the word BOX
        {
            type = EmailType.ConsignmentBox;
        }
        else if (((preProcessedBody.Length - currentMessage.SubjectLine.Length) < 40 && (preProcessedBody.Trim().ToUpper().Contains("INLINE IMAGE"))) ||
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
            preProcessedBody.Trim().ToUpper().Contains("VIEW ATTACHED FILE") ||
            preProcessedBody.Trim().ToUpper().Contains("VIEW ATTACHED LETTER") ||
            preProcessedBody.Trim().ToUpper().Contains("VIEW ATTACHED.") ||
            preProcessedBody.Trim().ToUpper().Contains("VIEW THE ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("VIEW THE ATTACHMENT"))
        {
            type = EmailType.BlankWithAttachment;
        }
        else
        {
            for (int i = pastMessages.Count() - 1; i >= 0; i--)
            {
                if (pastMessages[i].MessageType != 0)
                {
                    type = (EmailType)pastMessages[i].MessageType;
                    break;
                }
            }
        }

        if((EmailType)currentMessage.MessageType == EmailType.Unknown || type != EmailType.Unknown)
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
        string newEmailAddress = AttemptToFindReplyToEmailAddress(preProcessedBody).Trim();
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

        switch ((EmailType)currentMessage.MessageType)
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
                    rtnResponse = GetRandomContinuedResponseForConsignmentBox(rand, greeting, name, attachmentType, currentMessage, pastMessages);
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
            case EmailType.ForeignLanguage:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForForeignLanguage(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForForeignLanguage(rand, greeting, name, currentMessage);
                break;
            case EmailType.GenericAdvertisement:
                if (pastMessages.Count() > 0)
                    rtnResponse = GetRandomContinuedResponseForGenericAdvertisement(rand, greeting, name, currentMessage);
                else
                    rtnResponse = GetRandomOpeningResponseForGenericAdvertisement(rand, greeting, name, currentMessage);
                break;
            case EmailType.MessageTooLong:
                rtnResponse = GetRandomOpeningResponseLongMessageType(rand, greeting, name, currentMessage);
                break;
            case EmailType.MessageTooShort:
                rtnResponse = GetRandomOpeningResponseShortMessageType(rand, greeting, name, currentMessage);
                break;
        }

        //If we cannot determine a type but we can answer a direct question then reply with that and see if they reveal what they are really after
        if (String.IsNullOrEmpty(rtnResponse))
        {
            rtnResponse += HandleDirectQuestions(preProcessedBody, ref currentMessage, rand).Trim('\r').Trim('\n').Trim();
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