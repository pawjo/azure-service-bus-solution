using CreationApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using ValidationApp.Models;

namespace ValidationApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IReportService _reportService;

        public HomeController(ILogger<HomeController> logger, IReportService reportService)
        {
            _logger = logger;
            _reportService = reportService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var vm = await _reportService.GetListAsync();
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
