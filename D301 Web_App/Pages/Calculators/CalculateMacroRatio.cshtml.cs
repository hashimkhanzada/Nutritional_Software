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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace D301_WebApp.Pages.Calculators
{
    public class CalculateMacroRatioModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private List<RDI> RDIs;

        public CalculateMacroRatioModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public RDI RDI_Category { get; set; }

        public bool userisOwner { get; set; }

        public int BMR { get; set; }
        public int TDEE { get; set; }

        public List<SelectListItem> GoalPercentages { get; set; }

        public float SelectGoal { get; set; }

        [BindProperty]
        public bool SetMicroGoals { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [DataType(DataType.Currency)]
            [Display(Name = "Water Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float WaterGoal { get; set; }

            [Required]
            [DataType(DataType.Currency)]
            [Display(Name = "Energy Goal")]
            [Column(TypeName = "decimal(18, 2)")]
            [Range(1, float.MaxValue, ErrorMessage = "Your {0} must be at least 1.")]
            public float EnegryGoal { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Preferred Energy Unit")]
            public string GoalUnit { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Protein Goal %")]
            [Column(TypeName = "decimal(18, 2)")]
            public float ProteinGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Fat Goal %")]
            [Column(TypeName = "decimal(18, 2)")]
            public float FatGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Carbohydrates Goal %")]
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
            [Display(Name = "Cholestrol Goal in g")]
            [Column(TypeName = "decimal(18, 9)")]
            public float CholesterolGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Sodium (Na) Goal in g")]
            [Column(TypeName = "decimal(18, 9)")]
            public float SodiumGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Iodine Goal in g")]
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
            [Display(Name = "Iron (Fe) Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float IronGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Zinc (Zn) Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float ZincGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Selenium (Se) Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float SeleniumGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Vitamin A Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float VitaminAGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Beta-carotene Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float BetaCaroteneGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Thiamin (B1) Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float ThiaminGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Riboflavin Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float RiboflavinGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Niacin Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float NiacinGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Vitamin B6 Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float VitaminB6Goal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Vitamin B12 Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float VitaminB12Goal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Dietary Folate Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float DietaryFolateGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Vitamin C Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float VitaminCGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Vitamin D Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float VitaminDGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Vitamin E Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float VitaminEGoal { get; set; }

            [DataType(DataType.Currency)]
            [Display(Name = "Alcohol Goal in g")]
            [Column(TypeName = "decimal(18, 2)")]
            public float AlcoholGoal { get; set; }
        }

        private RDI getRDICategory(int age, String gender)
        {
            String ageRange;

            if (age > 70)
            {
                ageRange = "70+";
            }
            else if (age > 50)
            {
                ageRange = "51-70";
            }
            else if (age > 30)
            {
                ageRange = "31-50";
            }
            else if (age > 18)
            {
                ageRange = "19-30";
            }
            else if (age > 13)
            {
                ageRange = "14-18";
            }
            else if (age > 8)
            {
                ageRange = "9-13";
            }
            else if (age > 3)
            {
                ageRange = "4-8";
            }
            else
            {
                ageRange = "1-3";
            }

            //get RDI category from age and gender
            RDI temp = RDIs.Where(r => r.AgeRange == ageRange && r.Gender == gender).ToList()[0];

            //multiply by number of days selected
            return new RDI()
            {
                Water = temp.Water,
                DietaryFibre = temp.DietaryFibre,
                Sugars = temp.Sugars,
                Starch = temp.Starch,
                SFA = temp.SFA ,
                MUFA = temp.MUFA ,
                PUFA = temp.PUFA ,
                AlphaLinolenicAcid = temp.AlphaLinolenicAcid,
                LinoleicAcid = temp.LinoleicAcid,
                Cholesterol = temp.Cholesterol,
                Sodium = temp.Sodium,
                Iodine = temp.Iodine,
                Potassium = temp.Potassium,
                Phosphorus = temp.Phosphorus,
                Calcium = temp.Calcium,
                Iron = temp.Iron,
                Zinc = temp.Zinc,
                Selenium = temp.Selenium,
                VitaminA = temp.VitaminA,
                BetaCarotene = temp.BetaCarotene,
                Thiamin = temp.Thiamin,
                Riboflavin = temp.Riboflavin,
                Niacin = temp.Niacin,
                VitaminB6 = temp.VitaminB6,
                VitaminB12 = temp.VitaminB12,
                DietaryFolate = temp.DietaryFolate,
                VitaminC = temp.VitaminC,
                VitaminD = temp.VitaminD,
                VitaminE = temp.VitaminE,
                Alcohol = temp.Alcohol
            };
        }
        private void LoadAsync(ApplicationUser user)
        {
            Input = new InputModel
            {
                WaterGoal = user.WaterGoal,
                EnegryGoal = user.EnegryGoal,
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

            RDIs = await _context.Rdis.ToListAsync();
            RDI_Category = getRDICategory(user.Age, user.Gender);

            

            LoadAsync(user);

            Input.WaterGoal = RDI_Category.Water;
            Input.DietaryFibreGoal = RDI_Category.DietaryFibre;
            Input.SugarsGoal = RDI_Category.Sugars;
            Input.AlphaLinolenicAcidGoal = RDI_Category.AlphaLinolenicAcid;
            Input.LinoleicAcidGoal = RDI_Category.LinoleicAcid;
            Input.SodiumGoal = RDI_Category.Sodium;
            Input.IodineGoal = RDI_Category.Iodine;
            Input.PotassiumGoal = RDI_Category.Potassium;
            Input.PhosphorusGoal = RDI_Category.Phosphorus;
            Input.CalciumGoal = RDI_Category.Calcium;
            Input.IronGoal = RDI_Category.Iron;
            Input.ZincGoal = RDI_Category.Zinc;
            Input.SeleniumGoal = RDI_Category.Selenium;
            Input.VitaminAGoal = RDI_Category.VitaminA;
            Input.ThiaminGoal = RDI_Category.Thiamin;
            Input.RiboflavinGoal = RDI_Category.Riboflavin;
            Input.NiacinGoal = RDI_Category.Niacin;
            Input.VitaminB6Goal = RDI_Category.VitaminB6;
            Input.VitaminB12Goal = RDI_Category.VitaminB12;
            Input.DietaryFolateGoal = RDI_Category.DietaryFolate;
            Input.VitaminCGoal = RDI_Category.VitaminC;
            Input.VitaminDGoal = RDI_Category.VitaminD;
            Input.VitaminEGoal = RDI_Category.VitaminE;

            if (user.GoalUnit == "KJ")
            {
                BMR = user.ConvertToKj((int)user.UserBMR);
                TDEE = user.ConvertToKj((int)user.UserTdee);
            }
            else
            {
                BMR = (int)user.UserBMR;
                TDEE = (int)user.UserTdee;
            } 

            GoalPercentages = new List<SelectListItem>();

            for (int i = 0; i < 105; i += 5)
            {
                GoalPercentages.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() + "%" });
            }

            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            string activeUserId = HttpContext.Session.GetString("activeUserId");
            if (String.IsNullOrEmpty(activeUserId))
            {
                HttpContext.Session.SetString("activeUserId", user.Id);
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
                //LoadAsync(user);
                StatusMessage = "Error: " + string.Join("\r\n ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

                //return Page(); //this doesn't reload data behind the scenes
                return RedirectToPage("CalculateMacroRatio");
            }

            //Macro-nutrients
            user.EnegryGoal = Input.EnegryGoal;
            user.ProteinGoal = Input.ProteinGoal;
            user.FatGoal = Input.FatGoal;
            user.CarbohydratesGoal = Input.CarbohydratesGoal;
            user.SFAGoal = Input.SFAGoal;
            user.MUFAGoal = Input.MUFAGoal;
            user.PUFAGoal = Input.PUFAGoal;

            if (SetMicroGoals == true)
            {
                //Micro-nutrients
                user.WaterGoal = Input.WaterGoal;
                user.DietaryFibreGoal = Input.DietaryFibreGoal;
                user.SugarsGoal = Input.SugarsGoal;
                user.AlphaLinolenicAcidGoal = Input.AlphaLinolenicAcidGoal;
                user.LinoleicAcidGoal = Input.LinoleicAcidGoal;
                user.SodiumGoal = Input.SodiumGoal;
                user.IodineGoal = Input.IodineGoal;
                user.PotassiumGoal = Input.PotassiumGoal;
                user.PhosphorusGoal = Input.PhosphorusGoal;
                user.CalciumGoal = Input.CalciumGoal;
                user.IronGoal = Input.IronGoal;
                user.ZincGoal = Input.ZincGoal;
                user.SeleniumGoal = Input.SeleniumGoal;
                user.VitaminAGoal = Input.VitaminAGoal;
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
            }
            
            await _userManager.UpdateAsync(user);

            LoadAsync(user);
            return RedirectToPage("../Dashboard");
        }
    }
}
