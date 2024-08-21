
using Microsoft.AspNetCore.SignalR;

namespace OddoBhf.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UserId { get; set; }
        public int? LicenceId { get; set; }
    }
}
