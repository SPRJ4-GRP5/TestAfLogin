using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestAfLogin.Controllers
{
    [Authorize("IsAdmin")]
    public class AdminController : Controller
    {
        public IActionResult AdminPage()
        {
            return View();
        }
    }

}