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
using System.Text;
using System.Collections.ObjectModel;

namespace D301_WebApp.Pages
{
    public class ExportIntakesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; } = DateTime.Now.Date;
        [BindProperty(SupportsGet = true)]
        public DateTime StopDate { get; set; } = DateTime.Now.Date;

        [BindProperty(SupportsGet = true)]
        public IList<Intake> IntakeList { get; set; }
        public string SelectedGoalUnit { get; set; }
        public string AbrevGoalUnit { get; set; }

        public ExportIntakesModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        public async Task<IActionResult> OnGetAsync(int id, string date)
        {
            ApplicationUser applicationUser;
            applicationUser = await _userManager.GetUserAsync(User);

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
            };

            //get intake for specified user and timeframe, grouping by meal type
            List<Intake> temp1 = await _context.Intakes.Include(s => s.Food).Include(s => s.CustomFood).Where(s => s.User == applicationUser && s.Date.Date >= StartDate.Date && s.Date.Date <= StopDate.Date).Where(s => s.MealType == "Breakfast" || s.MealType == "Lunch").OrderBy(s => s.MealType).ToListAsync();
            List<Intake> temp2 = await _context.Intakes.Include(s => s.Food).Include(s => s.CustomFood).Where(s => s.User == applicationUser && s.Date.Date >= StartDate.Date && s.Date.Date <= StopDate.Date).Where(s => s.MealType == "Dinner" || s.MealType == "Snack").OrderBy(s => s.MealType).ToListAsync();
            IntakeList = temp1.Concat(temp2).ToList();

            if (applicationUser.GoalUnit == "Calorie")
            {
                foreach (var item in IntakeList)
                {
                    if (item.FoodId != null)
                    {
                        item.Food.Enegry = applicationUser.ConvertToCalorie((int)item.Food.Enegry);
                    }
                    else if (item.CustomFoodId != null)
                    {
                        item.CustomFood.Enegry = applicationUser.ConvertToCalorie((int)item.CustomFood.Enegry);
                    }
                }

                SelectedGoalUnit = "Calories";
                AbrevGoalUnit = "kCal";
            }
            else
            {
                SelectedGoalUnit = "KiloJoules";
                AbrevGoalUnit = "KJ";
            }

            return Page();
        }

        public async Task<IActionResult> OnGetCSV(int id, string date)
        {
            ApplicationUser applicationUser;
            applicationUser = await _userManager.GetUserAsync(User);

            string headerString = "";

            //if there are dates in route, use them rather than the current date
            var start = RouteData.Values["startdate"];
            var stop = RouteData.Values["stopdate"];

            if (start != null || stop != null)
            {
                StartDate = Convert.ToDateTime(start);
                StopDate = Convert.ToDateTime(stop);
            }

            IntakeList = await _context.Intakes
                .Include(s => s.Food).Include(s => s.CustomFood).Where(s => s.User == applicationUser)
                .Where(s => s.Date.Date >= StartDate.Date && s.Date.Date <= StopDate.Date)
                .AsNoTracking()
                .ToListAsync();

            if (applicationUser.GoalUnit == "Calorie")
            {
                foreach (var item in IntakeList)
                {
                    if (item.FoodId != null)
                    {
                        item.Food.Enegry = applicationUser.ConvertToCalorie((int)item.Food.Enegry);
                    }
                    else if (item.CustomFoodId != null)
                    {
                        item.CustomFood.Enegry = applicationUser.ConvertToCalorie((int)item.CustomFood.Enegry);
                    }
                }
            }

            //builds CSV header
            foreach (var info in typeof(Intake).GetProperties())
            {
                if (info.Name.ToString().Contains("Id"))
                {
                    //skip this data
                }
                else if (info.Name.ToString().Contains("VariationId"))
                {
                    //skip this data
                }
                else if (info.Name.ToString().Contains("Measure"))
                {
                    //skip this data
                }
                else if (info.Name.ToString().Contains("Food"))
                {
                    //skip this data
                }
                else if (info.Name.ToString().Contains("CustomFood"))
                {
                    //skip this data
                }
                else if (info.Name.ToString().Contains("FoodId"))
                {
                    //skip this data
                }
                else if (info.Name.ToString().Contains("CustomFoodId"))
                {
                    //skip this data
                }
                else if (info.Name.ToString().Contains("User"))
                {
                    //skip this data
                }
                else
                {
                    headerString += info.ToString().Split(' ').Last() + ",";
                }
            }

            foreach (var nutrient in typeof(Food).GetProperties())
            {
                if (nutrient.Name.ToString().Contains("Id"))
                {
                    //skip this data
                }
                else if (nutrient.Name.ToString().Contains("Amount"))
                {
                    //skip this data
                }
                else if (nutrient.Name.ToString().Contains("Intakes"))
                {
                    //skip this data
                }
                else
                {
                    headerString += nutrient.ToString().Split(' ').Last() + ",";
                }
            }

            //builds CSV from intakes selected
            var builder = new StringBuilder();
            builder.AppendLine(headerString);
            foreach (var intake in IntakeList)
            {
                //info from intake table
                foreach (var info in typeof(Intake).GetProperties())
                {
                    if (info.Name.ToString().Contains("Id"))
                    {
                        //skip this data
                    }
                    else if (info.Name.ToString().Contains("VariationId"))
                    {
                        //skip this data
                    }
                    else if (info.Name.ToString().Contains("Measure"))
                    {
                        //skip this data
                    }
                    else if (info.Name.ToString().Contains("Food"))
                    {
                        //skip this data
                    }
                    else if (info.Name.ToString().Contains("CustomFood"))
                    {
                        //skip this data
                    }
                    else if (info.Name.ToString().Contains("FoodId"))
                    {
                        //skip this data
                    }
                    else if (info.Name.ToString().Contains("CustomFoodId"))
                    {
                        //skip this data
                    }
                    else if (info.Name.ToString().Contains("User"))
                    {
                        //skip this data
                    }
                    else
                    {
                        var intakeDetails = info.GetValue(intake, null);
                        builder.Append($"{intakeDetails},");
                    }
                }

                //info from food table
                if(intake.Food != null)
                {
                    foreach (var nutrient in typeof(Food).GetProperties())
                    {
                        if (nutrient.Name.ToString().Contains("Name"))
                        {
                            string foodDetails = nutrient.GetValue(intake.Food, null).ToString().Replace(",", "");
                            builder.Append($"{foodDetails},");
                            Debug.WriteLine(foodDetails);
                        }
                        else if (nutrient.Name.ToString().Contains("Id"))
                        {
                            //skip this data
                        }
                        else if (nutrient.Name.ToString().Contains("Amount"))
                        {
                            //skip this data
                        }
                        else if (nutrient.Name.ToString().Contains("Intakes"))
                        {
                            //skip this data
                        }
                        else
                        {
                            var foodDetails = intake.CalculateValueToString((float)nutrient.GetValue(intake.Food, null), intake.Food.Amount);
                            builder.Append($"{foodDetails},");
                        }
                    }
                }
                else if(intake.CustomFood != null)
                {
                    foreach (var nutrient in typeof(CustomFood).GetProperties())
                    {
                        if (nutrient.Name.ToString().Contains("Name"))
                        {
                            string foodDetails = nutrient.GetValue(intake.CustomFood, null).ToString().Replace(",", "");
                            builder.Append($"{foodDetails},");
                            Debug.WriteLine(foodDetails);
                        }
                        else if (nutrient.Name.ToString().Contains("Id"))
                        {
                            //skip this data
                        }
                        else if (nutrient.Name.ToString().Contains("Amount"))
                        {
                            //skip this data
                        }
                        else if (nutrient.Name.ToString().Contains("Intakes"))
                        {
                            //skip this data
                        }
                        else
                        {
                            var foodDetails = intake.CalculateValueToString((float)nutrient.GetValue(intake.CustomFood, null), intake.CustomFood.Amount);
                            builder.Append($"{foodDetails},");
                        }
                    }
                }
                builder.AppendLine();


            }

            // names file based on dates and application user name
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", applicationUser.FullName +
                " Intake " + StartDate.Date.ToString("dd-MM-yyyy") + "-" + StopDate.Date.ToString("dd-MM-yyyy") + ".csv");
        }
    }
}
