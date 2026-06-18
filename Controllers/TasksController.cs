using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly TodoContext context;

        public TasksController(TodoContext context)
        {
            this.context = context;
        }

        // list + search by title (only the signed-in user's tasks)
        public IActionResult Index(string searchTitle)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("SignIn", "Account");
            }

            var query = context.Tasks.Where(t => t.UserId == userId);

            if (!string.IsNullOrEmpty(searchTitle))
            {
                query = query.Where(t => t.Title.Contains(searchTitle));
            }

            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.SearchTitle = searchTitle;
            return View(query.OrderBy(t => t.Title).ToList());
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            return View(new TaskItem());
        }

        [HttpPost]
        public IActionResult Add(TaskItem task)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("SignIn", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(task);
            }

            task.UserId = (int)userId;
            context.Tasks.Add(task);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Toggle(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("SignIn", "Account");
            }

            var task = context.Tasks.Find(id);
            if (task != null && task.UserId == userId)
            {
                task.IsDone = !task.IsDone;
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("SignIn", "Account");
            }

            var task = context.Tasks.Find(id);
            if (task != null && task.UserId == userId)
            {
                context.Tasks.Remove(task);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
