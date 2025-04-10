﻿using System;
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
            var bars = await _context.Bars.ToListAsync();

            // Отримуємо ID барів з непереглянутими оновленнями
            var barsWithUnviewedUpdates = await _context.Reservations
                .Where(r => (r.Status == "Confirmed" || r.Status == "Declined") && !r.IsStatusViewed)
                .Select(r => r.ReservedInId)
                .Distinct()
                .ToListAsync();

            ViewBag.BarsWithUnviewedUpdates = barsWithUnviewedUpdates;

            return View(bars);
        }

        // GET: Bars/Details/5
        // GET: Bars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Отримуємо бар з бази даних
            var bar = await _context.Bars
                .FirstOrDefaultAsync(m => m.Id == id);

            if (bar == null)
            {
                return NotFound();
            }

            // Отримуємо ВСІ резервування для цього бару
            var allReservations = await _context.Reservations
                .Where(r => r.ReservedInId == id)
                .Include(r => r.ReservedBy)  // Підвантажуємо дані клієнта
                .ToListAsync();

            // Фільтруємо резервування для відображення:
            // 1. Всі зі статусом "Pending" (завжди показуються)
            // 2. Ті, що мають статус "Confirmed/Declined" і ще не переглянуті
            var reservationsToShow = allReservations
                .Where(r => r.Status == "Pending" ||
                           ((r.Status == "Confirmed" || r.Status == "Declined") && !r.IsStatusViewed))
                .ToList();

            // Позначаємо "Confirmed/Declined" резервування як переглянуті
            var reservationsToMark = allReservations
                .Where(r => (r.Status == "Confirmed" || r.Status == "Declined") && !r.IsStatusViewed)
                .ToList();

            foreach (var reservation in reservationsToMark)
            {
                reservation.IsStatusViewed = true;
            }

            // Зберігаємо зміни в базі даних
            if (reservationsToMark.Any())
            {
                await _context.SaveChangesAsync();
            }

            // Передаємо дані у View
            ViewBag.Reservations = reservationsToShow;
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
        public async Task<IActionResult> Create([Bind("Name,Theme,Address,Latitude,Longitude")] Bar bar, IFormFile? photo)
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
        /*[HttpPost]
        public async Task<IActionResult> UpdateReservationStatuses([FromBody] Dictionary<int, string> statusUpdates)
        {
            if (statusUpdates == null || !statusUpdates.Any())
                return BadRequest("No changes to apply");

            // Список Id барів, для яких змінився статус
            var updatedBarIds = new HashSet<int>();

            foreach (var update in statusUpdates)
            {
                var reservation = await _context.Reservations.FindAsync(update.Key);
                if (reservation != null)
                {
                    reservation.Status = update.Value;
                    updatedBarIds.Add(reservation.ReservedInId); // Додаємо id бару до списку
                }
            }

            await _context.SaveChangesAsync();

            // Передаємо змінені barId в ViewData для використання в списку барів
            ViewData["UpdatedBars"] = updatedBarIds;

            return Ok();
        }*/
        /*
        [HttpPost]
        public async Task<IActionResult> UpdateReservationStatuses([FromBody] Dictionary<int, string> statusUpdates)
        {
            if (statusUpdates == null || !statusUpdates.Any())
                return BadRequest("No changes to apply");

            var updatedBarIds = new HashSet<int>();

            foreach (var update in statusUpdates)
            {
                var reservation = await _context.Reservations.FindAsync(update.Key);
                if (reservation != null)
                {
                    reservation.Status = update.Value;
                    reservation.IsStatusViewed = false; // Додаємо флаг, що статус не переглянутий
                    updatedBarIds.Add(reservation.ReservedInId);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(updatedBarIds); // Повертаємо ID оновлених барів
        }
        */

    }
}
