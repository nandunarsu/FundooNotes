using BussinesLayer.Interface;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ICollaboration _collabbl;
        private readonly ILogger<CollaborationController> _logger;

        public CollaborationController(ICollaboration collabbl, ILogger<CollaborationController> _logger)
        {
            this._collabbl = collabbl;
            this._logger = _logger;
        }
        [HttpPost]

        public async Task<IActionResult> AddCollaborator(int noteid, [FromBody] CollaborationRequestModel model, int userId)
        {
            try
            {

                await _collabbl.AddCollaborator(noteid, model, userId);
                _logger.LogInformation("Collabarator Added");
                return Ok("Collabarator Added Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Invalid Request {ex.Message}");
                return StatusCode(500, ex.Message);
            }

        }
        [HttpDelete()]

        public async Task<IActionResult> RemoveCollaborator(int CollabId)
        {
            try
            {
                await _collabbl.RemoveCollaborator(CollabId);
                return Ok("Collabarator Removed Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Invalid Request {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize]
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
                _logger.LogError($"Invalid Request {ex.Message}");
                return StatusCode(500, ex.Message);
            }

        }
    }
}
