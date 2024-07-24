using System.ComponentModel.DataAnnotations.Schema;

namespace OddoBhf.Models
{
    public class Licence
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool IsAvailable { get; set; }
        public int? CurrentSessionId { get; set; }

        public Session? CurrentSession { get; set; }
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
