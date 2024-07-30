namespace OddoBhf.Interfaces;
using OddoBhf.Models;


public interface IUserRepository
{
    ICollection<User> GetAllUsers();

    User GetUserById(int id);
    void AddUser(User User);
    void UpdateUser(User User);
    void DeleteUser(int id);
    public void SaveChanges();

}
