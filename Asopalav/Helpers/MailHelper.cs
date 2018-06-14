using System;
using System.Configuration;
using System.Net.Mail;

/// <summary>
/// Sends an mail message
/// </summary>
/// <param name="from">Sender address</param>
/// <param name="to">Recepient address</param>
/// <param name="bcc">Bcc recepient</param>
/// <param name="cc">Cc recepient</param>
/// <param name="subject">Subject of mail message</param>
/// <param name="body">Body of mail message</param>
namespace Asopalav.Helpers
{
    public class MailHelper
    {
        public MailHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static void SendMailMessage(string from, string to, string bcc, string cc, string subject, string body)
        {

            MailMessage mMailMessage = new MailMessage();

            mMailMessage.From = new MailAddress(from, ConfigurationManager.AppSettings["MailFromText"]);

            //mMailMessage.To.Add(new MailAddress(to));

            //Spliting to

            char[] tosplitter = { ';' };
            string[] tos = to.Split(tosplitter);
            foreach (string d in tos)
            {
                mMailMessage.To.Add(new MailAddress(d));
            }
            try
            {
                if ((bcc != null) && (bcc != string.Empty))
                {
                    //Spliting to bcc
                    char[] bccsplitter = { ';' };
                    string[] bccs = bcc.Split(bccsplitter);
                    foreach (string d in bccs)
                    {
                        mMailMessage.Bcc.Add(new MailAddress(d));
                    }
                    //mMailMessage.CC.Add(new MailAddress(cc));
                }
            }
            catch (Exception exp)
            {
                string str = exp.Message.ToString();
                throw exp;
            }
            try
            {
                if ((cc != null) && (cc != string.Empty))
                {
                    //Spliting to cc
                    char[] ccsplitter = { ';' };
                    string[] ccs = cc.Split(ccsplitter);
                    foreach (string d in ccs)
                    {
                        mMailMessage.CC.Add(new MailAddress(d));
                    }
                    //mMailMessage.CC.Add(new MailAddress(cc));
                }
            }
            catch (Exception exp)
            {
                string str = exp.Message.ToString();
                throw exp;
            }

            mMailMessage.Subject = subject;

            mMailMessage.Body = body;

            mMailMessage.IsBodyHtml = true;

            mMailMessage.Priority = MailPriority.Normal;

            //adding authentication part

            SmtpClient mSmtpClient = new SmtpClient();
            mSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mSmtpClient.Host = ConfigurationManager.AppSettings["MailServer"];//"smtp.gmail.com";
            mSmtpClient.EnableSsl = true;
            mSmtpClient.Port = 587;

            try
            {
                mSmtpClient.Send(mMailMessage);
            }
            catch (Exception exp)
            {
                string str = exp.Message.ToString();
                throw exp;
            }
        }


        public static void SendMailMessageWithAttachment(string from, string to, string bcc, string cc, string subject, string body, string file)
        {
            MailMessage mMailMessage = new MailMessage();

            mMailMessage.From = new MailAddress(from);

            //mMailMessage.To.Add(new MailAddress(to));

            //Spliting to

            char[] tosplitter = { ';' };
            string[] tos = to.Split(tosplitter);
            foreach (string d in tos)
            {
                mMailMessage.To.Add(new MailAddress(d));
            }
            try
            {
                if ((bcc != null) && (bcc != string.Empty))
                {
                    //Spliting to bcc
                    char[] bccsplitter = { ';' };
                    string[] bccs = bcc.Split(bccsplitter);
                    foreach (string d in bccs)
                    {
                        mMailMessage.Bcc.Add(new MailAddress(d));
                    }
                    //mMailMessage.CC.Add(new MailAddress(cc));
                }
            }
            catch (Exception exp)
            {
                string str = exp.Message.ToString();
                throw exp;
            }

            try
            {
                if ((cc != null) && (cc != string.Empty))
                {
                    //Spliting to cc
                    char[] ccsplitter = { ';' };
                    string[] ccs = cc.Split(ccsplitter);
                    foreach (string d in ccs)
                    {
                        mMailMessage.CC.Add(new MailAddress(d));
                    }
                    //mMailMessage.CC.Add(new MailAddress(cc));
                }
            }
            catch (Exception exp)
            {
                string str = exp.Message.ToString();
                throw exp;
            }
            mMailMessage.Subject = subject;

            mMailMessage.Body = body;


            if (file != "EmptyFile")
            {
                Attachment attachfile = new Attachment(file);

                mMailMessage.Attachments.Add(attachfile);
            }

            mMailMessage.IsBodyHtml = true;

            mMailMessage.Priority = MailPriority.Normal;

            //adding authentication part

            SmtpClient mSmtpClient = new SmtpClient();
            mSmtpClient.EnableSsl = true;
            mSmtpClient.UseDefaultCredentials = false;
            mSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mSmtpClient.Host = ConfigurationManager.AppSettings["MailServer"]; //"smtp.gmail.com";
            mSmtpClient.Port = 587;

            try
            {
                mSmtpClient.Send(mMailMessage);

                mMailMessage.Dispose();
            }
            catch (Exception exp)
            {
                string str = exp.Message.ToString();
                throw exp;

            }
        }


        public static void SendMailMessageOnbehalf(string from, string fromDisplayName, string to, string bcc, string cc, string subject, string body)
        {

            MailMessage mMailMessage = new MailMessage();

            mMailMessage.From = new MailAddress(from, ConfigurationManager.AppSettings["MailFromText"] +" On Behalf Of " + fromDisplayName + "<" + from + ">");
            mMailMessage.ReplyToList.Add(new MailAddress(from, fromDisplayName));

            //mMailMessage.To.Add(new MailAddress(to));

            //Spliting to

            char[] tosplitter = { ';' };
            string[] tos = to.Split(tosplitter);
            foreach (string d in tos)
            {
                mMailMessage.To.Add(new MailAddress(d));
            }
            try
            {
                if ((bcc != null) && (bcc != string.Empty))
                {
                    //Spliting to bcc
                    char[] bccsplitter = { ';' };
                    string[] bccs = bcc.Split(bccsplitter);
                    foreach (string d in bccs)
                    {
                        mMailMessage.Bcc.Add(new MailAddress(d));
                    }
                    //mMailMessage.CC.Add(new MailAddress(cc));
                }
            }
            catch (Exception exp)
            {
                string str = exp.Message.ToString();
                throw exp;
            }
            try
            {
                if ((cc != null) && (cc != string.Empty))
                {
                    //Spliting to cc
                    char[] ccsplitter = { ';' };
                    string[] ccs = cc.Split(ccsplitter);
                    foreach (string d in ccs)
                    {
                        mMailMessage.CC.Add(new MailAddress(d));
                    }
                    //mMailMessage.CC.Add(new MailAddress(cc));
                }
            }
            catch (Exception exp)
            {
                string str = exp.Message.ToString();
                throw exp;
            }
            mMailMessage.Subject = subject;

            mMailMessage.Body = body;

            mMailMessage.IsBodyHtml = true;

            mMailMessage.Priority = MailPriority.Normal;

            //adding authentication part

            SmtpClient mSmtpClient = new SmtpClient();
            mSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mSmtpClient.Host = ConfigurationManager.AppSettings["MailServer"];//"smtp.gmail.com";
            mSmtpClient.EnableSsl = true;
            mSmtpClient.Port = 587;

            try
            {
                mSmtpClient.Send(mMailMessage);
            }
            catch (Exception exp)
            {
                string str = exp.Message.ToString();
                throw exp;
            }
        }

    }
}
