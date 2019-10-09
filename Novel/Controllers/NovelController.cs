using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Novel.Common.DB;

namespace Novel.Controllers
{
    [Authorize]
    public class NovelController : Controller
    {
        private readonly NovelDBContext _novelDBContext;
        public NovelController(NovelDBContext novelDBContext)
        {
            _novelDBContext = novelDBContext;
        }
        public IActionResult Index()
        {
            var bookShelves = _novelDBContext.BookShelve.ToList();
            ViewBag.BookShelves = bookShelves;
            return View();
        }
    }
}