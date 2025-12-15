using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shelter.Models;

namespace Shelter.Controllers
{
    public class AnimalsController : Controller
    {
        private ShelterDbContext db;

        public AnimalsController(ShelterDbContext context)
        {
            db = context;
        }

        public IActionResult Index(string species)
        {
            var animals = db.Animals.Where(a => a.Status == "Available").ToList();

            if (species != null && species != "All")
            {
                animals = animals.Where(a => a.Species == species).ToList();
            }

            ViewBag.Species = species;
            return View(animals);
        }

        public IActionResult Details(int id)
        {
            var animal = db.Animals
                .Include(a => a.VeterinaryVisits)
                .FirstOrDefault(a => a.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }
    }
}
