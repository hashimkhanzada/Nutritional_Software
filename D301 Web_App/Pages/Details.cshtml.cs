using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using D301_WebApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace D301_WebApp.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Intake Intake { get; set; }
        

        public async Task<IActionResult> OnGetAsync(int id)
        {
            //Get active User from session
            ApplicationUser applicationUser;
            string activeUserId = HttpContext.Session.GetString("activeUserId");
            applicationUser = await _userManager.FindByIdAsync(activeUserId);

            //find intake based on id and the user who created it
            Intake = await _context.Intakes.Where(m => m.User == applicationUser).FirstOrDefaultAsync(m => m.Id == id);

            if (Intake == null)
            {
                return NotFound();
            }
            else
            {
                Intake.Food = await _context.Foods.FindAsync(Intake.FoodId);
                return Page();
            }
        }
    }
}
