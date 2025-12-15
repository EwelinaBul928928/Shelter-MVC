using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shelter.Models
{
    public class NewsPost
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(5000)]
        public string Content { get; set; } = string.Empty;

        public DateTime PublicationDate { get; set; }

        public int? AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public User? Author { get; set; }

        [StringLength(500)]
        public string? PhotoUrl { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = "Information";
    }
}
