using System;
using System.IO;
using InternetMarket.Interfaces.IService;
using InternetMarket.Models.ViewModels.Ad;
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
    [HttpGet]
    public IActionResult CheckCaptcha()
    {
        string code = new Random(DateTime.Now.Millisecond).Next(1111, 9999).ToString();
        CaptchaImage captchaImage = new CaptchaImage(code, 110, 50);
        this.ControllerContext.HttpContext.Response.Cookies.Append("captcha", code);
        Captcha captcha = new Captcha { PathToCaptcha = captchaImage.PathToImage};
        return View(captcha);
    }
   
    [HttpPost]
    public IActionResult CheckCaptcha(Captcha captcha)
    {
        var s = Request.Cookies["captcha"];
        if (captcha.Text != s)
        {
            ModelState.AddModelError("Captcha", "Текст с картинки введен неверно");
        }
        else
        {
            FileInfo fileInf = new FileInfo("wwwroot/images/" + captcha);
            if (fileInf.Exists)
            {
                fileInf.Delete();
            }

            return RedirectToAction("Index", "Home");
        }
        return View(captcha);
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