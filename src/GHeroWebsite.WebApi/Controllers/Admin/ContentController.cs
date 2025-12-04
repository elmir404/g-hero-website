using GHeroWebsite.Application.DTOs;
using GHeroWebsite.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GHeroWebsite.WebApi.Controllers.Admin;

[ApiController]
[Route("api/admin/content")]
[Authorize(Roles = "Admin")]
public class ContentController : ControllerBase
{
    private readonly IContentService _contentService;
    private readonly ILogger<ContentController> _logger;

    public ContentController(IContentService contentService, ILogger<ContentController> logger)
    {
        _contentService = contentService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? lang = null)
    {
        var sections = await _contentService.GetAllSectionsAsync(lang);
        return Ok(new { data = sections });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var section = await _contentService.GetSectionByIdAsync(id);
        if (section == null)
            return NotFound(new { message = "Content section not found" });

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

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ContentSectionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { errors = ModelState });

        try
        {
            var id = await _contentService.CreateOrUpdateSectionAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, new { id, message = "Content section created successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating content section");
            return StatusCode(500, new { message = "An error occurred while creating the section" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ContentSectionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { errors = ModelState });

        dto.Id = id;
        try
        {
            await _contentService.CreateOrUpdateSectionAsync(dto);
            return Ok(new { message = "Content section updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating content section {Id}", id);
            return StatusCode(500, new { message = "An error occurred while updating the section" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _contentService.DeleteSectionAsync(id);
            if (!result)
                return NotFound(new { message = "Content section not found" });

            return Ok(new { message = "Content section deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting content section {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the section" });
        }
    }

    [HttpPatch("{id}/toggle-active")]
    public async Task<IActionResult> ToggleActive(int id)
    {
        try
        {
            await _contentService.ToggleSectionActiveAsync(id);
            return Ok(new { message = "Content section status updated" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling content section {Id}", id);
            return StatusCode(500, new { message = "An error occurred" });
        }
    }
}

