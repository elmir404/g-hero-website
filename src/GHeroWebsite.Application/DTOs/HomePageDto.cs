namespace GHeroWebsite.Application.DTOs;

public class HomePageDto
{
    public HomeHeroSectionDto? Hero { get; set; }
    public ContentSectionDto? Philosophy { get; set; }
    public ContentSectionDto? Mission { get; set; }
    public ContentSectionDto? Vision { get; set; }
    public ContentSectionDto? Values { get; set; }
    public ContentSectionDto? Domains { get; set; }
    public ContentSectionDto? About { get; set; }
    public List<HomeServiceDto> Services { get; set; } = new();
    public List<HomeTestimonialDto> Testimonials { get; set; } = new();
    public List<HomeFaqDto> Faqs { get; set; } = new();
    public HomeContactInfoDto? ContactInfo { get; set; }
}

public class HomeHeroSectionDto
{
    public int Id { get; set; }
    public string LanguageCode { get; set; } = "en";
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string? Description { get; set; }
    public string? PrimaryButtonText { get; set; }
    public string? PrimaryButtonUrl { get; set; }
    public string? SecondaryButtonText { get; set; }
    public string? SecondaryButtonUrl { get; set; }
    public string? BackgroundImageUrl { get; set; }
}

public class HomeServiceDto
{
    public int Id { get; set; }
    public string LanguageCode { get; set; } = "en";
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconName { get; set; }
    public string? IconUrl { get; set; }
    public string? ImageUrl { get; set; }
    public int DisplayOrder { get; set; }
}

public class HomeTestimonialDto
{
    public int Id { get; set; }
    public string LanguageCode { get; set; } = "en";
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorTitle { get; set; }
    public string? AuthorCompany { get; set; }
    public string? AuthorImageUrl { get; set; }
    public string Content { get; set; } = string.Empty;
    public int? Rating { get; set; }
    public int DisplayOrder { get; set; }
}

public class HomeFaqDto
{
    public int Id { get; set; }
    public string LanguageCode { get; set; } = "en";
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}

public class HomeContactInfoDto
{
    public int Id { get; set; }
    public string LanguageCode { get; set; } = "en";
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? FacebookUrl { get; set; }
    public string? TwitterUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? InstagramUrl { get; set; }
}

