using OddoBhf.Models;

namespace OddoBhf.Hub
{
    public interface INotificationHub
    {
        public Task SendMessage(Notification notification);

    }
}
