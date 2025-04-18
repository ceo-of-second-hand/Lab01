﻿using Microsoft.AspNetCore.Mvc;
using KyivBarGuideDomain.Model;
using KyivBarGuideInfrastructure;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace KyivBarGuideInfrastructure.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly KyivBarGuideContext _context;

        public ReservationsController(KyivBarGuideContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // Get the admin using the UserId
            var admin = await _context.Admins
                .Include(a => a.WorkIn)
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (admin?.WorkIn == null)
            {
                return NotFound("You are not associated with any bar.");
            }

            // Get ONLY PENDING reservations for the admin's bar
            var reservations = await _context.Reservations
                .Include(r => r.ReservedBy)
                    .ThenInclude(c => c.User)
                .Where(r => r.ReservedInId == admin.WorkIn.Id)
                .Where(r => r.Status == "Pending") // Only show pending reservations
                .OrderByDescending(r => r.Date)
                .ThenBy(r => r.Time)
                .ToListAsync();

            ViewBag.BarName = admin.WorkIn.Name;
            return View(reservations);
        }

        // New method for creating reservations with barId
        // GET: Reservations/Create?barId=5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportReservations(IFormFile file, int barId)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please select a file to upload";
                return RedirectToAction("Details", "Bars", new { id = barId });
            }

            try
            {
                // Get the current user's client ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
                
                if (client == null)
                {
                    return NotFound("Client profile not found.");
                }

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var reservation = new Reservation
                            {
                                ReservedInId = barId,
                                Date = DateOnly.FromDateTime(DateTime.Parse(worksheet.Cells[row, 1].Text)),
                                SmokerStatus = worksheet.Cells[row, 2].Text.Equals("Yes", StringComparison.OrdinalIgnoreCase),
                                ReservedById = client.Id // Set the actual client ID
                            };

                            _context.Reservations.Add(reservation);
                        }

                        await _context.SaveChangesAsync();
                    }
                }

                TempData["Success"] = "Reservations imported successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error importing file: {ex.Message}";
            }

            return RedirectToAction("Details", "Bars", new { id = barId });
        }

        [HttpGet]
        public IActionResult ExportReservationsToExcel(
     [FromQuery] int barId,              // Required bar ID
     [FromQuery] DateTime? endDate)      // Optional end date
        {
            // Set default end date (today + 7 days)
            var endDateValue = endDate ?? DateTime.Now.AddDays(7);
            var today = DateOnly.FromDateTime(DateTime.Now);
            var endDateOnly = DateOnly.FromDateTime(endDateValue);

            // Get reservations ONLY for the specified bar
            var reservations = _context.Reservations
                .Where(r => r.ReservedInId == barId)          // Filter by bar
                .Where(r => r.Date >= today && r.Date <= endDateOnly)  // Date range
                .OrderBy(r => r.Date)
                .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // or LicenseContext.Commercial


            // Generate Excel
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Reservations");

                // Headers
                worksheet.Cells[1, 1].Value = "Date";
                worksheet.Cells[1, 2].Value = "Smoker Status";

                // Data
                for (int i = 0; i < reservations.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reservations[i].Date.ToString("yyyy-MM-dd");
                    worksheet.Cells[i + 2, 2].Value = reservations[i].SmokerStatus ? "Yes" : "No";
                }

                // Formatting
                worksheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;
                worksheet.Cells.AutoFitColumns();

                // Return file
                var stream = new MemoryStream(package.GetAsByteArray());
                return File(
                    stream,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Bar_{barId}_Reservations_{today:yyyyMMdd}_To_{endDateOnly:yyyyMMdd}.xlsx"
                );
            }
        }
        public IActionResult Create(int? barId)
        {
            if (barId == null)
            {
                return NotFound();
            }

            var bar = _context.Bars.Find(barId);
            if (bar == null)
            {
                return NotFound();
            }

            // Pass barId using ViewBag (new functionality)
            ViewBag.BarId = barId;
            return View();
        }

        // New POST method for creating a reservation based on barId, smoker status, and date
        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int barId, bool smokerStatus, DateOnly date, TimeOnly time, string status)
        {
            // Get the current user's client ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
            
            if (client == null)
            {
                return NotFound("Client profile not found.");
            }

            // check
            if (date < DateOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError("Date", "Reservation date can't be in the past");
                ViewBag.BarId = barId;
                return View();
            }
            var bar = await _context.Bars.FindAsync(barId);
            if (bar == null)
            {
                return NotFound();
            }

            // Creating a new reservation
            var reservation = new Reservation
            {
                ReservedInId = barId,
                SmokerStatus = smokerStatus,
                Date = date,
                Time = time,
                ReservedById = client.Id // Set the actual client ID
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            TempData["ProcessingMessage"] = $"Reservation #{reservation.Id} is : BEING PROCESSED⚙️";
            return RedirectToAction("Details", "Bars", new { id = barId });
        }
        // Додайте ці методи в кінець вашого контролера, перед закриваючою дужкою класу

        [HttpGet]
        public async Task<IActionResult> GetReservationsForBar(int barId)
        {
            var reservations = await _context.Reservations
                .Include(r => r.ReservedBy)  // Додаємо клієнта
                .Where(r => r.ReservedInId == barId)
                .Where(r => r.Status != "Confirmed" && r.Status != "Declined") // Додаємо фільтр
                .Select(r => new
                {
                    id = r.Id,
                    reservedBy = r.ReservedBy != null ? new { name = r.ReservedBy.Name } : null,
                    smokerStatus = r.SmokerStatus,
                    concertVisit = r.ConcertVisit,
                    date = r.Date,
                    time = r.Time.ToString("hh\\:mm"),
                    status = r.Status
                })
                .ToListAsync();

            return Json(reservations);
        }

        /*[HttpPost]
        public async Task<IActionResult> UpdateReservationStatuses([FromBody] Dictionary<int, string> statusUpdates)
        {
            if (statusUpdates == null || !statusUpdates.Any())
                return BadRequest("No changes to apply");

            foreach (var update in statusUpdates)
            {
                var reservation = await _context.Reservations.FindAsync(update.Key);
                if (reservation != null)
                {
                    reservation.Status = update.Value;
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
        */
        [HttpPost]
        public async Task<IActionResult> UpdateReservationStatuses([FromBody] Dictionary<int, string> statusUpdates)
        {
            if (statusUpdates == null || !statusUpdates.Any())
                return BadRequest("No changes to apply");

            var updatedBarIds = new HashSet<int>();

            foreach (var update in statusUpdates)
            {
                var reservation = await _context.Reservations
                    .FirstOrDefaultAsync(r => r.Id == update.Key);

                if (reservation != null && (reservation.Status != update.Value))
                {
                    reservation.Status = update.Value;

                    if (update.Value == "Confirmed" || update.Value == "Declined")
                    {
                        reservation.IsStatusViewed = false;
                        updatedBarIds.Add(reservation.ReservedInId);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok(updatedBarIds.ToList()); // Return the list of updated bar IDs
        }
    }

}
