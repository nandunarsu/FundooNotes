using BussinesLayer.Interface;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Service
{
    public class EmailServicebl : Interface.IEmail
    {
        private readonly Repository.Interface.IEmailRL emailrepo;

        public EmailServicebl(Repository.Interface.IEmailRL _emailrepo)
        {
            this.emailrepo = _emailrepo;
        }

        public Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            return emailrepo.SendEmailAsync(to, subject, body);
        }
    }
}
