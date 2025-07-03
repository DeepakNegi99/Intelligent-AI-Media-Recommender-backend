using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("polls")]
    public class PollsController : ControllerBase
    {
        private readonly PollService _pollService;

        public PollsController(PollService pollService)
        {
            _pollService = pollService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Poll>>> Get()
        {
            var polls = await _pollService.GetPollsAsync();
            if (polls.Count == 0)
                return BadRequest("No poll data found.");
            return Ok(polls);
        }

        [HttpPut("create")]
        public async Task<IActionResult> CreatePoll([FromBody] Poll newPoll)
        {
            try
            {
                await _pollService.CreateOrReplacePollAsync(newPoll);
                return StatusCode(201, "Poll created successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest("Something went wrong while creating the poll.");
            }
        }

    }


}
