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
    public class FoodBookmarksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FoodBookmarksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FoodBookmarks
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodBookmark>>> GetFoodBookmarks()
        {
            
            string userId = HttpContext.Request.Query["userId"].ToString();
            List<FoodBookmark> foodBookmarks = await _context.FoodBookmarks.Include(s => s.Food).Include(s => s.CustomFood).Where(s => s.UserId == userId).ToListAsync();

            return foodBookmarks;
        }

        // GET: api/FoodBookmarks/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<FoodBookmark>>> GetFoodBookmark(string id)
        {
            string userId = HttpContext.Request.Query["userId"].ToString();
            var foodBookmark = await _context.FoodBookmarks.Where(s => s.FoodId == id || s.CustomFoodId == id).Where(s => s.UserId == userId).ToListAsync();

            if (foodBookmark == null)
            {
                return NotFound();
            }

            return foodBookmark;
        }

        // PUT: api/FoodBookmarks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[Authorize]
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutFoodBookmark(int id, FoodBookmark foodBookmark)
        //{
        //    if (id != foodBookmark.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(foodBookmark).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!FoodBookmarkExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/FoodBookmarks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<FoodBookmark>> PostFoodBookmark([FromBody]FoodBookmark foodBookmark)
        {

            if (foodBookmark.FoodId != null) 
            {
                if (FoodBookmarkExists(foodBookmark.FoodId, "nfood", foodBookmark.UserId) == false)
                {
                    _context.FoodBookmarks.Add(foodBookmark);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetFoodBookmark", new { id = foodBookmark.Id }, foodBookmark);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (FoodBookmarkExists(foodBookmark.CustomFoodId, "cfood", foodBookmark.UserId) == false)
                {
                    _context.FoodBookmarks.Add(foodBookmark);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetFoodBookmark", new { id = foodBookmark.Id }, foodBookmark);
                }
                else
                {
                    return null;
                }
            } 
            
        }

        // DELETE: api/FoodBookmarks/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<FoodBookmark>> DeleteFoodBookmark(int id)
        {
            var foodBookmark = await _context.FoodBookmarks.FindAsync(id);
            if (foodBookmark == null)
            {
                return NotFound();
            }

            _context.FoodBookmarks.Remove(foodBookmark);
            await _context.SaveChangesAsync();

            return foodBookmark;
        }

        private bool FoodBookmarkExists(string id, string typeFood, string userid)
        {
            if(typeFood == "nfood")
            {
                return _context.FoodBookmarks.Any(e => e.FoodId == id && e.UserId == userid);
            }
            else 
            {
                return _context.FoodBookmarks.Any(e => e.CustomFoodId == id && e.UserId == userid);
            }
            
        }
    }
}
