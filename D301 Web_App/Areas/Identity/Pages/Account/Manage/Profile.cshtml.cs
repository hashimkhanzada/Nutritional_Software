using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using D301_WebApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace D301_WebApp.Areas.Identity.Pages.Account.Manage
{
    public partial class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        

        public ProfileModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool userisOwner;

        public string Gender;

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.Currency)]
            [Display(Name = "Weight")]
            [Column(TypeName = "decimal(18, 2)")]
            public float Weight { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string WeightUnit { get; set; }

            [Required]
            [DataType(DataType.Currency)]
            [Display(Name = "Height")]
            [Column(TypeName = "decimal(18, 2)")]
            public float Height { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string HeightUnit { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Medical Conditions")]
            public string MedicalConditions { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Activity Level")]
            public string ActivityLevel { get; set; }

            [Display(Name = "Pregnant?")]
            public bool Pregnant { get; set; }

            [Display(Name = "Lactating?")]
            public bool Lactating { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            
            Username = userName;

            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Weight = user.Weight,
                Height = user.Height,
                WeightUnit = user.WeightUnit,
                HeightUnit = user.HeightUnit,
                MedicalConditions = user.MedicalConditions,
                Pregnant = user.Pregnant,
                Lactating = user.Lactating
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            string activeUserId = HttpContext.Session.GetString("activeUserId");
            if (String.IsNullOrEmpty(activeUserId))
            {
                HttpContext.Session.SetString("activeUserId", user.Id);
                HttpContext.Session.SetString("fullName", user.FullName);
            }
            else
            {
                //view logged in user's profile page, unless trainer viewing user's profile
                userisOwner = true;
                if (activeUserId != user.Id)
                {
                    userisOwner = false;
                    user = await _userManager.FindByIdAsync(activeUserId);
                }

                //get gender of user for use on page
                Gender = user.Gender;
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            string activeUserId = HttpContext.Session.GetString("activeUserId");
            if (String.IsNullOrEmpty(activeUserId))
            {
                HttpContext.Session.SetString("activeUserId", user.Id);
                HttpContext.Session.SetString("fullName", user.FullName);
            }
            else
            {
                userisOwner = true;
                if (activeUserId != user.Id)
                {
                    userisOwner = false;
                    user = await _userManager.FindByIdAsync(activeUserId);
                }
            }

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (userisOwner) {
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.Weight = Input.Weight;
                user.Height = Input.Height;
                user.WeightUnit = Input.WeightUnit;
                user.HeightUnit = Input.HeightUnit;
                user.ActivityLevel = Input.ActivityLevel;
                user.MedicalConditions = Input.MedicalConditions;
                user.Pregnant = Input.Pregnant;
                user.Lactating = Input.Lactating;
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
