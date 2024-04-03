using BussinesLayer.Interface;
using ModelLayer.Registration;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Service
{
    public class RegistrationServicebl : IRegistrationbl
    {
        private readonly IRegistration _registration;

        public RegistrationServicebl(IRegistration registration)
        {
            _registration = registration;
        }
        public Task<bool> RegisterUser(UserRegistrationModel userRegistrationModel)
        {

            return _registration.RegisterUser(userRegistrationModel);
        }
        public Task<string> UserLogin(UserLoginModel userLogin)
        {
            return _registration.UserLogin(userLogin);
        }
        public Task<string> ForgotPassword(ForgotPassword forgetPassword)
        {
            return _registration.ForgotPassword(forgetPassword);
        }
        public Task<bool> ResetPassword(string token, string newPassword)
        {
            return (_registration.ResetPassword(token, newPassword));
        }

    }
}
