using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spoondrift.Web.Models;
using Spoondrift.Web.Tasks;

namespace Spoondrift.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NomicTaskService _nomicTaskService;

        public HomeController(ILogger<HomeController> logger, NomicTaskService nomicTaskService)
        {
            _logger = logger;
            _nomicTaskService = nomicTaskService;
        }

        public IActionResult Index()
        {
            RecurringJob.AddOrUpdate(() => _nomicTaskService.GetNomicToDb(), Cron.MinuteInterval(10));
            return View();
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
