using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MimeKit;
using System.IO;
using System.Xml;
using Ionic.Zip;

public class EmailMessage
{
    private List<TextPart> textParts;
    private List<MimePart> attachments;
    private List<FileAttachment> fileAttachments; //Parsed list of attachments ready to be inserted into the database
    private List<MessagePart> messageParts; //MessagePart is an embedded message within the email, treat MessagePart as an attachment that does not come down
    private List<Header> headers; //Can contain special header information to indicate sender program or spam server
    private InternetAddressList bcc; //Standard Email policy is to not show the Bcc
    private InternetAddressList cc, from, replyTo, resentBcc, resentCc, resentFrom, resentReplyTo, to;
    private string bccString, ccString, fromString, inReplyTo, messageId, refMsgIdListString, replyToString, resentBccString, resentCcString, resentFromString, resentMessageId;
    private string resentReplyToString, senderString, subject, toString, messageDump, processingNotes, processedMsgId;
    private int processSuccess = 0, processFailure = 0, processZipSuccess = 0, processZipFailure = 0;
    private DateTime messageDttm, resentTime;
    private MessageIdList refMsgIdList;
    private MailboxAddress sender;
    private StandardResponse returnResponse;

    #region Getters and Setters
    public StandardResponse ReturnResponse
    {
        get { return returnResponse; }
        set { returnResponse = value; }
    }
    public List<FileAttachment> FileAttachments
    {
        get { return fileAttachments; }
        set { fileAttachments = value; }
    }
    public List<MimePart> Attachments
    {
        get { return attachments; }
        set { attachments = value; }
    }
    public InternetAddressList Bcc
    {
        get { return bcc; }
        set { bcc = value; }
    }
    public string BccString
    {
        get { return bccString; }
        set { bccString = value; }
    }
    public InternetAddressList Cc
    {
        get { return cc; }
        set { cc = value; }
    }
    public string CcString
    {
        get { return ccString; }
        set { ccString = value; }
    }
    public InternetAddressList FromAddress
    {
        get { return from; }
        set { from = value; }
    }
    public string FromAddressString
    {
        get { return fromString; }
        set { fromString = value; }
    }
    public string InReplyTo
    {
        get { return inReplyTo; }
        set { inReplyTo = value; }
    }
    public DateTime MessageDttm
    {
        get { return messageDttm; }
        set { messageDttm = value; }
    }
    public string MessageId
    {
        get { return messageId; }
        set { messageId = value; }
    }
    public List<MessagePart> MessageParts
    {
        get { return messageParts; }
        set { messageParts = value; }
    }
    public List<Header> Headers
    {
        get { return headers; }
        set { headers = value; }
    }
    public MessageIdList RefMsgIdList
    {
        get { return refMsgIdList; }
        set { refMsgIdList = value; }
    }
    public string RefMsgIdListString
    {
        get { return refMsgIdListString; }
        set { refMsgIdListString = value; }
    }
    public InternetAddressList ReplyTo
    {
        get { return replyTo; }
        set { replyTo = value; }
    }
    public string ReplyToString
    {
        get { return replyToString; }
        set { replyToString = value; }
    }
    public InternetAddressList ResentBcc
    {
        get { return resentBcc; }
        set { resentBcc = value; }
    }
    public string ResentBccString
    {
        get { return resentBccString; }
        set { resentBccString = value; }
    }
    public InternetAddressList ResentCc
    {
        get { return resentCc; }
        set { resentCc = value; }
    }
    public string ResentCcString
    {
        get { return resentCcString; }
        set { resentCcString = value; }
    }
    public InternetAddressList ResentFrom
    {
        get { return resentFrom; }
        set { resentFrom = value; }
    }
    public string ResentFromString
    {
        get { return resentFromString; }
        set { resentFromString = value; }
    }
    public string ResentMessageId
    {
        get { return resentMessageId; }
        set { resentMessageId = value; }
    }
    public InternetAddressList ResentReplyTo
    {
        get { return resentReplyTo; }
        set { resentReplyTo = value; }
    }
    public string ResentReplyToString
    {
        get { return resentReplyToString; }
        set { resentReplyToString = value; }
    }
    public DateTime ResentTime
    {
        get { return resentTime; }
        set { resentTime = value; }
    }
    public MailboxAddress Sender
    {
        get { return sender; }
        set { sender = value; }
    }
    public string SenderString
    {
        get { return senderString; }
        set { senderString = value; }
    }
    public string Subject
    {
        get { return subject; }
        set { subject = value; }
    }
    public List<TextPart> TextParts
    {
        get { return textParts; }
        set { textParts = value; }
    }
    public InternetAddressList ToAddress
    {
        get { return to; }
        set { to = value; }
    }
    public string ToAddressString
    {
        get { return toString; }
        set { toString = value; }
    }
    public string MessageDump
    {
        get { return messageDump; }
        set { messageDump = value; }
    }
    public string ProcessingNotes
    {
        get { return processingNotes; }
        set { processingNotes = value; }
    }
    public string ProcessedMsgId
    {
        get { return processedMsgId; }
        set { processedMsgId = value; }
    }
    #endregion

