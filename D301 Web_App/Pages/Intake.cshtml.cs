using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using D301_WebApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace D301_WebApp.Pages
{
    public class IntakeModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        //Declare variables to be used for model
        [BindProperty]
        public Intake Intake { get; set; }
        [BindProperty]
        public bool NewIntake { get; set; }
        public DateTime NewDate { get; set; }
        public string userFullName;

        //Intake model constructor
        public IntakeModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //Load existing Intake
        public async Task<IActionResult> OnGetAsync(int id,string date)
        {
            //Set date if provided, otherwise redirect to current date
            try
            {
                NewDate = DateTime.ParseExact(date, "yyyy-MM-dd", null);

                if(NewDate > DateTime.Now.Date)
                {
                    //trying to access day in the future
                    return Redirect("./Intake?date=" + DateTime.Now.ToString("yyyy-MM-dd"));
                }
            }
            catch
            {
                //unexpected input for date
                return Redirect("./Intake?date=" + DateTime.Now.ToString("yyyy-MM-dd"));
            }

            //Get Intake to be edited
            Intake = await _context.Intakes.Include(i => i.Food).Include(i => i.CustomFood).FirstOrDefaultAsync(m => m.Id == id);
            //Set flag to indicate new or existing intake based on response.
            NewIntake = (id == 0) ? true : false;

            //Declare user variable to be used for intake
            ApplicationUser applicationUser;
            //Get active User from session
            string activeUserId = HttpContext.Session.GetString("activeUserId");
            //If not set, set session value of activeUserId to current user
            if (String.IsNullOrEmpty(activeUserId))
            {
                applicationUser = await _userManager.GetUserAsync(User);
                HttpContext.Session.SetString("activeUserId", applicationUser.Id);
                HttpContext.Session.SetString("fullName", applicationUser.FullName);
            }
            else
            {
                //Else get user to do intake for.
                applicationUser = await _userManager.FindByIdAsync(activeUserId);
            }
            //Get user fullname to display on page
            userFullName = applicationUser.FullName;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //Validate all fields for input using ModelState
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //If new Intake, set variables and add intake.
            if (NewIntake) {

                string activeUserId = HttpContext.Session.GetString("activeUserId");
                Intake.User = await _userManager.FindByIdAsync(activeUserId);
                Intake.Food = await _context.Foods.FindAsync(Intake.Food.Id);
                _context.Intakes.Add(Intake);
                await _context.SaveChangesAsync();
            } else {
                //Else flag as modified and save changes
                _context.Attach(Intake).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IntakeExists(Intake.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToPage("/Dashboard", new { Date = Intake.Date.ToString("yyyy-MM-dd") });
        }

        //Method to check if Intake exists
        private bool IntakeExists(int id)
        {
            return _context.Intakes.Any(e => e.Id == id);
        }
    }
}