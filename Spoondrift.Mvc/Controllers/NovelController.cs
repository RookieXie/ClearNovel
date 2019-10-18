using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Novel.Common.Models;
using Novel.Common.Services;

namespace Spoondrift.Mvc.Controllers
{
    public class NovelController : Controller
    {
        private readonly ILogger<NovelController> _logger;
        private readonly NovelService _novelService;

        public NovelController(ILogger<NovelController> logger, NovelService novelService)
        {
            _logger = logger;
            _novelService = novelService;
        }
        public async Task<IActionResult> Index(int order=0)
        {
            List<NovelTab> tabs = await _novelService.GetTabs();
            List<NearlyUpdateNovel> nearlyUpdateNovels = new List<NearlyUpdateNovel>();
            if (order == 0)
            {
                nearlyUpdateNovels = await _novelService.GetNearlyUpdateList();
            }
            else
            {
                nearlyUpdateNovels = await _novelService.GetTongRenList(order);
            }
            ViewBag.NovelTabs = tabs;
            ViewBag.NearlyUpdateNovels = nearlyUpdateNovels;
            ViewBag.SelectNum = order;
            return View();
        }
    }
}