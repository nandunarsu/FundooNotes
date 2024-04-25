using BussinesLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using ModelLayer.Collaboration;
using System.Reflection;
using System.Security.Claims;

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
        [Authorize]
        [HttpPost]

        public async Task<IActionResult> AddCollaborator(int noteid, [FromBody] CollaborationRequestModel model)
        {
            try
            {
                var userIdClaim = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(userIdClaim);
                await _collabbl.AddCollaborator(noteid, model, userId);
                _logger.LogInformation("Collabarator Added");
                var response = new ResponseModel<string>
                {
                    StatusCode = 200,
                    Message = "Collaboration Successfull",

                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
                return NotFound(response);
            }

        }
        [Authorize]
        [HttpDelete]

        public async Task<IActionResult> RemoveCollaborator(int CollabId)
        {
            try
            {
                await _collabbl.RemoveCollaborator(CollabId);
                var response = new ResponseModel<string>
                {
                    Message = "Collaborator removed successfully",
                    Data = null
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Invalid Request {ex.Message}");
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
                return BadRequest(response);
            }
        }
        [Authorize]
        [HttpGet("GetByid")]

        public async Task<IActionResult> GetCollabbyid()
        {
            try
            {
                var collab = await _collabbl.GetCollaboration();
                var response = new ResponseModel<IEnumerable<CollabInfoModel>>
                {
                    Message = "Collaborators Fetched Successfully",
                    Data = collab
                };
                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Invalid Request {ex.Message}");
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,

                };
                return Ok(response);

            }

        }
    }
}
