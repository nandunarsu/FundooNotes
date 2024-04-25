using BussinesLayer.Interface;
using ModelLayer.Notes;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinesLayer.Interface;

namespace BussinesLayer.Service
{
    public class NotesServicebl : INotes
    {
        private readonly NotesInterface _notesInterface;

        public NotesServicebl(Repository.Interface.NotesInterface notesInterface)
        {
            _notesInterface = notesInterface;
        }
        public Task CreateNote(CreateNoteRequest createNoteRequest, int userid)
        {
            return _notesInterface.CreateNote(createNoteRequest,userid);
        }
        public Task<IEnumerable<NoteResponse>> GetAllNoteAsync( int userid)
        {
            return _notesInterface.GetAllNoteAsync(userid);
        }
        public Task UpdateNote(int noteId, int UserId, CreateNoteRequest updatedNote)
        {
            return _notesInterface.UpdateNote(noteId,UserId,updatedNote);
        }
        public Task DeleteNote(int noteId, int userId)
        {
            return _notesInterface.DeleteNote(noteId, userId);
        }


    }
}
