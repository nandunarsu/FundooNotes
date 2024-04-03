using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Interface
{
    public interface ICollaborationbl 
    {
        public Task<bool> AddCollaborator(int noteid, CollaborationRequestModel model, int userId);
        public Task RemoveCollaborator(int NoteId, CollaborationRequestModel Request, int UserId);

        public Task<IEnumerable<object>> GetCollaborationbyid(int CollabId);
    }
}
