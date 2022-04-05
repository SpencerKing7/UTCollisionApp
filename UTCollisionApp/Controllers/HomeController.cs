using Microsoft.AspNetCore.Mvc;
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
            ViewBag.Button = "Admin Sign In";
            ViewBag.Controller = "Admin";
            ViewBag.Action = "AdminHome";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
