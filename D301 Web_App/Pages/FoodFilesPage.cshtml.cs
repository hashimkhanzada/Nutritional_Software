using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using D301_WebApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace D301_WebApp.Pages
{
    public class FoodFilesPageModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        //Declare variables to be used for model
        [BindProperty]
        public Food Food { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        //Intake model constructor
        public FoodFilesPageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Food> FoodList { get; set; }

        public async Task OnGetAsync()
        {
            var foods = from f in _context.Foods select f;

            if (!String.IsNullOrEmpty(SearchString))
            {
                foods = foods.Where(s => s.Name.Contains(SearchString));
            }

            FoodList = await foods.ToListAsync();
        }


    }
}