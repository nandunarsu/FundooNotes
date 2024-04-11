using BussinesLayer.Interface;
using Microsoft.Win32;
using ModelLayer.Registration;
using Repository.Entity;
using Repository.GlobalExceptions;
using Repository.Interface;
using Repository.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using static System.Net.WebRequestMethods;

namespace BussinesLayer.Service
{
    public class RegistrationServicebl : Interface.IRegistration
    {
        private readonly Repository.Interface.IRegistration _registration;
        private static string otp;
        private static string mailid;
        private static Registration entity;

        public RegistrationServicebl(Repository.Interface.IRegistration registration)
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
        public Task<string> ForgotPassword(string Email)
        {
            try
            {
                entity = _registration.GetByEmailAsync(Email).Result;
            }
            catch (Exception e)
            {
                throw new UserNotFoundException("UserNotFoundByEmailId" + e.Message);
            }

            Random r = new Random();
            otp = r.Next(100000, 999999).ToString();
            mailid = Email;

            try
            {
                MailSender.sendMail(Email, otp);
                Console.WriteLine(otp);
                return Task.FromResult("OTP sent");
            }
            catch (EmailSendingException ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
                return Task.FromResult(ex.Message);
            }
        }

        public async Task<string> ResetPassword(string otp, string userpassword)
        {
            if (otp == null)
            {
                return "Generate Otp First";
            }

            // Hash the user's provided password
            string hashedUserPassword = HashPassword(userpassword);

            // Check if the hashed password matches the stored password
            if (entity != null && VerifyPassword(hashedUserPassword, entity.Password))
            {
                throw new PasswordMismatchException("Don't give the existing password");
            }

            // Check password complexity using regex
            if (!Regex.IsMatch(userpassword, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[a-zA-Z\d!@#$%^&*]{8,16}$"))
            {
                return "Password does not meet complexity requirements";
            }

            // Verify OTP
            if (RegistrationServicebl.otp != otp)
            {
                return "OTP mismatch";
            }

            // Update the password in the database with the hashed password
            int result = await _registration.UpdatePassword(mailid, hashedUserPassword);

            if (result == 1)
            {
                // Clear sensitive data
                entity = null;
                otp = null;
                mailid = null;

                return "Password changed successfully";
            }
            else
            {
                return "Password not changed";
            }
        }
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        public Task<IEnumerable<Registration>> GetUserDetails()
        {
            return _registration.GetUserDetails();
        }
        public Task DeleteUser(string firstname)
        {
            return _registration.DeleteUser(firstname);
        }

        public Task UpdateUser(string firstname, string lastname, string email, string password)
        {
            return _registration.UpdateUser(firstname, lastname, email, password);
        }

    }
}
