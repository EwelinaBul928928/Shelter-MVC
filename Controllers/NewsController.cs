using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shelter.Models;

namespace Shelter.Controllers
{
    public class NewsController : Controller
    {
        private ShelterDbContext db;

        public NewsController(ShelterDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var posts = db.NewsPosts.OrderByDescending(p => p.PublicationDate).ToList();
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = db.NewsPosts
                .Include(p => p.Author)
                .FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
    }
}
