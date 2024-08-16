namespace OddoBhf.Models
{
    public class Queue
    {
        public int Id { get; set; }
        public User? User { get; set; }
        public DateTime? RequestedAt { get; set; }

    }
}
