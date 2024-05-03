using BussinesLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using ModelLayer.Label;
using ModelLayer.Notes;
using Repository.Entity;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabel labelbl;
        private readonly ILogger<LabelController> _logger;
        public LabelController(ILabel label,ILogger<LabelController> logger)
        {
            this.labelbl = label;
            this._logger = logger;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddLabel(CreateLabel label)
        {
            try
            {
                var userIdClaim = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(userIdClaim);
                await labelbl.CreateLabel(label, userId);
                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "Label created "

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
        [HttpDelete("{note_id}")]
        public async Task<IActionResult> Removelabel(int LabelId)
        {
            try
            {
                await labelbl.DeleteLabel(LabelId);
                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "Label deleted"

                };
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while updating the notes.",
                    Data = null
                });
            }
        }
        [Authorize]
        [HttpPut("{label_id}")]
        public async Task<IActionResult> UpdateLabel(CreateLabel label, int LabelId)
        {
            try
            {
                var userIdClaim = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(userIdClaim);

                await labelbl.UpdateLabel(label,LabelId,userId);
                var response = new ResponseModel<String>
                {
                    Success = true,
                    Message = "Label Updated",
                    Data = null

                };
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while updating the notes.",
                    Data = null
                });
            }
        }
        [Authorize]
        [HttpGet("{label_id}")]

        public async Task<IActionResult> GetAllLabelbyId()
        {
            try
            {
                _logger.LogInformation("getnotesbyid");
               var label = await labelbl.GetAllLabelbyId();
                return Ok(new ResponseModel<IEnumerable<LabelEntity>>
                {
                    Success = true,
                    Message = "Label retrieved successfully",
                    Data = label
                });
            }
            catch(Exception ex)
            {

                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while updating the notes.",
                    Data = null
                });
            }
        }
        [Authorize]
        [HttpGet("{note_id}")]

        public async Task<IActionResult> GetAllNotebyId(int LabelId)
        {
            try
            {
                var label = await labelbl.GetAllNotesbyId(LabelId);
                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = "Label retrieved successfully",
                    Data = label
                });
            }
            catch (Exception ex)
            {
                return BadRequest( new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while updating the notes.",
                    Data = null
                });
            }
        }

    }
}
