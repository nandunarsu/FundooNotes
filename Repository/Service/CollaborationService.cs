using Dapper;
using ModelLayer;
using Repository.Context;
using Repository.GlobalExceptions;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repository.Service
{
    public class CollaborationService : ICollaboration
    {
        private readonly DapperContext _Context;
        private readonly IEmail EmailService;

        public CollaborationService(DapperContext context, IEmail emailService)
        {
            _Context = context;
            EmailService = emailService;
        }
        private bool Isvalid(string email)
        {
            string pattern = @"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z]{3,}$";
            return Regex.IsMatch(email, pattern);
        }
        public async Task<bool> AddCollaborator(int noteid, CollaborationRequestModel model, int userId)
        {
            var query = "INSERT INTO Collaboration(UserId,NoteId,CollabEmail) VALUES (@UserId,@NoteId,@CollabEmail);";
            var parameter = new DynamicParameters();

            parameter.Add("Noteid", noteid, DbType.Int64);
            parameter.Add("Userid", userId, DbType.Int64);

            if (!Isvalid(model.Email))
            {
                throw new InvalidEmailFormatException("Invalid Email Format");
            }
            parameter.Add("CollabEmail", model.Email, DbType.String);

            var emailexistquery = "SELECT COUNT(*) FROM Users where Email = @CollabEmail;";

            var emailExistparameter = new { CollabEmail = model.Email };
            using (var connection = _Context.CreateConnection())
            {
                int emailcount = await connection.ExecuteAsync(emailexistquery, emailExistparameter);
                if (emailcount == 0)
                {
                    throw new NotFoundException($"Collaborator with the Email '{model.Email}'is not registered user");
                }
                bool tableexist = await connection.QueryFirstOrDefaultAsync<bool>("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='Collaboration';");
                if (!tableexist)
                {

                    await connection.ExecuteAsync("CREATE TABLE Collaboration(CollaborationId int identity(1,1),UserId int FOREIGN KEY REFERENCES Users(UserId),NoteId int FOREIGN KEY REFERENCES Notes(NoteId),CollabEmail nvarchar(100) FOREIGN KEY REFERENCES Users(Email));");
                }
                else
                {
                    var rowsAffected = await connection.ExecuteAsync(query, parameter);
                    var emailBody = $"You have been added as a collaborator.";
                    await EmailService.SendEmailAsync(model.Email, "Added as Collaborator", emailBody);
                }
                    
            }
            return true;

        }
        public async Task RemoveCollaborator(int NoteId, CollaborationRequestModel Request, int UserId)
        {
            var query = "DELETE FROM Collaboration WHERE UserId = @UserId AND NoteId = @NoteId AND CollabEmail = @CollabEmail;";
            var parameter = new DynamicParameters();
            parameter.Add("NoteId", NoteId, DbType.Int64);
            parameter.Add("UserId", UserId, DbType.Int64);
            parameter.Add("CollabEmail", Request.Email, DbType.String);
            using( var connection = _Context.CreateConnection())
            {
                 await connection.ExecuteAsync(query, parameter);
              
            }
        }
        public async Task<IEnumerable<object>> GetCollaborationbyid(int CollabId)
        {
            var query = "select * from Collaboration c join Notes n on c.NoteId = n.NoteId join Users u on c.UserId = u.UserId where c.CollaborationId = @CollabId;";
            var parameter = new DynamicParameters();
            parameter.Add("CollabId",CollabId,DbType.Int64);
            using(var connection = _Context.CreateConnection())
            {
                var collab = await connection.QueryAsync<object>(query, parameter);
                return collab.ToList();
            }
        }
    
}
}
