using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.GlobalExceptions
{
    public class InvalidEmailFormatException : Exception

    { 
        public InvalidEmailFormatException(string message) : base(message) { }
    }
}
