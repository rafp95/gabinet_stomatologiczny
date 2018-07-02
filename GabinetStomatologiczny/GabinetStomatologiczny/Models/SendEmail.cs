using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace GabinetStomatologiczny.Models
{
    public class SendEmail
    {

        private const string email = "p.rafal123@gmail.com";
        private const string smpt = "smtp.gmail.com";
        private const string password = "PatrykRafal12.pl";
        private const int port = 587;

        public void SendMail(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress(email);
            mail.Subject = subject;

            mail.Body = body;

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = smpt; //Or Your SMTP Server Address
            smtp.Credentials = new System.Net.NetworkCredential
                 (email, password); 
            smtp.Port = port;

            //Or your Smtp Email ID and Password
            smtp.EnableSsl = true;
            smtp.Send(mail);

        }
    }
}