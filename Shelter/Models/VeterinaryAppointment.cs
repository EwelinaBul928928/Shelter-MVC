using System.ComponentModel.DataAnnotations;

namespace Shelter.Models
{
    public class VeterinaryAppointment
    {
        public int Id { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Scheduled";

        public int? VeterinarianId { get; set; }

        public Veterinarian? Veterinarian { get; set; }

        public int? ServiceId { get; set; }

        public VeterinaryService? Service { get; set; }

        public int? AnimalId { get; set; }

        public Animal? Animal { get; set; }

        public int? ClientAnimalId { get; set; }

        public ClientAnimal? ClientAnimal { get; set; }

        public int? UserId { get; set; }

        public User? User { get; set; }

        public decimal? Cost { get; set; }

        public bool IsFree { get; set; } = false;
    }
}
