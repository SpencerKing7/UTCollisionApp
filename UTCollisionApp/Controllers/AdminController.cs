using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UTCollisionApp.Models;

namespace UTCollisionApp.Controllers
{
    public class AdminController : Controller
    {
        private ICollisionRepository _repo { get; set; }

        //Constructor
        public AdminController(ICollisionRepository temp)
        {
            _repo = temp;
        }

        public IActionResult AdminHome()
        {
            ViewBag.Button = "Sign Out";
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";

            return View();
        }

        [HttpGet]
        public IActionResult AddCrashForm()
        {
            ViewBag.Button = "Sign Out";
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";

            var x = _repo.Locations
                
                .ToList();

            return View(x);
        }

        //[HttpPost]
        //public IActionResult AddCrashForm()
        //{
        //    return RedirectToAction("AdminHome");
        //}

        public IActionResult CrashTable()
        {
            ViewBag.Button = "Sign Out";
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";

            return View();
        }
    }
}
