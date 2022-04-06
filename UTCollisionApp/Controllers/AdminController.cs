using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UTCollisionApp.Models;
using UTCollisionApp.Models.ViewModels;

namespace UTCollisionApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private ICollisionRepository _repo { get; set; }

        //Constructor
        public AdminController(ICollisionRepository temp, UserManager<IdentityUser> um, SignInManager<IdentityUser> sim, RoleManager<IdentityRole> rm)
        {
            this.roleManager = rm;
            this.userManager = um;
            this.signInManager = sim;
            _repo = temp;
        }

        public IActionResult AdminHome()
        {
            //Button Viewbags
            ViewBag.Button = "Sign Out";
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";

            return View();
        }

        [HttpGet]
        public IActionResult AddCrashForm()
        {
            //Button Viewbags
            ViewBag.Button = "Sign Out";
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";

            //County Option Viewbag
            ViewBag.Counties = _repo.Locations
                .Select(x => x.COUNTY_NAME)
                .Distinct()
                .ToList();

            //IDs Viewbags
            ViewBag.CrashID = _repo.Crashes.Max(x => x.CRASH_ID) + 1;
            ViewBag.LocationID = _repo.Locations.Max(x => x.LOCATION_ID) + 1;
            ViewBag.FactorID = _repo.Factors.Max(x => x.FACTOR_ID) + 1;

            return View(new Crash());
        }

        [HttpPost]
        public IActionResult AddCrashForm(Crash c)
        {
            _repo.CreateCrash(c);

            return RedirectToAction("AdminHome");
        }

        public IActionResult CrashTable(string county, int pageNum = 1)
        {
            //Button Viewbags
            ViewBag.Button = "Sign Out";
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";

            //Pagination and Table Data
            int pageSize = 25;

            var x = new CrashViewModel
            {
                Crashes = _repo.Crashes
                .Include(x => x.Location)
                .Include(x => x.Factor)
                .Where(x => x.Location.COUNTY_NAME == county || county == null)
                .OrderByDescending(c => c.CRASH_DATETIME)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                PageInfo = new PageInfo
                {
                    TotalNumCrashes = 
                        (county == null
                        ? _repo.Crashes.Count()
                        : _repo.Crashes.Where(x => x.Location.COUNTY_NAME == county).Count()),
                    CrashesPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };

            return View(x);
        }

        [HttpGet]
        public IActionResult EditCrashForm(int CRASH_ID)
        {
            //Button Viewbags
            ViewBag.Button = "Sign Out";
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";

            //County Option Viewbag
            ViewBag.Counties = _repo.Locations
                .Select(x => x.COUNTY_NAME)
                .Distinct()
                .ToList();

            var crash = _repo.Crashes
                .Include(x => x.Location)
                .Include(x => x.Factor)
                .Single(x => x.CRASH_ID == CRASH_ID);

            return View("EditCrashForm", crash);
        }

        [HttpPost]
        public IActionResult EditCrashForm(Crash c)
        {
            _repo.SaveCrash(c);

            return RedirectToAction("CrashTable");
        }

        [HttpGet]
        public IActionResult DeleteCrash(int CRASH_ID)
        {
            var crash = _repo.Crashes.Single(x => x.CRASH_ID == CRASH_ID);

            return View("DeleteCrash", crash);
        }

        [HttpPost]
        public IActionResult DeleteCrash(Crash c)
        {
            var crash = _repo.Crashes.Single(x => x.CRASH_ID == c.CRASH_ID);

            _repo.DeleteCrash(crash);

            return RedirectToAction("CrashTable");
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if(ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if(result.Succeeded)
                {
                    return RedirectToAction("AdminHome", "Admin");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            

            return View(model);
        }

        

        

        public IActionResult Login (string returnUrl)
        {
            //Button Viewbags
            ViewBag.Button = "Sign Out";
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index"; 

            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login (LoginModel loginModel)
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
                        return Redirect(loginModel?.ReturnUrl ?? "/Admin/AdminHome");
                    }
                }

                
            }
            ModelState.AddModelError("", "Invalid Name or Password");
            return View(loginModel);
        }






    }

}
