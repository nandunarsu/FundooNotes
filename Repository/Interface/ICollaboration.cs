using ModelLayer;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ICollaboration
    {
        public Task<bool> AddCollaborator(int noteid, CollaborationRequestModel model, int userId);
        public Task RemoveCollaborator(int NoteId, CollaborationRequestModel Request, int UserId);

        public Task<IEnumerable<object>> GetCollaborationbyid(int CollabId);
    }
}
