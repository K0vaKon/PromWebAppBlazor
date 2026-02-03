namespace PhotoPromAPI.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime Tijd { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending"; // "Pending", "Approved", "Rejected"
        public string? Gebruiker { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

    }
}
