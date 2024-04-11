using BussinesLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Notes;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INotes _notesBL;

        public NoteController(INotes notesBL)
        {
            this._notesBL = notesBL;
        }
        [HttpPost()]

        public async Task<IActionResult> CreateNote(CreateNoteRequest createNoteRequest)
        {
            try
            {
                await _notesBL.CreateNote(createNoteRequest);
                return Ok("note created");
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("DisplayNotesbyid")]

        public async Task<IActionResult> DisplayNote(int userId)
        {
            try
            {

                var note = await _notesBL.GetAllNotebyuserid(userId);
                return Ok(note);
            }
            catch (Exception ex)
            {
               
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateNote(CreateNoteRequest updatenote, int NoteId)
        {
            try
            {
                await _notesBL.UpdateNote(updatenote, NoteId);
                return Ok("Note updated");
            }
            catch (Exception ex)
            {
               
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteNote(int noteId, int userId)
        {
            try
            {
                await _notesBL.DeleteNote(noteId, userId);
                return Ok("Note deleted");
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }
    }
}
