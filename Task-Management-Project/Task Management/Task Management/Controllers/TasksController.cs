using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Xml.Linq;
using Task_Management.Models;
namespace Task_Management.Controllers
{
    public class TasksController : Controller
    {
        ApplicationDbContext _context;
        public TasksController(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<IActionResult> Index()
        {
            ViewBag.CurrentPage = "Tasks";
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                ViewBag.tasks = await _context.Tasks
                    .Include(t => t.Project) 
                    .Include(t => t.AssignedUser)
                    .Where(t => t.AssignedUserId == userId)
                    .ToListAsync();

                return View();
            }
            else
            {
                TempData["Error"] = "User ID claim not found or invalid.";
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> addTask(Models.Task task)
        {
            if (ModelState.IsValid)
            {
                task.Status="pending";
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewProjectTasks), new { id = task.ProjectId }); // Redirect to the list of tasks or another appropriate page
            }
            return View();

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> addTask(int? id)
        {
            ViewBag.CurrentPage = "Tasks";
            var project = await _context.Projects
               .FirstOrDefaultAsync(m => m.ProjectId == id);
            ViewData["employees"] = await _context.Users
                .Where(u => u.Role == "Employee")
                .ToListAsync();
            ViewData["project"] = project;
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> EditTask(int? id)
        {
            ViewBag.CurrentPage = "Tasks";
            var task = await _context.Tasks
            .Include(t => t.Comments)
            .ThenInclude(c => c.User)
            .Include(t => t.Project)
            .Include(t => t.AssignedUser)
            .FirstOrDefaultAsync(t => t.TaskId == id);
            ViewData["task"] = task;
            if (User.IsInRole("Admin"))
            {
                ViewData["employees"] = await _context.Users
                                .Where(u => u.Role == "Employee")
                                .ToListAsync();
                return View("EditTaskAdmin");
            }
            else
            {
                return View("EditTaskEmployee");

            }
        }

        [HttpPost]
        public async Task<IActionResult> CompleteTask(int? id)
        {
            var task = await _context.Tasks
               .FirstOrDefaultAsync(m => m.TaskId == id);
            if (task != null)
            {
                task.CompletedByEmployee = true;
                task.ApprovedByAdmin = "pending";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ViewProjectTasks(int? id)
        {
            IQueryable<Models.Task> query = _context.Tasks
                .Include(m => m.AssignedUser)
                .Include(m => m.Project);

            if (id.HasValue)
            {
                query = query.Where(t => t.ProjectId == id);
            }
            else
            {
                ViewBag.CurrentPage = "Tasks";

            }

            var tasks = await query.ToListAsync();
            ViewBag.tasks = tasks;

            return View("Index");
        }



        [HttpPost]
        public IActionResult EditTaskAdmin(int id, int? AssignedUserId, string ApprovedByAdmin)
        {
            var existingTask = _context.Tasks.Find(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            if (AssignedUserId.HasValue)
            {
                existingTask.AssignedUserId = AssignedUserId.Value;
            }
            existingTask.ApprovedByAdmin = ApprovedByAdmin;
            if (ApprovedByAdmin == "Approved")
            {
                existingTask.Status = "Completed";
            }
            else
            {
                existingTask.CompletedByEmployee = false;
            }

            _context.SaveChanges();

            return RedirectToAction("ViewProjectTasks", new { id = existingTask.ProjectId } );
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            // Redirect to the action where comments are displayed, e.g., return RedirectToAction("ViewProjectTasks", new { id = comment.TaskId });
            return RedirectToAction("EditTask", new {id=comment.TaskId}); // Redirect wherever appropriate
        }



    }
}
