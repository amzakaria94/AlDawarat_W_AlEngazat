using AlDawarat_W_AlEngazat.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AlDawarat_W_AlEngazat.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult DisabledFeature()
    {
        return View("DisabledFeature");
    }

    //public IActionResult Privacy()
    //{
    //	return View();
    //}

    //public IActionResult About()
    //{
    //	return View();
    //}

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
