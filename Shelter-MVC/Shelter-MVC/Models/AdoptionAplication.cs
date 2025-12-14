using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shelter_MVC.Models
{
    public enum ApplicationStatus
    {
        Oczekujący, Zaakceptowany, Odrzucony
    }

    public class AdoptionApplication
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Data zgłoszenia")]
        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Napisz dlaczego chcesz adoptować")]
        [Display(Name = "Wiadomość do schroniska")]
        public string Message { get; set; }

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Oczekujący;

        public string? UserId { get; set; }

        public int AnimalId { get; set; }

        [ForeignKey("AnimalId")]
        public virtual Animal? Animal { get; set; }
    }
}