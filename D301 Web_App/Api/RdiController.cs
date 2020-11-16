using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using D301_WebApp.Data;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace D301_WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RdiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RdiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Rdi
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RDI>>> GetRDIs()
        {
            return await _context.Rdis.ToListAsync();
        }
    }
}
