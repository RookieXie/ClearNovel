using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Novel.Common.Models;
using Novel.Common.Services;

namespace Novel.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SearchService _searchService; 
        private readonly TaskGetService  _taskGetService;

        public HomeController(ILogger<HomeController> logger, SearchService searchService, TaskGetService taskGetService)
        {
            _logger = logger;
            _searchService = searchService;
            _taskGetService = taskGetService;
        }

        public async Task<IActionResult> Index(string searchName)
        {
            var res = new SearchResult();
            if (!string.IsNullOrEmpty(searchName))
            {
                 res = await _searchService.SearchNovel(searchName);
            }
            ViewBag.SearchName = searchName;
            ViewBag.SearchResult = res;
            Task task = new Task(()=>
            {
                if (res.Contents != null && res.Contents.Count > 0)
                {
                    _taskGetService.GetNovels(res.Contents);
                }
            });
            task.Start();
            return View();
        }

        public async Task<IActionResult> Catalog(string url)
        {
            List<Catalog> catalogs = await _searchService.GetCatalog(url);
            ViewBag.Catalogs = catalogs;
            return View();

        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult BookShelf()
        {
            
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new  { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<JsonResult> Getsearch()
        {
            var res = await _searchService.SearchNovel("裁判");
            return Json(res);
        }
        public async Task<JsonResult> GetCatalog()
        {
            var res = await _searchService.GetCatalog("https://www.biquge5200.cc/1_1477/");
            return Json(res);
        }
        public async Task<JsonResult> GetChapterContent()
        {
            var res = await _searchService.GetChapterContent("https://www.biquge5200.cc/1_1477/1145666.html");
            return Json(res);
        }
    }
}
