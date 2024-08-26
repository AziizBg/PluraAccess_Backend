using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OddoBhf.Data;
using OddoBhf.Interfaces;
using OddoBhf.Models;

namespace OddoBhf.Repositories
{
    public class NotificationRepository: INotificationRepository
    {
        public readonly DataContext _context;

        public NotificationRepository(DataContext context)
        {
            _context = context;
        }

        public void AddNotification(Notification Notification)
        {
            _context.Notifications.Add(Notification);
            _context.SaveChanges();

        }

        public void DeleteNotification(int id)
        {
            var Notification = _context.Notifications.Find(id);
            _context.Notifications.Remove(Notification);
            _context.SaveChanges();

        }

        public ICollection<Notification> GetAllNotifications()
        {
            return _context.Notifications.ToList();
        }
        public async Task<PaginatedList<Notification>> GetUserNotifications(int userId, int pageIndex, int pageSize)
        {
            var notifications =  _context.Notifications
                .Where(n=>n.UserId == userId)
                .OrderByDescending(n=>n.CreatedAt)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();

            var length = await _context.Notifications
                .Where(n => n.UserId == userId)
                .CountAsync();
            var totalPages = (int)Math.Ceiling(length / (double)pageSize);

            return new PaginatedList<Notification>(notifications, pageIndex, totalPages, length);
        }

        public Notification GetNotificationById(int id)
        {
            return _context.Notifications.First(l => l.Id == id);
        }

        public void UpdateNotification(Notification Notification)
        {
            _context.Notifications.Update(Notification);
            _context.SaveChanges();

        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }


    }
}
