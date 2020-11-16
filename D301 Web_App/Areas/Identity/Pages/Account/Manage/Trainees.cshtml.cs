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
    public partial class TraineesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;


        public TraineesModel(
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

        public List<ApplicationUser> Trainees;

        public class InputModel
        {
            [DataType(DataType.Text)]
            [Display(Name = "Current trainer")]
            public string TrainerName { get; set; }

            public string TrainerId { get; set; }
            public string RemoveTraineeId { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            ApplicationUser trainer = await _context.Users
                  .Where(u => u.Id == user.TrainerId)
                  .AsNoTracking()
                  .FirstOrDefaultAsync();

            Trainees = await _context.Users
                  .Where(u => u.TrainerId == user.Id)
                  .AsNoTracking()
                  .ToListAsync();

            isTrainer = (Trainees.Count > 0) ? true : false;

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

            //remove clicked trainee
            ApplicationUser trainee = await _context.Users
                    .Where(u => u.Id == Input.RemoveTraineeId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

            //set trainee back to regular user
            trainee.TrainerId = "-1";

            //if that was the trainer's last trainee, set them back to a regular user as well
            Trainees = await _context.Users
                  .Where(u => u.TrainerId == user.Id)
                  .AsNoTracking()
                  .ToListAsync();
            if(Trainees.Count() == 1)
            {
                user.TrainerId = "-1";
            }

            _context.Attach(trainee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(trainee.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            await LoadAsync(user);
            return Page();
        }

        //Method to check if Intake exists
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
