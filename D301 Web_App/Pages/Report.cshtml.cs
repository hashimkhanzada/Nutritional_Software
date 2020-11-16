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

namespace D301_WebApp.Pages
{
    public class ReportModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private List<RDI> RDIs;
        public ReportModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; } = DateTime.Now.Date;
        [BindProperty(SupportsGet = true)]
        public DateTime StopDate { get; set; } = DateTime.Now.Date;
        [BindProperty(SupportsGet = true)]
        public String View { get; set; } = "Goals";
        public IList<Intake> Intake { get; set; }

        public string userFullName;
        public Food FoodTotal { get; set; }
        public Nutrient Goals { get; set; }
        public RDI RDI_Category { get; set; }

        public String[] trackedNutrients { get; set; }
        public string SelectedGoalUnit { get; set; }
        public string AbrevGoalUnit { get; set; }

        private AllNutrients getTotalNutrients()
        {
            //create food object which will hold the total nutrients for the day
            AllNutrients allNutrients = new AllNutrients();

            //regular foods
            if(Intake.Where(i => i.FoodId != null).Count() > 0)
            {
                //add total for each element
                //eg. select only the "fat" values from intake, calculate value based on serving, and add all fat values together
                allNutrients.Fat += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Fat, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Carbohydrates += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Carbohydrates, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Protein += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Protein, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Water += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Water, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Enegry += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Enegry, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.EnegryNIP += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.EnegryNIP, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.DietaryFibre += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.DietaryFibre, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Sugars += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Sugars, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Starch += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Starch, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.SFA += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.SFA, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.MUFA += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.MUFA, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.PUFA += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.PUFA, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.AlphaLinolenicAcid += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.AlphaLinolenicAcid, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.LinoleicAcid += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.LinoleicAcid, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Cholesterol += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Cholesterol, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Sodium += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Sodium, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Iodine += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Iodine, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Potassium += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Potassium, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Phosphorus += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Phosphorus, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Calcium += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Calcium, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Iron += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Iron, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Zinc += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Zinc, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Selenium += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Selenium, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.VitaminA += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.VitaminA, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.BetaCarotene += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.BetaCarotene, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Thiamin += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Thiamin, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Riboflavin += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Riboflavin, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Niacin += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Niacin, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.VitaminB6 += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.VitaminB6, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.VitaminB12 += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.VitaminB12, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.DietaryFolate += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.DietaryFolate, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.VitaminC += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.VitaminC, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.VitaminD += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.VitaminD, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.VitaminE += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.VitaminE, i.Food.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Alcohol += Intake.Where(i => i.FoodId != null).Select(i => i.CalculateValueToFloat(i.Food.Alcohol, i.Food.Amount)).Aggregate((a, b) => a + b);
            }

            //custom foods
            if (Intake.Where(i => i.CustomFoodId != null).Count() > 0)
            {
                allNutrients.Fat += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Fat, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Carbohydrates += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Carbohydrates, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Protein += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Protein, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Water += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Water, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Enegry += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Enegry, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.EnegryNIP += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.EnegryNIP, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.DietaryFibre += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.DietaryFibre, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Sugars += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Sugars, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Starch += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Starch, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.SFA += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.SFA, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.MUFA += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.MUFA, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.PUFA += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.PUFA, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.AlphaLinolenicAcid += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.AlphaLinolenicAcid, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.LinoleicAcid += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.LinoleicAcid, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Cholesterol += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Cholesterol, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Sodium +=  Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Sodium, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Iodine += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Iodine, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Potassium += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Potassium, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Phosphorus += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Phosphorus, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Calcium += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Calcium, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Iron += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Iron, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Zinc += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Zinc, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Selenium += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Selenium, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.VitaminA += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.VitaminA, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.BetaCarotene += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.BetaCarotene, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Thiamin += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Thiamin, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Riboflavin += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Riboflavin, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Niacin += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Niacin, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.VitaminB6 += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.VitaminB6, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.DietaryFolate += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.DietaryFolate, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.VitaminB12 += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.VitaminB12, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.VitaminC += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.VitaminC, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.VitaminD += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.VitaminD, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.VitaminE += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.VitaminE, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
                allNutrients.Alcohol += Intake.Where(i => i.CustomFoodId != null).Select(i => i.CalculateValueToFloat(i.CustomFood.Alcohol, i.CustomFood.Amount)).Aggregate((a, b) => a + b);
            }

            return allNutrients;
        }

        public class AllNutrients : Food
        {
            public AllNutrients()
            {
                Fat = 0;
                Carbohydrates = 0;
                Protein = 0;
                Water = 0;
                Enegry = 0;
                EnegryNIP = 0;
                DietaryFibre = 0;
                Sugars = 0;
                Starch = 0;
                SFA = 0;
                MUFA = 0;
                PUFA = 0;
                AlphaLinolenicAcid = 0;
                LinoleicAcid = 0;
                Cholesterol = 0;
                Sodium = 0;
                Iodine = 0;
                Potassium = 0;
                Phosphorus = 0;
                Calcium = 0;
                Iron = 0;
                Zinc = 0;
                Selenium = 0;
                VitaminA = 0;
                BetaCarotene = 0;
                Thiamin = 0;
                Riboflavin = 0;
                Niacin = 0;
                VitaminB6 = 0;
                VitaminB12 = 0;
                DietaryFolate = 0;
                VitaminC = 0;
                VitaminD = 0;
                VitaminE = 0;
                Alcohol = 0;
            }
        }

        private Nutrient getGoals (ApplicationUser user)
        {
            return new Nutrient
            {
                Water = user.WaterGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Enegry = user.EnegryGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                EnegryNIP = user.EnegryNIPGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Protein = user.ProteinGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Fat = user.FatGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Carbohydrates = user.CarbohydratesGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                DietaryFibre = user.DietaryFibreGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Sugars = user.SugarsGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Starch = user.StarchGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                SFA = user.SFAGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                MUFA = user.MUFAGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                PUFA = user.PUFAGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                AlphaLinolenicAcid = user.AlphaLinolenicAcidGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                LinoleicAcid = user.LinoleicAcidGoal * ((StopDate.Date - StartDate.Date).Days + 1), 
                Cholesterol = user.CholesterolGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Sodium = user.SodiumGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Iodine = user.IodineGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Potassium = user.PotassiumGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Phosphorus = user.PhosphorusGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Calcium = user.CalciumGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Iron = user.IronGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Zinc = user.ZincGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Selenium = user.SeleniumGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                VitaminA = user.VitaminAGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                BetaCarotene = user.BetaCaroteneGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Thiamin = user.ThiaminGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Riboflavin = user.RiboflavinGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Niacin = user.NiacinGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                VitaminB6 = user.VitaminB6Goal * ((StopDate.Date - StartDate.Date).Days + 1),
                VitaminB12 = user.VitaminB12Goal * ((StopDate.Date - StartDate.Date).Days + 1),
                DietaryFolate = user.DietaryFolateGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                VitaminC = user.VitaminCGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                VitaminD = user.VitaminDGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                VitaminE = user.VitaminEGoal * ((StopDate.Date - StartDate.Date).Days + 1),
                Alcohol = user.AlcoholGoal * ((StopDate.Date - StartDate.Date).Days + 1)
            };
        }

        private RDI getRDICategory(int age, String gender, bool pregnant, bool lactating)
        {
            String ageRange;

            //check pregnant and lactating first as they use different ranges
            if (pregnant || lactating)
            {
                if (age >= 19)
                {
                    ageRange = "19-50";
                }
                else
                {
                    ageRange = "14-18";
                }
            }
            else
            {
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
            }

            //if both pregnant and lactating, use the lactating RDI
            if(pregnant && lactating)
            {
                pregnant = false;
            }

            //get RDI category from age and gender
            RDI temp = RDIs.Where(r => r.AgeRange == ageRange && r.Gender == gender && r.Pregnant == pregnant && r.Lactating == lactating).ToList()[0];

            //multiply by number of days selected
            return new RDI()
            {
                Water = temp.Water * ((StopDate.Date - StartDate.Date).Days + 1),
                Enegry = temp.Enegry * ((StopDate.Date - StartDate.Date).Days + 1),
                EnegryNIP = temp.EnegryNIP * ((StopDate.Date - StartDate.Date).Days + 1),
                Protein = temp.Protein * ((StopDate.Date - StartDate.Date).Days + 1),
                Fat = temp.Fat * ((StopDate.Date - StartDate.Date).Days + 1),
                Carbohydrates = temp.Carbohydrates * ((StopDate.Date - StartDate.Date).Days + 1),
                DietaryFibre = temp.DietaryFibre * ((StopDate.Date - StartDate.Date).Days + 1),
                Sugars = temp.Sugars * ((StopDate.Date - StartDate.Date).Days + 1),
                Starch = temp.Starch * ((StopDate.Date - StartDate.Date).Days + 1),
                SFA = temp.SFA * ((StopDate.Date - StartDate.Date).Days + 1),
                MUFA = temp.MUFA * ((StopDate.Date - StartDate.Date).Days + 1),
                PUFA = temp.PUFA * ((StopDate.Date - StartDate.Date).Days + 1),
                AlphaLinolenicAcid = temp.AlphaLinolenicAcid * ((StopDate.Date - StartDate.Date).Days + 1),
                LinoleicAcid = temp.LinoleicAcid * ((StopDate.Date - StartDate.Date).Days + 1),
                Cholesterol = temp.Cholesterol * ((StopDate.Date - StartDate.Date).Days + 1),
                Sodium = temp.Sodium * ((StopDate.Date - StartDate.Date).Days + 1),
                Iodine = temp.Iodine * ((StopDate.Date - StartDate.Date).Days + 1),
                Potassium = temp.Potassium * ((StopDate.Date - StartDate.Date).Days + 1),
                Phosphorus = temp.Phosphorus * ((StopDate.Date - StartDate.Date).Days + 1),
                Calcium = temp.Calcium * ((StopDate.Date - StartDate.Date).Days + 1),
                Iron = temp.Iron * ((StopDate.Date - StartDate.Date).Days + 1),
                Zinc = temp.Zinc * ((StopDate.Date - StartDate.Date).Days + 1),
                Selenium = temp.Selenium * ((StopDate.Date - StartDate.Date).Days + 1),
                VitaminA = temp.VitaminA * ((StopDate.Date - StartDate.Date).Days + 1),
                BetaCarotene = temp.BetaCarotene * ((StopDate.Date - StartDate.Date).Days + 1),
                Thiamin = temp.Thiamin * ((StopDate.Date - StartDate.Date).Days + 1),
                Riboflavin = temp.Riboflavin * ((StopDate.Date - StartDate.Date).Days + 1),
                Niacin = temp.Niacin * ((StopDate.Date - StartDate.Date).Days + 1),
                VitaminB6 = temp.VitaminB6 * ((StopDate.Date - StartDate.Date).Days + 1),
                VitaminB12 = temp.VitaminB12 * ((StopDate.Date - StartDate.Date).Days + 1),
                DietaryFolate = temp.DietaryFolate * ((StopDate.Date - StartDate.Date).Days + 1),
                VitaminC = temp.VitaminC * ((StopDate.Date - StartDate.Date).Days + 1),
                VitaminD = temp.VitaminD * ((StopDate.Date - StartDate.Date).Days + 1),
                VitaminE = temp.VitaminE * ((StopDate.Date - StartDate.Date).Days + 1),
                Alcohol = temp.Alcohol * ((StopDate.Date - StartDate.Date).Days + 1)
            };
        }

        public async Task OnGetAsync()
        {
            //get info from db
            ApplicationUser applicationUser;
            string activeUserId = HttpContext.Session.GetString("activeUserId"); 
            if (String.IsNullOrEmpty(activeUserId))
            {
                applicationUser = await _userManager.GetUserAsync(User);
                HttpContext.Session.SetString("activeUserId", applicationUser.Id);
                HttpContext.Session.SetString("fullName", applicationUser.FullName);
            }
            else
            {
                applicationUser = await _userManager.FindByIdAsync(activeUserId);
            }

            userFullName = applicationUser.FullName;

            Intake = await _context.Intakes
                .Include(s => s.Food).Include(s => s.CustomFood).Where(s => s.User == applicationUser)
                .Where(s => s.Date.Date >= StartDate.Date && s.Date.Date <= StopDate.Date)
                .AsNoTracking()
                .ToListAsync();

            RDIs = await _context.Rdis.ToListAsync();

            if (applicationUser.GoalUnit == "Calorie")
            {
                foreach (var item in Intake)
                {
                    if (item.FoodId != null)
                    {
                        item.Food.Enegry = applicationUser.ConvertToCalorie((int)item.Food.Enegry);
                    }
                    //else if (item.CustomFoodId != null)
                    //{
                    //    item.CustomFood.Enegry = applicationUser.ConvertToCalorie((int)item.CustomFood.Enegry);
                    //}
                }

                SelectedGoalUnit = "Calories";
                AbrevGoalUnit = "kCal";
            }
            else
            {
                SelectedGoalUnit = "KiloJoules";
                AbrevGoalUnit = "KJ";
            }

            if (Intake.Count > 0)
            {
                //calculate intake, goals and rdi for progress bars
                FoodTotal = getTotalNutrients();
                Goals = getGoals(applicationUser);
                RDI_Category = getRDICategory(applicationUser.Age, applicationUser.Gender, applicationUser.Pregnant, applicationUser.Lactating);

                //convert macros numbers to names of tracked nutrients
                string Macros = applicationUser.Macros;
                List<int> numbers = new List<int>(Array.ConvertAll(Macros.Split(','), int.Parse));
                var goalPropertyList = Goals.GetType().GetProperties();

                trackedNutrients = new String[] { 
                    goalPropertyList[numbers[0]].Name, 
                    goalPropertyList[numbers[1]].Name, 
                    goalPropertyList[numbers[2]].Name 
                };
            }
        }
    }
}