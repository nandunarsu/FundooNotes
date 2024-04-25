using ModelLayer;
using ModelLayer.Collaboration;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ICollaborationRL
    {
        public Task<bool> AddCollaborator(int noteid, CollaborationRequestModel model, int userId);
        public Task RemoveCollaborator(int CollabId);

        public Task<IEnumerable<CollabInfoModel>> GetCollaboration();
    }
}
