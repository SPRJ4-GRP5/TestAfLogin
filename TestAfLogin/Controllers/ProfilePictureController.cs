using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestAfLogin.Data;
using TestAfLogin.Models;

namespace TestAfLogin.Controllers
{
    public class ProfilePictureController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProfilePictureController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: ProfilePicture
        public async Task<IActionResult> Index()
        {
            return View(await _context.MProfilePictureModel.ToListAsync());
        }

        // GET: ProfilePicture/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profilePictureModel = await _context.MProfilePictureModel
                .FirstOrDefaultAsync(m => m.PictureId == id);
            if (profilePictureModel == null)
            {
                return NotFound();
            }

            return View(profilePictureModel);
        }

        // GET: ProfilePicture/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProfilePicture/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PictureId,NameOfUser,ImageFile")] ProfilePictureModel profilePictureModel)
        {
            if (ModelState.IsValid)
            {
                //Saving image to wwwroot/image with filename and timestamp 
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(profilePictureModel.ImageFile.FileName); //name of the file
                string extension = Path.GetExtension(profilePictureModel.ImageFile.FileName); //example .jpg .png
                profilePictureModel.imageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension; //combine name and filetype and date
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await profilePictureModel.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(profilePictureModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(profilePictureModel);
        }

        // GET: ProfilePicture/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profilePictureModel = await _context.MProfilePictureModel.FindAsync(id);
            if (profilePictureModel == null)
            {
                return NotFound();
            }
            return View(profilePictureModel);
        }

        // POST: ProfilePicture/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PictureId,NameOfUser,imageName")] ProfilePictureModel profilePictureModel)
        {
            if (id != profilePictureModel.PictureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profilePictureModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfilePictureModelExists(profilePictureModel.PictureId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(profilePictureModel);
        }

        // GET: ProfilePicture/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profilePictureModel = await _context.MProfilePictureModel
                .FirstOrDefaultAsync(m => m.PictureId == id);
            if (profilePictureModel == null)
            {
                return NotFound();
            }

            return View(profilePictureModel);
        }

        // POST: ProfilePicture/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profilePictureModel = await _context.MProfilePictureModel.FindAsync(id);
            //Delete an image from wwwroot image folder
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", profilePictureModel.imageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }


            _context.MProfilePictureModel.Remove(profilePictureModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfilePictureModelExists(int id)
        {
            return _context.MProfilePictureModel.Any(e => e.PictureId == id);
        }
    }
}
