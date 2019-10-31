﻿using System;
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
        private readonly SearchService _searchService;

        public NomicController(ILogger<NomicController> logger, SearchService searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }
        public async Task<IActionResult> Index()
        {
            var list=await _searchService.GetNomics(1);
            ViewBag.Nomics = list;
            return View();
        }
        public async Task<IActionResult> Detail(string  id)
        {
            var list = await _searchService.GetcaomicCatalog(id);
            ViewBag.Nomics = list;
            return View();
        }
    }
}