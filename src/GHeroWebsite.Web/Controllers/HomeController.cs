using GHeroWebsite.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GHeroWebsite.Web.Controllers;

public class HomeController : Controller
{
    private readonly IContentService _contentService;

    public HomeController(IContentService contentService)
    {
        _contentService = contentService;
    }

    public async Task<IActionResult> Index(string? lang = null)
    {
        // Get language from query string, cookie, or default to "en"
        var languageCode = lang ?? Request.Cookies["Language"] ?? "en";
        
        // Validate language code
        if (!new[] { "en", "az", "jp" }.Contains(languageCode))
        {
            languageCode = "en";
        }
        
        // Save language preference in cookie
        if (lang != null)
        {
            Response.Cookies.Append("Language", languageCode, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                HttpOnly = false,
                SameSite = SameSiteMode.Lax
            });
        }
        
        // Load all active content sections for the selected language
        var contentDict = await _contentService.GetSectionsByLanguageAsync(languageCode);
        
        ViewBag.ContentSections = contentDict;
        ViewBag.CurrentLanguage = languageCode;
        
        return View();
    }

    [HttpPost]
    public IActionResult SetLanguage(string language, string returnUrl)
    {
        // Validate language code
        if (!new[] { "en", "az", "jp" }.Contains(language))
        {
            language = "en";
        }
        
        Response.Cookies.Append("Language", language, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            HttpOnly = false,
            SameSite = SameSiteMode.Lax
        });
        
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        
        return RedirectToAction("Index", new { lang = language });
    }
}

