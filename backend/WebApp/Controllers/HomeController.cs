using System.Diagnostics;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

/// <summary>
/// Controller for general site actions like home, privacy and error.
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Landing page.
    /// </summary>
    public IActionResult Index()
    {
        _logger.LogInformation("Loading home page");
        return View();
    }

    /// <summary>
    /// Privacy policy page.
    /// </summary>
    public IActionResult Privacy()
    {
        _logger.LogInformation("Loading privacy page");
        return View();
    }

    /// <summary>
    /// Handles application errors.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        _logger.LogError("Unhandled error occurred, RequestId: {RequestId}", requestId);
        return View(new ErrorViewModel { RequestId = requestId });
    }
    
    /// <summary>
    /// Sets the user's preferred language.
    /// </summary>
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        _logger.LogInformation("Setting language to {Culture}, redirecting to {ReturnUrl}", culture, returnUrl);

        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            }
        );
        return LocalRedirect(returnUrl);
    }
}