using Microsoft.AspNetCore.Mvc;
using OddoBhf.Models;

namespace OddoBhf.Interfaces
{
    public interface INotificationService
    {
        public ICollection<Notification> GetAllNotifications();
        public Task<ActionResult<PaginatedList<Notification>>> GetUserNotifications(int user_id, int pageIndex = 1, int pageSize = 10);
        public Notification GetNotificationById(int id);
        public void AddNotification(Notification Notification);
        public void UpdateNotification(int id, Notification Notification);
        public void DeleteNotification(int id);

    }
}
