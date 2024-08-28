using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OddoBhf.Dto.User;
using OddoBhf.Helpers;
using OddoBhf.Interfaces;
using OddoBhf.Models;


namespace OddoBhf.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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

            var user = _userService.AddUser(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] CreateUserDto dto)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return BadRequest(new { message = "User Not Found." });
            }
            user.Email = dto.Email;
            user.UserName = dto.UserName;
            user.Password = dto.Password;
            user.Role = dto.Role;
            _userService.UpdateUser(id, user);
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

        // POST api/<UserController>
        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult Loign([FromBody] LoginDto dto)
        {
            var user = _userService.GetUserByEmail(dto.Email);
            if (user == null)
            {
                return BadRequest("Invalid Email");
            }
            if(!PasswordHasher.VerifyPassword(dto.Password, user.Password)) {
                return BadRequest("Invalid Password");
            }
            user.Token = _userService.CreateJwt(user);
            _userService.UpdateUser(user.Id, user);

            return Ok(new { Token = user.Token, message = "Logged in Successfully"  });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult Register([FromBody] CreateUserDto dto)
        {
            var user = _userService.GetUserByEmail(dto.Email);
            if (user != null)
            {
                return BadRequest("Email Already Taken");
            }
            _userService.AddUser(dto);
            return Ok(new { message= "Registered Successfully"});
        }

    }
}
