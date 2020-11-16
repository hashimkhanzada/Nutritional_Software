using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace D301_WebApp.Data
{
    public class CustomFoodVariation
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }

        [ForeignKey("CustomFoodId")]
        public string CustomFoodId { get; set; }
    }
}
