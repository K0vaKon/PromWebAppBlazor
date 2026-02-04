using PhotoPromAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PromAPI.Models
{
    [Table("zender")] // Tells EF that this class maps to the "zender" table
    public class Zender
    {
        [Key] // Primary Key
        [Column("idzender")] // Maps to database column "idzender"
        public int Id { get; set; }

        [Column("zenderNaam")] // Maps to "zenderNaam" (Cactus, etc.)
        public string Name { get; set; } = string.Empty;

        [Column("zenderEmail")] // Maps to "zenderEmail"
        public string Email { get; set; } = string.Empty;

        // Optional: Navigation property to get all photos of this sender
        public List<Photo> Photos { get; set; } = new();
    }
}
