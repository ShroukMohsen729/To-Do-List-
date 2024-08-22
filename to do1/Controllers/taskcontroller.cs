// Controllers/TaskController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using to_do1;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        public toDoContext Context = new toDoContext();
        
        // GET: api/Task
        [HttpGet]
        public async Task<ActionResult<IEnumerable<task>>> GetTasks()
        {
            return await Context.tasks.ToListAsync();
            return Unauthorized();

        }

        // GET: api/Task/5
        [HttpGet("{id}")]
        public async Task<ActionResult<task>> GetTask(int id)
        {

            if (Context.tasks.Where(x => x.Id == id).LastAsync() == null)
                return NotFound();
            return Ok(Context.tasks.Where(x=>x.Id==id).LastAsync());
        }

        // POST: api/Task
        [HttpPost]
        public async Task<ActionResult<task>>PostTask(task task)
        {
            if(task == null)
                return BadRequest("Task couldn't be null!");
            //i think we don't need to check if the values are null or not cuz we handel it in the constrains

            if (task.deadline.HasValue && task.deadline <= task.CreatedAt)
                return BadRequest("Deadline must be greater than the creation date.");


            var person =await Context.People.FindAsync(task.personId);
            if (person == null)
                return NotFound($"Person with ID {task.personId} not found.");

            var category = await Context.Categories.FindAsync(task.CategoryId);
            if (category == null)
                return NotFound($"Category with ID {task.CategoryId} not found.");

            Context.tasks.Add(task);
            await Context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostTask), new { id = task.Id }, task);              //????

        }

        // PUT: api/Task/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, task updatedTask)
        {
            if (updatedTask == null)
                return BadRequest("Update data cannot be null!");

            var existingTask = await Context.tasks.FindAsync(id);

            if (existingTask == null)
                return NotFound();

            if (existingTask.Id != updatedTask.Id)
                throw new InvalidOperationException("Cannot change the ID of an existing task.");

            if (existingTask.CategoryId != updatedTask.CategoryId)
                throw new InvalidOperationException("Cannot change the CategoryId of an existing task.");

            if (existingTask.personId != updatedTask.personId)
                throw new InvalidOperationException("Cannot change the personId of an existing task.");

            if (updatedTask.deadline.HasValue && updatedTask.deadline <= updatedTask.CreatedAt)
                return BadRequest("Deadline must be greater than the creation date.");


            existingTask.name = updatedTask.name ?? existingTask.name;
            existingTask.description = updatedTask.description ?? existingTask.description;
            //existingTask.Category = updatedTask.Category ?? existingTask.Category;
            existingTask.deadline = updatedTask.deadline ?? existingTask.deadline;
            existingTask.CreatedAt = updatedTask.CreatedAt != default(DateTime) ? updatedTask.CreatedAt : existingTask.CreatedAt;
            existingTask.CompletedAt = updatedTask.CompletedAt ?? existingTask.CompletedAt;
            existingTask.status = updatedTask.status ?? existingTask.status;
            // existingTask.after_deadline = updatedTask.after_deadline ?? existingTask.after_deadline;
            existingTask.CategoryId = updatedTask.CategoryId != default(int) ? updatedTask.CategoryId : existingTask.CategoryId;

            await Context.SaveChangesAsync();
            return Ok(existingTask);
        }


        // DELETE: api/Task/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
          var task=await Context.tasks.FindAsync(id);
            if (task == null)
                return NotFound();
            var person=await Context.People.FindAsync(task.personId);
            if (person == null)
                return NotFound($"Person with ID {task.personId} not found.");
            person.Tasks.Remove(task);
            Context.SaveChangesAsync(); 
            return NoContent();
        }


        // POST: api/Task/Complete/5
        [HttpPost("Complete/{id}")]
        public async Task<IActionResult> CompleteTask(int id)
        {
            var task = await Context.tasks.FindAsync(id);

            if (task == null)
                return NotFound("Task not found.");

            if (task.deadline.HasValue && DateTime.UtcNow > task.deadline)
                return BadRequest("Cannot complete the task as it is past the deadline.");
            
            
            task.CompletedAt = DateTime.UtcNow;
            task.status = "Completed"; 

            await Context.SaveChangesAsync();

            return Ok(task);
        }



    }
}
