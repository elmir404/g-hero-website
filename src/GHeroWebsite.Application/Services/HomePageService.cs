using GHeroWebsite.Application.DTOs;
using GHeroWebsite.Domain.Entities;
using GHeroWebsite.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GHeroWebsite.Application.Services;

public class HomePageService : IHomePageService
{
    private readonly ApplicationDbContext _context;
    private readonly IContentService _contentService;

    public HomePageService(ApplicationDbContext context, IContentService contentService)
    {
        _context = context;
        _contentService = contentService;
    }

    public async Task<HomePageDto> GetHomePageAsync(string languageCode = "en")
    {
        var homePage = new HomePageDto
        {
            Hero = await GetHeroSectionAsync(languageCode),
            Services = await GetServicesAsync(languageCode),
            Testimonials = await GetTestimonialsAsync(languageCode),
            Faqs = await GetFaqsAsync(languageCode),
            ContactInfo = await GetContactInfoAsync(languageCode)
        };

        // Get content sections
        var philosophy = await _contentService.GetSectionByKeyAsync("philosophy.title", languageCode);
        var mission = await _contentService.GetSectionByKeyAsync("mission.title", languageCode);
        var vision = await _contentService.GetSectionByKeyAsync("vision.title", languageCode);
        var values = await _contentService.GetSectionByKeyAsync("values.title", languageCode);
        var domains = await _contentService.GetSectionByKeyAsync("domains.title", languageCode);
        var about = await _contentService.GetSectionByKeyAsync("about.title", languageCode);

        if (philosophy != null)
        {
            homePage.Philosophy = new ContentSectionDto
            {
                Id = philosophy.Id,
                SectionKey = philosophy.SectionKey,
                LanguageCode = philosophy.LanguageCode,
                Title = philosophy.Title,
                Content = philosophy.Content,
                Subtitle = philosophy.Subtitle,
                AdditionalData = philosophy.AdditionalData
            };
        }

        if (mission != null)
        {
            homePage.Mission = new ContentSectionDto
            {
                Id = mission.Id,
                SectionKey = mission.SectionKey,
                LanguageCode = mission.LanguageCode,
                Title = mission.Title,
                Content = mission.Content,
                Subtitle = mission.Subtitle,
                AdditionalData = mission.AdditionalData
            };
        }

        if (vision != null)
        {
            homePage.Vision = new ContentSectionDto
            {
                Id = vision.Id,
                SectionKey = vision.SectionKey,
                LanguageCode = vision.LanguageCode,
                Title = vision.Title,
                Content = vision.Content,
                Subtitle = vision.Subtitle,
                AdditionalData = vision.AdditionalData
            };
        }

        if (values != null)
        {
            homePage.Values = new ContentSectionDto
            {
                Id = values.Id,
                SectionKey = values.SectionKey,
                LanguageCode = values.LanguageCode,
                Title = values.Title,
                Content = values.Content,
                Subtitle = values.Subtitle,
                AdditionalData = values.AdditionalData
            };
        }

        if (domains != null)
        {
            homePage.Domains = new ContentSectionDto
            {
                Id = domains.Id,
                SectionKey = domains.SectionKey,
                LanguageCode = domains.LanguageCode,
                Title = domains.Title,
                Content = domains.Content,
                Subtitle = domains.Subtitle,
                AdditionalData = domains.AdditionalData
            };
        }

        if (about != null)
        {
            homePage.About = new ContentSectionDto
            {
                Id = about.Id,
                SectionKey = about.SectionKey,
                LanguageCode = about.LanguageCode,
                Title = about.Title,
                Content = about.Content,
                Subtitle = about.Subtitle,
                AdditionalData = about.AdditionalData
            };
        }

        return homePage;
    }

    public async Task<HomeHeroSectionDto?> GetHeroSectionAsync(string languageCode = "en")
    {
        var hero = await _context.HomeHeroSections
            .Where(h => h.LanguageCode == languageCode && h.IsActive)
            .OrderByDescending(h => h.DisplayOrder)
            .FirstOrDefaultAsync();

        if (hero == null) return null;

        return new HomeHeroSectionDto
        {
            Id = hero.Id,
            LanguageCode = hero.LanguageCode,
            Title = hero.Title,
            Subtitle = hero.Subtitle,
            Description = hero.Description,
            PrimaryButtonText = hero.PrimaryButtonText,
            PrimaryButtonUrl = hero.PrimaryButtonUrl,
            SecondaryButtonText = hero.SecondaryButtonText,
            SecondaryButtonUrl = hero.SecondaryButtonUrl,
            BackgroundImageUrl = hero.BackgroundImageUrl
        };
    }

    public async Task<List<HomeServiceDto>> GetServicesAsync(string languageCode = "en")
    {
        var services = await _context.HomeServices
            .Where(s => s.LanguageCode == languageCode && s.IsActive)
            .OrderBy(s => s.DisplayOrder)
            .ToListAsync();

        return services.Select(s => new HomeServiceDto
        {
            Id = s.Id,
            LanguageCode = s.LanguageCode,
            Title = s.Title,
            Description = s.Description,
            IconName = s.IconName,
            IconUrl = s.IconUrl,
            ImageUrl = s.ImageUrl,
            DisplayOrder = s.DisplayOrder
        }).ToList();
    }

    public async Task<List<HomeTestimonialDto>> GetTestimonialsAsync(string languageCode = "en")
    {
        var testimonials = await _context.HomeTestimonials
            .Where(t => t.LanguageCode == languageCode && t.IsActive)
            .OrderBy(t => t.DisplayOrder)
            .ToListAsync();

        return testimonials.Select(t => new HomeTestimonialDto
        {
            Id = t.Id,
            LanguageCode = t.LanguageCode,
            AuthorName = t.AuthorName,
            AuthorTitle = t.AuthorTitle,
            AuthorCompany = t.AuthorCompany,
            AuthorImageUrl = t.AuthorImageUrl,
            Content = t.Content,
            Rating = t.Rating,
            DisplayOrder = t.DisplayOrder
        }).ToList();
    }

    public async Task<List<HomeFaqDto>> GetFaqsAsync(string languageCode = "en")
    {
        var faqs = await _context.HomeFaqs
            .Where(f => f.LanguageCode == languageCode && f.IsActive)
            .OrderBy(f => f.DisplayOrder)
            .ToListAsync();

        return faqs.Select(f => new HomeFaqDto
        {
            Id = f.Id,
            LanguageCode = f.LanguageCode,
            Question = f.Question,
            Answer = f.Answer,
            DisplayOrder = f.DisplayOrder
        }).ToList();
    }

    public async Task<HomeContactInfoDto?> GetContactInfoAsync(string languageCode = "en")
    {
        var contactInfo = await _context.HomeContactInfos
            .Where(c => c.LanguageCode == languageCode && c.IsActive)
            .FirstOrDefaultAsync();

        if (contactInfo == null) return null;

        return new HomeContactInfoDto
        {
            Id = contactInfo.Id,
            LanguageCode = contactInfo.LanguageCode,
            Address = contactInfo.Address,
            Phone = contactInfo.Phone,
            Email = contactInfo.Email,
            Website = contactInfo.Website,
            FacebookUrl = contactInfo.FacebookUrl,
            TwitterUrl = contactInfo.TwitterUrl,
            LinkedInUrl = contactInfo.LinkedInUrl,
            InstagramUrl = contactInfo.InstagramUrl
        };
    }
}

