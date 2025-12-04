using System.ComponentModel.DataAnnotations;

namespace GHeroWebsite.Application.DTOs;

public class ContentSectionDto
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Section key is required")]
    [StringLength(100, ErrorMessage = "Section key cannot exceed 100 characters")]
    public string SectionKey { get; set; } = string.Empty;

    [Required(ErrorMessage = "Language code is required")]
    [StringLength(10, ErrorMessage = "Language code cannot exceed 10 characters")]
    public string LanguageCode { get; set; } = "en";

    [StringLength(500, ErrorMessage = "Title cannot exceed 500 characters")]
    public string? Title { get; set; }

    public string? Content { get; set; }

    [StringLength(1000, ErrorMessage = "Subtitle cannot exceed 1000 characters")]
    public string? Subtitle { get; set; }

    public string? AdditionalData { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Display order must be a positive number")]
    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;
}

