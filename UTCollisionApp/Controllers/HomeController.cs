using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UTCollisionApp.Models;
using UTCollisionApp.Models.ViewModels;

namespace UTCollisionApp.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private ICollisionRepository _repo { get; set; }

        //Constructor
        public HomeController(ICollisionRepository temp, UserManager<IdentityUser> um, SignInManager<IdentityUser> sim)
        {
            userManager = um;
            signInManager = sim;
            _repo = temp;
        }

        public IActionResult Index()
        {
            //Button Viewbags
            ViewBag.Button = "Admin Sign In";
            ViewBag.Controller = "Admin";
            ViewBag.Action = "Login";

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

        public IActionResult Statistics()
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
            //Button Viewbags
            ViewBag.Button = "Admin Sign In";
            ViewBag.Controller = "Admin";
            ViewBag.Action = "Login";

            return View();
        }

        public IActionResult Login(string returnUrl)
        {
            //Button Viewbags
            ViewBag.Button = "Sign Out";
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";

            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            //Button Viewbags
            ViewBag.Button = "Sign Out";
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";

            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync(loginModel.Username);

                if (user != null)
                {
                    await signInManager.SignOutAsync();

                    if ((await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                    {
                        return Redirect(loginModel?.ReturnUrl ?? "/");
                    }
                }


            }
            ModelState.AddModelError("", "Invalid Name or Password");
            return View(loginModel);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            //Button Viewbags
            ViewBag.Button = "Sign Out";
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser (CreateUserViewModel model)
        {
            //Button Viewbags
            ViewBag.Button = "Sign Out";
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";

            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.UserName,  Email = model.Email};
                var result = await userManager.CreateAsync(user, model.Password);

                if(result.Succeeded)
                {
                    // If the user is signed in and in the Admin role, then it is
                    // the Admin user that is creating a new user. So redirect the
                    // Admin user to ListRoles action
                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Admin");
                    }

                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");

        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
