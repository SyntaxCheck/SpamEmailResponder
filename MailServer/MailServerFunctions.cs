using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;

public class MailServerFunctions
{
    private const string settingFileLocation = "Settings.json";
    public const string ProcessFolderName = "AutoProcessedMail";
    public const int Timeout = 50000; //TODO turn this into a setting
    public int LastInboxCount;
    public List<InboxCountHistory> InboxCountHistory;
    public string UserName;
    public string Password;
    private int messagesSendSinceRandomize;
    public Settings settings;
    private ResponseProcessing respProc;

    public MailServerFunctions()
    {
        LastInboxCount = 0;
        messagesSendSinceRandomize = 0;
        InboxCountHistory = new List<InboxCountHistory>();
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
            settings.EmailAddressesToIgnore = new List<string>() { "mailer-daemon@googlemail.com" };
            settings.Acquaintance = new List<string>() { "Bob", "Steve", "Bill", "Chad", "Mary", "Margret", "Joe", "Frank", "Cathy" };
            settings.Products = new List<string>() { "Cars", "Boats", "Lava Lamps", "Blinker Fluid" };
            settings.Devices = new List<string>() { "Computer", "Phone", "Tablet" };
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
            settings.QuestionsEmail = new List<string>() { "My email is |Email|" };
            settings.QuestionsPassword = new List<string>() { "Why do you need my password?" };
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
            settings.QuestionsContactThePerson = new List<string>() { "I do not know who that is." };
            settings.QuestionsAreYouMember = new List<string>() { "I want to be a member." };
            settings.QuestionsDidYouSeeOurMessage = new List<string>() { "I did not see it." };
            settings.QuestionsInvalidAddress = new List<string>() { "My house is new and it does not show up on some places.", "I recently built my house so the address might not showup." };
            settings.QuestionsTellUsWhatTheyAskedYouToDo = new List<string>() { "They asked me to leave when I read them the emails. Maybe I said something wrong?", "They told me to stop wasting their time with all these emails I bring to them. Maybe you need to tell me exactly what to say?", "They told me they would not read the emails from my phone.", "They told me to stop coming in there." };
            settings.QuestionsTellUsAboutYourself = new List<string>() { "|introduction|",  };
            settings.QuestionsAutomatedProgram = new List<string>() { "I am a real person just trying to respond to your emails.", "I am not sure why you think I am not a real person. Don't I reply to your messages?" };
            settings.QuestionsUseWalmartToPay = new List<string>() { "I do not live near a walmart that has those payment options.", "I called the local walmart and they do not offer those payment options." };
            settings.QuestionsHowMuchMoneyDoIHave = new List<string>() { "I have $|GetRandomNumber10000|." };
            settings.QuestionsSendTransferReceipt = new List<string>() { "I have attached the receipt." };
            settings.QuestionsAlreadyToldYou = new List<string>() { "I have forgetten what you said earlier." };
            settings.QuestionsTrustUs = new List<string>() { "How can I trust you?" };

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
            settings.ResponseOpeningForeignLanguage = new List<string>() { "I only speak english.", "Can you resend the email in english?" };
            settings.ResponseOpeningGenericAdvertisement = new List<string>() { "I am very interested. Can you tell me why I would go with you over your competitor?", "Can you tell me more about it?" };
            settings.ResponseOpeningShipping = new List<string>() { "What is the problem with the shipment?" };
            settings.ResponseOpeningRefugee = new List<string>() { "I would love to help, I just need some more details." };
            settings.ResponseOpeningProductSupplier = new List<string>() { "Which product are you interested in?" };
            settings.ResponseOpeningClickTheLink = new List<string>() { "I do not see a link." };

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
            settings.ResponseContinuedShipping = new List<string>() { "Okay, how can we resolve this issue?" };
            settings.ResponseContinuedRefugee = new List<string>() { "I have noted your message. I just need some more details." };
            settings.ResponseContinuedProductSupplier = new List<string>() { "Which product prompted you to message me?", "How many products are you looking for? My favorite is |GetRandomProduct|." };
            settings.ResponseContinuedClickTheLink = new List<string>() { "I still do not see any link." };

            string json = new JavaScriptSerializer().Serialize(settings);
            File.WriteAllText(settingFileLocation, JsonHelper.FormatJson(json));

            MessageBox.Show("Default settings file has been created. Please modify the settings file before starting the program. The settings file has been filled with example responses, see the readme for the full list of commands that can be put into the settings.", "Please fill in settings");
        }

