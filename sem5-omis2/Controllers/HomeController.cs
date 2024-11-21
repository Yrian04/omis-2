using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using sem5_omis2.Models;

namespace sem5_omis2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        ILogger<HomeController> logger
    )
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            return RedirectToAction("Login", "Account");
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
