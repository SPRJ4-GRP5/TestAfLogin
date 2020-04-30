using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TestAfLogin.Models
{
    public class UserModel
    {
        public ApplicationUser User { get; set; }

        public List<ApplicationUser> Users { get; set; }
    }
}
