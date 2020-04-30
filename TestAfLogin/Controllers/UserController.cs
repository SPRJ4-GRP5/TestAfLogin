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
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //[HttpPost("{Name}", Name = "Search")]
        [HttpGet]
        public async Task<IActionResult> Search(string userName)
        {
            if (userName == null)
            {
                return NotFound();
            }

            var sUser = await _context.MApplicationUsers
                .FirstOrDefaultAsync(u => u.UserName == userName);
            if (sUser == null)
            {
                return NotFound();
            }

            return View(sUser);
        }
    }
}