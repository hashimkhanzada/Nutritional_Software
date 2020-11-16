using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace D301_WebApp.Data
{
    public class CustomFood : Nutrient
    {
		[Key]
		public string Id { get; set; }
		public string Name { get; set; }
		public float Amount { get; set; }
		
		public ICollection<Intake> Intakes { get; set; }

		public string GenerateId(string CFoodName)
        {
			Random random = new Random();
			string num = random.Next(1,10000).ToString();
			string GeneratedId;
			if(CFoodName.Length > 3)
            {
				GeneratedId = CFoodName.Substring(0, 3) + num;
            } else
            {
				GeneratedId = CFoodName.Substring(0, 1) + num;
			}
			
			return GeneratedId;
        }
	}
}
