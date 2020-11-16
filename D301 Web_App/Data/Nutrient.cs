using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace D301_WebApp.Data
{
    public class Nutrient
    {
		public float Water { get; set; }
		[Display(Name = "Energy")]
		public float Enegry { get; set; }
		[Display(Name = "Energy NIP")]
		public float EnegryNIP { get; set; }
		public float Protein { get; set; }
		public float Fat { get; set; }
		public float Carbohydrates { get; set; }
		[Display(Name = "Dietary Fibre")]
		public float DietaryFibre { get; set; }
		public float Sugars { get; set; }
		public float Starch { get; set; }
		public float SFA { get; set; }
		public float MUFA { get; set; }
		public float PUFA { get; set; }
		[Display(Name = "Alpha Linolenic Acid")]
		public float AlphaLinolenicAcid { get; set; }
		[Display(Name = "Linoleic Acid")]
		public float LinoleicAcid { get; set; }
		public float Cholesterol { get; set; }
		public float Sodium { get; set; }
		public float Iodine { get; set; }
		public float Potassium { get; set; }
		public float Phosphorus { get; set; }
		public float Calcium { get; set; }
		public float Iron { get; set; }
		public float Zinc { get; set; }
		public float Selenium { get; set; }
		[Display(Name = "Vitamin A")]
		public float VitaminA { get; set; }
		[Display(Name = "Beta Carotene")]
		public float BetaCarotene { get; set; }
		public float Thiamin { get; set; }
		public float Riboflavin { get; set; }
		public float Niacin { get; set; }
		[Display(Name = "Vitamin B6")]
		public float VitaminB6 { get; set; }
		[Display(Name = "Vitamin B12")]
		public float VitaminB12 { get; set; }
		[Display(Name = "Dietary Folate")]
		public float DietaryFolate { get; set; }
		[Display(Name = "Vitamin C")]
		public float VitaminC { get; set; }
		[Display(Name = "Vitamin D")]
		public float VitaminD { get; set; }
		[Display(Name = "Vitamin E")]
		public float VitaminE { get; set; }
		public float Alcohol { get; set; }
	}
}
