using Dapper;
using ModelLayer.Label;
using Repository.Context;
using Repository.Entity;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Service
{
    public class LabelRepository : ILabelRL
    {
        private readonly DapperContext _Context;
        public LabelRepository(DapperContext context)
        {
            _Context = context;
        }
        public async Task CreateLabel(CreateLabel label, int UserId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("LabelName", label.LabelName, DbType.String);
            parameter.Add("NoteId", label.NoteId, DbType.Int64);
            parameter.Add("UserId",UserId, DbType.Int64);

            var query = "INSERT INTO Label (LabelName,NoteId,UserId) VALUES (@LabelName,@NoteId,@UserId);";

            using(var connection = _Context.CreateConnection())
            {
                await connection.ExecuteAsync(query,parameter);
            }
        }
        public async Task DeleteLabel(int LabelId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("LabelId", LabelId, DbType.Int64);

            var query = "DELETE FROM Label WHERE LabelId = @LabelId;";

            using (var connection = _Context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameter);
            }
        }
        public async Task UpdateLabel(CreateLabel label, int LabelId,int UserId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("LabelId", LabelId, DbType.Int64);
            parameter.Add("LabelName", label.LabelName, DbType.String);
            parameter.Add("NoteId", label.NoteId, DbType.Int64);
            parameter.Add("UserId", UserId, DbType.Int64);

            var query = "UPDATE Label SET LabelName =@LabelName ,NoteId = @NoteId,UserId = @UserId WHERE LabelId = @LabelId;";

            using (var connection = _Context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameter);
            }
        }
        public async Task<IEnumerable<LabelEntity>> GetAllLabelbyId()
        {
           

            var query = "SELECT * FROM Label;";

            using(var connection = _Context.CreateConnection())
            {
                var Label = await connection.QueryAsync<LabelEntity>(query);
                return Label.ToList();
            }
        }

        public async Task<IEnumerable<object>> GetAllNotesbyId(int LabelId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("LabelId", LabelId, DbType.Int64);

            var query = "select n.NoteId,l.LabelId,n.Title as Notestitle,n.Description as NotesDescription,n.Colour,n.IsArchived,n.IsDeleted,l.LabelName from Notes n inner join  Label l on n.NoteId = l.NoteId where l.LabelId = @LabelId;";
            using(var connection = _Context.CreateConnection()) {
                var Label = await connection.QueryAsync<object>(query, parameter);
                return Label.ToList();
            }
        }

    }
}
