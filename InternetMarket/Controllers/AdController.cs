using Microsoft.AspNetCore.Mvc;

namespace InternetMarket.Controllers;

public class AdController : Controller
{
    public IActionResult Index()
    {
        var acceptHeaderValue = HttpContext.Request.Headers["accept"];
        return View();
    }


    public IActionResult Trick()
    {
        var acceptHeaderValue = HttpContext.Request.Headers["accept"];
        return View();
    }
}