﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace TestAfLogin.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [PersonalData]
        public string Name { get; set; }

        [Required]
        [PersonalData]
        public string Birthday { get; set; }

        [Required]
        [PersonalData]
        public string FieldOfStudy { get; set; }

        [PersonalData]
        public int Term { get; set; }

        [PersonalData]
        public List<CourseModel> courses { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string NameOfUser { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]
        public string imageName { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }









    }
}
