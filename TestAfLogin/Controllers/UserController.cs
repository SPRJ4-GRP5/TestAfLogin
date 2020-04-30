using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAfLogin.Data;
using TestAfLogin.Models;

namespace TestAfLogin.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        //[HttpPost("{Name}", Name = "Search")]
        [HttpGet]
        public async Task<IActionResult> Search(string Name)
        {
            if (Name == null)
            {
                return NotFound();
            }

            var sUser = await _context.MApplicationUsers
                .FirstOrDefaultAsync(u => u.Name == Name);
            if (sUser == null)
            {
                return NotFound();
            }
            return View(sUser);
        }
    }
}