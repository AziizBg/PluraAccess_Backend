using Microsoft.AspNetCore.Mvc;
using OddoBhf.Interfaces;
using OddoBhf.Models;


namespace OddoBhf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : Controller { 

        private readonly ISessionRepository _sessionRepository;

        public SessionController(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

    
        // GET: api/<SessionController>
        [HttpGet]
        public ICollection<Session> GetAllSessions()
        {
            return _sessionRepository.GetAllSessions().ToList();
        }

        // GET api/<SessionController>/5
        [HttpGet("{id}")]
        public Session GetSessionById(int id)
        {
            return _sessionRepository.GetSessionById(id);
        }

        // GET api/<SessionController>/5
        [HttpGet("user/{user_id}")]
        public ICollection<Session> GetSessionsByUserId(int user_id)
        {
            return _sessionRepository.GetSessionsByUserId(user_id);
            //            return new ApiResponse(true, null, players);
        }

        // POST api/<SessionController>
        [HttpPost]
        public void AddSession([FromBody] Session session)
        {
            _sessionRepository.AddSession(session);
        }

        // PUT api/<SessionController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateSession(int id, [FromBody] Session session)
        {
                _sessionRepository.UpdateSession(session);
                return Ok(session);

        }

        // DELETE api/<SessionController>/5
        [HttpDelete("{id}")]
        public void DeleteSession(int id)
        {
            _sessionRepository.DeleteSession(id);
        }




    }
}
