using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using D301_WebApp.Data;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace D301_WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FoodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Foods
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoods()
        {
            return await _context.Foods.ToListAsync();
        }

        // GET: api/Foods/
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Food>> GetFood(string id)
        {
            var food = await _context.Foods.FindAsync(id);

            if (food == null)
            {
                return NotFound();
            }

            return food;
        }

        // PUT: api/Foods/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFood(string id, Food food)
        {
            if (id != food.Id)
            {
                return BadRequest();
            }

            _context.Entry(food).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodExists(id))
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

        // POST: api/Foods
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Food>> PostFood(Food food)
        {
            _context.Foods.Add(food);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FoodExists(food.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFood", new { id = food.Id }, food);
        }

        // DELETE: api/Foods/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Food>> DeleteFood(string id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();

            return NoContent(); // returns a 204 OK response 
        }

        private bool FoodExists(string id)
        {
            return _context.Foods.Any(e => e.Id == id);
        }


        // GET: api/Foods/search
        [Authorize]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Food>>> SearchFood()
        {
            string term = HttpContext.Request.Query["term"].ToString();
            string offset_string = HttpContext.Request.Query["offset"].ToString();
            string max_string = HttpContext.Request.Query["max"].ToString();
            int max = 10;
            int offset = 0;
            if (offset_string != "") 
            {
                Int32.TryParse(HttpContext.Request.Query["offset"].ToString(), out offset);
            }
            if (max_string != "")
            {
                Int32.TryParse(HttpContext.Request.Query["max"].ToString(), out max);
            }

            //food names that start with the search term should appear first
            List<Food> result = await _context.Foods.Where(p => p.Name.ToLower().StartsWith(term.ToLower())).ToListAsync();
            //next comes words that start with the search term, but aren't at the start of the food name
            result.AddRange(await _context.Foods.Where(p => p.Name.ToLower().Contains(' ' + term.ToLower())).ToListAsync());
            //then everything else
            result.AddRange(await _context.Foods.Where(p => p.Name.ToLower().Contains(term.ToLower())).ToListAsync());

            //remove duplicates
            result = result.Distinct().ToList();

            int max_count = result.Count();

            List<Food> foods = result.Skip(offset).Take(max).ToList();
            if (foods.Count < max_count) {
                Food more = new Food();
                more.Id = "0";
                more.Name = "(Showing " + foods.Count.ToString() + " of " + max_count.ToString() + ")  More...";
                foods.Add(more);
            }
            
            return foods;
        }

        // GET: api/Foods/search
        [Authorize]
        [HttpGet("variation")]
        public async Task<ActionResult<IEnumerable<FoodVariation>>> SearchVariation()
        {
            string id = HttpContext.Request.Query["id"].ToString();
            return await _context.FoodVariations.Where(v => v.FoodId == id).ToListAsync();
        }
    }
}
