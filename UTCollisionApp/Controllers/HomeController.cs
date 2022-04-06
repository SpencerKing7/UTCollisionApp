using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UTCollisionApp.Models;

namespace UTCollisionApp.Controllers
{
    public class HomeController : Controller
    {
        private ICollisionRepository _repo { get; set; }

        //Constructor
        public HomeController(ICollisionRepository temp)
        {
            _repo = temp;
        }

        public IActionResult Index()
        {
            //Button Viewbags
            ViewBag.Button = "Admin Sign In";
            ViewBag.Controller = "Admin";
            ViewBag.Action = "AdminHome";

            //Stats Viewbags
            ViewBag.Deaths = _repo.Crashes
                .Where(x => x.CRASH_SEVERITY_ID == 5 && x.CRASH_DATETIME.ToString().Contains("2019"))
                .Count();
            ViewBag.Injuries = _repo.Crashes
                .Where(x => (x.CRASH_SEVERITY_ID == 2 || x.CRASH_SEVERITY_ID == 3 || x.CRASH_SEVERITY_ID == 4) && x.CRASH_DATETIME.ToString().Contains("2019"))
                .Count();
            ViewBag.Accidents = _repo.Crashes
                .Where(x => x.CRASH_DATETIME.ToString().Contains("2019"))
                .Count();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
