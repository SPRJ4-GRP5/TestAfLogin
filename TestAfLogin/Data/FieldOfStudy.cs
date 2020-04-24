using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TestAfLogin.Data
{
    public class FieldOfStudy
    {
        public List<SelectListItem> FosList { get; set; }

        public FieldOfStudy()
        {
            FosList = new List<SelectListItem>();
            FosList.Add(new SelectListItem() { Text = "Softwareteknologi", Value = "Softwareteknologi" });
            FosList.Add(new SelectListItem() { Text = "Sundhedsteknologi", Value = "Sundhedsteknologi" });
            FosList.Add(new SelectListItem() { Text = "Elektronikteknologi", Value = "Elektronikteknologi" });
            FosList.Add(new SelectListItem() { Text = "NikolajSlikkerNumse", Value = "NikolajSlikkerNumse" });
        }
    }
}

