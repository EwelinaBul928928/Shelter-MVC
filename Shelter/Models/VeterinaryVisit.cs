using System.ComponentModel.DataAnnotations;

namespace Shelter.Models
{
    public class VeterinaryVisit
    {
        public int Id { get; set; }

        [Required]
        public int AnimalId { get; set; }

        public Animal Animal { get; set; } = null!;

        [Required]
        public DateTime VisitDate { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? Diagnosis { get; set; }

        public decimal? Cost { get; set; }

        public int? VeterinarianId { get; set; }

        public Veterinarian? Veterinarian { get; set; }

        public int? ServiceId { get; set; }

        public VeterinaryService? Service { get; set; }
    }
}
