using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace to_do1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public toDoContext Context = new toDoContext();


        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {

            var category = await Context.Categories.Include(p => p.Tasks).ToListAsync();
            if (category == null || !category.Any())
                return NotFound("No persons found.");

            return Ok(category);
        }

       
        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Category>>> GetCategory(int id)
        {
             if (Context.Categories.Include(p => p.Tasks).ToListAsync() == null)
                return NotFound();

            return Ok(Context.Categories.Include(p => p.Tasks).ToListAsync());

        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            Context.Categories.Add(category);
            await Context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            Context.Entry(category).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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


        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await Context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            Context.Categories.Remove(category);
            await Context.SaveChangesAsync();

            return NoContent();
        }


        private bool CategoryExists(int id)
        {
            return Context.Categories.Any(e => e.Id == id);
        }
    }
}
