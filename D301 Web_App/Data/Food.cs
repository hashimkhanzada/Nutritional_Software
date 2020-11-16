using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace D301_WebApp.Data
{
	public class Food : Nutrient
	{
		[Key]
		public string Id { get; set; }
		public string Name { get; set; }
		public float Amount { get; set; }

		public ICollection<Intake> Intakes { get; set; }
		public ICollection<FoodVariation> Amounts { get; set; }
	}
}
