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

        // GET: AdoptionApplications
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AdoptionApplications.Include(a => a.Animal);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AdoptionApplications/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: AdoptionApplications/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["AnimalId"] = new SelectList(_context.Animals, "Id", "Name");
            return View();
        }

        // POST: AdoptionApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,ApplicationDate,Message,Status,UserId,AnimalId")] AdoptionApplication adoptionApplication)
        {
            if (ModelState.IsValid)
            {
                adoptionApplication.UserId = User.Identity?.Name;
                adoptionApplication.ApplicationDate = DateTime.Now;
                _context.Add(adoptionApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var animalsQuery = _context.Animals.Where(a => !a.IsAdopted).AsQueryable();
            ViewData["AnimalId"] = new SelectList(animalsQuery, "Id", "Name", adoptionApplication.AnimalId);
            return View(adoptionApplication);
        }

        // GET: AdoptionApplications/Edit/5
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

        // POST: AdoptionApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    // Jeśli status zmienia się na Zaakceptowany, ustaw zwierzę jako zaadoptowane
                    var existingApplication = await _context.AdoptionApplications
                        .AsNoTracking()
                        .FirstOrDefaultAsync(a => a.Id == id);
                    
                    if (existingApplication != null && 
                        existingApplication.Status != ApplicationStatus.Zaakceptowany &&
                        adoptionApplication.Status == ApplicationStatus.Zaakceptowany)
                    {
                        var animal = await _context.Animals.FindAsync(adoptionApplication.AnimalId);
                        if (animal != null)
                        {
                            animal.IsAdopted = true;
                            animal.AdoptedByUserId = adoptionApplication.UserId;
                            _context.Update(animal);
                        }
                    }

                    _context.Update(adoptionApplication);
                    await _context.SaveChangesAsync();
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

        // GET: AdoptionApplications/Delete/5
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

        // POST: AdoptionApplications/Delete/5
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

        private bool AdoptionApplicationExists(int id)
        {
            return _context.AdoptionApplications.Any(e => e.Id == id);
        }
    }
}
