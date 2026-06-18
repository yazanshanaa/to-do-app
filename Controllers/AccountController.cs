using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly TodoContext context;

        public AccountController(TodoContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View(new User());
        }

        [HttpPost]
        public IActionResult SignUp(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            bool taken = context.Users.Any(u => u.Username == user.Username);
            if (taken)
            {
                ModelState.AddModelError("Username", "This username is already taken.");
                return View(user);
            }

            context.Users.Add(user);
            context.SaveChanges();
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View(new User());
        }

        [HttpPost]
        public IActionResult SignIn(User user)
        {
            var found = context.Users.FirstOrDefault(
                u => u.Username == user.Username && u.Password == user.Password);

            if (found == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(user);
            }

            // store user data in session state (Chapter 9)
            HttpContext.Session.SetInt32("UserId", found.UserId);
            HttpContext.Session.SetString("Username", found.Username);

            return RedirectToAction("Index", "Tasks");
        }

        public IActionResult Logout()
        {
            // clear the session state (Chapter 9)
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("SignIn");
            }
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("SignIn");
            }

            var user = context.Users.Find(userId);
            if (user == null || user.Password != oldPassword)
            {
                ModelState.AddModelError("", "The old password is not correct.");
                return View();
            }

            user.Password = newPassword;
            context.SaveChanges();
            ViewBag.Message = "Your password has been changed.";
            return View();
        }

        public IActionResult Details()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("SignIn");
            }

            var user = context.Users.Find(userId);
            return View(user);
        }
    }
}
