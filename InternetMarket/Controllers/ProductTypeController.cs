using InternetMarket.Interfaces.IService;
using InternetMarket.Models;
using InternetMarket.Models.DbModels;
using InternetMarket.Models.ViewModels.ProductTypeViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace InternetMarket.Controllers
{
    public class ProductTypeController : Controller
    {
        private readonly IProductTypeService _productTypeService;
        public ProductTypeController(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProductTypeList()
        {
            var roles = _productTypeService.GetAll();
            return View(roles);
        }
        [HttpGet]
        public IActionResult CreateProductType()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateProductType(CreateProductType model)
        {
            if (ModelState.IsValid)
            {
                ProductType productType = new ProductType
                {
                    Name = model.Name
                };
                try
                {
                    _productTypeService.Add(productType);
                    return RedirectToAction("ProductTypeList");
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

        [HttpGet]
        public IActionResult EditProductType(Guid Id)
        {
            var productType = _productTypeService.GetById(Id);
            if (productType is null)
            {
                ViewBag.ErrorMessage = $" Role with Id = {Id} don't exist";
                return View("NotFound");
            }
            var model = new EditProductTypeViewModel
            {
                Id = productType.Id,
                Name = productType.Name
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult EditProductType(EditProductTypeViewModel model)
        {
            var productType = _productTypeService.GetById(model.Id);
            if (productType is null)
            {
                ViewBag.ErrorMessage = $" Role with Id = {model.Id.ToString()} don't exist";
                return View("NotFound");
            }
            else
            {
                try
                {
                    productType.Name = model.Name;
                    var result = _productTypeService.Update(model.Id, productType);
                    return RedirectToAction("ProductTypeList");
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
        
      
        public IActionResult RemoveProductType(Guid id)
        {
            var productType = _productTypeService.GetById(id);
            if (productType is null)
            {
                ViewBag.ErrorMessage = $" Role with Id = {id} don't exist";
                return View("NotFound");
            }
            try
            {
                _productTypeService.Remove(productType.Id);
                return RedirectToAction("ProductTypeList");
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
