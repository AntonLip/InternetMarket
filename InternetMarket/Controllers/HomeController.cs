using InternetMarket.Interfaces.IService;
using InternetMarket.Models;
using InternetMarket.Models.DbModels;
using InternetMarket.Models.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace InternetMarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IProductTypeService _productTypeService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConnectionParamsService _connectionParamsService;
        IWebHostEnvironment _appEnvironment;
        public HomeController(ILogger<HomeController> logger, IProductService productService,
                                IProductTypeService productTypeService, IWebHostEnvironment appEnvironment,
                                UserManager<ApplicationUser> userManager, IShoppingCartService shoppingCartService,
                                IConnectionParamsService connectionParamsService)
        {
            _logger = logger;
            _connectionParamsService = connectionParamsService;
            _productService = productService;
            _productTypeService = productTypeService;
            _userManager = userManager;
            _shoppingCartService = shoppingCartService;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            try
            {
                if (_connectionParamsService.IsBotConnection(HttpContext))
                {
                    return RedirectToAction("CheckCaptcha", "Ad");
                }
                _connectionParamsService.AddFromContext(HttpContext, "Home/Index");
                var products = _productService.GetAll();
                var productType = _productTypeService.GetAll();
                IndexViewModel model = new IndexViewModel
                {
                    Products = products,
                    ProductTypes = productType
                };
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
        [Route("Home/CreateProduct")]
        [HttpGet]
        public IActionResult CreateProduct()
        {
            try
            {
                var types = _productTypeService.GetAll();
                ViewData["Types"] = new SelectList(types, "Id", "Name");
                return View();
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

        [Route("Home/CreateProduct")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult CreateProduct(AddProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var photePath = UploadedFile(model.Photo);
                Product product = new Product
                {
                    Name = model.Name,
                    Count = model.Count,
                    Description = model.Description,
                    Coast = model.Coast,
                    ProductTypeId = model.TypeId,
                    PisturePath = photePath,
                    ArticleNumber = model.ArticleNumber
                };
                try
                {
                    _productService.Add(product);
                    return RedirectToAction("Details", new { Id = product.Id });
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
            return View();
        }

        [Route("Home/UpdateProduct")]
        [HttpGet]
        public IActionResult UpdateProduct(Guid id)
        {
            try
            {
                var types = _productTypeService.GetAll();
                var prod = _productService.GetById(id);
                UpdateProductViewModel model = new UpdateProductViewModel
                {
                    Id = prod.Id,
                    Coast = prod.Coast,
                    Count = prod.Count,
                    Description = prod.Description,
                    Name = prod.Name,
                    TypeId = prod.ProductTypeId,
                    ArticleNumber = prod.ArticleNumber
                };
                ViewData["Types"] = new SelectList(types, "Id", "Name");
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
        [Route("Home/UpdateProduct")]
        [HttpPost]
        public IActionResult UpdateProduct(UpdateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = _productService.GetById(model.Id);
                if (product is null)
                    return NotFound();
                
                if(!(model.Photo is null))
                {
                    if (product.PisturePath != null)
                    {
                        var filePath = product.PisturePath;
                        RemoveFile(filePath);
                    }
                    var photePath = UploadedFile(model.Photo);
                    product.PisturePath = photePath;
                }
                product.Name = model.Name;
                product.ProductTypeId = model.TypeId;
                product.Coast = model.Coast;
                product.Count = model.Count;
                product.ArticleNumber = model.ArticleNumber;
                try
                {
                    _productService.Update(product.Id, product);
                    return RedirectToAction("Details", new { Id = product.Id });
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
            return View();
        }

        public IActionResult DeleteProduct(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound();
            try
            {
                _productService.Remove(id);
                return RedirectToAction("Index");
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




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult MyError(ErrorViewModel model)
        {
            return View(model);
        }
        [Route("Home/ShoppingСart")]
        public IActionResult ShoppingСart()
        {
            try
            {
                var user = _userManager.GetUserId(User);
                var shpcart = _shoppingCartService.GetUserShoppingCarts(user, false);
                if (shpcart is null)
                    return NotFound();
                ShoppingСartViewModel model = new ShoppingСartViewModel();
                model.ShoppingСarts = new System.Collections.Generic.List<ShoppingCartDto>();
                foreach (var s in shpcart)
                {
                    var pr = _productService.GetById(s.ProductId);
                    model.TotalCost += s.Cost;
                    model.ShoppingСarts.Add(
                        new ShoppingCartDto
                        {
                            BuyDate = s.BuyDate.Date.ToShortDateString(),
                            Cost = s.Cost,
                            Count = s.Count,
                            Id = s.Id,
                            ProductName = pr.Name
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

        [Route("Home/MakePurchase")]

        public IActionResult MakePurchase()
        {
            try
            {
                var user = _userManager.GetUserId(User);
                var shpcart = _shoppingCartService.GetUserShoppingCarts(user, false);
                if (shpcart is null)
                    return NotFound();
                foreach (var s in shpcart)
                {
                    var currProduct = _productService.GetById(s.ProductId);
                    if (currProduct is null)
                    {
                        ErrorViewModel errorViewModel = new ErrorViewModel
                        {
                            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                            Messge = string.Format("Продукта под номером {0} больше нет", s.Id.ToString())
                        };

                        return RedirectToAction("MyError", errorViewModel);
                    }

                    if (currProduct.Count - s.Count < 0)
                    {
                        ErrorViewModel errorViewModel = new ErrorViewModel
                        {
                            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                            Messge = string.Format("Продукта {0} на складе осталось только {1}", currProduct.Name, currProduct.Count)
                        };

                        return RedirectToAction("MyError", errorViewModel);
                    }
                    currProduct.Count = currProduct.Count - s.Count;
                    s.IsBying = true;
                    _productService.Update(s.ProductId, currProduct);
                    _shoppingCartService.Update(s.Id, s);

                }
                return RedirectToAction("Index");
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
        [Route("Home/DeleteShoppingСart/{id:Guid}")]
        public IActionResult DeleteShoppingСart([FromRoute] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return NotFound();
                _shoppingCartService.Remove(id);
                return RedirectToAction("ShoppingСart");
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

        [Route("Home/BuyProduct")]
        [HttpPost]
        public IActionResult BuyProduct(ProductDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                try
                {
                    if (_connectionParamsService.IsBotConnection(HttpContext))
                    {
                        return RedirectToAction("CheckCaptcha", "Ad");
                    }
                    _connectionParamsService.AddFromContext(HttpContext, "Home/BuyProduct");
                    if (model.CountChosen == 0)
                        Redirect("Home");
                    var userId = _userManager.GetUserId(User);
                    if (userId is null)
                        return BadRequest();
                    ShoppingCart shoppingCart = new ShoppingCart
                    {
                        Id = Guid.NewGuid(),
                        Cost = model.CountChosen * model.Product.Coast,
                        Count = model.CountChosen,
                        UserId = userId,
                        ProductId = model.Product.Id,
                        BuyDate = DateTime.Today,
                    };
                    _shoppingCartService.Add(shoppingCart);
                    return RedirectToAction("ShoppingСart");
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
            return BadRequest();
        }
        [AllowAnonymous]
        [Route("Home/Sort/")]
        public IActionResult Sort(Guid Id)
        {
            try
            {
                if (_connectionParamsService.IsBotConnection(HttpContext))
                {
                    return RedirectToAction("CheckCaptcha", "Ad");
                }
                _connectionParamsService.AddFromContext(HttpContext, "Home/Sort/");
                if (Id == Guid.Empty)
                {
                    Response.StatusCode = 404;
                    return View("ProductNotFound", Id);
                }
                var product = _productService.GetByType(Id);
                var types = _productTypeService.GetAll();
                IndexViewModel model = new IndexViewModel
                {
                    Products = product,
                    ProductTypes = types
                };

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

        [AllowAnonymous]
        public IActionResult Details(Guid id)
        {
            try
            {
                var product = _productService.GetById(id);
                if (product == null)
                {
                    Response.StatusCode = 404;
                    return View("Error");
                }
                ProductDetailsViewModel model = new ProductDetailsViewModel()
                {
                    Product = product,
                    PageTitle = "Page title"
                };
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

        public IActionResult About()
        {
            if (_connectionParamsService.IsBotConnection(HttpContext))
            {
                return RedirectToAction("CheckCaptcha", "Ad");
            }
            _connectionParamsService.AddFromContext(HttpContext, "Home/About");
            return View();
        }

        private string UploadedFile(IFormFile model)
        {
            string uniqueFileName = null;

            if (model != null)
            {
                string uploadsFolder = Path.Combine(_appEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        private bool RemoveFile(string file)
        {

            if (file != null)
            {
                string uploadsFolder = Path.Combine(_appEnvironment.WebRootPath, "images");
                string filePath = Path.Combine(uploadsFolder, file);
                FileInfo fileInf = new FileInfo(filePath);
                if (fileInf.Exists)
                {
                    fileInf.Delete();
                    return true;
                }
            }
            return false;
        }
        [AllowAnonymous]
        public IActionResult Find(string stringFind)
        {
            try
            {
                if (_connectionParamsService.IsBotConnection(HttpContext))
                {
                    return RedirectToAction("CheckCaptcha", "Ad");
                }
                _connectionParamsService.AddFromContext(HttpContext, "Home/Find");
                if (string.IsNullOrEmpty(stringFind))
                {                  
                    return RedirectToAction("Index");
                }
                var model = _productService.Find(stringFind);
                if(model is null)
                {
                    return RedirectToAction("Index");
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


    }
}
