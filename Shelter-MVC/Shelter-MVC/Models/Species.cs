using System.ComponentModel.DataAnnotations;

namespace Shelter_MVC.Models 
{
    public class Species
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Podaj nazwę gatunku")]
        [Display(Name = "Gatunek")]
        public string Name { get; set; }

        public virtual ICollection<Animal>? Animals { get; set; }
    }
}