using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using D301_WebApp.Data;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace D301_WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomFoodsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _clientFactory;

        public CustomFoodsController(ApplicationDbContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;
        }

        // GET: api/CustomFoods
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomFood>>> GetCustomFoods()
        {
            return await _context.CustomFoods.ToListAsync();
        }

        // GET: api/CustomFoods/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomFood>> GetCustomFood(string id)
        {
            var customFood = await _context.CustomFoods.FindAsync(id);

            if (customFood == null)
            {
                return NotFound();
            }

            return customFood;
        }

        // PUT: api/CustomFoods/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomFood(string id, CustomFood customFood)
        {
            if (id != customFood.Id)
            {
                return BadRequest();
            }

            _context.Entry(customFood).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomFoodExists(id))
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

        // POST: api/CustomFoods
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CustomFood>> PostCustomFood([FromBody]CustomFood customFood)
        {
            if(CustomFoodExists(customFood.Id) == false)
            {
                _context.CustomFoods.Add(customFood);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetCustomFood", new { id = customFood.Id }, customFood);
            }
            else
            {
                return null;
            }
            
        }

        // POST: api/CustomFoods/variations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost("variations")]
        public async Task<ActionResult<CustomFoodVariation>> PostCustomFoodVariation([FromBody] CustomFoodVariation customFoodVariation)
        {
            if (CustomFoodVariationExists(customFoodVariation.Id) == false)
            {
                _context.CustomFoodVariations.Add(customFoodVariation);
                await _context.SaveChangesAsync();
                return customFoodVariation;
            }
            else
            {
                return null;
            }

        }

        // DELETE: api/CustomFoods/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomFood>> DeleteCustomFood(string id)
        {
            var customFood = await _context.CustomFoods.FindAsync(id);
            if (customFood == null)
            {
                return NotFound();
            }

            _context.CustomFoods.Remove(customFood);
            await _context.SaveChangesAsync();

            return customFood;
        }

        // GET: api/CustomFoods/search
        [Authorize]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CustomFood>>> SearchFood()
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
            List<CustomFood> result = await _context.CustomFoods.Where(p => p.Name.ToLower().StartsWith(term.ToLower())).ToListAsync();
            //next comes words that start with the search term, but aren't at the start of the food name
            result.AddRange(await _context.CustomFoods.Where(p => p.Name.ToLower().Contains(' ' + term.ToLower())).ToListAsync());
            //then everything else
            result.AddRange(await _context.CustomFoods.Where(p => p.Name.ToLower().Contains(term.ToLower())).ToListAsync());
            result.AddRange(await _context.CustomFoods.Where(p => p.Name.ToLower().Replace("-", "").Replace(" ", "").Contains(term.ToLower().Replace(" ", ""))).ToListAsync());

            //remove duplicates
            result = result.Distinct().ToList();

            int max_count = result.Count();

            List<CustomFood> customFoods = result.Skip(offset).Take(max).ToList();
            if (customFoods.Count < max_count)
            {
                CustomFood more = new CustomFood();
                more.Id = "0";
                more.Name = "(Showing " + customFoods.Count.ToString() + " of " + max_count.ToString() + ")  More...";
                customFoods.Add(more);
            }

            return customFoods;
        }


        // GET: api/CustomFoods/token
        [Authorize]
        [HttpGet("token")]
        public async Task<string> GetToken()
        {
            var client = _clientFactory.CreateClient();

            var byteArray = Encoding.ASCII.GetBytes("572906c59b144f3f829e23a69932eb74:80dfa3d3deff46afa9a524773c2ffd1a");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var values = new Dictionary<string, string>
                {
                   { "scope", "basic" },
                   { "grant_type", "client_credentials" }
                };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://oauth.fatsecret.com/connect/token", content);

            return await response.Content.ReadAsStringAsync();
        }

        // GET: api/CustomFoods/searchExternal
        [Authorize]
        [HttpGet("searchExternal")]
        public async Task<string> SearchExternalFood()
        {
            string BearerToken = HttpContext.Request.Query["bearerToken"].ToString();
            string Term = HttpContext.Request.Query["term"].ToString();
            //string Term = "toast";

            var client = _clientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

            var values = new Dictionary<string, string>
                {
                   { "scope", "basic" },
                   { "grant_type", "client_credentials" }
                };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync($"https://platform.fatsecret.com/rest/server.api?method=foods.search&search_expression={Term}&format=json&max_results=10", content);

            return await response.Content.ReadAsStringAsync();
        }
        //"38821"

        // GET: api/CustomFoods/getExternalFood
        [Authorize]
        [HttpGet("getExternalFood")]
        public async Task<string> GetExternalFood()
        {
            string BearerToken = HttpContext.Request.Query["bearerToken"].ToString();
            string ExFoodId = HttpContext.Request.Query["exFoodId"].ToString();
            //long ExFoodId = 38821;

            var client = _clientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

            var values = new Dictionary<string, string>
                {
                   { "scope", "basic" },
                   { "grant_type", "client_credentials" }
                };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync($"https://platform.fatsecret.com/rest/server.api?method=food.get.v2&food_id={ExFoodId}&format=json", content);

            return await response.Content.ReadAsStringAsync();
        }


        

        // GET: api/CustomFoods/variation
        [Authorize]
        [HttpGet("variation")]
        public async Task<ActionResult<IEnumerable<CustomFoodVariation>>> SearchVariation()
        {
            string id = HttpContext.Request.Query["id"].ToString();
            return await _context.CustomFoodVariations.Where(v => v.CustomFoodId == id).ToListAsync();
        }

        private bool CustomFoodExists(string id)
        {
            return _context.CustomFoods.Any(e => e.Id == id);
        }

        private bool CustomFoodVariationExists(string id)
        {
            return _context.CustomFoodVariations.Any(e => e.Id == id);
        }
    }
}
