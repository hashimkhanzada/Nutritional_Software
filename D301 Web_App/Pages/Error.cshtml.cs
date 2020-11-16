using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using D301_WebApp.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace D301_WebApp.Pages
{
    public class ErrorModel : PageModel
    {
        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
        }

        public String Message { get; set; }

        [BindProperty(SupportsGet = true)]
        public String Code { get; set; } = "404";
        

        public async Task<IActionResult> OnGet()
        {
            //if trying to access /Error page directly
            var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (statusCodeReExecuteFeature == null)
            {
                return new StatusCodeResult(404);
            }

            return Page();
        }
    }
}
