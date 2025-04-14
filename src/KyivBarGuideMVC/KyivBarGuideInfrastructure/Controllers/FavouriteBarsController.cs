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
using Microsoft.AspNetCore.Identity;

namespace KyivBarGuideInfrastructure.Controllers
{
    [Authorize]
    public class FavouriteBarsController : Controller
    {
        private readonly KyivBarGuideContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavouriteBarsController(KyivBarGuideContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: FavouriteBars
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == currentUser.Id);

            if (client == null)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var favouriteBars = await _context.FavouriteBars
                .Include(f => f.Added)
                .Where(f => f.AddedById == client.Id)
                .ToListAsync();

            return View(favouriteBars);
        }

        // GET: FavouriteBars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == currentUser.Id);

            if (client == null)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var favouriteBar = await _context.FavouriteBars
                .Include(f => f.Added)
                .FirstOrDefaultAsync(m => m.Id == id && m.AddedById == client.Id);

            if (favouriteBar == null)
            {
                return NotFound();
            }

            return View(favouriteBar);
        }

        // GET: FavouriteBars/Create
        public IActionResult Create()
        {
            ViewData["AddedId"] = new SelectList(_context.Bars, "Id", "Name");
            ViewData["AddedById"] = new SelectList(_context.Clients, "Id", "Name");
            return View();
        }

        // POST: FavouriteBars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AddedById,AddedId")] FavouriteBar favouriteBar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favouriteBar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddedId"] = new SelectList(_context.Bars, "Id", "Name", favouriteBar.AddedId);
            ViewData["AddedById"] = new SelectList(_context.Clients, "Id", "Name", favouriteBar.AddedById);
            return View(favouriteBar);
        }

        // GET: FavouriteBars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favouriteBar = await _context.FavouriteBars.FindAsync(id);
            if (favouriteBar == null)
            {
                return NotFound();
            }
            ViewData["AddedId"] = new SelectList(_context.Bars, "Id", "Name", favouriteBar.AddedId);
            ViewData["AddedById"] = new SelectList(_context.Clients, "Id", "Name", favouriteBar.AddedById);
            return View(favouriteBar);
        }

        // POST: FavouriteBars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AddedById,AddedId")] FavouriteBar favouriteBar)
        {
            if (id != favouriteBar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favouriteBar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavouriteBarExists(favouriteBar.Id))
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
            ViewData["AddedId"] = new SelectList(_context.Bars, "Id", "Name", favouriteBar.AddedId);
            ViewData["AddedById"] = new SelectList(_context.Clients, "Id", "Name", favouriteBar.AddedById);
            return View(favouriteBar);
        }

        // GET: FavouriteBars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favouriteBar = await _context.FavouriteBars
                .Include(f => f.Added)
                .Include(f => f.AddedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favouriteBar == null)
            {
                return NotFound();
            }

            return View(favouriteBar);
        }

        // POST: FavouriteBars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favouriteBar = await _context.FavouriteBars.FindAsync(id);
            if (favouriteBar != null)
            {
                _context.FavouriteBars.Remove(favouriteBar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavouriteBarExists(int id)
        {
            return _context.FavouriteBars.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToFavourites(int barId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == currentUser.Id);

            if (client == null)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var favouriteBar = new FavouriteBar
            {
                AddedId = barId,
                AddedById = client.Id
            };

            _context.Add(favouriteBar);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Bar added to favourites!";
            return RedirectToAction("Details", "Bars", new { id = barId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromFavourites(int barId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == currentUser.Id);

            if (client == null)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var favouriteBar = await _context.FavouriteBars
                .FirstOrDefaultAsync(fb => fb.AddedId == barId && fb.AddedById == client.Id);

            if (favouriteBar != null)
            {
                _context.FavouriteBars.Remove(favouriteBar);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Bar removed from favourites!";
            }

            return RedirectToAction("Details", "Bars", new { id = barId });
        }
    }
}

