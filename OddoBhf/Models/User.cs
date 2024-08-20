using System.Text.Json.Serialization;

namespace OddoBhf.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ConnectionId { get; set; }

        [JsonIgnore]
        public ICollection<Session>? Sessions { get; set; }
    }
}
