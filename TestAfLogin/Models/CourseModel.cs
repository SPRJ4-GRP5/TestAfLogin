using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAfLogin.Models
{
    public class CourseModel : ApplicationUser
    {
        public string CourseName { get; set; }
    }
}
