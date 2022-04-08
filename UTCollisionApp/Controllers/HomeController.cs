using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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

        public IActionResult AccidentTable(string severity, string counties, int pageNum = 1)
        {
            //Button Viewbags
            ViewBag.Button = "Sign Out";
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";

            ViewBag.County = counties;
            ViewBag.Severity = severity;

            //Pagination and Table Data
            int pageSize = 15;

            var x = new CrashViewModel
            {
                Crashes = _repo.Crashes
                .Include(x => x.Location)
                .Include(x => x.Factor)
                .Where(x => (x.Location.COUNTY_NAME == counties || counties == null) && (x.CRASH_SEVERITY_ID.ToString() == severity || severity == null)) 
                .OrderByDescending(c => c.CRASH_DATETIME)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                PageInfo = new PageInfo
                {
                    TotalNumCrashes =
                        (counties == null
                        ? _repo.Crashes.Count()
                        : _repo.Crashes.Where(x => x.Location.COUNTY_NAME == counties).Count()),
                    CrashesPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };

            return View(x);
        }

        //View details about a specific crash
        public IActionResult Details(int CRASH_ID)
        {
            var crash = _repo.Crashes
                .Include(x => x.Location)
                .Include(x => x.Factor)
                .Single(x => x.CRASH_ID == CRASH_ID);



            return View("Details", crash);
        }

        public IActionResult Privacy()
        {
            //Button Viewbags
            ViewBag.Button = "Admin Sign In";
            ViewBag.Controller = "Admin";
            ViewBag.Action = "Login";

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginModel model = new LoginModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            model.ExternalLogins =
                (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null && !user.EmailConfirmed &&
                            (await userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(model.Email,
                                        model.Password, false, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(model);
        }
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Home",
                new { ReturnUrl = returnUrl });

            var properties = signInManager
                .ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

         public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
            {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginModel loginViewModel = new LoginModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
                        (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState
                    .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }

            // Get the login information about the user from the external login provider
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState
                    .AddModelError(string.Empty, "Error loading external login information.");

                return View("Login", loginViewModel);
            }

            // If the user already has a login (i.e if there is a record in AspNetUserLogins
            // table) then sign-in the user with this external login provider
            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: false);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            // If there is no record in AspNetUserLogins table, the user may not have
            // a local account
            else
            {
                // Get the email claim value
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    // Create a new user without password if we do not have a user already
                    var user = await userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new IdentityUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await userManager.CreateAsync(user);
                    }

                    // Add a login (i.e insert a row for the user in AspNetUserLogins table)
                    await userManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }


                return View("Login");
            }
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
