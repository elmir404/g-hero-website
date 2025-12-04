namespace GHeroWebsite.Domain.Entities;

public class HomeTestimonial
{
    public int Id { get; set; }
    public string LanguageCode { get; set; } = "en";
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorTitle { get; set; }
    public string? AuthorCompany { get; set; }
    public string? AuthorImageUrl { get; set; }
    public string Content { get; set; } = string.Empty;
    public int? Rating { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

