using InternetMarket.Models.DbModels;
using InternetMarket.Models.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InternetMarket.Interfaces.IService;

namespace InternetMarket.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConnectionParamsService _connectionParamsService;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConnectionParamsService connectionParamsService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _connectionParamsService = connectionParamsService;
        }
       

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            _connectionParamsService.AddFromContext(HttpContext, "Account/Register");
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViweModel registerViweModel)
        {
            if (ModelState.IsValid)
            {
                _connectionParamsService.AddFromContext(HttpContext, "Account/Register");
                var user = new ApplicationUser
                {
                    UserName = registerViweModel.Email,
                    Email = registerViweModel.Email
                };
                var result = _userManager.CreateAsync(user, registerViweModel.Password);
                if (result.Result.Succeeded)
                {
                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Admin");
                    }
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "Home");
                }
                foreach (var er in result.Result.Errors)
                {
                    ModelState.AddModelError(string.Empty, er.Description);
                }
            }
            return View(registerViweModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            _connectionParamsService.AddFromContext(HttpContext, "Account/Login");
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LogginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
                if(user is null)
                    ModelState.AddModelError(string.Empty, "Invalid Log In Attempt");

                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);
               
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Log In Attempt");
                }
            }
            return View(loginViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Logout() 
        {
            _connectionParamsService.AddFromContext(HttpContext, "Account/Logout");
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailInUse(string email)
        {
            _connectionParamsService.AddFromContext(HttpContext, "Account/EmailInUse");
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already use");
            }
        }

    }
}
