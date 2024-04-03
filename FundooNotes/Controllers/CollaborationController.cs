using BussinesLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using System.Reflection;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaborationController : ControllerBase
    {
        private readonly ICollaborationbl _collabbl;

        public CollaborationController(ICollaborationbl collabbl)
        {
            this._collabbl = collabbl;
        }
        [HttpPost("AddCollaboration")]

        public async Task<IActionResult> AddCollaborator(int noteid, [FromBody] CollaborationRequestModel model, int userId)
        {
            try
            {
                await _collabbl.AddCollaborator(noteid, model, userId);
                return Ok("Collabarator Added Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpDelete("RemoveCollaborator")]

        public async Task<IActionResult> RemoveCollaborator(int NoteId, CollaborationRequestModel Request, int UserId)
        {
            try
            {
                await _collabbl.RemoveCollaborator(NoteId, Request, UserId);
                return Ok("Collabarator Removed Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetByid")]

        public async Task<IActionResult> GetCollabbyid(int Collabid)
        {
            try
            {
                var collab = await _collabbl.GetCollaborationbyid(Collabid);
                return Ok(collab);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
