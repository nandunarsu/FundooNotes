using BussinesLayer.Interface;
using ModelLayer;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Service
{
    public class CollaborationServicebl : Interface.ICollaboration
    {
        private readonly Repository.Interface.ICollaboration _collaboration;

        public CollaborationServicebl(Repository.Interface.ICollaboration collaboration)
        {
            this._collaboration = collaboration;
        }
        public Task<bool> AddCollaborator(int noteid, CollaborationRequestModel model, int userId)
        {
            return _collaboration.AddCollaborator(noteid, model, userId);
        }
        public Task RemoveCollaborator(int CollabId)
        {
            return _collaboration.RemoveCollaborator(CollabId);
        }
        public Task<IEnumerable<object>> GetCollaborationbyid(int CollabId)
        {
            return _collaboration.GetCollaborationbyid(CollabId);
        }
    }
}
