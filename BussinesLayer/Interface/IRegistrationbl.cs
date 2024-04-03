using ModelLayer.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Interface
{
    public interface IRegistrationbl
    {
        public Task<bool> RegisterUser(UserRegistrationModel userRegistrationModel);
        public Task<string> UserLogin(UserLoginModel userLogin);
        public Task<string> ForgotPassword(ForgotPassword forgetPassword);

        public Task<bool> ResetPassword(string token, string newPassword);

    }
}
