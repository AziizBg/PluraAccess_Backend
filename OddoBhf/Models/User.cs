using System.Text.Json.Serialization;

namespace OddoBhf.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }
        public string? ConnectionId { get; set; }
        public int? BookedLicenceId { get; set; }

        [JsonIgnore]
        public ICollection<Session>? Sessions { get; set; }
    }
}
