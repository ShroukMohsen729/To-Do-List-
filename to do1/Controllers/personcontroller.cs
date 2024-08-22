// Controllers/PersonController.cs
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using to_do1;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity.Data;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
       
       public toDoContext Context = new toDoContext();


        [HttpGet]
        public async Task<ActionResult<List<person>>> GetAll()
        {

            var persons = await Context.People.Include(p => p.Tasks).ToListAsync();
            if (persons == null || !persons.Any())
                return NotFound("No persons found.");

            return Ok(persons);
        }


        // GET: api/Person/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<person>>> GetPerson(int id)
        {

            if (Context.People.Include(p => p.Tasks).ToListAsync() == null)
                return NotFound();

            return Ok(Context.People.Include(p => p.Tasks).ToListAsync());

        }
      


        // POST: api/Person
        [HttpPost("Sign up")]    
        public async Task<ActionResult<person>> PostPerson([FromBody] person per)
        {
            if (per == null)
                return BadRequest("Member cannot be null.");

            if (string.IsNullOrWhiteSpace(per.name) || string.IsNullOrWhiteSpace(per.email) || string.IsNullOrWhiteSpace(per.password))
                return BadRequest("Make Sure That All Data Is True.");

            var existingPerson = await Context.People.FirstOrDefaultAsync(p => p.email == per.email);
            if (existingPerson != null)
                return BadRequest("An account with this email already exists.");

            per.password = BCrypt.Net.BCrypt.HashPassword(per.password);

            Context.People.Add(per);
            await Context.SaveChangesAsync();



            return CreatedAtAction(nameof(GetPerson), new { id = per.Id }, per);
        }

         //confirm email
        // reset password!


        // PUT: api/Person/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] person per)
        {
            if (per == null)
                return BadRequest("Update data cannot be null.");

            var pers = await Context.People.FindAsync(id);

            if (pers == null)
                return NotFound();

            if (pers.Id != per.Id)
                return BadRequest("cannot Change ID!");

            pers.Id = pers.Id;                                       //cannot change id
            pers.name = per.name??pers.name;
            pers.email = per.email??pers.email;
            pers.password = per.password??pers.password;

            await Context.SaveChangesAsync();

            return Ok(pers);
        }




        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] rules.Login l1)
        {
            try
            {

                var person = Context.People.FirstOrDefault(x => x.email == l1.Username);
                if (person == null)
                    return Unauthorized("Invalid email or password.");

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(l1.Password, person.password);
                if (!isPasswordValid)
                    return Unauthorized("Invalid email or password.");


                return Ok("Login successful.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        // DELETE: api/Person/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var per = await Context.People.FindAsync(id);

            if (per == null)
                return NotFound();
            Context.People.Remove(per);
            await Context.SaveChangesAsync();
            return NoContent();
        }
    }
}
