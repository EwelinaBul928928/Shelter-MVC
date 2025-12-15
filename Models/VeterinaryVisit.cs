using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shelter.Models
{
    public class VeterinaryVisit
    {
        public int Id { get; set; }

        [Required]
        public int AnimalId { get; set; }

        [ForeignKey("AnimalId")]
        public Animal Animal { get; set; } = null!;

        [Required]
        public DateTime VisitDate { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? Diagnosis { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Cost { get; set; }

        [StringLength(100)]
        public string? VeterinarianName { get; set; }
    }
}
