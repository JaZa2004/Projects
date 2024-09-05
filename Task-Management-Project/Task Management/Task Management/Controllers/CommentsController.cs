using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Task_Management.Models;

namespace Task_Management.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }



        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized();
                }

                comment.UserId = int.Parse(userId);
                comment.CreatedDate = DateTime.Now;

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                return RedirectToAction("EditTask","Tasks", new { id = comment.TaskId });
            }

            var task = await _context.Tasks
                .Include(p=>p.Project)
                .Include(t => t.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(t => t.TaskId == comment.TaskId);

            if (task == null)
            {
                return NotFound();
            }

            ViewData["task"] = task;
            return View("EditTask", task);
        }

        // Your existing actions for editing, completing tasks, etc.
    }

}

