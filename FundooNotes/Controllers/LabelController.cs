using BussinesLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Entity;

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

        [HttpPost]
        public async Task<IActionResult> AddLabel(LabelEntity labelEntity)
        {
            try
            {
                await labelbl.CreateLabel(labelEntity);
                return Ok("Label created");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Removelabel(int LabelId)
        {
            try
            {
                await labelbl.DeleteLabel(LabelId);
                return Ok("Label deleted");
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateLabel(LabelEntity labelentity)
        {
            try
            {
                await labelbl.UpdateLabel(labelentity);
                return Ok("Label updated");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Getbyid")]

        public async Task<IActionResult> GetAllLabelbyId(int LabelId)
        {
            try
            {
                _logger.LogInformation("getnotesbyid");
               var label = await labelbl.GetAllLabelbyId(LabelId);
                return Ok(label);
            }
            catch(Exception ex)
            {
                
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("Getnotesbyid")]

        public async Task<IActionResult> GetAllNotebyId(int LabelId)
        {
            try
            {
                var label = await labelbl.GetAllNotesbyId(LabelId);
                return Ok(label);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
