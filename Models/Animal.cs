using System.ComponentModel.DataAnnotations;

namespace Shelter.Models
{
    public class Animal
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Species { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Breed { get; set; }

        public int Age { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? PhotoUrl { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Available";

        public DateTime AdmissionDate { get; set; }

        public ICollection<AdoptionApplication> AdoptionApplications { get; set; } = new List<AdoptionApplication>();
        public ICollection<VeterinaryVisit> VeterinaryVisits { get; set; } = new List<VeterinaryVisit>();
    }
}
