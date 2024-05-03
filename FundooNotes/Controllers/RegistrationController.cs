using BussinesLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Registration;
using Repository.Entity;
using System.Reflection;
using NLog;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using ModelLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
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
                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "User Registered Successfully"

                };
                return Ok(response);
            }
            catch (Exception ex)
            {
               _logger.LogError("Invalid Request");
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
                return Ok(response);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> UserLogin(UserLoginModel userLogin)
        {
            try
            {
                
                // Authenticate the user and generate JWT token
                var Token = await _registrationbl.UserLogin(userLogin);
                //Console.WriteLine(Token);
                // return Ok(Token);
                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "User Login Successfully",
                    Data = Token

                };
                return Ok(response);

            }
            catch(Exception ex)
            {
               _logger.LogError($"Failed to login {ex.Message}");
                var response = new ResponseModel<string>
                {
                    Success=false,
                    Message = ex.Message,
                    
                };
                throw ex;
               
            }
        }
       
        [HttpPost("Forgotpassword")]

        public async Task<IActionResult> ForgotPassword(String Email)
        {
            try
            {
                _logger.LogInformation("Email Sent");
                await _registrationbl.ForgotPassword(Email);
                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "Email Sent Successfully"

                };
                return Ok(response);

            }
            catch(Exception ex)

            {
                _logger.LogError($"error occured while sending mail {ex.Message}");
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    
                };
                return Ok(response);
            }
        }

       
        [HttpPost("ResetPassword")]

        public async Task<IActionResult> ResetPassword(String otp, String Newpassword)
        {
            try
            {
                _logger.LogInformation("Password reset successful");
                await _registrationbl.ResetPassword(otp, Newpassword);
                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "Password Reset done"

                };
                return Ok(response);
            }
            catch(Exception ex)
            {
                var response = new ResponseModel<string>
                {
                    Success=false,
                    Message = ex.Message,
                    Data = null 
                };
                return Ok(response);
            }
            
        }
      
        [HttpGet]

        public async Task<IActionResult> GetUserDetails()
        {
            try
            {
                
                var registration = await _registrationbl.GetUserDetails();
                _logger.LogInformation("got details");
                var response = new ResponseModel<IEnumerable<Registration>>
                {
                    Success = true,
                    Message = "User Details are:",
                    Data = registration

                };
                return Ok(response);
            }
            catch (Exception ex)
            {

                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null // Ensure Data is null in case of error
                };
                return BadRequest(response);
            }
        }
        
        [HttpDelete("{firstname}")]
        public async Task<IActionResult> DeleteUser(string firstname)
        {
            try
            {
                await _registrationbl.DeleteUser(firstname);

                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "User Deleted"

                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null 
                };
                return Ok(response);
            }
        }
       
        [HttpPut("{firstname}")]
        public async Task<IActionResult> UpdateUser(string firstname, string lastname, string email, string password)
        {
            try
            {
                await _registrationbl.UpdateUser(firstname, lastname, email, password);
                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "User Updated"

                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured while updated{ex.Message}");
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null 
                };
                return Ok(response);
            }
        }
    }
}
