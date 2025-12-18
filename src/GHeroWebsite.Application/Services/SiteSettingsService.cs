using GHeroWebsite.Application.DTOs;
using GHeroWebsite.Domain.Entities;
using GHeroWebsite.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GHeroWebsite.Application.Services;

public class SiteSettingsService : ISiteSettingsService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SiteSettingsService> _logger;

    public SiteSettingsService(ApplicationDbContext context, ILogger<SiteSettingsService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<SiteSettingsDto> GetSiteSettingsAsync()
    {
        var settings = await _context.SiteSettings.FirstOrDefaultAsync();
        
        if (settings == null)
        {
            // Return empty DTO if no settings exist
            _logger.LogInformation("No site settings found, returning empty settings");
            return new SiteSettingsDto();
        }

        return new SiteSettingsDto
        {
            Id = settings.Id,
            Phone = settings.Phone,
            Email = settings.Email,
            Address = settings.Address,
            WorkingHours = settings.WorkingHours,
            WhatsAppUrl = settings.WhatsAppUrl,
            InstagramUrl = settings.InstagramUrl,
            FacebookUrl = settings.FacebookUrl,
            LinkedInUrl = settings.LinkedInUrl,
            MapEmbedUrl = settings.MapEmbedUrl
        };
    }

    public async Task<SiteSettingsDto> UpdateSiteSettingsAsync(SiteSettingsDto dto)
    {
        var existing = await _context.SiteSettings.FirstOrDefaultAsync();
        
        if (existing == null)
        {
            // Create new settings if none exist
            existing = new SiteSettings
            {
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                WorkingHours = dto.WorkingHours,
                WhatsAppUrl = dto.WhatsAppUrl,
                InstagramUrl = dto.InstagramUrl,
                FacebookUrl = dto.FacebookUrl,
                LinkedInUrl = dto.LinkedInUrl,
                MapEmbedUrl = dto.MapEmbedUrl,
                UpdatedAt = DateTime.UtcNow
            };
            _context.SiteSettings.Add(existing);
            _logger.LogInformation("Creating new site settings");
        }
        else
        {
            // Update existing settings
            existing.Phone = dto.Phone;
            existing.Email = dto.Email;
            existing.Address = dto.Address;
            existing.WorkingHours = dto.WorkingHours;
            existing.WhatsAppUrl = dto.WhatsAppUrl;
            existing.InstagramUrl = dto.InstagramUrl;
            existing.FacebookUrl = dto.FacebookUrl;
            existing.LinkedInUrl = dto.LinkedInUrl;
            existing.MapEmbedUrl = dto.MapEmbedUrl;
            existing.UpdatedAt = DateTime.UtcNow;
            _logger.LogInformation("Updating site settings with ID: {Id}", existing.Id);
        }

        await _context.SaveChangesAsync();
        
        return new SiteSettingsDto
        {
            Id = existing.Id,
            Phone = existing.Phone,
            Email = existing.Email,
            Address = existing.Address,
            WorkingHours = existing.WorkingHours,
            WhatsAppUrl = existing.WhatsAppUrl,
            InstagramUrl = existing.InstagramUrl,
            FacebookUrl = existing.FacebookUrl,
            LinkedInUrl = existing.LinkedInUrl,
            MapEmbedUrl = existing.MapEmbedUrl
        };
    }
}

