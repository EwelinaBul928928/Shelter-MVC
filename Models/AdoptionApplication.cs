using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shelter.Models
{
    public class AdoptionApplication
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        public int AnimalId { get; set; }

        [ForeignKey("AnimalId")]
        public Animal Animal { get; set; } = null!;

        public DateTime ApplicationDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string ExperienceWithAnimals { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string HasOtherPets { get; set; } = "Nie";

        [Required]
        [StringLength(10)]
        public string HasGarden { get; set; } = "Nie";

        [Required]
        [StringLength(500)]
        public string LivingSituation { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Notes { get; set; }
    }
}
