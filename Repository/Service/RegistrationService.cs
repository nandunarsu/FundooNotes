using Dapper;
using ModelLayer.Registration;
using Repository.Context;
using Repository.Entity;
using Repository.GlobalExceptions;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repository.Service
{
    public class RegistrationService : IRegistration
    {
        private readonly DapperContext Context;
        private readonly IAuthService _authService;
        private readonly IEmail EmailService;

        public RegistrationService(DapperContext context,IAuthService authService,IEmail email)
        {
            Context = context;
            _authService = authService;
            EmailService = email;
        }
        public bool IsValidEmail(string email)
        {

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        public async Task<bool> RegisterUser(UserRegistrationModel userRegModel)
        {

            var parametersToCheckEmailIsValid = new DynamicParameters();
            parametersToCheckEmailIsValid.Add("Email", userRegModel.Email, DbType.String);

            var querytoCheckEmailIsNotDuplicate = @"SELECT COUNT(*)FROM Users WHERE Email = @Email;";



            var query = @"INSERT INTO Users (FirstName, LastName, Email, Password)VALUES (@FirstName, @LastName, @Email, @Password);";


            var parameters = new DynamicParameters();
            parameters.Add("FirstName", userRegModel.FirstName, DbType.String);
            parameters.Add("LastName", userRegModel.LastName, DbType.String);

            //Check Emailformat Using Regex
            if (!IsValidEmail(userRegModel.Email))
            {
                throw new InvalidEmailFormatException("Invalid email format");
            }

            parameters.Add("Email", userRegModel.Email, DbType.String);

            //convert Plain Password into crytpographic String 
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegModel.Password);

            parameters.Add("Password", hashedPassword, DbType.String);

            using (var connection = Context.CreateConnection())
            {

                // Check if table exists
                bool tableExists = await connection.QueryFirstOrDefaultAsync<bool>(
                    @"
                    SELECT COUNT(*)
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Users';
                     "
                );

                // Create table if it doesn't exist
                if (!tableExists)
                {
                    await connection.ExecuteAsync(
                                                    @" CREATE TABLE Users (
                                                             UserId INT IDENTITY(1, 1) PRIMARY KEY,
                                                             FirstName NVARCHAR(100) NOT NULL,
                                                             LastName NVARCHAR(100) NOT NULL,
                                                             Email NVARCHAR(100) UNIQUE NOT NULL,
                                                             Password NVARCHAR(100) UNIQUE NOT NULL )"
                                                 );
                }

                // Check if email already exist
                bool emailExists = await connection.QueryFirstOrDefaultAsync<bool>(querytoCheckEmailIsNotDuplicate, parametersToCheckEmailIsValid);

                if (emailExists)
                {
                    throw new DuplicateEmailExceptions("Email address is already in use");
                }

                // Insert new user
                await connection.ExecuteAsync(query, parameters);
            }

            return true;
        }
        public async Task<string> UserLogin(UserLoginModel userLogin)
        {

            var parameters = new DynamicParameters();
            parameters.Add("Email", userLogin.Email);


            string query = @"SELECT UserId, Email, Password FROM Users WHERE Email = @Email;";


            using (var connection = Context.CreateConnection())
            {
                var user = await connection.QueryFirstOrDefaultAsync<Registration>(query, parameters);

                if (user == null)
                {
                    throw new NotFoundException($"User with email '{userLogin.Email}' not found.");
                }

                if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password))
                {
                    throw new InvalidPasswordException($"User with Password '{userLogin.Password}' not Found.");
                }

                //if password enterd from user and password in db match then generate Token 
                var token = _authService.GenerateJwtToken(user);
                return token;
            }
        }

        public async Task<string> ForgotPassword(ForgotPassword forgetPassword)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Email", forgetPassword.Email);

            string query = "SELECT UserId,Email,Password from Users where Email = @Email;";

            using (var connection = Context.CreateConnection())
            {
                var user = await connection.QueryFirstOrDefaultAsync<Registration>(query, parameters);
                if (user == null)
                {
                    throw new NotFoundException($"User Email {forgetPassword.Email} not found ");
                }

                var token = _authService.GenerateJwtToken(user);

                var resetpasswordurl = $"https://localhost:8080/api/User/ResetPassword?token={token}";
                var emailbody = $"Reset your password using the following link: {resetpasswordurl}";

                await EmailService.SendEmailAsync(forgetPassword.Email, "Reset password", emailbody);
                return token;
            }
        }
        public async Task<bool> ResetPassword(string token, string newPassword)
        {
            try
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                {
                    // Invalid token
                    return false;
                }

                // Extract user ID from the token
                var userId = jwtToken.Claims.First(claim => claim.Type == "userId").Value;

                // Retrieve user from the database
                var user = await GetUserByIdAsync(int.Parse(userId));

                if (user == null)
                {
                    // User not found
                    return false;
                }

                // Update user's password
                await UpdatePasswordAsync(user.UserId, newPassword);

                return true;
            }
            catch
            {
                
                return false;
            }
        }

        private async Task<Registration> GetUserByIdAsync(int userId)
        {
            var query = "SELECT * FROM Users WHERE UserId = @UserId;";
            var parameters = new { UserId = userId };

            using (var connection = Context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Registration>(query, parameters);
            }
        }

        private async Task UpdatePasswordAsync(int userId, string newPassword)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
            var query = "UPDATE Users SET Password = @Password WHERE UserId = @UserId;";
            var parameters = new { Password = hashedPassword, UserId = userId };

            using (var connection = Context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}
