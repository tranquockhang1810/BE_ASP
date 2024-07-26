using BE_ASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BE_ASP.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BE_ASP.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly AppDBContext _context;
        public TaskController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Task
        [HttpGet]
        public async Task<ActionResult<object>> GetTasks(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int? userId,
            [FromQuery] int? priority,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10)
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }

            var query = _context.Tasks.AsQueryable();

            // Filtering
            if (fromDate.HasValue)
            {
                query = query.Where(t => t.CreatedDate >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                query = query.Where(t => t.CreatedDate <= toDate.Value);
            }
            if (userId.HasValue)
            {
                query = query.Where(t => t.UserId == userId.Value);
            }
            if (priority.HasValue)
            {
                query = query.Where(t => t.Priority == priority.Value);
            }

            // Sorting by CreatedDate
            query = query.OrderBy(t => t.CreatedDate);

            // Pagination
            var totalItems = await query.CountAsync();
            var tasks = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();

            // Response
            var response = new
            {
                data = tasks,
                paging = new
                {
                    total = totalItems,
                    page = page,
                    limit = limit
                }
            };

            return Ok(response);
        }

        // GET: api/Task/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTask>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // POST: api/Task
        [HttpPost]
        public async Task<ActionResult<UserTask>> PostTask([FromBody] UserTask task)
        {
            // Ensure the ID is not set
            task.Id = 0;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // PUT: api/Task/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, [FromBody] UserTask task)
        {
            if (id != task.Id)
            {
                return BadRequest("Task ID in URL and body do not match.");
            }

            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            // Update the properties of the existing task
            existingTask.Name = task.Name;
            existingTask.Description = task.Description;
            existingTask.UserId = task.UserId;
            existingTask.CreatedDate = task.CreatedDate;
            existingTask.DueDate = task.DueDate;
            existingTask.Priority = task.Priority;
            existingTask.Status = task.Status;

            _context.Entry(existingTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Task/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
