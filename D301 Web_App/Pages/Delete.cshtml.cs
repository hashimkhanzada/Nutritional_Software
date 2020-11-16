using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using D301_WebApp.Data;

namespace D301_WebApp.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly D301_WebApp.Data.ApplicationDbContext _context;

        public DeleteModel(D301_WebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Intake Intake { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Intake = await _context.Intakes.FirstOrDefaultAsync(m => m.Id == id);

            if (Intake == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Intake = await _context.Intakes.FindAsync(id);
            DateTime IntakeDate=DateTime.Now;
            if (Intake != null)
            {
                IntakeDate = Intake.Date;
                _context.Intakes.Remove(Intake);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Dashboard", new { Date = IntakeDate.ToString("yyyy-MM-dd") });
        }
    }
}
