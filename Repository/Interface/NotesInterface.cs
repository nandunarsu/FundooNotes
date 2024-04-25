using ModelLayer.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface NotesInterface
    {
        public Task CreateNote(CreateNoteRequest createNoteRequest,int userid);

        public Task<IEnumerable<NoteResponse>> GetAllNoteAsync(int userid);

        public Task UpdateNote(int noteId, int UserId, CreateNoteRequest updatedNote);

        public Task DeleteNote(int noteId, int userId);
    }
}
