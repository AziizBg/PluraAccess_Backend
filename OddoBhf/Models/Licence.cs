using System.ComponentModel.DataAnnotations.Schema;

namespace OddoBhf.Models
{
    public class Licence
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public Session? CurrentSession { get; set; }
    }
}
