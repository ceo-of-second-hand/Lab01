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
    public class ChartsAPIController : ControllerBase
    {
        private readonly KyivBarGuideContext _context;

        public ChartsAPIController(KyivBarGuideContext context)
        {
            _context = context;
        }

        [HttpGet("bars-with-photo")]
        public IActionResult GetBarsWithPhoto()
        {
            var data = _context.Bars
                .GroupBy(b => b.Picture != null) 
                .Select(g => new BarCategoryStat //creating new object with corresponding theme
                {
                    Category = g.Key ? "With Photo" : "Without Photo", 
                    Count = g.Count() 
                })
                .ToList();

            return Ok(data);
        }

        [HttpGet("bars-by-theme")]
        public IActionResult GetBarsByTheme()
        {
            var data = _context.Bars
                .GroupBy(b => b.Theme) 
                .Select(g => new BarCategoryStat
                {
                    Category = g.Key ?? "No Theme",
                    Count = g.Count() 
                })
                .ToList();

            return Ok(data);
        }
    }
}