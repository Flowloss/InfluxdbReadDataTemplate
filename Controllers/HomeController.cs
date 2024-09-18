using InfluxDBTest.Services;
using InfluxDBTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InfluxDBTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index([FromServices] InfluxDBService service)
        {
            var results = await service.QueryAsync(async query =>
            {
                var flux = "from(bucket:\"Bucket name here \") |> range(start: 0)";
                var tables = await query.QueryAsync(flux, "org name here");
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
