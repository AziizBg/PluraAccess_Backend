namespace OddoBhf.Interfaces;
using OddoBhf.Models;


public interface INotificationRepository
{
    public ICollection<Notification> GetAllNotifications();
    public  Task<PaginatedList<Notification>> GetUserNotifications(int userId, int pageIndex, int pageSize);
    public Notification GetNotificationById(int id);
    public void AddNotification(Notification Notification);
    public void UpdateNotification(Notification Notification);
    public void DeleteNotification(int id);
     public void SaveChanges();


}
