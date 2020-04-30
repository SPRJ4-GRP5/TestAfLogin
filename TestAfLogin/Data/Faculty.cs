using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TestAfLogin.Data
{
    public class Faculty
    { 
        public List<SelectListItem> FacList { get; set; }

        public Faculty()
        {
            FacList = new List<SelectListItem>();
            FacList.Add(new SelectListItem(){Text = "Science and Technology", Value = "Science and Technology" });
            FacList.Add(new SelectListItem() { Text = "Aarhus BSS", Value = "Aarhus BSS" });
            FacList.Add(new SelectListItem() { Text = "Arts", Value = "Arts" });
        }
    }
}
