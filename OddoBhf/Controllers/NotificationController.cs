using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OddoBhf.Interfaces;
using OddoBhf.Models;


namespace OddoBhf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]

    public class NotificationController : Controller {

        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;


        public NotificationController(INotificationService NotificationService, IUserService UserService)
        {
            _notificationService = NotificationService;
            _userService = UserService;
        }

        // GET: api/<NotificationController>
        [HttpGet]
        public ActionResult<ICollection<Notification>> GetAllNotifications()
        {
            var Notifications = _notificationService.GetAllNotifications();
            return Ok(Notifications);
        }

        // GET api/<NotificationController>/5
        [HttpGet("user/{id}")]
        public async Task<ActionResult<PaginatedList<Notification>>> GetUserNotifications(int id, int pageIndex = 0, int pageSize = 10)
        {
            var User = _userService.GetUserById(id);
            if (User == null)
            {
                return NotFound("User not found");
            }
            return await _notificationService.GetUserNotifications(id, pageIndex, pageSize);
        }

        // GET api/<NotificationController>/5
        [HttpGet("{id}")]
        public ActionResult<Notification> GetNotificationById(int id)
        {
            var Notification = _notificationService.GetNotificationById(id);
            if (Notification == null)
            {
                return NotFound();
            }
            return Ok(Notification);
        }

        // POST api/<NotificationController>
        [HttpPost]
        public ActionResult<Notification> AddNotification([FromBody] Notification NotificationDto)
        {
            if (NotificationDto == null)
            {
                return BadRequest("Notification cannot be null.");
            }
            var Notification = new Notification
            {
                Id = NotificationDto.Id,
                CreatedAt = NotificationDto.CreatedAt,
                LicenceId = NotificationDto.LicenceId,
                Message = NotificationDto.Message,
                Title = NotificationDto.Title,
                UserId = NotificationDto.UserId
            };
            _notificationService.AddNotification(Notification);
            return CreatedAtAction(nameof(GetNotificationById), new { id = Notification.Id }, Notification);
        }

        // PUT api/<NotificationController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateNotification(int id, [FromBody] Notification NotificationDto)
        {
            if (NotificationDto == null)
            {
                return BadRequest("Notification Not Provided.");
            }

            _notificationService.UpdateNotification(id, NotificationDto);
            return NoContent();
        }

        // DELETE api/<NotificationController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteNotification(int id)
        {
            var existingNotification = _notificationService.GetNotificationById(id);
            if (existingNotification == null)
            {
                return NotFound();
            }

            _notificationService.DeleteNotification(id);
            return NoContent();
        }




    }
}
