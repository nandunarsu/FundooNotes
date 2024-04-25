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
using Microsoft.Extensions.Caching.Distributed;
using Repository.GlobalExceptions;

namespace Repository.Service
{
    public class NotesService : NotesInterface
    {
        private readonly DapperContext _Context;
        private readonly IDistributedCache _cache;


        public NotesService(DapperContext context,IDistributedCache cache)
        {
            _Context = context;
            _cache = cache;
        }
        public bool RemoveData(string key)
        {
     
            var cachedData = _cache.GetString(key);
            if (cachedData != null)
            {
                _cache.Remove(key);
                return true;
            }
            return false;
        }

        public async Task CreateNote(CreateNoteRequest createNoteRequest,int Userid)
        {
            var parameter = new DynamicParameters();
            parameter.Add("Description", createNoteRequest.Description, DbType.String);
            parameter.Add("Title", createNoteRequest.Title, DbType.String);
            parameter.Add("Colour", createNoteRequest.Colour, DbType.String);
            parameter.Add("IsArchived", createNoteRequest.IsArchived, DbType.Boolean);
            parameter.Add("IsDeleted", createNoteRequest.IsDeleted, DbType.Boolean);
            parameter.Add("UserId", Userid, DbType.Int64);

            var insertquery = @"INSERT INTO Notes (Description, [Title], Colour, IsArchived, IsDeleted, UserId) VALUES (@Description, @Title, @Colour, @IsArchived, @IsDeleted, @UserId);";

            using (var connection = _Context.CreateConnection())
            {
            await connection.ExecuteAsync(insertquery, parameter);

            }
            await _cache.RemoveAsync($"Notes_{Userid}");
        }
        public async Task<NoteResponse> GetAllNotebyuserid(int NoteId,int UserId)
        {
            var selectQuery = "SELECT * FROM Notes WHERE NoteId = @NoteId AND UserId = @UserId";

            using (var connection = _Context.CreateConnection())
            {
                try
                {
                    var note = await connection.QuerySingleOrDefaultAsync<NoteResponse>(selectQuery, new { UserId = UserId, NoteId = NoteId });

                    if (note == null)
                    {
                        throw new NotFoundException($"Note with NoteId '{NoteId}' does not exist for User with UserId '{UserId}'.");
                    }

                    return note;
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task UpdateNote(int NoteId, int UserId, CreateNoteRequest updatedNote)
        {
            var parameter = new DynamicParameters();
            parameter.Add("Description", updatedNote.Description, DbType.String);
            parameter.Add("Title", updatedNote.Title, DbType.String);
            parameter.Add("Colour", updatedNote.Colour, DbType.String);
            parameter.Add("IsArchived", updatedNote.IsArchived, DbType.Boolean);
            parameter.Add("IsDeleted", updatedNote.IsDeleted, DbType.Boolean);
            parameter.Add("NoteId", NoteId, DbType.Int64);
            parameter.Add("UserId",UserId, DbType.Int64);
            var query = "UPDATE Notes SET Description=@Description,Title=@Title,Colour= @Colour,IsArchived=@IsArchived,IsDeleted=@Isdeleted WHERE UserId=@UserId AND NoteId = @NoteId";
            using (var connection = _Context.CreateConnection())
            {

            var note =  await connection.ExecuteAsync(query, parameter);
               
            }
            await _cache.RemoveAsync($"Notes_{UserId}");

            
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

            await _cache.RemoveAsync($"Notes_{userId}");

        }
        public async Task<IEnumerable<NoteResponse>> GetAllNoteAsync(int userId)
        {
            var selectQuery = "SELECT * FROM Notes WHERE UserId = @UserId AND IsDeleted = 0 AND IsArchived = 0";


            using (var connection = _Context.CreateConnection())
            {
                try
                {
                    var notes = await connection.QueryAsync<NoteResponse>(selectQuery, new { UserId = userId });
                    return notes.Reverse().ToList();
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
