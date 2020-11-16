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
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
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

        public class InputModel
        {
            public string Macros { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Water Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float WaterGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Energy Goal")]
            [Column(TypeName = "decimal(18, 2)")]
            public float EnegryGoal { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Preferred Energy Unit")]
            public string GoalUnit { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Energy Goal (NIP)")]
            [Column(TypeName = "decimal(18, 2)")]
            public float EnegryNIPGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Protein Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float ProteinGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Fat Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float FatGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Carbohydrates Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float CarbohydratesGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Dietary fibre Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float DietaryFibreGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Sugars Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float SugarsGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Starch Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float StarchGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "SFA Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float SFAGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "MUFA Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float MUFAGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "PUFA Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float PUFAGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Alpha-linolenic acid Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float AlphaLinolenicAcidGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Linoleic acid Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float LinoleicAcidGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Cholestrol Goal in mg")]
            [Column(TypeName = "decimal(18, 9)")]
            public float CholesterolGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Sodium (Na) Goal in mg")]
            [Column(TypeName = "decimal(18, 9)")]
            public float SodiumGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Iodine Goal in μg")]
            [Column(TypeName = "decimal(18, 9)")]
            public float IodineGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Potassium Goal in mg")]
            [Column(TypeName = "decimal(18, 2)")]
            public float PotassiumGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Phosphorus Goal in mg")]
            [Column(TypeName = "decimal(18, 2)")]
            public float PhosphorusGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Calcium Goal in mg")]
            [Column(TypeName = "decimal(18, 2)")]
            public float CalciumGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Iron (Fe) Goal in mg")]
            [Column(TypeName = "decimal(18, 2)")]
            public float IronGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Zinc (Zn) Goal in mg")]
            [Column(TypeName = "decimal(18, 2)")]
            public float ZincGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Selenium (Se) Goal in μg")]
            [Column(TypeName = "decimal(18, 2)")]
            public float SeleniumGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Vitamin A Goal in μg")]
            [Column(TypeName = "decimal(18, 2)")]
            public float VitaminAGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Beta-carotene Goal in μg")]
            [Column(TypeName = "decimal(18, 2)")]
            public float BetaCaroteneGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Thiamin (B1) Goal in mg")]
            [Column(TypeName = "decimal(18, 2)")]
            public float ThiaminGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Riboflavin (B2) Goal in mg")]
            [Column(TypeName = "decimal(18, 2)")]
            public float RiboflavinGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Niacin Goal (B3) in mg")]
            [Column(TypeName = "decimal(18, 2)")]
            public float NiacinGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Vitamin B6 Goal in mg")]
            [Column(TypeName = "decimal(18, 2)")]
            public float VitaminB6Goal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Vitamin B12 Goal in μg")]
            [Column(TypeName = "decimal(18, 2)")]
            public float VitaminB12Goal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Dietary Folate Goal in μg")]
            [Column(TypeName = "decimal(18, 2)")]
            public float DietaryFolateGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Vitamin C Goal in mg")]
            [Column(TypeName = "decimal(18, 2)")]
            public float VitaminCGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Vitamin D Goal in μg")]
            [Column(TypeName = "decimal(18, 2)")]
            public float VitaminDGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Vitamin E Goal in mg")]
            [Column(TypeName = "decimal(18, 2)")]
            public float VitaminEGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Alcohol Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float AlcoholGoal { get; set; }
        }

        private void LoadAsync(ApplicationUser user)
        {
            Input = new InputModel
            {
                Macros = user.Macros,
                WaterGoal = user.WaterGoal,
                EnegryGoal = user.EnegryGoal,
                EnegryNIPGoal = user.EnegryNIPGoal,
                GoalUnit = user.GoalUnit,
                ProteinGoal = user.ProteinGoal,
                FatGoal = user.FatGoal,
                CarbohydratesGoal = user.CarbohydratesGoal,
                DietaryFibreGoal = user.DietaryFibreGoal,
                SugarsGoal = user.SugarsGoal,
                StarchGoal = user.StarchGoal,
                SFAGoal = user.SFAGoal,
                MUFAGoal = user.MUFAGoal,
                PUFAGoal = user.PUFAGoal,
                AlphaLinolenicAcidGoal = user.AlphaLinolenicAcidGoal,
                LinoleicAcidGoal = user.LinoleicAcidGoal,
                CholesterolGoal = user.CholesterolGoal,
                SodiumGoal = user.SodiumGoal,
                IodineGoal = user.IodineGoal,
                PotassiumGoal = user.PotassiumGoal,
                PhosphorusGoal = user.PhosphorusGoal,
                CalciumGoal = user.CalciumGoal,
                IronGoal = user.IronGoal,
                ZincGoal = user.ZincGoal,
                SeleniumGoal = user.SeleniumGoal,
                VitaminAGoal = user.VitaminAGoal,
                BetaCaroteneGoal = user.BetaCaroteneGoal,
                ThiaminGoal = user.ThiaminGoal,
                RiboflavinGoal = user.RiboflavinGoal,
                NiacinGoal = user.NiacinGoal,
                VitaminB6Goal = user.VitaminB6Goal,
                VitaminB12Goal = user.VitaminB12Goal,
                DietaryFolateGoal = user.DietaryFolateGoal,
                VitaminCGoal = user.VitaminCGoal,
                VitaminDGoal = user.VitaminDGoal,
                VitaminEGoal = user.VitaminEGoal,
                AlcoholGoal = user.AlcoholGoal
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
                userisOwner = true;
                if (activeUserId != user.Id)
                {
                    userisOwner = false;
                    user = await _userManager.FindByIdAsync(activeUserId);
                }

            }

            LoadAsync(user);
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
                LoadAsync(user);
                return Page();
            }

            if(Input.Macros == "" || Input.Macros.Split(",").Length != 3)
            {
                StatusMessage = "Error: You must have exactly 3 tracked nutrients";
                return Page();
            }

            user.Macros = Input.Macros;
            user.WaterGoal = Input.WaterGoal;
            user.EnegryGoal = Input.EnegryGoal;
            user.EnegryNIPGoal = Input.EnegryNIPGoal;
            user.GoalUnit = Input.GoalUnit;
            user.ProteinGoal = Input.ProteinGoal;
            user.FatGoal = Input.FatGoal;
            user.CarbohydratesGoal = Input.CarbohydratesGoal;
            user.DietaryFibreGoal = Input.DietaryFibreGoal;
            user.SugarsGoal = Input.SugarsGoal;
            user.StarchGoal = Input.StarchGoal;
            user.SFAGoal = Input.SFAGoal;
            user.MUFAGoal = Input.MUFAGoal;
            user.PUFAGoal = Input.PUFAGoal;
            user.AlphaLinolenicAcidGoal = Input.AlphaLinolenicAcidGoal;
            user.LinoleicAcidGoal = Input.LinoleicAcidGoal;
            user.CholesterolGoal = Input.CholesterolGoal;
            user.SodiumGoal = Input.SodiumGoal;
            user.IodineGoal = Input.IodineGoal;
            user.PotassiumGoal = Input.PotassiumGoal;
            user.PhosphorusGoal = Input.PhosphorusGoal;
            user.CalciumGoal = Input.CalciumGoal;
            user.IronGoal = Input.IronGoal;
            user.ZincGoal = Input.ZincGoal;
            user.SeleniumGoal = Input.SeleniumGoal;
            user.VitaminAGoal = Input.VitaminAGoal;
            user.BetaCaroteneGoal = Input.BetaCaroteneGoal;
            user.ThiaminGoal = Input.ThiaminGoal;
            user.RiboflavinGoal = Input.RiboflavinGoal;
            user.NiacinGoal = Input.NiacinGoal;
            user.VitaminB6Goal = Input.VitaminB6Goal;
            user.VitaminB12Goal = Input.VitaminB12Goal;
            user.DietaryFolateGoal = Input.DietaryFolateGoal;
            user.VitaminCGoal = Input.VitaminCGoal;
            user.VitaminDGoal = Input.VitaminDGoal;
            user.VitaminEGoal = Input.VitaminEGoal;
            user.AlcoholGoal = Input.AlcoholGoal;

            await _userManager.UpdateAsync(user);

            StatusMessage = "Your profile has been updated";
            LoadAsync(user);
            return Page();
        }
    }
}
