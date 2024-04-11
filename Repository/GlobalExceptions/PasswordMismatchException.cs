using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.GlobalExceptions
{
    public class PasswordMismatchException : Exception
    {
        public PasswordMismatchException(string message):base(message) { }
    }
}
