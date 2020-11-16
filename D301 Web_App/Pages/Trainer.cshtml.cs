using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using D301_WebApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;

namespace D301_WebApp.Pages
{
    public class TrainerModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        //Trainer Model constructor
        public TrainerModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //Public variables to be used for model
        public IList<ApplicationUser> Users { get; set; }
        [BindProperty]
        public string ActiveUserId { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime Date { get; set; } = DateTime.Now;

        public async Task<IActionResult> OnGetAsync()
        {

            //Get current user information
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            //If user is not Trainer, redirect to Dashboard. Prevent unauthorised access
            if (HttpContext.Session.GetString("isTrainer") != "true")
            {
                return RedirectToPage("/Dashboard");
            }
            else
            {
                //Get current active user if, create if not exist
                string activeUserId = HttpContext.Session.GetString("activeUserId");
                if (String.IsNullOrEmpty(activeUserId))
                {
                    HttpContext.Session.SetString("activeUserId", applicationUser.Id);
                    HttpContext.Session.SetString("fullName", applicationUser.FullName);
                    ActiveUserId = applicationUser.Id;
                }
                else
                {
                    ActiveUserId = activeUserId;
                }

                //Get list of users that are assigned to current user.
                Users = await _context.Users
                    .Where(u => u.TrainerId == applicationUser.Id || u.Id == applicationUser.Id)
                    .AsNoTracking()
                    .ToListAsync();
  
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //Validate model state before proceeding. Failsafe
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Create variable for both activeUser and current user
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            ApplicationUser activeUser = await _userManager.FindByIdAsync(ActiveUserId);

            //Get current user
            applicationUser = await _userManager.GetUserAsync(User);

            //set active user session information
            HttpContext.Session.SetString("activeUserId", ActiveUserId);
            HttpContext.Session.SetString("fullName", activeUser.FullName);

            //Flag and save current user
            _context.Attach(applicationUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            //Get List of users to be displayed
            Users = await _context.Users
                   .Where(u => u.TrainerId == applicationUser.Id || u.Id == applicationUser.Id)
                   .AsNoTracking()
                   .ToListAsync();

            return Page();
        }
    }
}
