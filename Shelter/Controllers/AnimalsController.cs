using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shelter.Models;

namespace Shelter.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly ShelterDbContext db;

        public AnimalsController(ShelterDbContext context)
        {
            db = context;
        }

        public IActionResult Index(string species)
        {
            var query = db.Animals.Where(a => a.Status == "Available");

            if (species != null && species != "All")
            {
                if (species == "Inne")
                {
                    query = query.Where(a => a.Species != "Pies" && a.Species != "Kot");
                }
                else
                {
                    query = query.Where(a => a.Species == species);
                }
            }

            ViewBag.Species = species;
            return View(query.ToList());
        }

        public IActionResult Details(int id)
        {
            var animal = db.Animals
                .Include(a => a.VeterinaryVisits)
                    .ThenInclude(v => v.Veterinarian)
                .Include(a => a.VeterinaryVisits)
                    .ThenInclude(v => v.Service)
                .FirstOrDefault(a => a.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }
    }
}
