using OddoBhf.Models;

namespace OddoBhf.Dto
{
    public class GetLicenceDto
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public int? BookedByUserId { get; set; }
        public DateTime? BookedUntil { get; set; }
        public Models.Session? CurrentSession { get; set; }
    }
}
