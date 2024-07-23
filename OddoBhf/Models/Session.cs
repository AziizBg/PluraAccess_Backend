namespace OddoBhf.Models
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? UserNotes{ get; set; }
        public Licence? Licence { get; set; }
        public int LicenceId { get; set; }
        public User? User { get; set; }

    }
}
