using System.ComponentModel.DataAnnotations.Schema;

namespace OddoBhf.Models
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Course { get; set; }

        public string? UserNotes { get; set; }
        public Licence? Licence { get; set; }
        public User? User { get; set; }
    }
}
