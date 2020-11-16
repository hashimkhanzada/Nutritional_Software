using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using D301_WebApp.Data;
using Microsoft.AspNetCore.Authorization;

namespace D301_WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntakesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IntakesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Intakes
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Intake>>> GetIntakes()
        {
            return await _context.Intakes.ToListAsync();
        }

        // GET: api/Intakes/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Intake>> GetIntake(int id)
        {
            var intake = await _context.Intakes.FindAsync(id);

            if (intake == null)
            {
                return NotFound();
            }

            return intake;
        }

        // PUT: api/Intakes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIntake(int id, Intake intake)
        {
            if (id != intake.Id)
            {
                return BadRequest();
            }

            _context.Entry(intake).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IntakeExists(id))
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

        // POST: api/Intakes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Intake>> PostIntake(Intake intake)
        {
            _context.Intakes.Add(intake);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIntake", new { id = intake.Id }, intake);
        }

        // DELETE: api/Intakes/5
        [Authorize]
        [Route("Delete")]
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult<Intake>> Delete (int id)
        {
            var intake = await _context.Intakes.FindAsync(id);
            if (intake == null)
            {
                return NotFound();
            } 
            else if (id < 1)
            {
                return BadRequest();
            }

            _context.Intakes.Remove(intake);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool IntakeExists(int id)
        {
            return _context.Intakes.Any(e => e.Id == id);
        }
    }
}
