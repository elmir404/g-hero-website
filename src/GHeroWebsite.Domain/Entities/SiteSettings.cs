namespace GHeroWebsite.Domain.Entities;

public class SiteSettings
{
    public int Id { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? WorkingHours { get; set; }
    public string? WhatsAppUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? FacebookUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? MapEmbedUrl { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

