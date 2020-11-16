using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D301_WebApp.Data
{
    public static class Unit
    {
		public static string GetUnit(string nutrient)
		{
			switch (nutrient)
			{
				case "Enegry": return "kJ";
				case "EnegryNIP": return "kJ";
				case "Cholesterol": return "mg";
				case "Sodium": return "mg";
				case "Iodine": return "μg";
				case "Potassium": return "mg";
				case "Phosphorus": return "mg";
				case "Calcium": return "mg";
				case "Iron": return "mg";
				case "Zinc": return "mg";
				case "Selenium": return "μg";
				case "VitaminA": return "μg";
				case "BetaCarotene": return "μg";
				case "Thiamin": return "mg";
				case "Riboflavin": return "mg";
				case "Niacin": return "mg";
				case "VitaminB6": return "mg";
				case "VitaminB12": return "μg";
				case "DietaryFolate": return "μg";
				case "VitaminC": return "mg";
				case "VitaminD": return "μg";
				case "VitaminE": return "mg";
				default: return "g";
			}
		}
	}
}
