using System;
using InternetMarket.Interfaces.IService;
using InternetMarket.Models.DbModels;
using Microsoft.AspNetCore.Mvc;

namespace InternetMarket.Controllers;

public class AdController : Controller
{
    private readonly IConnectionParamsService _service;

    public AdController(IConnectionParamsService service)
    {
        _service = service;
    }
    public IActionResult Index()
    {
        try
        {
            var connectionParams = _service.GetAll();
            return View(connectionParams);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction("Error", "Home");
        }
    }


    public IActionResult Trick()
    {
        try
        {
            _service.AddFromContext(HttpContext);
            return View();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction("Error", "Home");
        }
    }

    public IActionResult Remove(Guid id)
    {
        try
        {
            _service.Remove(id);
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction("Error", "Home");

        }
    }
}