using Microsoft.AspNetCore.Mvc;
using Shelter.Models;

namespace Shelter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsApiController : ControllerBase
    {
        private readonly ShelterDbContext db;

        public AnimalsApiController(ShelterDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult GetAnimals()
        {
            var animals = db.Animals.ToList();
            return Ok(animals);
        }

        [HttpGet("{id}")]
        public IActionResult GetAnimal(int id)
        {
            var animal = db.Animals.Find(id);

            if (animal == null)
            {
                return NotFound();
            }

            return Ok(animal);
        }

        [HttpPost]
        public IActionResult PostAnimal(Animal animal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Animals.Add(animal);
            db.SaveChanges();

            return CreatedAtAction("GetAnimal", new { id = animal.Id }, animal);
        }

        [HttpPut("{id}")]
        public IActionResult PutAnimal(int id, Animal animal)
        {
            if (id != animal.Id)
            {
                return BadRequest();
            }

            var existingAnimal = db.Animals.Find(id);
            if (existingAnimal == null)
            {
                return NotFound();
            }

            existingAnimal.Name = animal.Name;
            existingAnimal.Species = animal.Species;
            existingAnimal.Breed = animal.Breed;
            existingAnimal.Age = animal.Age;
            existingAnimal.Gender = animal.Gender;
            existingAnimal.Description = animal.Description;
            existingAnimal.PhotoUrl = animal.PhotoUrl;
            existingAnimal.Status = animal.Status;

            db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAnimal(int id)
        {
            var animal = db.Animals.Find(id);
            if (animal == null)
            {
                return NotFound();
            }

            db.Animals.Remove(animal);
            db.SaveChanges();

            return NoContent();
        }
    }
}
