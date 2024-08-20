using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OddoBhf.Hub;
using OddoBhf.Interfaces;
using OddoBhf.Models;

namespace OddoBhf.Services
{
    public class QueueService:IQueueService
    {
        private readonly IHubContext<NotificationHub, INotificationHub> _notification;
        private readonly IQueueRepository _queueRepository;

        public QueueService(IQueueRepository queueRepository, IHubContext<NotificationHub, INotificationHub> hubContext)
        {
            _queueRepository = queueRepository;
            _notification = hubContext;
        }

        public ICollection<Queue> GetAll()
        {
            return _queueRepository.GetAll().ToList();
        }
        public Queue GetById(int id)
        {
            return _queueRepository.GetById(id);
        }
        public Queue GetByUserId(int id)
        {
            return _queueRepository.GetByUserId(id);
        }

        public void Add(Queue queue)
        {
            _queueRepository.Add(queue);
            _notification.Clients.All.SendMessage(new Notification
            {
                Message = "User added to the queue",
                Title = "Queue Extended",
                UserId= queue.Id
            });
        }
        public bool IsUserInQueue(int userId)
        {
            return _queueRepository.IsUserInQueue(userId);
        }
        public int GetPosition(int userId)
        {
            return _queueRepository.GetPosition(userId);
        }
        public Queue GetFirst()
        {
            return _queueRepository.GetFirst();
        }
        public void RemoveFirst()
        {
            var queue = GetFirst();
            _queueRepository.Delete(queue.Id);
        }
            public void Update(Queue queue)
        {
            _queueRepository.Update(queue);

        }
        public void Delete(int id)
        {
            _queueRepository.Delete(id);
        }
        public void RemoveUser(int id)
        {
            var queue = _queueRepository.GetByUserId(id);
            _queueRepository.Delete(queue.Id);
        }
    }
}
