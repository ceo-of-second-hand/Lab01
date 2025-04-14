using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KyivBarGuideDomain.Model;
using KyivBarGuideInfrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using OfficeOpenXml;

namespace KyivBarGuideInfrastructure.Controllers
{
    [Authorize]
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

        // GET: Bars/Edit
        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var admin = await _context.Admins
                .Include(a => a.WorkIn)
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (admin?.WorkIn == null)
            {
                return NotFound("You are not associated with any bar.");
            }

            var bar = await _context.Bars.FindAsync(admin.WorkIn.Id);
            if (bar == null)
            {
                return NotFound();
            }

            return View(bar);
        }

        // POST: Bars/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name,Theme,Address,Latitude,Longitude")] Bar bar, IFormFile? photo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingBar = await _context.Bars.FindAsync(bar.Id);
                    if (existingBar == null)
                    {
                        return NotFound();
                    }

                    existingBar.Name = bar.Name;
                    existingBar.Theme = bar.Theme;
                    existingBar.Address = bar.Address;
                    existingBar.Latitude = bar.Latitude;
                    existingBar.Longitude = bar.Longitude;

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

                        existingBar.Picture = "/images/" + fileName;
                    }

                    _context.Update(existingBar);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
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

        // GET: Bars/ExportToExcel
        [HttpGet]
        [Route("Bars/ExportToExcel")]
        public async Task<IActionResult> ExportToExcel(DateTime? startDate, DateTime? endDate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var admin = await _context.Admins
                .Include(a => a.WorkIn)
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (admin?.WorkIn == null)
            {
                return NotFound("You are not associated with any bar.");
            }

            // Set default dates if not provided
            startDate ??= DateTime.Today;
            endDate ??= DateTime.Today.AddDays(7);

            // Convert DateTime to DateOnly for comparison
            var startDateOnly = DateOnly.FromDateTime(startDate.Value);
            var endDateOnly = DateOnly.FromDateTime(endDate.Value);

            var reservations = await _context.Reservations
                .Include(r => r.ReservedBy)
                .Where(r => r.ReservedInId == admin.WorkIn.Id &&
                           r.Date >= startDateOnly &&
                           r.Date <= endDateOnly)
                .OrderByDescending(r => r.Date)
                .ThenBy(r => r.Time)
                .ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Reservations");
                
                // Add headers
                worksheet.Cells[1, 1].Value = "Date";
                worksheet.Cells[1, 2].Value = "Time";
                worksheet.Cells[1, 3].Value = "Client Name";
                worksheet.Cells[1, 4].Value = "Status";
                worksheet.Cells[1, 5].Value = "Smoker Status";
                worksheet.Cells[1, 6].Value = "Concert Visit";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                for (int i = 0; i < reservations.Count; i++)
                {
                    var reservation = reservations[i];
                    worksheet.Cells[i + 2, 1].Value = reservation.Date.ToString("yyyy-MM-dd");
                    worksheet.Cells[i + 2, 2].Value = reservation.Time.ToString(@"hh\:mm");
                    worksheet.Cells[i + 2, 3].Value = reservation.ReservedBy?.Name;
                    worksheet.Cells[i + 2, 4].Value = reservation.Status;
                    worksheet.Cells[i + 2, 5].Value = reservation.SmokerStatus ? "Yes" : "No";
                }

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Set the content type and file name
                var fileName = $"Reservations_{startDateOnly:yyyyMMdd}-{endDateOnly:yyyyMMdd}.xlsx";
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                // Return the file
                return File(package.GetAsByteArray(), contentType, fileName);
            }
        }

    }
}
