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
        private readonly UserManager<ApplicationUser> userManagerQ;
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;
        public AdminController(RoleManager<IdentityRole> roleManager, IShoppingCartService shoppingCartService,
                               UserManager<ApplicationUser> userManager, IProductService productService)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _roleManagerQ = roleManager;
            userManagerQ = userManager;
        }
        
        [HttpGet]
        public IActionResult ListUsers()
        {            
            var users = userManagerQ.Users;
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
        public async Task<IActionResult> EditRoles(string Id)
        {
            var role = await _roleManagerQ.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $" Role with Id = {Id} don't exist";
                return View("NotFound");
            }
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            foreach (var user in userManagerQ.Users.ToList())
            {
                if (await userManagerQ.IsInRoleAsync(user, role.Name))
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
                return View("NotFound");
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
                return View("NotFound");
            }
            else
            {
                foreach (var user in userManagerQ.Users.ToList())
                {
                    var userRoleViewModel = new UserRoleViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };
                    if (await userManagerQ.IsInRoleAsync(user, role.Name))
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
                return View("NotFound");
            }
            else
            {
                for (int i = 0; i < model.Count; i++)
                {
                    var user = await userManagerQ.FindByIdAsync(model[i].UserId);

                    IdentityResult result = null;
                    if (model[i].IsSelected && !(await userManagerQ.IsInRoleAsync(user, role.Name)))
                    {
                        result = await userManagerQ.AddToRoleAsync(user, role.Name);
                    }
                    else if (!model[i].IsSelected && (await userManagerQ.IsInRoleAsync(user, role.Name)))
                    {
                        result = await userManagerQ.RemoveFromRoleAsync(user, role.Name);
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

        public async Task<IActionResult> EditUser(string Id)
        {
            var user = await userManagerQ.FindByIdAsync(Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {Id} not fount";
                return View("NotFound");
            }
            var userRoles = await userManagerQ.GetRolesAsync(user);
            var userClaims = await userManagerQ.GetClaimsAsync(user);
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
            var user = await userManagerQ.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} not fount";
                return View("NotFound");
            }
            else
            {
                user.UserName = model.UserName;
                user.Email = model.Email;

                var result = await userManagerQ.UpdateAsync(user);
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

        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await userManagerQ.FindByIdAsync(Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {Id} not fount";
                return View("Error/NotFound");
            }
            else
            {
                var result = await userManagerQ.DeleteAsync(user);
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
        public async Task<IActionResult> ManageUserRoles(string Id)
        {
            var user = await userManagerQ.FindByIdAsync(Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"user with Id = {Id} not fount";
                return View("Error/NotFound");
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
        public async Task<IActionResult> ManageUserRoles(List<EditRolesInUser> editRolesInUsers, string Id)
        {
            var user = await userManagerQ.FindByIdAsync(Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"user with Id = {Id} not fount";
                return View("Error/NotFound");
            }
            var role = await userManagerQ.GetRolesAsync(user);
            var result = await userManagerQ.RemoveFromRolesAsync(user, role);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove roles");
                return View(editRolesInUsers);
            }
            result = await userManagerQ.AddToRolesAsync(user, editRolesInUsers.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add roles");
                return View(editRolesInUsers);
            }
            return RedirectToAction("EditUser", new { Id = Id });

        }

        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await _roleManagerQ.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {Id} not fount";
                return View("Error/NotFound");
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
                var user = await userManagerQ.FindByIdAsync(id);
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

                return RedirectToAction("MyError", errorViewModel);
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetReportAsync(string Id)
        {          

            try
            {
                var user = await userManagerQ.FindByIdAsync(Id);
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

                return RedirectToAction("MyError", errorViewModel);
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
