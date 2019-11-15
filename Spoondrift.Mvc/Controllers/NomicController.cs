using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Novel.Common.Services;

namespace Spoondrift.Mvc.Controllers
{
    public class NomicController : Controller
    {
        private readonly ILogger<NomicController> _logger;
        private readonly WjsNomicService _searchService;

        public NomicController(ILogger<NomicController> logger, WjsNomicService searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }
        public async Task<IActionResult> Index(string title = "最新上传", int pageIndex = 1)
        {
            var banners = await _searchService.GetBanners();
            var nomics = await _searchService.GetNomics(title,pageIndex);
            if (nomics != null)
            {
                nomics.ForEach(a =>
                {
                    a.Url = "";
                });
            }
            ViewBag.Nomics = nomics;
            ViewBag.Banners = banners;
            ViewBag.SelectBanner = title;
            ViewBag.pageIndex = pageIndex;
            return View();
        }
        public async Task<IActionResult> Detail(string titleType,int pageIndex,string title)
        {
            var key = $"{titleType}_list_{pageIndex}";
            var list = await _searchService.GetcaomicCatalog(key,title);
            if (list != null)
            {
                list.ForEach(a => { a.Url = "";a.Title = a.Title.Trim(); });
            }
            ViewBag.Catalogs = list;
            ViewBag.NomicTitle = title;
            return View();
        }
        public async Task<IActionResult> NomicContent(string title, string catalog)
        {
            var nomicContent = await _searchService.NomicContent(title, catalog);
            ViewBag.NomicContent = nomicContent;
            return View();
        }
    }
}