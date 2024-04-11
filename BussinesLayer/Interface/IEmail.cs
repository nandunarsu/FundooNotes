using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Interface
{
    public interface IEmail
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
    }
}
