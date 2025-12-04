using GHeroWebsite.Application.DTOs;
using GHeroWebsite.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GHeroWebsite.Web.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
public class ContentController : Controller
{
    private readonly IContentService _contentService;
    private readonly ILogger<ContentController> _logger;

    public ContentController(IContentService contentService, ILogger<ContentController> logger)
    {
        _contentService = contentService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(string? lang = null)
    {
        var languageCode = lang ?? "en";
        var sections = await _contentService.GetAllSectionsAsync(languageCode);
        ViewBag.CurrentLanguage = languageCode;
        ViewBag.AvailableLanguages = new[] { "en", "az", "jp" };
        return View(sections);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id, string? lang = "en")
    {
        if (id.HasValue && id.Value > 0)
        {
            var section = await _contentService.GetSectionByIdAsync(id.Value);
            if (section == null)
            {
                return NotFound();
            }

            var dto = new ContentSectionDto
            {
                Id = section.Id,
                SectionKey = section.SectionKey,
                LanguageCode = section.LanguageCode,
                Title = section.Title,
                Content = section.Content,
                Subtitle = section.Subtitle,
                AdditionalData = section.AdditionalData,
                DisplayOrder = section.DisplayOrder,
                IsActive = section.IsActive
            };

            ViewBag.AvailableLanguages = new[] { "en", "az", "jp" };
            return View(dto);
        }

        // New section
        var newDto = new ContentSectionDto { LanguageCode = lang ?? "en" };
        ViewBag.AvailableLanguages = new[] { "en", "az", "jp" };
        return View(newDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ContentSectionDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        try
        {
            await _contentService.CreateOrUpdateSectionAsync(dto);
            TempData["Success"] = dto.Id.HasValue 
                ? "Content section updated successfully." 
                : "Content section created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving content section");
            ModelState.AddModelError("", "An error occurred while saving. Please try again.");
            return View(dto);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _contentService.DeleteSectionAsync(id);
            if (result)
            {
                TempData["Success"] = "Content section deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Content section not found.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting content section {Id}", id);
            TempData["Error"] = "An error occurred while deleting.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(int id)
    {
        try
        {
            await _contentService.ToggleSectionActiveAsync(id);
            TempData["Success"] = "Content section status updated.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling content section {Id}", id);
            TempData["Error"] = "An error occurred.";
        }

        return RedirectToAction(nameof(Index));
    }
}

