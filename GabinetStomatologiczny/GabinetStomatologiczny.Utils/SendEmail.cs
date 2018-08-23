using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace GabinetStomatologiczny.Utils
{
    public class SendEmail
    {

        public async Task SendMail(string to, string subject, string body)
        {

            var smtpCli = new SmtpClient();
            var credential = (System.Net.NetworkCredential)smtpCli.Credentials;

            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress(credential.UserName);
            mail.Subject = subject;

            mail.Body = body;

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = smtpCli.Host; //Or Your SMTP Server Address
            smtp.Credentials = new System.Net.NetworkCredential
                 (credential.UserName, credential.Password);
            smtp.Port = smtpCli.Port;

            //Or your Smtp Email ID and Password
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(mail);

        }
    }
}