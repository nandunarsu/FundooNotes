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
    public class CollaborationServicebl :ICollaborationbl
    {
        private readonly ICollaboration _collaboration;

        public CollaborationServicebl(ICollaboration collaboration)
        {
            this._collaboration = collaboration;
        }
        public Task<bool> AddCollaborator(int noteid, CollaborationRequestModel model, int userId)
        {
            return _collaboration.AddCollaborator(noteid, model, userId);
        }
        public Task RemoveCollaborator(int NoteId, CollaborationRequestModel Request, int UserId)
        {
            return _collaboration.RemoveCollaborator(NoteId, Request, UserId);
        }
        public Task<IEnumerable<object>> GetCollaborationbyid(int CollabId)
        {
            return _collaboration.GetCollaborationbyid(CollabId);
        }
    }
}
