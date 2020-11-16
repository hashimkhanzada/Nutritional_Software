using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace D301_WebApp.Data
{
    public class FoodVariation
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }

        [ForeignKey("FoodId")]
        public string FoodId { get; set; }
    }
}
