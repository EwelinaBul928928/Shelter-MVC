using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shelter.Models;

namespace Shelter.Controllers
{
    public class AdminController : Controller
    {
        private ShelterDbContext db;

        public AdminController(ShelterDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Animals()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var animals = db.Animals.ToList();
            return View(animals);
        }

        public IActionResult Adoptions()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var applications = db.AdoptionApplications
                .Include(a => a.User)
                .Include(a => a.Animal)
                .OrderByDescending(a => a.ApplicationDate)
                .ToList();

            return View(applications);
        }

        [HttpPost]
        public IActionResult UpdateAdoptionStatus(int id, string status)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var application = db.AdoptionApplications.Find(id);
            if (application != null)
            {
                application.Status = status;
                if (status == "Approved")
                {
                    var animal = db.Animals.Find(application.AnimalId);
                    if (animal != null)
                    {
                        animal.Status = "Adopted";
                    }
                }
                db.SaveChanges();
            }

            return RedirectToAction("Adoptions");
        }
    }
}
