using GHeroWebsite.Application.DTOs;
using GHeroWebsite.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GHeroWebsite.WebApi.Controllers.Admin;

[ApiController]
[Route("api/admin/site-settings")]
[Authorize(Roles = "Admin")]
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
    /// Update site settings (admin only)
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateSiteSettings([FromBody] SiteSettingsDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { errors = ModelState });
        }

        try
        {
            var updated = await _siteSettingsService.UpdateSiteSettingsAsync(dto);
            _logger.LogInformation("Site settings updated successfully by user: {User}", User.Identity?.Name);
            return Ok(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating site settings");
            return StatusCode(500, new { message = "An error occurred while updating site settings" });
        }
    }
}

