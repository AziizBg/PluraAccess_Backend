using OddoBhf.Dto.User;
using OddoBhf.Models;

namespace OddoBhf.Interfaces
{
    public interface IUserService
    {
        public ICollection<User> GetAllUsers();
        public User GetUserById(int id);
        public User AddUser(CreateUserDto user);
        public void UpdateUser(int id, User user);
        public void DeleteUser(int id);
        public User GetUserByConnectionId(string connectionId);
        public User GetUserByEmail(string email);

    }
}
