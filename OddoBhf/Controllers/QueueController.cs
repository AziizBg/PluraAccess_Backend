using Microsoft.AspNetCore.Mvc;
using OddoBhf.Dto.Queue;
using OddoBhf.Interfaces;
using OddoBhf.Models;

namespace OddoBhf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : Controller
    {

        private readonly IQueueService _queueService;
        private readonly IUserService _userService;

        public QueueController(IQueueService queueService, IUserService userService)
        {
            _queueService = queueService;
            _userService = userService;
        }


        // GET: api/<QueueController>
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_queueService.GetAll());
        }

        // GET api/<QueueController>/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_queueService.GetById(id));
        }


        // POST api/<QueueController>
        [HttpPost]
        public IActionResult Add([FromBody]AddQueueDto dto)
        {
            var user = _userService.GetUserById(dto.UserId);
            if (user == null)
            {
                return BadRequest("user not found");
            }
            if (_queueService.IsUserInQueue(dto.UserId) == true)
            {
                return BadRequest("user already in the queue");
            }
            var queue = new Queue
            {
                User = user,
                RequestedAt = DateTime.Now
            };
            _queueService.Add(queue);
            return Ok(_queueService.GetPosition(dto.UserId));
        }

        [HttpGet("isInQueue/{userId}")]
        public IActionResult IsUserInQueue(int userId)
        {
            return Ok(_queueService.IsUserInQueue(userId));
        }
        [HttpGet("getPosition/{userId}")]
        public IActionResult GetPosition(int userId)
        {
            return Ok(_queueService.GetPosition(userId));
        }

        // PUT api/<QueueController>/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Queue queue)
        {
            if (id != queue.Id)
            {
                return BadRequest();
            }
            _queueService.Update(queue);
            return NoContent();
        }

        // DELETE api/<QueueController>/5
        [HttpDelete("{userId}")]
        public IActionResult RemoveUser(int userId)
        {
            var queue = _queueService.GetByUserId(userId);
            if (queue == null)
            {
                return NotFound(); // 404 if the queue doesn't exist
            }
            _queueService.RemoveUser(userId);
            return NoContent(); // 204 if the queue was successfully deleted
        }
    }

    }
