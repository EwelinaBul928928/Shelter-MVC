using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shelter_MVC.Data;
using Shelter_MVC.Models;

namespace Shelter_MVC.Controllers
{
    [Authorize(Roles = "Administrator,Client")]
    public class VeterinarianController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VeterinarianController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var query = _context.Animals
                .Include(a => a.HealthRecords)
                .AsQueryable();

            if (User.IsInRole("Client") && !User.IsInRole("Administrator"))
            {
                var userId = User.Identity?.Name;
                query = query.Where(a => a.IsAdopted && a.AdoptedByUserId == userId);
            }

            var animals = await query.ToListAsync();
            return View(animals);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = _context.Animals
                .Include(a => a.HealthRecords)
                .AsQueryable();

            if (User.IsInRole("Client") && !User.IsInRole("Administrator"))
            {
                var userId = User.Identity?.Name;
                query = query.Where(a => a.AdoptedByUserId == userId);
            }

            var animal = await query.FirstOrDefaultAsync(m => m.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreateAppointment(int? animalId)
        {
            if (animalId == null)
            {
                return NotFound();
            }

            var userId = User.Identity?.Name;
            var animal = await _context.Animals
                .FirstOrDefaultAsync(a => a.Id == animalId && a.AdoptedByUserId == userId && a.IsAdopted);

            if (animal == null)
            {
                return NotFound();
            }

            ViewData["AnimalId"] = animalId;
            ViewData["AnimalName"] = animal.Name;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreateAppointment([Bind("AnimalId,VisitDate,Description,VetName,Status")] HealthRecord healthRecord)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity?.Name;
                var animal = await _context.Animals
                    .FirstOrDefaultAsync(a => a.Id == healthRecord.AnimalId && a.AdoptedByUserId == userId && a.IsAdopted);

                if (animal == null)
                {
                    return NotFound();
                }

                healthRecord.AppointmentDate = healthRecord.VisitDate;
                healthRecord.Status = VisitStatus.Zaplanowana;
                _context.Add(healthRecord);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Wizyta została umówiona pomyślnie!";
                return RedirectToAction(nameof(Details), new { id = healthRecord.AnimalId });
            }

            ViewData["AnimalId"] = healthRecord.AnimalId;
            var animalForName = await _context.Animals.FindAsync(healthRecord.AnimalId);
            ViewData["AnimalName"] = animalForName?.Name;
            return View(healthRecord);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateRecord(int? animalId)
        {
            if (animalId == null)
            {
                ViewData["AnimalId"] = new SelectList(_context.Animals, "Id", "Name");
            }
            else
            {
                var animal = await _context.Animals.FindAsync(animalId);
                if (animal == null)
                {
                    return NotFound();
                }
                ViewData["AnimalId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Animals, "Id", "Name", animalId);
                ViewData["AnimalName"] = animal.Name;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateRecord([Bind("AnimalId,VisitDate,Description,VetName,Status")] HealthRecord healthRecord)
        {
            if (ModelState.IsValid)
            {
                healthRecord.AppointmentDate = DateTime.Now;
                if (healthRecord.Status == VisitStatus.Zaplanowana && healthRecord.VisitDate <= DateTime.Now)
                {
                    healthRecord.Status = VisitStatus.Odbyta;
                }
                _context.Add(healthRecord);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Rekord zdrowia został dodany pomyślnie!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["AnimalId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Animals, "Id", "Name", healthRecord.AnimalId);
            return View(healthRecord);
        }
    }
}
