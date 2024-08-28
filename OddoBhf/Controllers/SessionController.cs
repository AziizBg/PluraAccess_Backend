using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OddoBhf.Interfaces;
using OddoBhf.Models;
using OddoBhf.Services;
using System.ComponentModel;


namespace OddoBhf.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SessionController : Controller { 

        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

    
        // GET: api/<SessionController>
        [HttpGet]
        public IActionResult GetAllSessions()
        {
            return Ok (_sessionService.GetAllSessions());
        }

        // GET api/<SessionController>/5
        [HttpGet("{id}")]
        public IActionResult GetSessionById(int id)
        {
            return Ok(_sessionService.GetSessionById(id));
        }

        [HttpGet("user/{user_id}")]
        public async Task<ActionResult<PaginatedList<Session>>> GetSessionsByUserId(int user_id, int pageIndex = 0, int pageSize = 10)
        {
            return await _sessionService.GetSessionsByUserId(user_id, pageIndex, pageSize);
        }

        // POST api/<SessionController>
        [HttpPost]
        public IActionResult AddSession([FromBody] Session session)
        {
            _sessionService.AddSession(session);
            return CreatedAtAction(nameof(GetSessionById), new { id = session.Id }, session);

        }

        // PUT api/<SessionController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateSession(int id, [FromBody] Session session)
        {
            if (id != session.Id)
            {
                return BadRequest();
            }
            _sessionService.UpdateSession(session);
            return NoContent();
        }

        // DELETE api/<SessionController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteSession(int id)
        {
            var session = _sessionService.GetSessionById(id);
            if (session == null)
            {
                return NotFound(); // 404 if the session doesn't exist
            }
            _sessionService.DeleteSession(id);
            return NoContent(); // 204 if the session was successfully deleted
        }






    }
}
