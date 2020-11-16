using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using D301_WebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace D301_WebApp.Pages
{
    public class _DeletePartialModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public _DeletePartialModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Intake Intake { get; set; }
        public void OnGet()
        {
            
        }


        public async Task<ActionResult> OnDeleteAsync()
        {
            var Id = await _context.Intakes.FindAsync(Intake.Id);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

                _context.Remove(Id);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Dashboard");
            // we will never reach here (((hopefully)))
        }
    }
}