using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class MailStorage
{
    private string msgId, emailBodyPlain, emailBodyRich, emailBodyHtml, subjectLine, determinedReply, toAddress, atachmentTypes, personName, attachmentNames;
    private int numberOfAttachments, messageType;
    private DateTime dateReceived;
    private bool replied, includeID, includedIDinPast;

    public DateTime DateReceived
    {
        get { return dateReceived; }
        set { dateReceived = value; }
    }
    public string EmailBodyHtml
    {
        get { return emailBodyHtml; }
        set { emailBodyHtml = value; }
    }
    public string EmailBodyPlain
    {
        get { return emailBodyPlain; }
        set { emailBodyPlain = value; }
    }
    public string EmailBodyRich
    {
        get { return emailBodyRich; }
        set { emailBodyRich = value; }
    }
    public string MsgId
    {
        get { return msgId; }
        set { msgId = value; }
    }
    public string SubjectLine
    {
        get { return subjectLine; }
        set { subjectLine = value; }
    }
    public string DeterminedReply
    {
        get { return determinedReply; }
        set { determinedReply = value; }
    }
    public string ToAddress
    {
        get { return toAddress; }
        set { toAddress = value; }
    }
    public List<string> ToAddressList
    {
        get
        {
            List<string> addresses = new List<string>();

            string[] split = toAddress.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            addresses = split.ToList();

            return addresses;
        }
    }
    public string AtachmentTypes
    {
        get { return atachmentTypes; }
        set { atachmentTypes = value; }
    }
    public string PersonName
    {
        get { return personName; }
        set { personName = value; }
    }
    public int NumberOfAttachments
    {
        get { return numberOfAttachments; }
        set { numberOfAttachments = value; }
    }
    public int MessageType
    {
        get { return messageType; }
        set { messageType = value; }
    }
    public bool Replied
    {
        get { return replied; }
        set { replied = value; }
    }
    public string AttachmentNames
    {
        get { return attachmentNames; }
        set { attachmentNames = value; }
    }
    public bool IncludeID
    {
        get { return includeID; }
        set { includeID = value; }
    }
    public bool IncludedIDinPast
    {
        get { return includedIDinPast; }
        set { includedIDinPast = value; }
    }

    public MailStorage()
    {
        msgId = emailBodyPlain = emailBodyRich = emailBodyHtml = subjectLine = determinedReply = toAddress = atachmentTypes = personName = attachmentNames = String.Empty;
        numberOfAttachments = messageType = 0;
        dateReceived = new DateTime(1900,01,01);
        replied = includeID = includedIDinPast = false;
    }
}