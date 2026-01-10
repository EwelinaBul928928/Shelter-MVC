namespace Shelter_MVC.Models
{
    public class HomeViewModel
    {
        public List<Animal> LatestAnimals { get; set; } = new();
        public List<News> LatestNews { get; set; } = new();
    }
}
