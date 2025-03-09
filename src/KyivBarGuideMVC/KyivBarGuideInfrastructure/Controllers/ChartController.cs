using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KyivBarGuideDomain.Model;
using KyivBarGuideInfrastructure;
using KyivBarGuideInfrastructure.Models.DTOS; 

namespace KyivBarGuideInfrastructure.Controllers
{
    [Route("api/chart")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly KyivBarGuideContext _context;

        public ChartController(KyivBarGuideContext context)
        {
            _context = context;
        }

        // Метод для отримання даних про кількість барів з фотографією та без фотографії
        [HttpGet("bars-with-photo")]
        public IActionResult GetBarsWithPhoto()
        {
            var data = _context.Bars
                .GroupBy(b => b.Picture != null) // Групуємо за наявністю фотографії
                .Select(g => new BarCategoryStat
                {
                    Category = g.Key ? "With Photo" : "Without Photo", // Категорія: з фото чи без
                    Count = g.Count() // Кількість барів
                })
                .ToList();

            return Ok(data);
        }

        // Метод для отримання даних про кількість барів за тематикою
        [HttpGet("bars-by-theme")]
        public IActionResult GetBarsByTheme()
        {
            var data = _context.Bars
                .GroupBy(b => b.Theme) // Групуємо за тематикою
                .Select(g => new BarCategoryStat
                {
                    Category = g.Key ?? "No Theme", // Якщо тематика відсутня, використовуємо "No Theme"
                    Count = g.Count() // Кількість барів
                })
                .ToList();

            return Ok(data);
        }
    }
}