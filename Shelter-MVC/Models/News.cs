using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shelter_MVC.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Treść jest wymagana")]
        [Display(Name = "Treść")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "Data publikacji")]
        [DataType(DataType.DateTime)]
        public DateTime PublicationDate { get; set; } = DateTime.Now;

        [Display(Name = "Autor")]
        public string? AuthorId { get; set; }
    }
}
