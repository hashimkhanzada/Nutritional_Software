using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace D301_WebApp.Data
{
	public class ApplicationUser : IdentityUser
	{
		[PersonalData]
		public string FirstName { get; set; }

		[PersonalData]
		public string LastName { get; set; }

		[PersonalData]
		public float Weight { get; set; }

		[PersonalData]
		public string WeightUnit { get; set; }

		[PersonalData]
		public float Height { get; set; }

		[PersonalData]
		public string HeightUnit { get; set; }

		[PersonalData]
        public string ActivityLevel { get; set; }

		[PersonalData]
		public string MedicalConditions { get; set; }

		[PersonalData]
		public string Gender { get; set; }

		[PersonalData]
		public DateTime DOB { get; set; }

		[PersonalData]
		public bool Pregnant { get; set; }

		[PersonalData]
		public bool Lactating { get; set; }

		[NotMapped]
		public string FullName
		{
			get
			{
				return FirstName + " " + LastName;
			}
		}

		[NotMapped]
		public string NormalizedFullName
		{
			get
			{
				return this.FullName.ToUpper();
			}
		}
		

		[NotMapped]
		public int Age
		{
			get
			{
				// Calculate the age.
				var age = DateTime.Today.Year - DOB.Year;
				// Go back to the year the person was born in case of a leap year
				if (DOB.Date > DateTime.Today.AddYears(-age)) age--;
				return age;
			}
		}

		[NotMapped]
		public float UserBMR
		{
			get
			{
				//bmr formula below uses Kg and cm as its measuring units
				float weight = Weight;
				float height = Height;
				if (WeightUnit == "lb") //converts to kg
				{
					weight = (float)(Weight / 2.2);
				}
				if (HeightUnit == "in") //converts to cm
				{
					height = (float)(Height / 0.39);
				}

                var CalorieFormula = (10 * weight) + (6.25 * height) - (5 * Age); //formula to calculate TDEE
                float bmr;
                if (Gender == "Male")
				{
                    bmr = (int)(CalorieFormula + 5);
                    //bmr = (float)(66 + (13.7 * Weight) + (5 * Height) - (6.8 * Age));

                }
				else 
				{
                    bmr = (int)(CalorieFormula - 161);
                    //bmr = (float)(655 + (9.6 * Weight) + (1.8 * Height) - (4.7 * Age));
                }

				return bmr;
			}
		}

		[NotMapped]
		public float UserTdee
		{
			get
			{
				float tdee;
				if (ActivityLevel == "Sedentary")
                {
					tdee = (float)(UserBMR * 1.2);
                }
				else if (ActivityLevel == "LightlyActive") {
					tdee = (float)(UserBMR * 1.375);
                }
				else if (ActivityLevel == "ModeratelyActive")
                {
					tdee = (float)(UserBMR * 1.55);
                }
				else if (ActivityLevel == "VeryActive")
                {
					tdee = (float)(UserBMR * 1.725);
				}
				else if (ActivityLevel == "ExtremelyActive")
                {
					tdee = (float)(UserBMR * 1.9);
				}
				else
                {
					tdee = (float)UserBMR;
                }

				return tdee;
			}
		}

        public int ConvertToCalorie(int KjValue)
        {
			int CalorieGoal = (int)(KjValue / 4.184);

			return CalorieGoal;
		}

        public int ConvertToKj(int CalorieValue)
        {
			int KjGoal = (int)(CalorieValue * 4.184);

			return KjGoal;
		}

        public float WaterGoal { get; set; }
		public float EnegryGoal { get; set; }
        public string GoalUnit { get; set; }
        public float EnegryNIPGoal { get; set; }
		public float ProteinGoal { get; set; }
		public float FatGoal { get; set; }
		public float CarbohydratesGoal { get; set; }
		public float DietaryFibreGoal { get; set; }
		public float SugarsGoal { get; set; }
		public float StarchGoal { get; set; }
		public float SFAGoal { get; set; }
		public float MUFAGoal { get; set; }
		public float PUFAGoal { get; set; }
		public float AlphaLinolenicAcidGoal { get; set; }
		public float LinoleicAcidGoal { get; set; }
		public float CholesterolGoal { get; set; }
		public float SodiumGoal { get; set; }
		public float IodineGoal { get; set; }
		public float PotassiumGoal { get; set; }
		public float PhosphorusGoal { get; set; }
		public float CalciumGoal { get; set; }
		public float IronGoal { get; set; }
		public float ZincGoal { get; set; }
		public float SeleniumGoal { get; set; }
		public float VitaminAGoal { get; set; }
		public float BetaCaroteneGoal { get; set; }
		public float ThiaminGoal { get; set; }
		public float RiboflavinGoal { get; set; }
		public float NiacinGoal { get; set; }
		public float VitaminB6Goal { get; set; }
		public float VitaminB12Goal { get; set; }
		public float DietaryFolateGoal { get; set; }
		public float VitaminCGoal { get; set; }
		public float VitaminDGoal { get; set; }
		public float VitaminEGoal { get; set; }
		public float AlcoholGoal { get; set; }

		//[DefaultValue("3,4,5")]
		public string Macros { get; set; } = "3,4,5";

		//If -1 = user
		//If 0 = trainer
		//If >0 user with trainer
		//[DefaultValue("-1")]
		public string TrainerId { get; set; } = "-1";
    }
}
