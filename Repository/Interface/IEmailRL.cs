﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IEmailRL
    {
        public Task<bool> SendEmailAsync(string to, string subject, string body);
        
    }
}
