using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RottenPotatoes.Models;
using RottenPotatoes.Services;

namespace RottenPotatoes.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SessionManager _session;

    public HomeController(ILogger<HomeController> logger, SessionManager session)
    {
        _logger = logger;
        _session = session;
    }

    public IActionResult Index()
    {
        User user = _session.Get<User>("user");
        if (user == null)
            return RedirectToAction("Login", "User");

        return View();
    }

    public IActionResult Privacy()
    {
        User user = _session.Get<User>("user");
        if (user == null)
            return RedirectToAction("Login", "User");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
