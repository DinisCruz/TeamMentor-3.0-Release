﻿using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.Network;

namespace TeamMentor.CoreLib
{
    public class SendEmails
    {   
        public static List<EmailMessage> Sent_EmailMessages { get; set; }
     
        public string From          { get; set; }
        public string Smtp_Server   { get; set; }
        public string Smtp_UserName { get; set; }
        public string Smtp_Password { get; set; }

        static SendEmails()
        {
            Sent_EmailMessages = new List<EmailMessage>();
        }


        public SendEmails()
        {            
            From = "TM_Security_Dude@securityinnovation.com";
            if (TM_Xml_Database.Current.notNull())
            { 
                var secretData = TM_Xml_Database.Current.UserData.SecretData;
                if (secretData.notNull())
                {
                    Smtp_Server = secretData.SMTP_Server;
                    Smtp_UserName = secretData.SMTP_UserName;
                    Smtp_Password = secretData.SMTP_Password;
                }
            }
        }
        public SendEmails(string smtpServer, string smtpUserName, string smtpPassword ) : this()
        {
            Smtp_Server     = smtpServer;
            Smtp_UserName   = smtpUserName;
            Smtp_Password   = smtpPassword;
        }
        

        public bool send_TestEmail()
        {
            return send("Test Email", "From TeamMentor");
        }        
        
        public bool send(string subject, string message)
        {
            return send(From, subject, message, false);
        }
        public bool send(string to, string subject, string message)
        {
            return send(to, subject, message, false);
        }

        public bool send(string to, string subject, string message, bool htmlMessage)
        {
            var emailMessage = new EmailMessage 
                                    {
                                        To = to, 
                                        From = this.From, 
                                        Subject = subject, 
                                        Message = message, 
                                        HtmlMessage = htmlMessage
                                    };
            return send(emailMessage);
        }
        
        //Refactor into SMTP class
        public bool send(EmailMessage emailMessage)
        {
            Sent_EmailMessages.Add(emailMessage);
            try
            {
                if (this.offlineMode())
                {                    
                    return false;   
                }
                "Sending email:\n  to: {0}\n  from: {0}\n  subject: {0} ".info(emailMessage.To, emailMessage.Subject, emailMessage.Message);
                var mailMsg = new MailMessage();

                emailMessage.Message += "Send by TeamMentor. ".format().lineBefore().lineBefore();
                // To
                mailMsg.To.Add(new MailAddress(emailMessage.To));
                // From
                mailMsg.From = new MailAddress(emailMessage.From);
                // Subject and multipart/alternative Body
                mailMsg.Subject = emailMessage.Subject;                
                if (emailMessage.HtmlMessage)
                {
                    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(emailMessage.Message, null, MediaTypeNames.Text.Plain));
                    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(emailMessage.Message, null, MediaTypeNames.Text.Html));
                }
                else
                {
                    mailMsg.Body = emailMessage.Message;
                }
                // Init SmtpClient and send
                var smtpClient = new SmtpClient(Smtp_Server, 587);
                var credentials = new System.Net.NetworkCredential(Smtp_UserName, Smtp_Password);
                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);                                
                return true;
            }
            catch (Exception ex)
            {
                ex.log();
                return false;
            }
        }
        public static void SendEmailToTM(string subject, TMUser tmUser)
        {
            var tmMessage =
@"New TeamMentor User Created:

    UserId: {0}
    UserName: {1}
    Company: {2}
    Email: {3}
    FirstName: {4}
    LastName: {5}
    Title: {6}
    Creation Date: {7}".format(tmUser.UserID, 
                              tmUser.UserName,
                              tmUser.Company,
                              tmUser.EMail,
                              tmUser.FirstName,
                              tmUser.LastName,
                              tmUser.Title,
                              tmUser.Stats.CreationDate.ToLongDateString());

            SendEmailToTM(subject, tmMessage);
            if (tmUser.EMail.valid())
            {

                var userMessage =
@"Hi {0} {1} Welcome to TeamMentor.

You can login with your {2} account at http://www.teammentor.net

TeamMentor Team.
                ".format(tmUser.FirstName, tmUser.LastName, tmUser.UserName);
                SendEmailToEmail(tmUser.EMail, "Welcome to TeamMentor", userMessage);
                userMessage = "(sent to: {0})\n\n{1}".format(tmUser.EMail, userMessage);
                SendEmailToTM("(user email) Welcome to TeamMentor", userMessage);
            }

        }

        [Assert_Admin]
        public static bool SendLoginTokenToUser(TMUser tmUser)
        {
            try
            {                 
var userMessage =
@"Hi {0} {1} A Login token was requested for your account.

You can login with your {2} account using http://www.teammentor.net/rest/{2}/{3})

TeamMentor Team.
             ".format(tmUser.FirstName, tmUser.LastName, tmUser.UserName, tmUser.current_SingleUseLoginToken());
             SendEmailToEmail(tmUser.EMail, "TeamMentor Login Link", userMessage);
             userMessage = "(sent to: {0})\n\n{1}".format(tmUser.EMail, userMessage);
             SendEmailToTM("(user email) Welcome to TeamMentor", userMessage);
                return true;
            }
            catch (Exception ex)
            {
                ex.log();
            }
            return false;

        }
        public static void SendEmailToTM(string subject, string message)
        {
            O2Thread.mtaThread(
                ()=>{
                        try
                        {
                            new SendEmails().send(subject, message);
                        }
                        catch (Exception ex)
                        {
                            ex.log("in SendEmailToTM");
                        }
                    });
        }
        public static void SendEmailToEmail(string to, string subject, string message)
        {
            O2Thread.mtaThread(
                ()=>{
                        try
                        {
                            new SendEmails().send(to, subject, message);
                        }
                        catch (Exception ex)
                        {
                            ex.log("in SendEmailToTM");
                        }
                    });
        }
    }

    public static class SendEmail_ExtensionMethods
    {
        public static bool offlineMode(this SendEmails sendEmails)
        {
            return sendEmails.Smtp_Server.notValid() ||
                   sendEmails.Smtp_UserName.notValid() ||
                   sendEmails.Smtp_Password.notValid() ||
                   new Web().online().isFalse();  
        }
    }
}
