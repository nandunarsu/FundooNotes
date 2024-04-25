using ModelLayer;
using ModelLayer.Collaboration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Interface
{
    public interface ICollaboration 
    {
        public Task<bool> AddCollaborator(int noteid, CollaborationRequestModel model, int userId);
        public Task RemoveCollaborator(int CollabId);

        public Task<IEnumerable<CollabInfoModel>> GetCollaboration();


    }
}
