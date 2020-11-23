using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using D301_WebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace D301_WebApp.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Intake Intake { get; set; }
        [BindProperty]
        public bool NewIntake { get; set; }
        public DateTime NewDate { get; set; }

        public IList<Intake> IntakeList { get;set; }
        [BindProperty]
        public bool CFoodType { get; set; } //true = custom food, false = nzfoodfiles food
        [BindProperty]
        public CustomFood CustomFood { get; set; }

        [BindProperty]
        public FoodBookmark FoodBookmark { get; set; }
        public string UserIdvar { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime Date { get; set; } = DateTime.Now.Date;
        public string SelectedGoalUnit { get; set; }
        public string AbrevGoalUnit { get; set; }

        public float TotalGoal1 { get; set; } = 0;
        public float TotalGoal2 { get; set; } = 0;
        public float TotalGoal3 { get; set; } = 0;
        public float Goal1 { get; set; }
        public float Goal2 { get; set; }
        public float Goal3 { get; set; }
        public string GoalName1 { get; set; }
        public string GoalName2 { get; set; }
        public string GoalName3 { get; set; }

        public string userFullName;

        public async Task<IActionResult> OnGetAsync(int id, string date)
        {
            //don't let user see pages in the future
            if(Date > DateTime.Now.Date)
            {
                Response.Redirect("./Dashboard");
            }

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            string activeUserId = HttpContext.Session.GetString("activeUserId");
            if (String.IsNullOrEmpty(activeUserId))
            {
                HttpContext.Session.SetString("activeUserId", applicationUser.Id);
                HttpContext.Session.SetString("fullName", applicationUser.FullName);
            }
            else
            {
                applicationUser = await _userManager.FindByIdAsync(activeUserId);
            }

            //Set isTrainer in session
            if (_context.Users.Where(u => u.TrainerId == applicationUser.Id).Count() > 0 && HttpContext.Session.GetString("isTrainer") != "true")
            {
                HttpContext.Session.SetString("isTrainer", "true");
            }

            //ApplicationUser applicationUser = await _userManager.FindByIdAsync(_userManager.GetUserAsync(User).Result.ActiveUserId);

            userFullName = applicationUser.FullName;
            UserIdvar = applicationUser.Id;

            //get all intake for today, grouping by meal type
            List<Intake> temp1 = await _context.Intakes.Include(s => s.Food).Include(s => s.CustomFood).Where(s => s.User == applicationUser && s.Date.Date == Date.Date).Where(s => s.MealType == "Breakfast" || s.MealType == "Lunch").OrderBy(s => s.MealType).ToListAsync();
            List<Intake> temp2 = await _context.Intakes.Include(s => s.Food).Include(s => s.CustomFood).Where(s => s.User == applicationUser && s.Date.Date == Date.Date).Where(s => s.MealType == "Dinner" || s.MealType == "Snack").OrderBy(s => s.MealType).ToListAsync();
            IntakeList = temp1.Concat(temp2).ToList();

            if(applicationUser.GoalUnit == "Calorie")
            {
                SelectedGoalUnit = "Calories";
                AbrevGoalUnit = "kCal";

                foreach (var item in IntakeList)
                {
                    if (item.FoodId != null)
                    {
                        item.Food.Enegry = applicationUser.ConvertToCalorie((int)item.Food.Enegry);
                    }
                    else if(item.CustomFoodId != null)
                    {
                        item.CustomFood.Enegry = applicationUser.ConvertToCalorie((int)item.CustomFood.Enegry);
                    }
                }
            }
            else
            {
                SelectedGoalUnit = "KiloJoules";
                AbrevGoalUnit = "KJ";
            }


            List<int> numbers = new List<int>(Array.ConvertAll(applicationUser.Macros.Split(','), int.Parse));

            //get goals for progress bars
            GoalName1 = getGoalName(numbers[0]);
            GoalName2 = getGoalName(numbers[1]);
            GoalName3 = getGoalName(numbers[2]);

            //get goals for progress bars
            Goal1 = getGoalValue(applicationUser,numbers[0]);
            Goal2 = getGoalValue(applicationUser,numbers[1]);
            Goal3 = getGoalValue(applicationUser,numbers[2]);

            //calculate macro totals
            var TotalFoodGoal1 = IntakeList.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(getFoodValue(i.Food, numbers[0]), i.Food.Amount)).Aggregate(0F, (a, b) => a + b);
            var TotalFoodGoal2 = IntakeList.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(getFoodValue(i.Food, numbers[1]), i.Food.Amount)).Aggregate(0F, (a, b) => a + b);
            var TotalFoodGoal3 = IntakeList.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(getFoodValue(i.Food, numbers[2]), i.Food.Amount)).Aggregate(0F, (a, b) => a + b);

            var TotalCustomFoodGoal1 = IntakeList.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(getCustomFoodValue(i.CustomFood, numbers[0]), i.CustomFood.Amount)).Aggregate(0F, (a, b) => a + b);
            var TotalCustomFoodGoal2 = IntakeList.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(getCustomFoodValue(i.CustomFood, numbers[1]), i.CustomFood.Amount)).Aggregate(0F, (a, b) => a + b);
            var TotalCustomFoodGoal3 = IntakeList.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(getCustomFoodValue(i.CustomFood, numbers[2]), i.CustomFood.Amount)).Aggregate(0F, (a, b) => a + b);


            TotalGoal1 = TotalFoodGoal1 + TotalCustomFoodGoal1;
            TotalGoal2 = TotalFoodGoal2 + TotalCustomFoodGoal2;
            TotalGoal3 = TotalFoodGoal3 + TotalCustomFoodGoal3;

            //Set date if provided, otherwise redirect to current date
            try
            {
                NewDate = DateTime.ParseExact(date, "yyyy-MM-dd", null);

                if (NewDate > DateTime.Now.Date)
                {
                    //trying to access day in the future
                    return Redirect("./Dashboard?date=" + DateTime.Now.ToString("yyyy-MM-dd"));
                }
            }
            catch
            {
                //unexpected input for date
                return Redirect("./Dashboard?date=" + DateTime.Now.ToString("yyyy-MM-dd"));
            }

            //Get Intake to be edited
            Intake = await _context.Intakes.Include(i => i.Food).Include(i => i.CustomFood).FirstOrDefaultAsync(m => m.Id == id);
            //Set flag to indicate new or existing intake based on response.
            NewIntake = (id == 0) ? true : false;

            //Declare user variable to be used for intake
            //Get active User from session
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

        private string getGoalName(int i)
        {
            switch (i)
            {
                case 0: return "Water";
                case 1: return "Enegry";
                case 2: return "EnegryNIP";
                case 3: return "Protein";
                case 4: return "Fat";
                case 5: return "Carbohydrates";
                case 6: return "DietaryFibre";
                case 7: return "Sugars";
                case 8: return "Starch";
                case 9: return "SFA";
                case 10: return "MUFA";
                case 11: return "PUFA";
                case 12: return "AlphaLinolenicAcid";
                case 13: return "LinoleicAcid";
                case 14: return "Cholesterol";
                case 15: return "Sodium";
                case 16: return "Iodine";
                case 17: return "Potassium";
                case 18: return "Phosphorus";
                case 19: return "Calcium";
                case 20: return "Iron";
                case 21: return "Zinc";
                case 22: return "Selenium";
                case 23: return "VitaminA";
                case 24: return "BetaCarotene";
                case 25: return "Thiamin";
                case 26: return "Riboflavin";
                case 27: return "Niacin";
                case 28: return "VitaminB6";
                case 29: return "VitaminB12";
                case 30: return "DietaryFolate";
                case 31: return "VitaminC";
                case 32: return "VitaminD";
                case 33: return "VitaminE";
                case 34: return "Alcohol";
                default: return "";
            }
        }

        private float getGoalValue(ApplicationUser u,int i) {
            switch(i)
            {
                case 0: return u.WaterGoal;
                case 1: return u.EnegryGoal;
                case 2: return u.EnegryNIPGoal;
                case 3: return u.ProteinGoal;
                case 4: return u.FatGoal;
                case 5: return u.CarbohydratesGoal;
                case 6: return u.DietaryFibreGoal;
                case 7: return u.SugarsGoal;
                case 8: return u.StarchGoal;
                case 9: return u.SFAGoal;
                case 10: return u.MUFAGoal;
                case 11: return u.PUFAGoal;
                case 12: return u.AlphaLinolenicAcidGoal;
                case 13: return u.LinoleicAcidGoal;
                case 14: return u.CholesterolGoal;
                case 15: return u.SodiumGoal;
                case 16: return u.IodineGoal;
                case 17: return u.PotassiumGoal;
                case 18: return u.PhosphorusGoal;
                case 19: return u.CalciumGoal;
                case 20: return u.IronGoal;
                case 21: return u.ZincGoal;
                case 22: return u.SeleniumGoal;
                case 23: return u.VitaminAGoal;
                case 24: return u.BetaCaroteneGoal;
                case 25: return u.ThiaminGoal;
                case 26: return u.RiboflavinGoal;
                case 27: return u.NiacinGoal;
                case 28: return u.VitaminB6Goal;
                case 29: return u.VitaminB12Goal;
                case 30: return u.DietaryFolateGoal;
                case 31: return u.VitaminCGoal;
                case 32: return u.VitaminDGoal;
                case 33: return u.VitaminEGoal;
                case 34: return u.AlcoholGoal;
                default: return 0;
            }
        }

        private float getFoodValue(Food f, int i)
        {
            switch (i)
            {
                case 0: return f.Water;
                case 1: return f.Enegry;
                case 2: return f.EnegryNIP;
                case 3: return f.Protein;
                case 4: return f.Fat;
                case 5: return f.Carbohydrates;
                case 6: return f.DietaryFibre;
                case 7: return f.Sugars;
                case 8: return f.Starch;
                case 9: return f.SFA;
                case 10: return f.MUFA;
                case 11: return f.PUFA;
                case 12: return f.AlphaLinolenicAcid;
                case 13: return f.LinoleicAcid;
                case 14: return f.Cholesterol;
                case 15: return f.Sodium;
                case 16: return f.Iodine;
                case 17: return f.Potassium;
                case 18: return f.Phosphorus;
                case 19: return f.Calcium;
                case 20: return f.Iron;
                case 21: return f.Zinc;
                case 22: return f.Selenium;
                case 23: return f.VitaminA;
                case 24: return f.BetaCarotene;
                case 25: return f.Thiamin;
                case 26: return f.Riboflavin;
                case 27: return f.Niacin;
                case 28: return f.VitaminB6;
                case 29: return f.VitaminB12;
                case 30: return f.DietaryFolate;
                case 31: return f.VitaminC;
                case 32: return f.VitaminD;
                case 33: return f.VitaminE;
                case 34: return f.Alcohol;
                default: return 0;
            }
        }

        private float getCustomFoodValue(CustomFood f, int i)
        {
            switch (i)
            {
                case 0: return f.Water;
                case 1: return f.Enegry;
                case 2: return f.EnegryNIP;
                case 3: return f.Protein;
                case 4: return f.Fat;
                case 5: return f.Carbohydrates;
                case 6: return f.DietaryFibre;
                case 7: return f.Sugars;
                case 8: return f.Starch;
                case 9: return f.SFA;
                case 10: return f.MUFA;
                case 11: return f.PUFA;
                case 12: return f.AlphaLinolenicAcid;
                case 13: return f.LinoleicAcid;
                case 14: return f.Cholesterol;
                case 15: return f.Sodium;
                case 16: return f.Iodine;
                case 17: return f.Potassium;
                case 18: return f.Phosphorus;
                case 19: return f.Calcium;
                case 20: return f.Iron;
                case 21: return f.Zinc;
                case 22: return f.Selenium;
                case 23: return f.VitaminA;
                case 24: return f.BetaCarotene;
                case 25: return f.Thiamin;
                case 26: return f.Riboflavin;
                case 27: return f.Niacin;
                case 28: return f.VitaminB6;
                case 29: return f.VitaminB12;
                case 30: return f.DietaryFolate;
                case 31: return f.VitaminC;
                case 32: return f.VitaminD;
                case 33: return f.VitaminE;
                default: return 0;
            }
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var intake = await _context.Intakes.FindAsync(id);

            if (intake == null)
            {
                return NotFound();
            } 
            else if (id < 1)
            {
                return BadRequest();
            }
            _context.Intakes.Remove(intake);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Dashboard");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //Validate all fields for input using ModelState
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //If new Intake, set variables and add intake.
            if (NewIntake)
            {
                string activeUserId = HttpContext.Session.GetString("activeUserId");
                Intake.User = await _userManager.FindByIdAsync(activeUserId);

                if (Intake.Food.Id != null)
                {
                    Intake.Food = await _context.Foods.FindAsync(Intake.Food.Id);
                    Intake.CustomFood = null;
                }
                else if (Intake.CustomFood.Id != null)
                {
                    Intake.CustomFood = await _context.CustomFoods.FindAsync(Intake.CustomFood.Id);
                    Intake.Food = null;
                }

                _context.Intakes.Add(Intake);
                await _context.SaveChangesAsync();
            }
            else
            {
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

        public async Task<IActionResult> OnPostAddCustomFoodAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            CustomFood.Id = CustomFood.GenerateId(CustomFood.Name);

            _context.CustomFoods.Add(CustomFood);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Dashboard");
        }

        //public async Task<IActionResult> OnPostAddFoodBookmarkAsync()
        //{

        //    ApplicationUser applicationUser;
        //    applicationUser = await _userManager.GetUserAsync(User);

        //    FoodBookmark.UserId = applicationUser.Id;

        //    if (CFoodType == true)
        //    {
        //        Intake.Food = null;
        //    }
        //    else
        //    {
        //        Intake.CustomFood = null;
        //    }
            
        //    _context.FoodBookmarks.Add(FoodBookmark);
        //    await _context.SaveChangesAsync();

        //    return RedirectToPage("/Dashboard");
        //}

        //Method to check if Intake exists
        private bool IntakeExists(int id)
        {
            return _context.Intakes.Any(e => e.Id == id);
        }

       


    }


}

