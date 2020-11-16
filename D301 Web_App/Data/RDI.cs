using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace D301_WebApp.Data
{
    public class RDI : Nutrient
    {
		[Key]
		public string Id { get; set; }
		public String AgeRange { get; set; }
        public String Gender { get; set; }
		public bool Pregnant { get; set; }
		public bool Lactating { get; set; }
	}
}
