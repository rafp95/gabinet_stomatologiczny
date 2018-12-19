using GabinetStomatologiczny.Services;
using GabinetStomatologiczny.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Gabinet.Tests
{
    [TestClass]
    public class SendEmailTest
    {
      
        private async Task SendMail(string to, string subject, string body)
        {

            var smtpCli = new SmtpClient();
            var credential = (System.Net.NetworkCredential)smtpCli.Credentials;

            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress("p.rafal123@gmail.com");
            mail.Subject = subject;

            mail.Body = body;

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
            smtp.Credentials = new System.Net.NetworkCredential
                 ("p.rafal123@gmail.com", "*************"); //login and password
            smtp.Port = 587;

            //Or your Smtp Email ID and Password
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(mail);

        }

        [TestMethod]
        public async Task SendEmail()
        {
            
            var conf = new Mock<ISendEmailConfig>();
            var send = new Mock<SendEmailService>(conf.Object);

            await SendMail("patrykrafal33@o2.pl", "test", "test");

            Assert.IsNotNull(send);
        }
    }
}
