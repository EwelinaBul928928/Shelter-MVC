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
    public class AdoptionApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdoptionApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var query = _context.AdoptionApplications.Include(a => a.Animal).AsQueryable();
            
            if (User.IsInRole("Client") && !User.IsInRole("Administrator"))
            {
                var userId = User.Identity?.Name;
                query = query.Where(a => a.UserId == userId);
            }
            
            var applications = await query.OrderByDescending(a => a.ApplicationDate).ToListAsync();
            return View(applications);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = _context.AdoptionApplications
                .Include(a => a.Animal)
                .AsQueryable();

            if (User.IsInRole("Client") && !User.IsInRole("Administrator"))
            {
                var userId = User.Identity?.Name;
                query = query.Where(a => a.UserId == userId);
            }

            var adoptionApplication = await query.FirstOrDefaultAsync(m => m.Id == id);
            if (adoptionApplication == null)
            {
                return NotFound();
            }

            return View(adoptionApplication);
        }

        [Authorize]
        public async Task<IActionResult> Create(int? animalId)
        {
            var animalsQuery = _context.Animals.Where(a => !a.IsAdopted).AsQueryable();
            
            if (animalId.HasValue)
            {
                var animal = await _context.Animals.FindAsync(animalId.Value);
                if (animal == null || animal.IsAdopted)
                {
                    TempData["ErrorMessage"] = "To zwierzę nie jest dostępne do adopcji.";
                    return RedirectToAction("Index", "Animals");
                }
                ViewBag.Animal = animal;
                ViewData["AnimalId"] = new SelectList(animalsQuery, "Id", "Name", animalId.Value);
            }
            else
            {
                ViewData["AnimalId"] = new SelectList(animalsQuery, "Id", "Name");
            }
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Message,AnimalId")] AdoptionApplication adoptionApplication)
        {
            var animal = await _context.Animals.FindAsync(adoptionApplication.AnimalId);
            if (animal == null)
            {
                TempData["ErrorMessage"] = "Wybrane zwierzę nie istnieje.";
                return RedirectToAction("Index", "Animals");
            }
            
            if (animal.IsAdopted)
            {
                TempData["ErrorMessage"] = "To zwierzę jest już zaadoptowane.";
                return RedirectToAction("Index", "Animals");
            }
            
            adoptionApplication.UserId = User.Identity?.Name;
            adoptionApplication.ApplicationDate = DateTime.Now;
            adoptionApplication.Status = ApplicationStatus.Oczekujący;
            
            if (string.IsNullOrWhiteSpace(adoptionApplication.Message))
            {
                ModelState.AddModelError(nameof(adoptionApplication.Message), "Wiadomość jest wymagana.");
            }
            
            if (adoptionApplication.AnimalId == 0)
            {
                ModelState.AddModelError(nameof(adoptionApplication.AnimalId), "Musisz wybrać zwierzę.");
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(adoptionApplication);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Wniosek adopcyjny został wysłany pomyślnie!";
                return RedirectToAction("Index", "Animals");
            }
            
            var animalsQuery = _context.Animals.Where(a => !a.IsAdopted).AsQueryable();
            if (adoptionApplication.AnimalId > 0)
            {
                ViewData["AnimalId"] = new SelectList(animalsQuery, "Id", "Name", adoptionApplication.AnimalId);
                var selectedAnimal = await _context.Animals.FindAsync(adoptionApplication.AnimalId);
                if (selectedAnimal != null)
                {
                    ViewBag.Animal = selectedAnimal;
                }
            }
            else
            {
                ViewData["AnimalId"] = new SelectList(animalsQuery, "Id", "Name");
            }
            
            return View(adoptionApplication);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adoptionApplication = await _context.AdoptionApplications.FindAsync(id);
            if (adoptionApplication == null)
            {
                return NotFound();
            }
            ViewData["AnimalId"] = new SelectList(_context.Animals, "Id", "Name", adoptionApplication.AnimalId);
            return View(adoptionApplication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApplicationDate,Message,Status,UserId,AnimalId")] AdoptionApplication adoptionApplication)
        {
            if (id != adoptionApplication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingApplication = await _context.AdoptionApplications
                        .AsNoTracking()
                        .FirstOrDefaultAsync(a => a.Id == id);
                    
                    var animal = await _context.Animals.FindAsync(adoptionApplication.AnimalId);
                    
                    if (existingApplication != null && 
                        existingApplication.Status != ApplicationStatus.Zaakceptowany &&
                        adoptionApplication.Status == ApplicationStatus.Zaakceptowany)
                    {
                        if (animal != null && !animal.IsAdopted)
                        {
                            animal.IsAdopted = true;
                            animal.AdoptedByUserId = adoptionApplication.UserId;
                            _context.Update(animal);
                            
                            var otherApplications = await _context.AdoptionApplications
                                .Where(a => a.AnimalId == adoptionApplication.AnimalId && 
                                           a.Id != id && 
                                           a.Status == ApplicationStatus.Oczekujący)
                                .ToListAsync();

                            foreach (var app in otherApplications)
                            {
                                app.Status = ApplicationStatus.Odrzucony;
                            }
                        }
                    }
                    else if (existingApplication != null && 
                             existingApplication.Status == ApplicationStatus.Zaakceptowany &&
                             adoptionApplication.Status != ApplicationStatus.Zaakceptowany &&
                             animal != null && 
                             animal.AdoptedByUserId == adoptionApplication.UserId)
                    {
                        animal.IsAdopted = false;
                        animal.AdoptedByUserId = null;
                        _context.Update(animal);
                    }

                    _context.Update(adoptionApplication);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Wniosek został zaktualizowany pomyślnie!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdoptionApplicationExists(adoptionApplication.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnimalId"] = new SelectList(_context.Animals, "Id", "Name", adoptionApplication.AnimalId);
            return View(adoptionApplication);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adoptionApplication = await _context.AdoptionApplications
                .Include(a => a.Animal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adoptionApplication == null)
            {
                return NotFound();
            }

            return View(adoptionApplication);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adoptionApplication = await _context.AdoptionApplications.FindAsync(id);
            if (adoptionApplication != null)
            {
                _context.AdoptionApplications.Remove(adoptionApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Accept(int id)
        {
            var adoptionApplication = await _context.AdoptionApplications.FindAsync(id);
            if (adoptionApplication == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals.FindAsync(adoptionApplication.AnimalId);
            if (animal == null)
            {
                TempData["ErrorMessage"] = "Zwierzę związane z tym wnioskiem nie istnieje.";
                return RedirectToAction(nameof(Index));
            }

            if (animal.IsAdopted && animal.AdoptedByUserId != adoptionApplication.UserId)
            {
                TempData["ErrorMessage"] = "To zwierzę jest już zaadoptowane przez innego użytkownika.";
                return RedirectToAction(nameof(Index));
            }

            adoptionApplication.Status = ApplicationStatus.Zaakceptowany;
            
            if (!animal.IsAdopted)
            {
                animal.IsAdopted = true;
                animal.AdoptedByUserId = adoptionApplication.UserId;
                _context.Update(animal);
            }

            var otherApplications = await _context.AdoptionApplications
                .Where(a => a.AnimalId == adoptionApplication.AnimalId && 
                           a.Id != id && 
                           a.Status == ApplicationStatus.Oczekujący)
                .ToListAsync();

            foreach (var app in otherApplications)
            {
                app.Status = ApplicationStatus.Odrzucony;
            }

            _context.Update(adoptionApplication);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = $"Wniosek został zaakceptowany! Zwierzę {animal.Name} zostało oznaczone jako zaadoptowane przez {adoptionApplication.UserId}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Reject(int id)
        {
            var adoptionApplication = await _context.AdoptionApplications
                .Include(a => a.Animal)
                .FirstOrDefaultAsync(a => a.Id == id);
            
            if (adoptionApplication == null)
            {
                return NotFound();
            }

            adoptionApplication.Status = ApplicationStatus.Odrzucony;
            
            _context.Update(adoptionApplication);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = $"Wniosek został odrzucony.";
            return RedirectToAction(nameof(Index));
        }

        private bool AdoptionApplicationExists(int id)
        {
            return _context.AdoptionApplications.Any(e => e.Id == id);
        }
    }
}
