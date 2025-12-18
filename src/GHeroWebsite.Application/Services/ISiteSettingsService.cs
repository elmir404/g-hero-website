using GHeroWebsite.Application.DTOs;

namespace GHeroWebsite.Application.Services;

public interface ISiteSettingsService
{
    Task<SiteSettingsDto> GetSiteSettingsAsync();
    Task<SiteSettingsDto> UpdateSiteSettingsAsync(SiteSettingsDto dto);
}

