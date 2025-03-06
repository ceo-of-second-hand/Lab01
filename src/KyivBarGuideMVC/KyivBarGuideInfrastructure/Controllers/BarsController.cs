using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KyivBarGuideDomain.Model;
using KyivBarGuideInfrastructure;

namespace KyivBarGuideInfrastructure.Controllers
{
    public class BarsController : Controller
    {
        private readonly KyivBarGuideContext _context;

        public BarsController(KyivBarGuideContext context)
        {
            _context = context;
        }

        // GET: Bars
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bars.ToListAsync());
        }

        // GET: Bars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bar = await _context.Bars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bar == null)
            {
                return NotFound();
            }

            //check whether bar was added to favourites
            ViewBag.IsFavourite = await IsBarFavourite(bar.Id);

            return View(bar);
        }

        // GET: Bars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        //deleted Rating and Picture since do not wanna show them on first page
        //removed id since it is not changable
        public async Task<IActionResult> Create([Bind("Name,Theme")] Bar bar, IFormFile? photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null && photo.Length > 0) //added new piece for uploading photos
                {
                    var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(photo.FileName).ToLower();

                    if (!permittedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("photo", "Invalid file type. Only images are allowed.");
                        return View(bar);
                    }

                    var fileName = Path.GetFileName(photo.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    bar.Picture = "/images/" + fileName; // saving path
                }

                _context.Add(bar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bar);
        }

        // GET: Bars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bar = await _context.Bars.FindAsync(id);
            if (bar == null)
            {
                return NotFound();
            }
            return View(bar);
        }

        // POST: Bars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        //deleted Rating and Picture since do not wanna show them on first page
        //removed id since it is set automatically
        public async Task<IActionResult> Edit(int id, [Bind("Name,Theme")] Bar bar, IFormFile? photo)
        {
            if (id != bar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // find the existing bar entry in the database
                    var existingBar = await _context.Bars.FindAsync(id);
                    if (existingBar == null)
                    {
                        return NotFound();
                    }

                    // update name and theme
                    existingBar.Name = bar.Name;
                    existingBar.Theme = bar.Theme;

                    // if a new photo is provided, save it and update the picture field
                    if (photo != null && photo.Length > 0)
                    {
                        var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var extension = Path.GetExtension(photo.FileName).ToLower();

                        if (!permittedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("photo", "Invalid file type. Only images are allowed.");
                            return View(bar);
                        }

                        var fileName = Path.GetFileName(photo.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream);
                        }

                        //suggetsion to add path.combina (cross platform)
                        existingBar.Picture = "/images/" + fileName; 
                    }
                    _context.Update(existingBar);
                    await _context.SaveChangesAsync();
                }

                //KINDA DOUBTFUL
                //checks whteher data was not changed simultaneously
                catch (DbUpdateConcurrencyException)
                {
                    if (!BarExists(bar.Id))
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
            return View(bar);
        }

        // GET: Bars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bar = await _context.Bars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bar == null)
            {
                return NotFound();
            }

            return View(bar);
        }

        /*
        // POST: Bars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bar = await _context.Bars.FindAsync(id);
            if (bar != null)
            {
                _context.Bars.Remove(bar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bar = await _context.Bars.FindAsync(id);
            if (bar != null)
            {
                var favouriteBars = _context.FavouriteBars.Where(fb => fb.AddedId == id);
                _context.FavouriteBars.RemoveRange(favouriteBars);
                _context.Bars.Remove(bar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<bool> IsBarFavourite(int barId)
        {
            return await _context.FavouriteBars
                //.AnyAsync(fb => fb.AddedById == userId && fb.AddedId == barId);  TO BE CHANGED
                .AnyAsync(fb =>fb.AddedId == barId);
        }

        private bool BarExists(int id)
        {
            return _context.Bars.Any(e => e.Id == id);
        }
    }
}
