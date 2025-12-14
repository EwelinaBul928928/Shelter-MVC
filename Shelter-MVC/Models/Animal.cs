using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shelter_MVC.Models
{
    public class Animal
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane")]
        [Display(Name = "Imię zwierzaka")]
        public string Name { get; set; }

        [Display(Name = "Wiek (w latach)")]
        [Range(0, 30, ErrorMessage = "Wiek musi być między 0 a 30 lat")]
        public int Age { get; set; }

        [Display(Name = "Opis")]
        public string? Description { get; set; }

        [Display(Name = "Zdjęcie (URL)")]
        public string? PhotoUrl { get; set; }

        [Display(Name = "Czy zaadoptowany?")]
        public bool IsAdopted { get; set; } = false;

        [Display(Name = "Gatunek")]
        public int SpeciesId { get; set; }

        [ForeignKey("SpeciesId")]
        public virtual Species? Species { get; set; }

        public virtual ICollection<HealthRecord>? HealthRecords { get; set; }
    }
}