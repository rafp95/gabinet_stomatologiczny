using GabinetStomatologiczny.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.Services
{
    public interface ISendEmailService
    {
        Task SendMail(string to, string subject, string body);
    }
    public class SendEmailService : ISendEmailService
    {
        private readonly ISendEmailConfig se;

        public SendEmailService(ISendEmailConfig se)
        {
            this.se = se;
        }

        public async Task SendMail(string to, string subject, string body)
        {
            await this.se.SendMail(to, subject, body);
        }
    }
}
