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
        void SendMail(string to, string subject, string body);
    }
    public class SendEmailService : ISendEmailService
    {
        private SendEmail _se;

        private SendEmail se
        {
            get
            {
                if(_se == null)
                {
                    _se = new SendEmail(); 
                }
               return _se;
            }
            
        }


        public void SendMail(string to, string subject, string body)
        {
            this.se.SendMail(to,subject,body);
        }
    }
}
