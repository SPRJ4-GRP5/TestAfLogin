using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using TestAfLogin.Models;
using TestAfLogin.Data;

namespace TestAfLogin.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _hostEnvironment;


        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IWebHostEnvironment hostEnvironment,
            ApplicationDbContext context 
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _hostEnvironment = hostEnvironment;
            _context = context;
        }


        public FieldOfStudy Fos = new FieldOfStudy();

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [PersonalData]
            [Display(Name = "Full name")]
            public string Name { get; set; }

            [Required]
            //[EmailAddress]
            [StringLength(8, ErrorMessage = "Only write your AU-ID!")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [PersonalData]
            [Display(Name = "Birthday")]
            public string Birthday { get; set; }

            [Required]
            [PersonalData]
            [Display(Name = "Phone number:")]
            public string PhoneNumber { get; set; }

            [Required]
            [PersonalData]
            [Display(Name = "Field of Study")]
            public string FieldOfStudy { get; set; }
            
            [PersonalData]
            [Display(Name = "Term")]
            public int Term { get; set; }

            [PersonalData]
            [Display(Name = "Name of user")]
            public string NameOfUser { get; set; }

            [PersonalData]
            [Display(Name = "Image name")]
            public string imageName { get; set; }

            [PersonalData]
            [Display(Name = "Upload file")]
            public IFormFile ImageFile { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }
       
        [HttpPost]
        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            returnUrl = null;
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                
                var user = new ApplicationUser()
                {
                    Name = Input.Name,
                    UserName = Input.Email + "@uni.au.dk", 
                    Email = Input.Email + "@uni.au.dk",
                    PhoneNumber = Input.PhoneNumber,
                    Birthday = Input.Birthday,
                    FieldOfStudy = Input.FieldOfStudy,
                    Term = Input.Term,
                    //NameOfUser = Input.NameOfUser
                    //imageName = Input.imageName,
                    ImageFile = Input.ImageFile
                };

                if (user.ImageFile != null)
                {
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

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);
                    //
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

    }
}
