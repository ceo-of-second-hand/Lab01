using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KyivBarGuideInfrastructure.Models.DTOS;

namespace KyivBarGuideInfrastructure.Controllers
{
    public class ChartsController : Controller
    {
        //THINK OF REPLACING IT TO PROGRAM.CS OR Move API calls from the controller into a dedicated service
        private readonly HttpClient _httpClient; //class used to send HTTP requests and receive HTTP responses from web services.

        public ChartsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //for displaying charts 
        public async Task<IActionResult> Index()
        {
            var barsWithPhotoResponse = await _httpClient.GetAsync("http://localhost:61668/api/chart/bars-with-photo");
            var barsWithPhotoData = await barsWithPhotoResponse.Content.ReadFromJsonAsync<List<BarCategoryStat>>();

            var barsByThemeResponse = await _httpClient.GetAsync("http://localhost:61668/api/chart/bars-by-theme");
            var barsByThemeData = await barsByThemeResponse.Content.ReadFromJsonAsync<List<BarCategoryStat>>();

            // transefing data to view
            ViewBag.BarsWithPhotoData = barsWithPhotoData; //BarsWithPhotoData - dynamic property of ViewBag (enabling usage of them in cshtml)
            ViewBag.BarsByThemeData = barsByThemeData;

            return View();
        }
    }
}