using Microsoft.AspNetCore.Mvc;
using OddoBhf.Interfaces;
using OddoBhf.Models;

namespace OddoBhf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : Controller
    {

        private readonly IQueueService _queueService;

        public QueueController(IQueueService queueService)
        {
            _queueService = queueService;
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
        public IActionResult Add([FromBody] Queue queue)
        {
            _queueService.Add(queue);
            return CreatedAtAction(nameof(GetById), new { id = queue.Id }, queue);

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
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var queue = _queueService.GetById(id);
            if (queue == null)
            {
                return NotFound(); // 404 if the queue doesn't exist
            }
            _queueService.Delete(id);
            return NoContent(); // 204 if the queue was successfully deleted
        }
    }

    }
