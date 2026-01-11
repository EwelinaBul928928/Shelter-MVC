using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shelter.Models;

namespace Shelter.Controllers
{
    public class AdminController : Controller
    {
        private readonly ShelterDbContext db;

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

            var totalAnimals = db.Animals.Count();
            var availableAnimals = db.Animals.Count(a => a.Status == "Available");
            var adoptedAnimals = db.Animals.Count(a => a.Status == "Adopted");
            var pendingApplications = db.AdoptionApplications.Count(a => a.Status == "Pending");
            var totalUsers = db.Users.Count();
            var totalNews = db.NewsPosts.Count();

            ViewBag.TotalAnimals = totalAnimals;
            ViewBag.AvailableAnimals = availableAnimals;
            ViewBag.AdoptedAnimals = adoptedAnimals;
            ViewBag.PendingApplications = pendingApplications;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalNews = totalNews;

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

        public IActionResult AdoptionDetails(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var application = db.AdoptionApplications
                .Include(a => a.User)
                .Include(a => a.Animal)
                .FirstOrDefault(a => a.Id == id);

            if (application == null)
            {
                return NotFound();
            }

            return View(application);
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

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(string name, string species, string breed, int age, string gender, string description, string photoUrl)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var animal = new Animal();
            animal.Name = name;
            animal.Species = species;
            animal.Breed = breed;
            animal.Age = age;
            animal.Gender = gender;
            animal.Description = description;
            animal.PhotoUrl = photoUrl;
            animal.Status = "Available";
            animal.AdmissionDate = DateTime.Now;

            db.Animals.Add(animal);
            db.SaveChanges();

            return RedirectToAction("Animals");
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var animal = db.Animals.Find(id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        [HttpPost]
        public IActionResult Edit(int id, string name, string species, string breed, int age, string gender, string description, string photoUrl, string status)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var animal = db.Animals.Find(id);
            if (animal != null)
            {
                animal.Name = name;
                animal.Species = species;
                animal.Breed = breed;
                animal.Age = age;
                animal.Gender = gender;
                animal.Description = description;
                animal.PhotoUrl = photoUrl;
                animal.Status = status;

                db.SaveChanges();
            }

            return RedirectToAction("Animals");
        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var animal = db.Animals.Find(id);
            if (animal != null)
            {
                db.Animals.Remove(animal);
                db.SaveChanges();
            }

            return RedirectToAction("Animals");
        }
    }
}
