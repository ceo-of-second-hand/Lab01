using Microsoft.AspNetCore.Mvc;
using KyivBarGuideDomain.Model;
using KyivBarGuideInfrastructure;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace KyivBarGuideInfrastructure.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly KyivBarGuideContext _context;

        public ReservationsController(KyivBarGuideContext context)
        {
            _context = context;
        }

        // New method for creating reservations with barId
        // GET: Reservations/Create?barId=5


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
        public async Task<IActionResult> Create(int barId, bool smokerStatus, DateOnly date)
        {
            // check
            if (date < DateOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError("Date", "Reservation date can't be in the past");
                ViewBag.BarId = barId; // Повертаємо barId для повторного відображення форми
                return View();
            }
            var bar = await _context.Bars.FindAsync(barId);
            if (bar == null)
            {
                return NotFound();
            }

            // Creating a new reservation (New logic introduced in second version)
            var reservation = new Reservation
            {

                ReservedInId = barId, // Storing barId in ReservedInId (This links the reservation to the bar)
                SmokerStatus = smokerStatus,
                Date = date,
                ReservedById = null, // dummy client Id (WAITING TO BE CHANGED LATER AND THAT FIELD TO BE SET BACK TO NOT NULLABLE)
                ConfirmedById = null // dummy admin Id (WAITING TO BE CHANGED LATER  AND THAT FIELD TO BE SET BACK TO NOT NULLABLE)
            };

            // Add new reservation to database
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            TempData["ProcessingMessage"] = $"Reservation #{reservation.Id} is : BEING PROCESSED⚙️";

            // Redirect to bar details page after reservation creation (New functionality)
            return RedirectToAction("Details", "Bars", new { id = barId });
        }

        // Old code (for comparison)
        // GET: Reservations
        // returns Index.cshtml with list of reservations
        // public async Task<IActionResult> Index() { ... }

        // GET: Reservations/Details/
        // public async Task<IActionResult> Details(int? id) { ... }

        // GET: Reservations/Create (Old version)
        // public IActionResult Create() { 
        //   ViewData["ConfirmedById"] = new SelectList(_context.Admins, "Id", "Name"); 
        //   ViewData["ReservedById"] = new SelectList(_context.Clients, "Id", "Name");
        //   return View(); 
        // }

        // POST: Reservations/Create (Old version)
        // public async Task<IActionResult> Create([Bind("Id,ReservedById,ReservedInId,ConfirmedById,SmokerStatus,ConcertVisit,Date")] Reservation reservation) { ... }

        // GET: Reservations/Edit/5
        // public async Task<IActionResult> Edit(int? id) { ... }

        // POST: Reservations/Edit/5
        // public async Task<IActionResult> Edit(int id, [Bind("Id,ReservedById,ReservedInId,ConfirmedById,SmokerStatus,ConcertVisit,Date")] Reservation reservation) { ... }

        // GET: Reservations/Delete/5
        // public async Task<IActionResult> Delete(int? id) { ... }

        // POST: Reservations/Delete/5
        // public async Task<IActionResult> DeleteConfirmed(int id) { ... }

        // Method to check if reservation exists
        // private bool ReservationExists(int id) { ... }
    }
}
