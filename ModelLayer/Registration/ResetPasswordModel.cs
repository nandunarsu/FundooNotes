﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Registration
{
    public class ResetPasswordModel
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
