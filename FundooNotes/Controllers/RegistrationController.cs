using BussinesLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Registration;
using Repository.Entity;
using System.Reflection;
using NLog;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistration _registrationbl;
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(IRegistration registrationbl, ILogger<RegistrationController> logger)
        {
            _registrationbl = registrationbl;
            _logger = logger; 
        }

        [HttpPost]
        public async Task<IActionResult> UserRegistration(UserRegistrationModel user)
        {
            try
            {
                await _registrationbl.RegisterUser(user);
               _logger.LogInformation("Registration successful");
                return Ok("Registration Successful");
            }
            catch (Exception ex)
            {
               _logger.LogError("Invalid Request");
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
                _logger.LogInformation("Login Successful");
                return Ok(Token);
            }
            catch(Exception ex)
            {
               _logger.LogError($"Failed to login {ex.Message}");
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPost("Forgotpassword")]

        public async Task<IActionResult> ForgotPassword(String Email)
        {
            try
            {
                _logger.LogInformation("Email Sent");
                return Ok(await _registrationbl.ForgotPassword(Email));
                
            }
            catch(Exception ex)

            {
                _logger.LogError($"error occured while sending mail {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
    

        [HttpPost("ResetPassword")]

        public async Task<IActionResult> ResetPassword(String otp, String Newpassword)
        {
            try
            {
                _logger.LogInformation("Password reset successful");
                return Ok(await _registrationbl.ResetPassword(otp, Newpassword));
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }
        [HttpGet]

        public async Task<IActionResult> GetUserDetails()
        {
            try
            {
                
                var registration = await _registrationbl.GetUserDetails();
                _logger.LogInformation("got details");
                return Ok(registration);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("DeleteUsingFirstname")]
        public async Task<IActionResult> DeleteUser(string firstname)
        {
            try
            {
                await _registrationbl.DeleteUser(firstname);
               
                return Ok("User deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, ex);
            }
        }
        [HttpPut("Updateusingname")]
        public async Task<IActionResult> UpdateUser(string firstname, string lastname, string email, string password)
        {
            try
            {
                await _registrationbl.UpdateUser(firstname, lastname, email, password);
                return Ok("User Updated");
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured while updated{ex.Message}");
                return StatusCode(500, ex);
            }
        }
    }
}
