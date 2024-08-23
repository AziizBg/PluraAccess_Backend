namespace OddoBhf.Interfaces;
using OddoBhf.Models;


public interface IUserRepository
{
    public ICollection<User> GetAllUsers();

    public User GetUserById(int id);
    public void AddUser(User User);
    public void UpdateUser(User User);
    public void DeleteUser(int id);
     public void SaveChanges();
     public User GetUserByConnectionId(string connectionId);


}
