using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestAfLogin.Data;
using TestAfLogin.Models;

namespace TestAfLogin.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        public ApplicationUser AppUser { get; set; }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostEnvironment = hostEnvironment;
        }

        public string Username { get; set; }
        public string Email { get; set; }

        public FieldOfStudy Fos = new FieldOfStudy();
        public Faculty Fac = new Faculty();

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Telefon nummer")]
            public string PhoneNumber { get; set; }

            [PersonalData]
            [Display(Name = "Skift profilbillede")]
            public IFormFile ImageFile { get; set; }

            [PersonalData]
            [Display(Name = "Om mig")]
            public string Description { get; set; }

            [PersonalData]
            [Display(Name = "Studieretning")]
            public string FieldOfStudy { get; set; }

            [PersonalData]
            [Display(Name = "Fødselsdag")]
            public string Birthday { get; set; }

            [PersonalData]
            [Display(Name = "Semester")]
            public int Term { get; set; }

            [PersonalData]
            [Display(Name = "Fakultet")]
            public string Faculty { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            this.AppUser = user;
            Username = userName;
            Email = email;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
            };      
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            await LoadAsync(user);
            return Page();
        }

        public async void ChangeProfilePicture(ApplicationUser user)
        {
            user.ImageFile = Input.ImageFile;
            if (user.ImageFile != null)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", user.imageName);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);

                //Saving image to wwwroot/image with filename and timestamp 
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(user.ImageFile.FileName); //name of the file
                string extension = Path.GetExtension(user.ImageFile.FileName); //example .jpg .png
                user.imageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension; //combine name and filetype and date
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await user.ImageFile.CopyToAsync(fileStream);
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }


            if (ModelState.IsValid)
            {
                ChangeProfilePicture(user);

                if (Input.FieldOfStudy != null)
                {
                    user.FieldOfStudy = Input.FieldOfStudy;
                }

                if(Input.Faculty != null)
                {
                    user.Faculty = Input.Faculty;
                }

                if (Input.Description != null)
                {
                    user.Description = Input.Description;
                }
                user.Term = Input.Term;
                await _userManager.UpdateAsync(user);
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Din profil er blevet opdateret";
            return RedirectToPage();
        }
    }
}
