using GHeroWebsite.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GHeroWebsite.WebApi.Controllers.Public;

[ApiController]
[Route("api/public/site-settings")]
public class SiteSettingsController : ControllerBase
{
    private readonly ISiteSettingsService _siteSettingsService;
    private readonly ILogger<SiteSettingsController> _logger;

    public SiteSettingsController(ISiteSettingsService siteSettingsService, ILogger<SiteSettingsController> logger)
    {
        _siteSettingsService = siteSettingsService;
        _logger = logger;
    }

    /// <summary>
    /// Get site settings (public, no auth required)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetSiteSettings()
    {
        try
        {
            var settings = await _siteSettingsService.GetSiteSettingsAsync();
            return Ok(settings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting site settings");
            return StatusCode(500, new { message = "An error occurred while retrieving site settings" });
        }
    }
}

