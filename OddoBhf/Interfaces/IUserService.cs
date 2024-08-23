using OddoBhf.Models;

namespace OddoBhf.Interfaces
{
    public interface IUserService
    {
        public ICollection<User> GetAllUsers();
        public User GetUserById(int id);
        public void AddUser(User user);
        public void UpdateUser(int id, User user);
        public void DeleteUser(int id);
        public User GetUserByConnectionId(string connectionId);

    }
}
