using BussinesLayer.Interface;
using BussinesLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer;
using ModelLayer.Notes;
using Newtonsoft.Json;
using Repository.Interface;
using RepositoryLayer.Entity;
using System.Security.Claims;
using System.Text.Json;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INotes _notesBL;
        private readonly IDistributedCache _cache;
        private readonly ILogger<NoteController> _logger;


        public NoteController(INotes notesBL,IDistributedCache cache, ILogger<NoteController> logger)
        {
            _notesBL = notesBL;
            _cache = cache;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateNote(CreateNoteRequest createNoteRequest)
        {
            try
            {
                var userIdClaim = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(userIdClaim);

                await _notesBL.CreateNote(createNoteRequest, userId);
                var response = new ResponseModel<string>
                {
                    StatusCode = 200,
                    Message = "Note Created Successfully",
                    
                };
                return Ok(response);
            }
            catch (Exception ex)
            {

                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null // Ensure Data is null in case of error
                };
                return Ok(response);
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DisplayNote()
        {
            try
            {
                var userIdClaim = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(userIdClaim);
                var key = $"Notes_{userId}";
                var cachedNote = await _cache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(cachedNote))
                {
                    var notesList = JsonConvert.DeserializeObject<List<NoteResponse>>(cachedNote);
                    var response = new ResponseModel<IEnumerable<NoteResponse>>
                    {
                        StatusCode = 200,
                        Message = "Note Fetched Successfully from cache",
                        Data = notesList
                    };
                    return Ok(response);
                }
                var notes = await _notesBL.GetAllNoteAsync(userId);
                if (notes != null)
                {
                    var serializedNote = JsonConvert.SerializeObject(notes);
                    await _cache.SetStringAsync(key, serializedNote, new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(10)
                    });
                    //await _cache.SetStringAsync(key, JsonConvert.SerializeObject(notes), TimeSpan.FromMinutes(10));
                    var response = new ResponseModel<IEnumerable<NoteResponse>>
                    {
                        StatusCode = 200,
                        Message = "Note Fetched Successfully from DB",
                        Data = notes
                    };
                    return Ok(response);
                }
                return Ok();
                }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateNote(int NoteId,CreateNoteRequest updateone )
        {
            try
            {
                var userIdClaim = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(userIdClaim);
              await _notesBL.UpdateNote(NoteId, userId, updateone);
                var response = new ResponseModel<NoteResponse>
                {
                    StatusCode = 200,
                    Message = "Note updated successfully",
                    Data = null
                    
                };

                return Ok(response);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteNote(int noteId)
        {
            try
            {

                var userIdClaim = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(userIdClaim);
                await _notesBL.DeleteNote(noteId, userId);
                return Ok(new ResponseModel<string>
                {
                    Message = "Note deleted successfully",
                    Data = null
                });
            }
            catch (Exception ex)
            {

                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
    }
}
