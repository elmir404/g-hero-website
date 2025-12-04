namespace GHeroWebsite.Domain.Entities;

public class ContentSection
{
    public int Id { get; set; }
    public string SectionKey { get; set; } = string.Empty; // e.g., "hero.title", "philosophy.content"
    public string LanguageCode { get; set; } = "en"; // "en", "az", "jp"
    public string? Title { get; set; }
    public string? Content { get; set; } // Main content (can be HTML)
    public string? Subtitle { get; set; }
    public string? AdditionalData { get; set; } // JSON for structured data (e.g., vision items, values)
    public int DisplayOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

