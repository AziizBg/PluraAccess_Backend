using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OddoBhf.Dto.User;
using OddoBhf.Helpers;
using OddoBhf.Interfaces;
using OddoBhf.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OddoBhf.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ICollection<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers().ToList();
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }
        public User GetUserByConnectionId(string connectionId)
        {
            return _userRepository.GetUserByConnectionId(connectionId);
        }


        public User AddUser(CreateUserDto userDto)
        {
            var password = PasswordHasher.HashPassword(userDto.Password);
            var user = new User
            {
                Email = userDto.Email,
                UserName = userDto.UserName,
                Password = password,
                Role = "User"
            };
            _userRepository.AddUser(user);
            return user;
        }

        public void UpdateUser(int id, User user)
        {
            _userRepository.UpdateUser(user);
        }

        public void DeleteUser(int id)
        {
            _userRepository.DeleteUser(id);
        }

        public User GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }

        public string CreateJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("secret@key@for@pluraaccess@project");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())

            });
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials,
                Issuer= "https://localhost:7189/",
                Audience= "https://localhost:7189/"
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
