using Confluent.Kafka;
using Dapper;
using ModelLayer.Registration;
using Repository.Context;
using Repository.Entity;
using Repository.GlobalExceptions;
using Repository.Interface;
using RepositoryLayer.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repository.Service
{
    public class RegistrationService : IRegistration
    {
        private readonly DapperContext Context;
        private readonly IAuthServiceRL _authService;
        private readonly IEmailRL EmailService;

        public RegistrationService(DapperContext context,IAuthServiceRL authService,IEmailRL email)
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

           
            if (!IsValidEmail(userRegModel.Email))
            {
                throw new InvalidEmailFormatException("Invalid email format");
            }

            parameters.Add("Email", userRegModel.Email, DbType.String);

            
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegModel.Password);

            parameters.Add("Password", hashedPassword, DbType.String);

            using (var connection = Context.CreateConnection())
            {

                
                bool tableExists = await connection.QueryFirstOrDefaultAsync<bool>(
                    @"
                    SELECT COUNT(*)
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Users';
                     "
                );

               
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

                
                bool emailExists = await connection.QueryFirstOrDefaultAsync<bool>(querytoCheckEmailIsNotDuplicate, parametersToCheckEmailIsValid);

                if (emailExists)
                {
                    throw new DuplicateEmailExceptions("Email address is already in use");
                }

                
                await connection.ExecuteAsync(query, parameters);
            }
            
            var registrationDetailsForPublishing = new RegistrationDetailsForPublishing(userRegModel);

            // Serialize registration details to a JSON string
            var registrationDetailsJson = Newtonsoft.Json.JsonConvert.SerializeObject(registrationDetailsForPublishing);

            // Get Kafka producer configuration
            var producerConfig = Helper.GetProducerConfig();

            // Create a Kafka producer
            using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build())
            {
                try
                {
                    // Publish registration details to Kafka topic
                    await producer.ProduceAsync("Registration-topic", new Message<Null, string> { Value = registrationDetailsJson });
                    Console.WriteLine("Registration details published to Kafka topic.");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Failed to publish registration details to Kafka topic: {e.Error.Reason}");
                }
            }
            

            return true;
        }
        public async Task<string> UserLogin(UserLoginModel userLogin)
        {

            var parameters = new DynamicParameters();
            parameters.Add("Email", userLogin.Email);


            string query = @"SELECT * FROM Users WHERE Email = @Email;";


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

        //public async Task<string> ForgotPassword(ForgotPassword forgetPassword)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("Email", forgetPassword.Email);

        //    string query = "SELECT UserId,Email,Password from Users where Email = @Email;";

        //    using (var connection = Context.CreateConnection())
        //    {
        //        var user = await connection.QueryFirstOrDefaultAsync<Registration>(query, parameters);
        //        if (user == null)
        //        {
        //            throw new NotFoundException($"User Email {forgetPassword.Email} not found ");
        //        }

        //        var token = _authService.GenerateJwtToken(user);

        //        var resetpasswordurl = $"https://localhost:8080/api/User/ResetPassword?token={token}";
        //        var emailbody = $"Reset your password using the following link: {resetpasswordurl}";

        //        await EmailService.SendEmailAsync(forgetPassword.Email, "Reset password", emailbody);
        //        return token;
        //    }
        //}
        //public async Task<bool> ResetPassword(string token, string newPassword)
        //{
        //    try
        //    {
        //        var jwtHandler = new JwtSecurityTokenHandler();
        //        var jwtToken = jwtHandler.ReadToken(token) as JwtSecurityToken;

        //        if (jwtToken == null)
        //        {
        //            // Invalid token
        //            return false;
        //        }

        //        // Extract user ID from the token
        //        var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "userId")?.Value;

        //        if (string.IsNullOrEmpty(userId))
        //        {
        //            // userId claim not found in the token
        //            return false;
        //        }

        //        // Retrieve user from the database
        //        var user = await GetUserByIdAsync(int.Parse(userId));

        //        if (user == null)
        //        {
        //            // User not found in the database
        //            return false;
        //        }

        //        // Update user's password
        //        await UpdatePasswordAsync(user.UserId, newPassword);

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception for debugging
        //        Console.WriteLine($"An error occurred during password reset: {ex}");
        //        return false;
        //    }
        //}

        private async Task<Registration> GetUserByIdAsync(int userId)
        {
            var query = "SELECT * FROM Users WHERE UserId = @UserId;";
            var parameters = new { UserId = userId };

            using (var connection = Context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Registration>(query, parameters);
            }
        }
        public async Task<Registration> GetByEmailAsync(string email)
        {
            var query = "Select * from Users where Email=@email";
            using (var connection = Context.CreateConnection())
            {

                return await connection.QueryFirstAsync<Registration>(query, new { email = email });
            }
        }


            public async Task<IEnumerable<Registration>> GetUserDetails()
        {

            var query = " SELECT * FROM Users";
            using (var connection = Context.CreateConnection())
            {
                var registration = await connection.QueryAsync<Registration>(query);
                return registration.ToList();
            }

        }
        public async Task DeleteUser(string firstname)
        {
            var query = "DELETE FROM Users WHERE FirstName = @firstname;";
            var Parameter = new DynamicParameters();
            Parameter.Add("FirstName", firstname, DbType.String);
            using (var connections = Context.CreateConnection())
            {
                await connections.ExecuteAsync(query, Parameter);
            }
        }

        public async Task UpdateUser(string firstname, string lastname, string email, string password)
        {
            var query = "UPDATE Users SET  LastName = @lastname,Email = @email,Password = @password where FirstName = @firstname;";
            var Parameter = new DynamicParameters();
            Parameter.Add("Firstname", firstname, DbType.String);
            Parameter.Add("Lastname", lastname, DbType.String);
            Parameter.Add("Email", email, DbType.String);
            Parameter.Add("Password", password, DbType.String);
            using (var connection = Context.CreateConnection())
            {
                await connection.ExecuteAsync(query, Parameter);
            }
        }
        public async Task<int> UpdatePassword(string mailid, string password)
        {
            var Parameter = new DynamicParameters();
            Parameter.Add("Email", mailid, DbType.String);
            Parameter.Add("Password", password, DbType.String);
            var query = "update Users set Password = @Password where Email" +
                " = @Email";
           
            using (var connection = Context.CreateConnection())
            {
                return (await connection.ExecuteAsync(query, Parameter));
            }
            
        }
    }
}
