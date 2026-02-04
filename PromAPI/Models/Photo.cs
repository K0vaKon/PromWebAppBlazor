using PromAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoPromAPI.Models
{
    [Table("fotos")] // Tells EF that this class maps to the "fotos" table
    public class Photo
    {
        [Key] // Primary Key
        [Column("idfotos")] // Connects this property to the "idfotos" column in SQL
        public int Id { get; set; }

        [Column("fotoNaam")]
        public string FileName { get; set; } = string.Empty;

        [Column("fotoDatum")]
        public DateTime Tijd { get; set; } = DateTime.UtcNow;

        [Column("goedgekeurd")]
        public int IsApproved { get; set; } // Matches your TINYINT/INT (0 or 1)

        // Foreign Key link to the sender
        [Column("zender_idzender")]
        public int ZenderId { get; set; }

        // This is a "Navigation Property" - it allows you to get 
        // the user's name and email without a separate query
        [ForeignKey("ZenderId")]
        public Zender? Sender { get; set; }
    }
}
