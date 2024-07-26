using Microsoft.AspNetCore.Mvc;
using OddoBhf.Interfaces;
using OddoBhf.Models;


namespace OddoBhf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller { 

        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

    
        // GET: api/<UserController>
        [HttpGet]
        public ICollection<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers().ToList();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        // POST api/<UserController>
        [HttpPost]
        public void AddUser([FromBody] User user)
        {
            _userRepository.AddUser(user);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void UpdateUser(int id, [FromBody] User user)
        {
            _userRepository.UpdateUser(user);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void DeleteUser(int id)
        {
            _userRepository.DeleteUser(id);
        }




    }
}
