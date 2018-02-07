/*
 * https://github.com/jstedfast/MailKit
 * http://jstedfast.github.io/MailKit/docs/MailKit.Net.Smtp/index.html
 */

using MailKit.Net.Smtp;
using MimeKit;
using System;

public class SendMailKitSMTP
{
    //TODO Implement Attachment using overloaded method
    public StandardResponse SendSMTP(LoggerInfo loggerInfo, string hostName, int port, string username, string password, string fromAddress, string fromAddressReadable, string toAddress, string toAddressReadable, string subject, string bodyText, int timeout)
    {
        StandardResponse response = new StandardResponse();
        SmtpClient client = new SmtpClient();
        MimeMessage message = new MimeMessage();
        
        try
        {
            if (String.IsNullOrEmpty(fromAddressReadable)) fromAddressReadable = fromAddress;
            if (String.IsNullOrEmpty(toAddressReadable)) toAddressReadable = toAddress;

            message.From.Add(new MailboxAddress(fromAddressReadable,fromAddress));
            message.To.Add(new MailboxAddress(toAddressReadable, toAddress));
            message.Subject = subject;
            
            if(!String.IsNullOrEmpty(bodyText))
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
        catch(Exception ex)
        {
            response.Code = -1;
            response.Message = "Failed to Send SMTP Message";
            response.Data = ex.Message;
            Logger.Write(loggerInfo,"Failed to Send SMTP Message: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace);
        }

        return response;
    }
}

