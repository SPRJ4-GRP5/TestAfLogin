using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAfLogin.Models
{
    public class FieldOfStudyModel : ApplicationUser
    {
        private List<string> fieldOfStudy;

        public FieldOfStudyModel()
        {
            fieldOfStudy = new List<string>();
            fieldOfStudy.Add("Softwareteknologi");
            fieldOfStudy.Add("Sundhedsteknologi");
            fieldOfStudy.Add("Elektronikteknologi");
        }
    }
}
