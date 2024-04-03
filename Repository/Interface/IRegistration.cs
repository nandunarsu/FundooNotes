using ModelLayer.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IRegistration
    {
        public Task<bool> RegisterUser(UserRegistrationModel userRegistrationModel);
        public Task<string> UserLogin(UserLoginModel userLogin);
        public Task<string> ForgotPassword(ForgotPassword forgetPassword);
        public Task<bool> ResetPassword(string token, string newPassword);
    }
}
