using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shelter.Models;

namespace Shelter.Controllers
{
    public class AdoptionController : Controller
    {
        private ShelterDbContext db;

        public AdoptionController(ShelterDbContext context)
        {
            db = context;
        }

        public IActionResult Apply(int animalId)
        {
            var animal = db.Animals.Find(animalId);
            if (animal == null)
            {
                return NotFound();
            }

            ViewBag.Animal = animal;
            return View();
        }

        [HttpPost]
        public IActionResult Apply(int animalId, string address, string phone, string experienceWithAnimals, string hasOtherPets, string hasGarden, string livingSituation, string? notes)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var application = new AdoptionApplication();
            application.UserId = userId.Value;
            application.AnimalId = animalId;
            application.ApplicationDate = DateTime.Now;
            application.Status = "Pending";
            application.Address = address;
            application.Phone = phone;
            application.ExperienceWithAnimals = experienceWithAnimals;
            application.HasOtherPets = hasOtherPets;
            application.HasGarden = hasGarden;
            application.LivingSituation = livingSituation;
            application.Notes = notes;

            db.AdoptionApplications.Add(application);
            db.SaveChanges();

            return RedirectToAction("MyApplications");
        }

        public IActionResult MyApplications()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var applications = db.AdoptionApplications
                .Include(a => a.Animal)
                .Where(a => a.UserId == userId.Value)
                .OrderByDescending(a => a.ApplicationDate)
                .ToList();

            return View(applications);
        }
    }
}
