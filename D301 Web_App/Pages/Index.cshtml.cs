using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using D301_WebApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace D301_WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async void OnGetAsync()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            //set isTrainer session if trainer is logged in already - for nav bar displaying Trainees link
            if (_context.Users.Local.Count() > 0) 
            {
                if (_context.Users.Where(u => u.TrainerId == applicationUser.Id).Count() > 0 && HttpContext.Session.GetString("isTrainer") != "true")
                {
                    HttpContext.Session.SetString("isTrainer", "true");
                }
            }
        }
    }
}
