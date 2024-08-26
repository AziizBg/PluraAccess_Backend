using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OddoBhf.Interfaces;
using OddoBhf.Models;

namespace OddoBhf.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository NotificationRepository)
        {
            _notificationRepository = NotificationRepository;
        }

        public ICollection<Notification> GetAllNotifications()
        {
            return _notificationRepository.GetAllNotifications().ToList();
        }
        public async Task<ActionResult<PaginatedList<Notification>>> GetUserNotifications(int user_id, int pageIndex = 0, int pageSize = 10)
        {
            return await _notificationRepository.GetUserNotifications(user_id, pageIndex, pageSize);
        }

        public Notification GetNotificationById(int id)
        {
            return _notificationRepository.GetNotificationById(id);
        }

        public void AddNotification(Notification Notification)
        {
            _notificationRepository.AddNotification(Notification);
        }

        public void UpdateNotification(int id, Notification Notification)
        {
            _notificationRepository.UpdateNotification(Notification);
        }

        public void DeleteNotification(int id)
        {
            _notificationRepository.DeleteNotification(id);
        }
    }
}
