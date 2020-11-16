using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using D301_WebApp.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace D301_WebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "First Name *")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Last Name *")]
            public string LastName { get; set; }

            [Required]
            //[StringLength(10, ErrorMessage = "Date of birth must be in MM/DD/YYYY format", MinimumLength = 10)]
            //[Range(typeof(DateTime), "01/01/1915", "01/01/2020")]
            [Display(Name = "Date of Birth *")]
            [DataType(DataType.Date)]
            public DateTime DOB { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Biological Sex *")]
            public string Gender { get; set; }

            [Required]
            [DataType(DataType.Currency)]
            [Display(Name = "Weight *")]
            [Range(1, float.MaxValue, ErrorMessage = "Your Weight must be at least 1.")]
            [Column(TypeName = "decimal(18, 2)")]
            public float Weight { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string WeightUnit { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Height *")]
            [Range(1, float.MaxValue, ErrorMessage = "Your Height must be at least 1.")]
            [Column(TypeName = "decimal(18, 2)")]
            public float Height { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string HeightUnit { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Activity Level *")]
            public string ActivityLevel { get; set; }


            //[DataType(DataType.Currency)]
            //[Display(Name = "Goal")]
            //[Column(TypeName = "decimal(18, 2)")]
            //public float Goal { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Preferred Energy Unit *")]
            public string GoalUnit { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Medical Conditions")]
            public string MedicalConditions { get; set; }

            [Display(Name = "Pregnant?")]
            public bool Pregnant { get; set; }

            [Display(Name = "Lactating?")]
            public bool Lactating { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/Calculators/CalculateMacroRatio");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if(Input.Gender != "F")
            {
                Input.Pregnant = false;
                Input.Lactating = false;
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    DOB = Input.DOB,
                    Gender = Input.Gender,
                    Weight = Input.Weight,
                    WeightUnit = Input.WeightUnit,
                    Height = Input.Height,
                    HeightUnit = Input.HeightUnit,
                    ActivityLevel = Input.ActivityLevel,
                    GoalUnit = Input.GoalUnit,
                    MedicalConditions = Input.MedicalConditions,
                    Pregnant = Input.Pregnant,
                    Lactating = Input.Lactating
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Register error", error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
