using BussinesLayer.Interface;
using ModelLayer.Notes;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Service
{
    public class NotesServicebl : INotesbl
    {
        private readonly INotes _notesInterface;

        public NotesServicebl(INotes notesInterface)
        {
            _notesInterface = notesInterface;
        }
        public Task CreateNote(CreateNoteRequest createNoteRequest)
        {
            return _notesInterface.CreateNote(createNoteRequest);
        }
        public Task<IEnumerable<NoteResponse>> GetAllNotebyuserid(int userId)
        {
            return _notesInterface.GetAllNotebyuserid(userId);
        }
        public Task UpdateNote(CreateNoteRequest updatedNote, int NoteId)
        {
            return _notesInterface.UpdateNote(updatedNote, NoteId);
        }
        public Task DeleteNote(int noteId, int userId)
        {
            return _notesInterface.DeleteNote(noteId, userId);
        }
    }
}
