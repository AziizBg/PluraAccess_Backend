using OddoBhf.Models;

namespace OddoBhf.Interfaces
{
    public interface IUserService
    {
        ICollection<User> GetAllUsers();
        User GetUserById(int id);
        void AddUser(User user);
        void UpdateUser(int id, User user);
        void DeleteUser(int id);
    }
}
