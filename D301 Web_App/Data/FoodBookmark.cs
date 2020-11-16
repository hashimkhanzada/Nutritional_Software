using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace D301_WebApp.Data
{
    public class FoodBookmark
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public string FoodId { get; set; }
        [ForeignKey("FoodId")]
        public Food Food { get; set; }

        public string CustomFoodId { get; set; }
        [ForeignKey("CustomFoodId")]
        public CustomFood CustomFood { get; set; }
    }
}
