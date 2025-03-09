using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KyivBarGuideInfrastructure.Models.DTOS;

namespace KyivBarGuideInfrastructure.Controllers
{
    public class ChartsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ChartsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Метод для відображення сторінки з графіками
        public async Task<IActionResult> Index()
        {
            // Отримуємо дані для графіка "кількість барів з фотографією та без"
            var barsWithPhotoResponse = await _httpClient.GetAsync("http://localhost:61668/api/chart/bars-with-photo");
            var barsWithPhotoData = await barsWithPhotoResponse.Content.ReadFromJsonAsync<List<BarCategoryStat>>();

            // Отримуємо дані для графіка "кількість барів за тематикою"
            var barsByThemeResponse = await _httpClient.GetAsync("http://localhost:61668/api/chart/bars-by-theme");
            var barsByThemeData = await barsByThemeResponse.Content.ReadFromJsonAsync<List<BarCategoryStat>>();

            // Передаємо дані у View
            ViewBag.BarsWithPhotoData = barsWithPhotoData;
            ViewBag.BarsByThemeData = barsByThemeData;

            return View();
        }
    }
}