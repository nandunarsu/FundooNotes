using BussinesLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Registration;
using System.Reflection;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationbl _registrationbl;

        public RegistrationController(IRegistrationbl registrationbl)
        {
            _registrationbl = registrationbl;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> UserRegistration(UserRegistrationModel user)
        {
            try
            {
                await _registrationbl.RegisterUser(user);
                return Ok("Registration Successful");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> UserLogin(UserLoginModel userLogin)
        {
            try
            {
                // Authenticate the user and generate JWT token
                var Token = await _registrationbl.UserLogin(userLogin);
                Console.WriteLine("Login Successful");
                return Ok(Token);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPost("Forgotpassword")]

        public async Task<IActionResult> ForgotPassword(ForgotPassword forgetPassword)
        {
            try
            {
                string token = await _registrationbl.ForgotPassword(forgetPassword);
                if (token != null)
                {
                    return Ok("Email Sent");
                }
                else
                {
                    return BadRequest();
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ResetPassword")]

        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                var success = await _registrationbl.ResetPassword(model.Token, model.NewPassword);

                if (success!=null)
                {
                    return Ok("Password has been reset successfully.");
                }
                else
                {
                    return BadRequest("Failed to reset password. Invalid token or user.");
                }



            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
    }
}
