using Microsoft.AspNetCore.Mvc;
using Shelter.Models;
using System.Security.Cryptography;
using System.Text;

namespace Shelter.Controllers
{
    public class AccountController : Controller
    {
        private ShelterDbContext db;

        public AccountController(ShelterDbContext context)
        {
            db = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string email, string password, string firstName, string lastName, string phone)
        {
            var user = new User();
            user.Email = email;
            user.PasswordHash = HashPassword(password);
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Phone = phone;
            user.Role = "Client";

            db.Users.Add(user);
            db.SaveChanges();

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetString("UserName", firstName + " " + lastName);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email);
            if (user != null && HashPassword(password) == user.PasswordHash)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetString("UserName", user.FirstName + " " + user.LastName);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Nieprawidłowy email lub hasło";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        private string HashPassword(string password)
        {
            var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
