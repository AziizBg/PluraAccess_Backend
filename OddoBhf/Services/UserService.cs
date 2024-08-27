using Microsoft.EntityFrameworkCore;
using OddoBhf.Dto.User;
using OddoBhf.Helpers;
using OddoBhf.Interfaces;
using OddoBhf.Models;

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

    }
}
