using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shelter.Models;

namespace Shelter.Controllers
{
    public class VeterinaryController : Controller
    {
        private readonly ShelterDbContext db;

        public VeterinaryController(ShelterDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var veterinarians = db.Veterinarians.ToList();
            var services = db.VeterinaryServices.ToList();

            ViewBag.Veterinarians = veterinarians;
            ViewBag.Services = services;

            return View();
        }

        public IActionResult AllAnimalsHistory()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var animals = db.Animals
                .Include(a => a.VeterinaryVisits)
                    .ThenInclude(v => v.Veterinarian)
                .Include(a => a.VeterinaryVisits)
                    .ThenInclude(v => v.Service)
                .Where(a => a.VeterinaryVisits.Any())
                .ToList();

            return View(animals);
        }

        public IActionResult RegisterClientAnimal()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        public IActionResult RegisterClientAnimal(string name, string species, string breed, int age, string gender, string description, string photoUrl)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var clientAnimal = new ClientAnimal();
            clientAnimal.UserId = userId.Value;
            clientAnimal.Name = name;
            clientAnimal.Species = species;
            clientAnimal.Breed = breed;
            clientAnimal.Age = age;
            clientAnimal.Gender = gender;
            clientAnimal.Description = description;
            clientAnimal.PhotoUrl = photoUrl;
            clientAnimal.RegistrationDate = DateTime.Now;

            db.ClientAnimals.Add(clientAnimal);
            db.SaveChanges();

            return RedirectToAction("MyClientAnimals");
        }

        public IActionResult MyClientAnimals()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var clientAnimals = db.ClientAnimals
                .Where(c => c.UserId == userId)
                .ToList();

            var userApplications = db.AdoptionApplications
                .Where(a => a.UserId == userId && a.Status == "Approved")
                .Include(a => a.Animal)
                    .ThenInclude(an => an.VeterinaryVisits)
                        .ThenInclude(v => v.Veterinarian)
                .Include(a => a.Animal)
                    .ThenInclude(an => an.VeterinaryVisits)
                        .ThenInclude(v => v.Service)
                .ToList();

            var appointments = db.VeterinaryAppointments
                .Where(a => a.UserId == userId && a.ClientAnimalId != null)
                .Include(a => a.ClientAnimal)
                .Include(a => a.Veterinarian)
                .Include(a => a.Service)
                .ToList();

            ViewBag.ClientAnimals = clientAnimals;
            ViewBag.AdoptedAnimals = userApplications.Select(a => a.Animal).ToList();
            ViewBag.Appointments = appointments;

            return View();
        }

        public IActionResult BookAppointment()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var clientAnimals = db.ClientAnimals
                .Where(c => c.UserId == userId)
                .ToList();

            var userApplications = db.AdoptionApplications
                .Where(a => a.UserId == userId && a.Status == "Approved")
                .Include(a => a.Animal)
                .ToList();

            var veterinarians = db.Veterinarians.ToList();
            var services = db.VeterinaryServices.ToList();

            ViewBag.ClientAnimals = clientAnimals;
            ViewBag.AdoptedAnimals = userApplications.Select(a => a.Animal).ToList();
            ViewBag.Veterinarians = veterinarians;
            ViewBag.Services = services;

            return View();
        }

        [HttpPost]
        public IActionResult BookAppointment(int? animalId, int? clientAnimalId, int veterinarianId, int serviceId, DateTime appointmentDate, string notes)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (animalId == null && clientAnimalId == null)
            {
                return RedirectToAction("BookAppointment");
            }

            var service = db.VeterinaryServices.Find(serviceId);
            if (service == null)
            {
                return RedirectToAction("BookAppointment");
            }

            var appointment = new VeterinaryAppointment();
            appointment.UserId = userId.Value;
            appointment.VeterinarianId = veterinarianId;
            appointment.ServiceId = serviceId;
            appointment.AnimalId = animalId;
            appointment.ClientAnimalId = clientAnimalId;
            appointment.AppointmentDate = appointmentDate;
            appointment.Notes = notes;
            appointment.Status = "Scheduled";
            appointment.Cost = service.Price;
            appointment.IsFree = false;

            db.VeterinaryAppointments.Add(appointment);
            db.SaveChanges();

            return RedirectToAction("MyAppointments");
        }

        public IActionResult MyAppointments()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var appointments = db.VeterinaryAppointments
                .Where(a => a.UserId == userId)
                .Include(a => a.Veterinarian)
                .Include(a => a.Service)
                .Include(a => a.Animal)
                .Include(a => a.ClientAnimal)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();

            return View(appointments);
        }

        public IActionResult BookAppointmentForShelterAnimal()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var animals = db.Animals
                .Where(a => a.Status == "Available")
                .ToList();

            var veterinarians = db.Veterinarians.ToList();
            var services = db.VeterinaryServices.ToList();

            ViewBag.Animals = animals;
            ViewBag.Veterinarians = veterinarians;
            ViewBag.Services = services;

            return View();
        }

        [HttpPost]
        public IActionResult BookAppointmentForShelterAnimal(int animalId, int veterinarianId, int serviceId, DateTime appointmentDate, string notes)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var appointment = new VeterinaryAppointment();
            appointment.AnimalId = animalId;
            appointment.VeterinarianId = veterinarianId;
            appointment.ServiceId = serviceId;
            appointment.AppointmentDate = appointmentDate;
            appointment.Notes = notes;
            appointment.Status = "Scheduled";
            appointment.Cost = 0;
            appointment.IsFree = true;

            db.VeterinaryAppointments.Add(appointment);
            db.SaveChanges();

            return RedirectToAction("ShelterAppointments");
        }

        public IActionResult ShelterAppointments()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var appointments = db.VeterinaryAppointments
                .Include(a => a.Veterinarian)
                .Include(a => a.Service)
                .Include(a => a.Animal)
                .Include(a => a.ClientAnimal)
                .Include(a => a.User)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();

            return View(appointments);
        }
    }
}