    /// <summary>
    /// Default Constructor
    /// </summary>
    /// <param name="loggerInfo"></param>
    /// <param name="message"></param>
    public EmailMessage()
    {
        InitVars();
    }
    /// <summary>
    /// Constructor calls the primary ConsumeMimeMessage function that does all the parsing
    /// </summary>
    /// <param name="loggerInfo"></param>
    /// <param name="message"></param>
    public EmailMessage(LoggerInfo loggerInfo, MimeMessage message)
    {
        InitVars();

        ConsumeMimeMessage(loggerInfo, message);
    }
    /// <summary>
    /// Populate address strings
    /// </summary>
    public void PopulateAddressStrings()
    {
        if(bcc != null)
            if (bcc.Count > 0) bccString = ConvertAddrListToSeperatedString(bcc, ", ");
        if (cc != null)
            if (cc.Count > 0) ccString = ConvertAddrListToSeperatedString(cc, ", ");
        if (from != null)
            if (from.Count > 0) fromString = ConvertAddrListToSeperatedString(from, ", ");
        if (refMsgIdList != null)
            if (refMsgIdList.Count > 0) refMsgIdListString = ConvertMsgIdListToSeperatedString(refMsgIdList, ", ");
        if (replyTo != null)
            if (replyTo.Count > 0) replyToString = ConvertAddrListToSeperatedString(replyTo, ", ");
        if (to != null)
            if (to.Count > 0) toString = ConvertAddrListToSeperatedString(to, ", ");
    }
    /// <summary>
    /// Initialize variables and set the column lengths
    /// </summary>
    private void InitVars()
    {
        returnResponse = new StandardResponse { Code = 1, Message = "", Data = "" };
        textParts = new List<TextPart>();
        attachments = new List<MimePart>();
        fileAttachments = new List<FileAttachment>();
        messageParts = new List<MessagePart>();
        headers = new List<Header>();
        cc = bcc = from = replyTo = resentBcc = resentCc = resentFrom = resentReplyTo = to = new InternetAddressList();
        bccString = ccString = fromString = inReplyTo = messageId = refMsgIdListString = replyToString = resentBccString = resentCcString = resentFromString
            = resentMessageId = resentReplyToString = senderString = subject = toString = messageDump = processingNotes = processedMsgId = String.Empty;
    }
    /// <summary>
    /// Used to translate a MimeMessage object to something easier to store in a database. Main goal get the whole message in XML format
    /// </summary>
    /// <param name="message"></param>
    private void ConsumeMimeMessage(LoggerInfo loggerInfo, MimeMessage message)
    {
        StandardResponse response = new StandardResponse { Code = 1, Message = "", Data = "" };

        try
        {
            List<MimeEntity> attmentEntity = new List<MimeEntity>();

            //first save the email as a string. This string could be saved as a .eml and opened using outlook to view it
            attmentEntity = message.Attachments.ToList();
            foreach (MimeEntity m in attmentEntity)
            {
                if (m is MimePart)
                {
                    attachments.Add((MimePart)m);
                }
            }
            textParts = message.BodyParts.OfType<TextPart>().Where(x => String.IsNullOrEmpty(x.FileName)).ToList();
            messageParts = message.BodyParts.OfType<MessagePart>().ToList();
            bcc = message.Bcc;
            cc = message.Cc;
            messageDttm = message.Date.LocalDateTime; //Convert to local time as DateTimeOffset carries timezone information and we cannot store it in the DB
            from = message.From;
            refMsgIdList = message.References;
            replyTo = message.ReplyTo;
            resentBcc = message.ResentBcc;
            resentCc = message.ResentCc;
            resentTime = message.ResentDate.LocalDateTime; //Convert to local time as DateTimeOffset carries timezone information and we cannot store it in the DB
            resentFrom = message.ResentFrom;
            resentReplyTo = message.ResentReplyTo;
            sender = message.Sender;
            to = message.To;
            inReplyTo = message.InReplyTo == null ? String.Empty : message.InReplyTo;
            messageId = message.MessageId == null ? String.Empty : message.MessageId;
            resentMessageId = message.ResentMessageId == null ? String.Empty : message.ResentMessageId;
            subject = message.Subject == null ? String.Empty : message.Subject;
            //headers.AddRange(message.Headers.Where(x => x.Field.ToUpper().Contains("X-"))); //Check for any custom X- headers, might be used by some spam engines to denote originator. Need more investigation

            PopulateAddressStrings();

            //Store the email as attachment but we do not process the email at this time
            //TODO review this, need a better way to handle these
            foreach (MessagePart m in messageParts)
            {
                FileAttachment attch = new FileAttachment();

                attch.FileName = m.Message.MessageId + ".eml";
                attch.ContentType = "text/plain"; //EML Is plain text, everything binary is Base64 encoded
                attch.ContainsBinary = false;
                attch.ProcessingMessage = "File extension not accepted";
                attch.FileException = false;
                attch.ParentZipName = "";

                fileAttachments.Add(attch);
                attch = null;
                processSuccess++;
            }

            FileMimeType fileMimeType = new FileMimeType(loggerInfo);
            foreach (MimePart m in attachments)
            {
                try
                {
                    using (MemoryStream mainStream = new MemoryStream())
                    {
                        m.ContentObject.DecodeTo(mainStream);
                        mainStream.Position = 0; //reset the memoryStream position, in order to read from the stream it must be set back to the beginning

                        if (m.FileName.ToLower().EndsWith(".zip"))
                        {
                            //unzip and process contents accordingly. This function will populate the list directly
                            response = ProcessZip(loggerInfo, mainStream, m.FileName);
                            if (response.Code < 0)
                            {
                                //If we receive an error back we cannot process the message. An invalid zip should be handled and would NOT return a Code -1
                                returnResponse = response;
                                processZipFailure++;
                                break;
                            }
                        }
                        else
                        {
                            string processingMessage = String.Empty;
                            bool exception = true;
                            FileAttachment attch = new FileAttachment();

                            attch.FileName = m.FileName;
                            attch.FileBytes = mainStream.ToArray();
                            attch.ContentType = fileMimeType.GetContentType(mainStream, m.FileName);
                            attch.ContainsBinary = fileMimeType.ContainsBinary(attch.FileBytes);
                            attch.ProcessingMessage = processingMessage;
                            attch.FileException = exception;
                            attch.ParentZipName = "";

                            fileAttachments.Add(attch);
                            attch = null;
                            processSuccess++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.Code = -1;
                    response.Message = "Failed to read attachment";
                    response.Data = ex.Message;

                    Logger.Write(loggerInfo, "General exception reading attachment. Filename during error: " + m.FileName + ", Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace);
                    returnResponse = response; //Any exceptions during the processing of the attachments stop processing and the error needs to be dealt with
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            response.Code = -1;
            response.Message = "Failed to process attachments";
            response.Data = ex.Message;

            Logger.Write(loggerInfo, "General exception processing attachments. Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace);
            if (ex.InnerException != null)
            {
                Logger.Write(loggerInfo, "  Inner exception. Exception: " + ex.InnerException.Message + Environment.NewLine + "Stack Trace: " + ex.InnerException.StackTrace);
            }
            returnResponse = response; //Any exceptions during the processing of the attachments stop processing and the error needs to be dealt with
        }

        //Add some processing notes for the message
        if (returnResponse.Code > 0)
        {
            AppendProcessingNotes("Successfully Processed " + processSuccess.ToString() + " of " + (processSuccess + processFailure).ToString() + " attachments.", true);
            if ((processZipSuccess + processZipFailure) > 0)
            {
                AppendProcessingNotes("Successfully Processed " + processZipSuccess.ToString() + " of " + (processZipSuccess + processZipFailure).ToString() + " zip attachments.", true);
            }
        }
        else
        {
            AppendProcessingNotes("Failed to process messages. Code: " + returnResponse.Code + ", Message: " + returnResponse.Message + ", Data: " + returnResponse.Data,true);
        }
    }
    /// <summary>
    /// Creates a character delimited string for a list
    /// </summary>
    /// <param name="msgList"></param>
    /// <param name="delimiterChar"></param>
    /// <returns></returns>
    private string ConvertMsgIdListToSeperatedString(MessageIdList msgList, string delimiterChar)
    {
        string rtn = String.Empty;

        if (msgList.Count() > 0)
        {
            for (int i = 0; i < msgList.Count; i++)
            {
                rtn += msgList[i];
                if (i < (msgList.Count - 1))
                {
                    rtn += delimiterChar;
                }
            }
        }

        return rtn;
    }
    /// <summary>
    /// Creates a character delimited string for a list
    /// </summary>
    /// <param name="addrList"></param>
    /// <param name="delimiterChar"></param>
    /// <returns></returns>
    private string ConvertAddrListToSeperatedString(InternetAddressList addrList, string delimiterChar)
    {
        string rtn = String.Empty;

        if (addrList.Count > 0)
        {
            for (int i = 0; i < addrList.Count; i++)
            {
                try
                {
                    if (addrList[i].ToString().Contains('@'))
                    {
                        System.Net.Mail.MailAddress ma = new System.Net.Mail.MailAddress(addrList[i].ToString().Replace(";", ""));

                        if (String.IsNullOrEmpty(addrList[i].ToString().Trim()) || String.IsNullOrEmpty(ma.Address))
                        {
                            string exc = String.Empty;

                            if (addrList[i].ToString() == null)
                                exc = "AddrList is null";
                            else
                                exc = "AddrList value: " + addrList[i].ToString();

                            if (ma.Address == null)
                                exc += ". ma.Address is null.";
                            else
                                exc += ". ma.Address value: " + ma.Address + ".";

                            throw new Exception("Failed to get address. " + exc);
                        }

                        rtn += ma.Address;
                        if (i < (addrList.Count - 1))
                        {
                            rtn += delimiterChar;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception newEx = new Exception("Orig: " + ex.Message + Environment.NewLine + "Address: " + addrList[i].ToString(), ex);
                    throw newEx;
                }
            }
        }

        return rtn;
    }
    /// <summary>
    /// Function to simplify appending to the processing Notes
    /// </summary>
    /// <param name="text"></param>
    /// <param name="newLineOnNew"></param>
    public void AppendProcessingNotes(string text, bool newLineOnNew)
    {
        if (processingNotes == null || processingNotes == "")
        {
            processingNotes = text;
        }
        else
        {
            if (newLineOnNew)
                processingNotes += Environment.NewLine;
            else
                processingNotes += ", ";

            processingNotes += text;
        }
    }
    /// <summary>
    /// Function to process zips, at this time we only support standard zips and this could be expanded upon in the future.
    /// Also the only format we accept is a zip with all files in root directory, any folders in the zip will cause an exception but if we start seeing this we can add support
    /// </summary>
    /// <param name="loggerInfo"></param>
    /// <param name="stream"></param>
    /// <param name="zipName"></param>
    /// <returns></returns>
    private StandardResponse ProcessZip(LoggerInfo loggerInfo, MemoryStream stream, string zipName)
    {
        //Currently Only handles standard zip
        StandardResponse response = new StandardResponse { Code = 1, Message = "", Data = "" };
        bool validFormat = true;
        FileMimeType fileMimeType = new FileMimeType(loggerInfo);
        try
        {
            using (ZipFile zip = ZipFile.Read(stream))
            {
                //Go through the zip file and check for a directory, we only support a zip file with the XML in the root of the zip
                foreach (ZipEntry entry in zip)
                {
                    if (entry.IsDirectory)
                    {
                        validFormat = false;

                        response.Code = -1;
                        response.Message = "Zip is in invalid format";
                        response.Data = "Zip contains a folder, at this time we can only process zip files that do not contain folders";

                        break;
                    }
                }
                if (validFormat)
                {
                    foreach (ZipEntry entry in zip)
                    {
                        try
                        {
                            using (MemoryStream indStream = new MemoryStream())
                            {
                                entry.Extract(indStream); //Extracts the individual file into the memory stream

                                string processingMessage = String.Empty;
                                bool exception = true;
                                FileAttachment attch = new FileAttachment();

                                attch.FileName = entry.FileName;
                                attch.FileBytes = indStream.ToArray();
                                attch.ContentType = fileMimeType.GetContentType(indStream, entry.FileName);
                                attch.ContainsBinary = fileMimeType.ContainsBinary(attch.FileBytes);
                                attch.ProcessingMessage = processingMessage;
                                attch.FileException = exception;
                                attch.ParentZipName = zipName;

                                fileAttachments.Add(attch);
                                attch = null;
                            }
                        }
                        catch (Exception e)
                        {
                            response.Code = -1;
                            response.Message = "Failed to read zip entry " + entry.FileName;
                            response.Data = "Entry FileName: " + entry.FileName + ", " + e.Message;

                            Logger.WriteDbg(loggerInfo, "Exception reading zip entry " + entry.FileName + ". Exception: " + e.Message + Environment.NewLine + "Stack Trace: " + e.StackTrace);
                            break;
                        }
                    }
                    if (response.Code > 0)
                    {
                        if(zip.Count() > 0)
                            processZipSuccess += zip.Count();
                    }
                }
                
                if(!validFormat || response.Code == -1)
                {
                    //Save the Zip file (without the contents)
                    FileAttachment attch = new FileAttachment();

                    attch.FileName = zipName;
                    attch.FileBytes = stream.ToArray();
                    attch.ContentType = fileMimeType.GetContentType(stream, zipName);
                    attch.ContainsBinary = fileMimeType.ContainsBinary(attch.FileBytes);
                    attch.ProcessingMessage = response.Message; //Store the error message to why zip was not processed in the processing message
                    attch.FileException = false;
                    attch.ParentZipName = "";

                    fileAttachments.Add(attch);
                    attch = null;

                    //reset the response variable, if we entered this IF because of a -1 code and we did not get an exception then we simply stored the zip and no contents
                    response.Code = 1;
                    response.Message = "";
                    response.Data = "";
                    processZipFailure++; //Zip failed, or in an invalid format
                    processSuccess++; //Successfully read the file, it might not be a zip or contains a password etc
                }
            }
        }
        catch (Exception ex)
        {
            response.Code = -1;
            response.Message = "Failed to read zip file";
            response.Data = ex.Message;

            Logger.WriteDbg(loggerInfo, "Exception reading zip. Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace);
        }

        return response;
    }
}
