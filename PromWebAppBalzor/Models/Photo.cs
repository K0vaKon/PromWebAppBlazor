namespace PromWebAppBalzor.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public DateTime Tijd { get; set; } // Важно: тип DateTime
        public int IsApproved { get; set; }

        public int ZenderId { get; set; }
        public Zender? Sender { get; set; } // Вложенный объект
    }
}
