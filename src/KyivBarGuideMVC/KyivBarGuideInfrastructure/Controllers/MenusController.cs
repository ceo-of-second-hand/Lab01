using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KyivBarGuideDomain.Model;
using Microsoft.AspNetCore.Hosting;
using System;

namespace KyivBarGuideInfrastructure.Controllers;

public class MenusController : Controller
{
    private readonly KyivBarGuideContext _context;
    private readonly IWebHostEnvironment _environment; //added for safer photo handling (in bars controller another approach is used)
    public MenusController(KyivBarGuideContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment; //getting environment to know the path to the wwwroot folder
    }

    [HttpGet]
    public IActionResult Index()
    {
        var bars = _context.Bars.ToList();
        return View(bars);
    }

    [HttpGet]
    public async Task<IActionResult> GetCocktailsForBar(int barId)
    {
        if (barId <= 0)
            return BadRequest("Bar ID is invalid");

        var cocktails = await _context.Cocktails
            .Include(c => c.Proportions)
            .Where(c => c.SellsIn == barId)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Price,
                c.Picture,
                Ingredients = c.Proportions.Select(p => new
                {
                    IngredientName = p.AmountOf.Name,
                    p.Amount
                })
            })
            .ToListAsync();

        return Json(cocktails);
    }

    [HttpGet]
    public async Task<IActionResult> SearchIngredients(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return Json(new List<string>());

        var matches = await _context.Ingredients
            .Where(i => i.Name.Contains(term))
            .Select(i => i.Name)
            .ToListAsync();

        return Json(matches);
    }

    [HttpPost]
    public async Task<IActionResult> AddCocktail(int barId, string name, decimal price, List<ProportionDto> proportions, IFormFile? picture)
    {
        if (barId <= 0 || string.IsNullOrWhiteSpace(name) || price <= 0 || proportions == null || !proportions.Any())
        {
            return BadRequest("Invalid cocktail data");
        }

        var bar = await _context.Bars.FindAsync(barId);
        if (bar == null)
        {
            return NotFound("Bar not found");
        }

        string? picturePath = null;
        if (picture != null && picture.Length > 0)
        {
            var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(picture.FileName).ToLower();

            if (!permittedExtensions.Contains(extension))
            {
                return BadRequest("Invalid file type. Only images are allowed.");
            }

            string uploadsFolder = Path.Combine(_environment.WebRootPath, "images"); //getting path to wwwroot/images
            Directory.CreateDirectory(uploadsFolder); //creating folder if it doesn't exist
            string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(picture.FileName)}"; //unique file name
            picturePath = Path.Combine("images", uniqueFileName); //relative path for saving in DB

            using (var fileStream = new FileStream(Path.Combine(uploadsFolder, uniqueFileName), FileMode.Create))
            {
                await picture.CopyToAsync(fileStream);
            }
        }

        var cocktail = new Cocktail
        {
            Name = name,
            Price = price,
            SellsIn = bar.Id,
            SellsInNavigation = bar,
            Picture = picturePath,
        };

        foreach (var prop in proportions)
        {
            //might be useful to switch to B-tree approach (O(N)->O(log N))
            var ingredient = await _context.Ingredients
                .FirstOrDefaultAsync(i => i.Name.ToLower() == prop.IngredientName.ToLower());

            if (ingredient == null)
            {
                ingredient = new Ingredient { Name = prop.IngredientName };
                _context.Ingredients.Add(ingredient);
                await _context.SaveChangesAsync();
            }

            var proportion = new Proportion
            {
                Amount = prop.Amount,
                AmountIn = cocktail,
                AmountOf = ingredient,
                SetById = null // TEMP (TO BE CHANGED LATER)
            };

            cocktail.Proportions.Add(proportion);
        }

        _context.Cocktails.Add(cocktail);
        await _context.SaveChangesAsync();

        return Ok("Cocktail added successfully");
    }

    // Видалення коктейлю
    [HttpPost]
    public async Task<IActionResult> DeleteCocktail(int cocktailId)
    {
        var cocktail = await _context.Cocktails
            .Include(c => c.Proportions)
            .FirstOrDefaultAsync(c => c.Id == cocktailId);

        if (cocktail == null)
        {
            return NotFound("Cocktail not found");
        }

        // Видаляємо пропорції (щоб не залишились "висячі" пропорції)
        _context.Proportions.RemoveRange(cocktail.Proportions);
        _context.Cocktails.Remove(cocktail);

        await _context.SaveChangesAsync();
        return Ok("Cocktail deleted");
    }
}

// DTO для пропорцій
public class ProportionDto
{
    public string IngredientName { get; set; } = null!;
    public string Amount { get; set; } = null!;
}
