using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using D301_WebApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace D301_WebApp.Areas.Identity.Pages.Account.Manage
{
    public partial class TrainerModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public TrainerModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool isTrainer;

        public class InputModel
        {
            [DataType(DataType.Text)]
            [Display(Name = "Current trainer")]
            public string TrainerName { get; set; }

            public string TrainerId { get; set; }

            [EmailAddress]
            public string TrainerEmail { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            ApplicationUser trainer = await _context.Users
                  .Where(u => u.Id == user.TrainerId)
                  .AsNoTracking()
                  .FirstOrDefaultAsync();

            List<ApplicationUser> trainees = await _context.Users
                  .Where(u => u.TrainerId == user.Id)
                  .AsNoTracking()
                  .ToListAsync();
            isTrainer = (trainees.Count > 0) ? true : false;

            Input = new InputModel
            {
                TrainerId = user.TrainerId,
                TrainerName = (trainer != null) ? trainer.FullName + " - " + trainer.UserName : null,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            //set trainer to the email entered
            if (Input.TrainerEmail != null)
            {
                //find trainer who has that email
                ApplicationUser trainer = await _context.Users
                      .Where(u => u.Email == Input.TrainerEmail)
                      .Where(u => u.TrainerId == null || u.TrainerId == "-1") 
                      .AsNoTracking()
                      .FirstOrDefaultAsync();

                if(trainer == null)
                {
                    StatusMessage = $"Error: Could not find trainer with email '{Input.TrainerEmail}'.";
                    return Page();
                } 
                else if(trainer.Id == user.Id)
                {
                    StatusMessage = "Error: Cannot add yourself as a trainer!";
                    return Page();
                }

                //set trainer id in user row
                user.TrainerId = trainer.Id;

                //flag trainer (if they are a new trainer) by setting trainer id to null
                trainer.TrainerId = null;

                //save changes to db
                try
                {
                    _context.Attach(trainer).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(trainer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            //remove trainer
            else
            {
                user.TrainerId = "-1";
                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);
            }

            await LoadAsync(user);

            return Page();

            //StatusMessage = "Your profile has been updated";
            //return RedirectToPage();
        }

        //Method to check if Intake exists
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
