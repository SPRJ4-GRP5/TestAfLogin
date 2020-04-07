using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TestAfLogin.Models;

namespace TestAfLogin.Data
{
    public class DbHelper
    {
        public static void SeedData(ApplicationDbContext db, UserManager<ApplicationUser> userManager, ILogger log)
        {
            SeedUsers(userManager, log);
        }


        public static void SeedUsers(UserManager<ApplicationUser> userManager, ILogger log)
        {
            const string adminEmail = "Hans@gmail.com";
            const string adminPassword = "Hans123!";

            if (userManager.FindByNameAsync(adminEmail).Result == null)
            {
                log.LogWarning("Seeding the admin user");
                var user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "Admin"
                };
                IdentityResult result = userManager.CreateAsync
                    (user, adminPassword).Result;
                if (result.Succeeded)
                {
                    var adminClaim = new Claim("Admin", "Yes");
                    userManager.AddClaimAsync(user, adminClaim);
                }
            }
        }


    }
}
