using BussinesLayer.Interface;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Service
{
    public class EmailServicebl : IEmailbl
    {
        private readonly IEmail emailrepo;

        public EmailServicebl(IEmail _emailrepo)
        {
            this.emailrepo = _emailrepo;
        }

        public Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            return emailrepo.SendEmailAsync(to, subject, body);
        }
    }
}
