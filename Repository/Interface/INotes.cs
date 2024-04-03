using ModelLayer.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface INotes
    {
        public Task CreateNote(CreateNoteRequest createNoteRequest);

        public Task<IEnumerable<NoteResponse>> GetAllNotebyuserid(int userId);

        public Task UpdateNote(CreateNoteRequest updatedNote, int NoteId);

        public Task DeleteNote(int noteId, int userId);
    }
}