        UserName = settings.EmailAddress;
        Password = settings.Password;

        respProc = new ResponseProcessing(settings);
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

                            LastInboxCount = inbox.Count;

                            if(InboxCountHistory.Count() == 0 || (DateTime.Now - InboxCountHistory[InboxCountHistory.Count() - 1].HistoryTime).TotalMinutes >= 10)
                                InboxCountHistory.Add(new InboxCountHistory() { HistoryTime = DateTime.Now, InboxCount = LastInboxCount });

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
            if (toAddress.ToUpper().Contains(".COM..."))
            {
                toAddress = toAddress.ToUpper().Replace(".COM...", ".COM...");
            }
            if (toAddress.ToUpper().Contains("MAIL="))
            {
                toAddress = toAddress.ToUpper().Replace("MAIL=", "");
            }
            if (toAddress.ToUpper().Contains("…"))
            {
                toAddress = toAddress.ToUpper().Replace("…", "");
            }

            ////Remove all non-ascii characters
            //toAddress = Regex.Replace(toAddress, @"[^\u0000-\u007F]+", string.Empty);

            //Remove all non-printable ascii characters
            toAddress = Regex.Replace(toAddress, @"[^ -~]+", string.Empty);

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
                inReplyToPartial = TextProcessing.ScrubText(inReplyTo.Substring(0, 5));
            }
            message.MessageId = Crypto.CalculateMD5Hash(DateTime.Now.ToString("yyyyMMddhhmmssfff")) + inReplyToPartial + respProc.GetRandomCharacters(rand, 15) + "@" + settings.OutgoingMessageIdDomainName; //Default is to Imitate the gmail message ID
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
                        realAddress = realAddress.Replace("<", "").Replace(">", "").Replace("\"","").Trim();
                    }
                    else
                    {
                        //friendly = s;
                        realAddress = s.Replace("<", "").Replace(">", "").Replace("\"", "").Trim();
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
                if (realAddress.Contains('/'))
                {
                    string[] addrSplit = realAddress.Split(new char[] { '/' });
                    foreach (string addrS in addrSplit)
                    {
                        if (addrS.Contains('@'))
                        {
                            realAddress = addrS;
                            break;
                        }
                    }
                }

                while (realAddress.EndsWith(".") || realAddress.EndsWith(","))
                {
                    realAddress = realAddress.Substring(0, realAddress.Length - 1);
                }

                if (TextProcessing.IsValidEmail(settings, realAddress))
                {
                    Logger.WriteDbg(loggerInfo, "Friendly: " + TextProcessing.ScrubText(friendly) + ", Email: " + realAddress);
                    message.To.Add(new MailboxAddress(TextProcessing.ScrubText(friendly), realAddress));
                }
            }

            if (message.To.Count() > 0)
            {
                if (!subject.StartsWith("RE:"))
                    subject = "RE: " + subject;

                message.Subject = subject;

                string htmlBodyText = TextProcessing.TextToHtml(bodyText);

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
                    string randomPath = respProc.GetRandomPaymentReceiptPath(rand);
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
            else
            {
                response.Code = -1;
                response.Message = "Failed to Send SMTP Message, no valid 'To' addresses found.";
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
                !msg.FromAddress[0].ToString().ToUpper().Contains("DONOTREPLY@") &&
                !msg.FromAddress[0].ToString().ToUpper().Contains("MAILER-DAEMON@") && 
                !msg.FromAddress[0].ToString().ToUpper().Contains("POSTMASTER@") &&
                !msg.FromAddress[0].ToString().ToUpper().Contains("@NIANTICLABS") &&
                !msg.FromAddress[0].ToString().ToUpper().Contains("NOTIFICATIONS@LIVE.NEXT") &&
                !msg.FromAddress[0].ToString().ToUpper().Contains("ROOT@POST.CV") &&
                !msg.FromAddress[0].ToString().ToUpper().Contains("UNITEDNATIONSECRETARY@XD.AE") && //For some reason we get stuck in a loop with this email address
                !msg.FromAddress[0].ToString().ToUpper().Contains("NOTIFICATIONS@LIVE.NEXT") &&
                !TextProcessing.RemoveUselessText(TextProcessing.ScrubText(msg.Subject)).Trim().ToUpper().Contains("REJECTED MAIL"))
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
                        storageObj.EmailBodyPlain = TextProcessing.MakeEmailEasierToRead(txtPart.Text);
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
                    {
                        if(!FoundIgnoreEmail(v.ToString()))
                            storageObj.ToAddress += v.ToString() + ";";
                    }
                }
                foreach (var v in msg.ReplyTo)
                {
                    if (!FoundIgnoreEmail(v.ToString()))
                        storageObj.ToAddress += v.ToString() + ";";
                }
                if (String.IsNullOrEmpty(storageObj.ToAddress))
                {
                    if(msg.Sender != null)
                    {
                        if (!FoundIgnoreEmail(msg.Sender.ToString()))
                            storageObj.ToAddress += msg.Sender.ToString() + ";";
                    }
                }
                foreach (var v in msg.FileAttachments)
                {
                    storageObj.AtachmentTypes += v.FileExtension + ",";
                    storageObj.AttachmentNames += v.FileName + ",";
                }

                //Get list of previous messages in the thread
                List<MailStorage> previousMessagesInThread = GetPreviousMessagesInThread(storage, storageObj);
                
                //Determine response
                storageObj.DeterminedReply = respProc.GetResponseForType(loggerInfo, ref storageObj, previousMessagesInThread.OrderBy(t => t.DateReceived).ToList());

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

    //Helper functions
    #region Helper functions
    public double GetHoursBetweenSending()
    {
        int tmpOut = 0;
        Int32.TryParse(settings.MinutesDelayBeforeAnsweringAnEmail, out tmpOut);

        return tmpOut / 60.0;
    }
    public bool FoundIgnoreEmail(string emailList)
    {
        emailList = emailList.ToUpper();

        foreach (string s in settings.EmailAddressesToIgnore)
        {
            if (emailList.Contains(s.ToUpper()))
            {
                return true;
            }
        }

        return false;
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
        else if (outInt < 0)
        {
            if (!String.IsNullOrEmpty(rtn))
                rtn += Environment.NewLine;

            rtn += "The MinutesDelayBeforeAnsweringAnEmail must be greater than or equal to 0.";
        }
        if (!String.IsNullOrEmpty(settings.PathToMyFakeID))
        {
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
//    public List<MailStorage> GetPreviousMessagesInThread(List<MailStorage> storage, MailStorage mail)
//    {
//        List<MailStorage> previousMessagesInThread = new List<MailStorage>();

//        foreach (MailStorage ms in storage)
//        {
//            if (ms.Ignored) //Dont add ignored messages to stats since it most likely is duplicates
//                continue;

//            if (ms.MsgId != mail.MsgId) //Skip including the message we are working on
//            {
//                if (!String.IsNullOrEmpty(ms.MyReplyMsgId) && !String.IsNullOrEmpty(mail.InReplyToMsgId) && ms.MyReplyMsgId == mail.InReplyToMsgId)
//                {
//                    if (!previousMessagesInThread.Contains(ms))
//                    {
//                        previousMessagesInThread.Add(ms);

//                        List<MailStorage> prevPreviousMessagesInThread = new List<MailStorage>();
//                        prevPreviousMessagesInThread = GetPreviousMessagesInThread(storage, ms);

//                        foreach (MailStorage prms in prevPreviousMessagesInThread)
//                        {
//                            if (!previousMessagesInThread.Contains(prms))
//                            {
//                                previousMessagesInThread.Add(prms);
//                            }
//                        }
//                    }
//                }
//                else if (ms.SubjectLine.Replace("RE:", "").Replace("FW:", "").Trim() == mail.SubjectLine.Replace("RE:", "").Replace("FW:", "").Trim())
//                {
//                    int foundCount = 0;

//                    foreach (string v in mail.ToAddressList)
//                    {
//                        string emailAddres = 
//String.Empty;

//                        try
//                        {
//                            System.Net.Mail.MailAddress address = new System.Net.Mail.MailAddress(v.Replace("\"", ""));
//                            emailAddres = address.Address;
//                        }
//                        catch (Exception)
//                        {
//                            emailAddres = TextProcessing.AttemptManualParseOfEmailAddress(v);
//                        }

//                        if (ms.ToAddress.Contains(emailAddres))
//                        {
//                            foundCount++;
//                            break;
//                        }
//                    }

//                    if (foundCount > 0)
//                    {
//                        if (!previousMessagesInThread.Contains(ms))
//                        {
//                            previousMessagesInThread.Add(ms);

//                            List<MailStorage> prevPreviousMessagesInThread = new List<MailStorage>();
//                            prevPreviousMessagesInThread = GetPreviousMessagesInThread(storage, ms);

//                            foreach (MailStorage prms in prevPreviousMessagesInThread)
//                            {
//                                if (!previousMessagesInThread.Contains(prms))
//                                {
//                                    previousMessagesInThread.Add(prms);
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        return previousMessagesInThread;
//    }
    public List<MailStorage> GetPreviousMessagesInThread(List<MailStorage> storage, MailStorage mail)
    {
        List<MailStorage> prevMsgs = new List<MailStorage>();

        GetPreviousMessagesInThreadRecursiveRecursive(storage, mail, ref prevMsgs);

        return prevMsgs;
    }
    public void GetPreviousMessagesInThreadRecursiveRecursive(List<MailStorage> storage, MailStorage mail, ref List<MailStorage> prevMsgs)
    {
        foreach (MailStorage ms in storage)
        {
            if (ms.MsgId != mail.MsgId)
            {
                if (!String.IsNullOrEmpty(ms.MyReplyMsgId) && !String.IsNullOrEmpty(mail.InReplyToMsgId) && (ms.MyReplyMsgId == mail.InReplyToMsgId || ms.MsgId == mail.InReplyToMsgId))
                {
                    if (!prevMsgs.Contains(ms))
                    {
                        prevMsgs.Add(ms);
                        GetPreviousMessagesInThreadRecursiveRecursive(storage, ms, ref prevMsgs);
                    }
                }
            }
        }
    }
    public string BuildPreviousMessageText(List<MailStorage> previousMessages)
    {
        StringBuilder sb = new StringBuilder();

        if (previousMessages.Count() > 0)
        {
            foreach (MailStorage ms in previousMessages)
            {
                sb.AppendLine("___________________________________________________________");
                if (ms.Replied)
                {
                    sb.AppendLine("From: Our Email, Type: " + ((ResponseProcessing.EmailType)ms.MessageType).ToString());
                    sb.AppendLine("Date: " + ms.DateProcessed.ToString("dddd, MMMM d, yyyy hh:mm tt"));
                    sb.AppendLine("To: " + ms.ToAddress);
                    sb.AppendLine("Subject: " + ms.SubjectLine + Environment.NewLine);
                    sb.AppendLine(ms.DeterminedReply);
                    if (ms.IncludeID)
                    {
                        sb.AppendLine("<<ID Included as attachment>>");
                    }
                    if (ms.IncludePaymentReceipt)
                    {
                        sb.AppendLine("<<Payment Receipt included as attachment>>");
                    }
                    sb.AppendLine("");
                }
                sb.AppendLine("___________________________________________________________");
                sb.AppendLine("From: " + ms.ToAddress);
                sb.AppendLine("Date: " + ms.DateReceived.ToString("dddd, MMMM d, yyyy hh:mm tt"));
                sb.AppendLine("To: Our Email");
                sb.AppendLine("Subject: " + ms.SubjectLine + Environment.NewLine);
                sb.AppendLine(TextProcessing.RemoveUselessText(TextProcessing.MakeEmailEasierToRead(TextProcessing.RemoveReplyTextFromMessage(settings, ms.EmailBodyPlain))));
                if (ms.NumberOfAttachments > 0)
                {
                    sb.AppendLine("<< Included Attachments: " + ms.AttachmentNames + " >>");
                }
                sb.AppendLine("");
            }
        }

        return sb.ToString();
    }
    #endregion
}