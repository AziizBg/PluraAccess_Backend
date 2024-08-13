using Microsoft.AspNetCore.Mvc;
using OddoBhf.Dto.User;
using OddoBhf.Interfaces;
using OddoBhf.Models;


namespace OddoBhf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<UserController>
        [HttpGet]
        public ActionResult<ICollection<User>> GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost]
        public ActionResult<User> AddUser([FromBody] CreateUserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User cannot be null.");
            }
            var user = new User { Name = userDto.Name };

            _userService.AddUser(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] CreateUserDto user)
        {
            if (user == null)
            {
                return BadRequest("User Not Provided.");
            }

            var User = new User
            {
                Id = id,
                Name = user.Name,
            };

            _userService.UpdateUser(id, User);
            return NoContent();
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var existingUser = _userService.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            _userService.DeleteUser(id);
            return NoContent();
        }




    }
}
