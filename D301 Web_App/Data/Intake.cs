using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace D301_WebApp.Data
{
	public class Intake
	{
		[Key]
		public int Id { get; set; }
		public int Quantity { get; set; }
		public string Measure { get; set; }
		public float Amount { get; set; }
		public DateTime Date { get; set; }
		public string VariationId { get; set; }
		public string VariationName { get; set; }
		public String MealType { get; set; }

		public string FoodId { get; set; }
		[ForeignKey("FoodId")]
		public Food Food { get; set; }

        public string CustomFoodId { get; set; }
		[ForeignKey("CustomFoodId")]
        public CustomFood CustomFood { get; set; }

        [ForeignKey("UserId")]
		public ApplicationUser User { get; set; }

		public string CalculateValueToString(float value, float foodtype) //food.amount, customfood.amount
		{
			if (value == 0)
			{
				return "0";
			}
			else if (value == -1)
			{
				return "<0.1";
			}
			else
			{
				return (Amount * Quantity * value / foodtype).ToString("0.0");
			}
		}

		public float CalculateValueToFloat(float value, float foodtype) //food.amount, customfood.amount
		{
			//treat "-1" as 0 for calculation
			if (value <= 0)
			{
				return 0;
			}
			else
			{
				return Amount * Quantity * value / foodtype;
			}
		}

	}
}