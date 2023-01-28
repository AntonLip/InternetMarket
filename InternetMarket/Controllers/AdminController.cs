using InternetMarket.Interfaces.IService;
using InternetMarket.Models;
using InternetMarket.Models.DbModels;
using InternetMarket.Models.ViewModels.AdminViewModels;
using InternetMarket.Models.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace InternetMarket.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManagerQ;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;
        public AdminController(RoleManager<IdentityRole> roleManager, IShoppingCartService shoppingCartService,
                               UserManager<ApplicationUser> userManager, IProductService productService)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _roleManagerQ = roleManager;
            this._userManager = userManager;
        }
        
        [HttpGet]
        public IActionResult ListUsers()
        {            
            var users = _userManager.Users;
            return View(users);
        }
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManagerQ.Roles;
            return View(roles);
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel createRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = createRoleViewModel.RoleManager
                };
                var result = await _roleManagerQ.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(createRoleViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditRoles(string id)
        {
            var role = await _roleManagerQ.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $" Role with Id = {id} don't exist";
                return View("Error");
            }
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRoles(EditRoleViewModel viewModel)
        {
            var role = await _roleManagerQ.FindByIdAsync(viewModel.Id.ToString());
            if (role == null)
            {
                ViewBag.ErrorMessage = $" Role with Id = {viewModel.Id.ToString()} don't exist";
                return View("Error");
            }
            else
            {
                role.Name = viewModel.RoleName;
                var result = await _roleManagerQ.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                return View(viewModel);

            }
        }
        
        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string roleId)
        {
            ViewBag.Title = roleId;
            var role = await _roleManagerQ.FindByIdAsync(roleId);
            var model = new List<UserRoleViewModel>();
            if (role == null)
            {
                ViewBag.ErrorMessage = $" Role with Id = {roleId} don't exist";
                return View("Error");
            }
            else
            {
                foreach (var user in _userManager.Users.ToList())
                {
                    var userRoleViewModel = new UserRoleViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        userRoleViewModel.IsSelected = true;
                    }
                    else
                    {
                        userRoleViewModel.IsSelected = false;
                    }
                    model.Add(userRoleViewModel);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManagerQ.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {roleId} don't exist";
                return View("Error");
            }
            else
            {
                for (int i = 0; i < model.Count; i++)
                {
                    var user = await _userManager.FindByIdAsync(model[i].UserId);

                    IdentityResult result = null;
                    if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                    {
                        result = await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    else if (!model[i].IsSelected && (await _userManager.IsInRoleAsync(user, role.Name)))
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }
                    if (result.Succeeded)
                    {
                        if (i < (model.Count - 1))
                            continue;
                        else
                            return RedirectToAction("EditRoles", new { Id = roleId });
                    }
                }
            }
            return RedirectToAction("EditRoles", new { Id = roleId }); ;
        }
        [HttpGet]

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} not fount";
                return View("Error");
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);
            var model = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} not fount";
                return View("Error");
            }
            else
            {
                user.UserName = model.UserName;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} not fount";
                return View("Error");
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListUsers");
            }

        }
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"user with Id = {id} not fount";
                return View("Error");
            }
            var model = new List<EditRolesInUser>();
            foreach (var role in _roleManagerQ.Roles)
            {
                model.Add(new EditRolesInUser
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                });
            }
            ViewBag.userId = user.Id;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<EditRolesInUser> editRolesInUsers, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"user with Id = {id} not fount";
                return View("Error");
            }
            var role = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, role);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove roles");
                return View(editRolesInUsers);
            }
            result = await _userManager.AddToRolesAsync(user, editRolesInUsers.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add roles");
                return View(editRolesInUsers);
            }
            return RedirectToAction("EditUser", new { Id = id });

        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManagerQ.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} not fount";
                return View("Error");
            }
            else
            {
                try
                {
                    var result = await _roleManagerQ.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("ListRoles");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorTitle = $"{role.Name} role in use";
                    ViewBag.TitleMessage = ex.Message + "\r\n" +
                        $"{role.Name} role have some users. If you want to  delete this roe I should delete users";
                    return View("Error");
                }
            }

        }
        [HttpGet]
        public async Task<IActionResult> ListUsersProducts(string id)
        {            
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                var shpcart = _shoppingCartService.GetUserShoppingCarts(user.Id, true);
                if (shpcart is null)
                    return NotFound();
                UsersProductsViewModel model = new UsersProductsViewModel();
                model.UserId = user.Id;
                model.UserName = user.UserName;
                model.Products = new List<ShoppingCartDto>();
                foreach (var s in shpcart)
                {
                    var pr = _productService.GetById(s.ProductId);
                    model.TotalCost += s.Cost;
                    model.Products.Add(
                        new ShoppingCartDto
                        {
                            BuyDate = s.BuyDate.Date.ToShortDateString(),
                            Cost = s.Cost,
                            Count = s.Count,
                            Id = s.Id,
                            ProductName = pr.Name,
                            ArticleNumber = pr.ArticleNumber
                        }
                    );
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ErrorViewModel errorViewModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Messge = ex.Message
                };

                return RedirectToAction("Error", "Home", errorViewModel);
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetReportAsync(string id)
        {          

            try
            {
                var user = await _userManager.FindByIdAsync(id);
                var shpcart = _shoppingCartService.GetUserShoppingCarts(user.Id, true);
                if (shpcart is null)
                    return NotFound();
                UsersProductsViewModel model = new UsersProductsViewModel();
                model.UserId = user.Id;
                model.UserName = user.UserName;
                model.Products = new List<ShoppingCartDto>();
                foreach (var s in shpcart)
                {
                    var pr = _productService.GetById(s.ProductId);
                    model.TotalCost += s.Cost;
                    model.Products.Add(
                        new ShoppingCartDto
                        {
                            BuyDate = s.BuyDate.Date.ToShortDateString(),
                            Cost = s.Cost,
                            Count = s.Count,
                            Id = s.Id,
                            ProductName = pr.Name,
                            ArticleNumber = pr.ArticleNumber
                        }
                    );
                }

                var fileDto =  _productService.GetReport(model);
                if (fileDto is null)
                    throw new ArgumentException();

                return File(fileDto.FileData, fileDto.FileType, fileDto.FileName);
            }
            catch (Exception ex)
            {
                ErrorViewModel errorViewModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Messge = ex.Message
                };

                return RedirectToAction("Error", "Home", errorViewModel);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenid()
        {
            return View();
        }
    }
}
