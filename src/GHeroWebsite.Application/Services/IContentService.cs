using GHeroWebsite.Application.DTOs;
using GHeroWebsite.Domain.Entities;

namespace GHeroWebsite.Application.Services;

public interface IContentService
{
    Task<ContentSection?> GetSectionByKeyAsync(string sectionKey, string languageCode = "en");
    Task<IEnumerable<ContentSection>> GetAllSectionsAsync(string? languageCode = null);
    Task<IEnumerable<ContentSection>> GetActiveSectionsAsync(string languageCode = "en");
    Task<ContentSection?> GetSectionByIdAsync(int id);
    Task<int> CreateOrUpdateSectionAsync(ContentSectionDto dto);
    Task<bool> DeleteSectionAsync(int id);
    Task<bool> ToggleSectionActiveAsync(int id);
    Task<Dictionary<string, ContentSection>> GetSectionsByLanguageAsync(string languageCode);
}

