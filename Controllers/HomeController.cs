using InfluxDBTest.Services;
using InfluxDBTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InfluxDBTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly InfluxDBService _service;

        public HomeController(ILogger<HomeController> logger, InfluxDBService service)
        {
            _logger = logger;
            _service = service;
        }

      
        public async Task<IActionResult> Index()
        {
            var results = await _service.QueryAsync(async query =>
            {
                var flux = "from(bucket:\"Add bucket name here\") |> range(start: -1h)";
                var tables = await query.QueryAsync(flux, "Add org here");
                return tables.SelectMany(table =>
                    table.Records.Select(record =>
                        new CounterModel
                        {
                            Time = record.GetTime().ToString(),
                            Counter = int.Parse(record.GetValue().ToString())
                        }));
            });

            return View(results);
        }

        [HttpGet("Home/GetLatestData")]
        public async Task<IActionResult> GetLatestData()
        {
            var results = await _service.QueryAsync(async query =>
            {
                var flux = "from(bucket:\"Add bucket name here\") |> range(start: -1h)";
                var tables = await query.QueryAsync(flux, "Add org here");
                return tables.SelectMany(table =>
                    table.Records.Select(record =>
                        new CounterModel
                        {
                            Time = record.GetTime().ToString(),
                            Counter = int.Parse(record.GetValue().ToString())
                        }));
            });

            return Json(results);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}