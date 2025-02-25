using Microsoft.AspNetCore.Mvc;
using KyivBarGuideDomain.Model;
using KyivBarGuideInfrastructure;
using System.Threading.Tasks;

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
            // Перевірка, чи дата не є минулою
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
                ReservedById = null, // dummy client Id (WAITING TO BE CHANGED LATER)
                ConfirmedById = null // dummy admin Id (WAITING TO BE CHANGED LATER)
            };

            // Add new reservation to database
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Додаємо повідомлення до TempData
            TempData["ReservationMessage"] = $"Reservation #{reservation.Id} is : processing";

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
