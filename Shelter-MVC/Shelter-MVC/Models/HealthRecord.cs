using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shelter_MVC.Models
{
    public class HealthRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Data wizyty")]
        [DataType(DataType.Date)]
        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = "Opis zabiegu jest wymagany")]
        [Display(Name = "Opis zabiegu/szczepienia")]
        public string Description { get; set; }

        [Display(Name = "Weterynarz")]
        public string? VetName { get; set; }

        public int AnimalId { get; set; }

        [ForeignKey("AnimalId")]
        public virtual Animal? Animal { get; set; }
    }
}