using Dapper;
using ModelLayer.Notes;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interface;

namespace Repository.Service
{
    public class NotesService : INotes
    {
        private readonly DapperContext _Context;

        public NotesService(DapperContext context)
        {
            _Context = context;
        }
        public async Task CreateNote(CreateNoteRequest createNoteRequest)
        {
            var parameter = new DynamicParameters();
            parameter.Add("Description", createNoteRequest.Description, DbType.String);
            parameter.Add("Title", createNoteRequest.Title, DbType.String);
            parameter.Add("Colour", createNoteRequest.Colour, DbType.String);
            parameter.Add("IsArchived", createNoteRequest.IsArchived, DbType.Boolean);
            parameter.Add("IsDeleted", createNoteRequest.IsDeleted, DbType.Boolean);
            parameter.Add("UserId", createNoteRequest.UserId, DbType.String);

            var insertquery = @"INSERT INTO Notes (Description, [Title], Colour, IsArchived, IsDeleted, UserId) VALUES (@Description, @Title, @Colour, @IsArchived, @IsDeleted, @UserId);";

            using (var connection = _Context.CreateConnection())
            {
                await connection.ExecuteAsync(insertquery, parameter);
            }
        }
        public async Task<IEnumerable<NoteResponse>> GetAllNotebyuserid(int userId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("UserId", userId, DbType.String);
            var query = "SELECT * FROM Notes where UserId = @userId;";
            using (var connection = _Context.CreateConnection())
            {
                var note = await connection.QueryAsync<NoteResponse>(query, parameter);
                return note.ToList();
            }
        }

        public async Task UpdateNote(CreateNoteRequest updatedNote, int NoteId)
        {

            var parameter = new DynamicParameters();
            parameter.Add("Description", updatedNote.Description, DbType.String);
            parameter.Add("Title", updatedNote.Title, DbType.String);
            parameter.Add("Colour", updatedNote.Colour, DbType.String);
            parameter.Add("IsArchived", updatedNote.IsArchived, DbType.Boolean);
            parameter.Add("IsDeleted", updatedNote.IsDeleted, DbType.Boolean);
            parameter.Add("Noteid", NoteId, DbType.Int64);
            var query = "UPDATE Notes SET  Description=@Description,Title=@Title,Colour= @Colour,IsArchived=@IsArchived,IsDeleted=@Isdeleted where NoteId = @Noteid;";
            using (var connection = _Context.CreateConnection())
            {

                await connection.ExecuteAsync(query, parameter);
            }
        }
        public async Task DeleteNote(int noteId, int userId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("Noteid", noteId, DbType.Int64);
            parameter.Add("userid", userId, DbType.Int64);
            var query = "DELETE FROM Notes where NoteId=@noteId and UserId = @userId;";
            using (var connection = _Context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameter);

            }

        }
        public async Task<IEnumerable<NoteResponse>> GetAllNoteAsync(int userId)
        {
            var selectQuery = "SELECT * FROM Note WHERE UserId = @UserId AND IsDeleted = 0 AND IsArchived = 0";


            using (var connection = _Context.CreateConnection())
            {
                try
                {
                    var notes = await connection.QueryAsync<NoteResponse>(selectQuery, new { UserId = userId });
                    return notes.Reverse().ToList();
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while retrieving notes from the database.", ex);
                }
            }
        }
    }
}
