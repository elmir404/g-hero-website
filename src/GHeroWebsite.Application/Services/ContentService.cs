using GHeroWebsite.Application.DTOs;
using GHeroWebsite.Domain.Entities;
using GHeroWebsite.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GHeroWebsite.Application.Services;

public class ContentService : IContentService
{
    private readonly ApplicationDbContext _context;

    public ContentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ContentSection?> GetSectionByKeyAsync(string sectionKey, string languageCode = "en")
    {
        return await _context.ContentSections
            .FirstOrDefaultAsync(s => s.SectionKey == sectionKey && s.LanguageCode == languageCode && s.IsActive);
    }

    public async Task<IEnumerable<ContentSection>> GetAllSectionsAsync(string? languageCode = null)
    {
        var query = _context.ContentSections.AsQueryable();
        
        if (!string.IsNullOrEmpty(languageCode))
        {
            query = query.Where(s => s.LanguageCode == languageCode);
        }
        
        return await query
            .OrderBy(s => s.LanguageCode)
            .ThenBy(s => s.DisplayOrder)
            .ThenBy(s => s.SectionKey)
            .ToListAsync();
    }

    public async Task<IEnumerable<ContentSection>> GetActiveSectionsAsync(string languageCode = "en")
    {
        return await _context.ContentSections
            .Where(s => s.IsActive && s.LanguageCode == languageCode)
            .OrderBy(s => s.DisplayOrder)
            .ThenBy(s => s.SectionKey)
            .ToListAsync();
    }

    public async Task<Dictionary<string, ContentSection>> GetSectionsByLanguageAsync(string languageCode)
    {
        var sections = await _context.ContentSections
            .Where(s => s.IsActive && s.LanguageCode == languageCode)
            .ToListAsync();
        
        return sections.ToDictionary(s => s.SectionKey, s => s);
    }

    public async Task<ContentSection?> GetSectionByIdAsync(int id)
    {
        return await _context.ContentSections.FindAsync(id);
    }

    public async Task<int> CreateOrUpdateSectionAsync(ContentSectionDto dto)
    {
        ContentSection section;

        if (dto.Id.HasValue && dto.Id.Value > 0)
        {
            // Update existing
            section = await _context.ContentSections.FindAsync(dto.Id.Value);
            if (section == null)
                throw new ArgumentException("Section not found", nameof(dto.Id));
        }
        else
        {
            // Create new
            section = new ContentSection();
            section.CreatedAt = DateTime.UtcNow;
        }

        section.SectionKey = dto.SectionKey;
        section.LanguageCode = dto.LanguageCode;
        section.Title = dto.Title;
        section.Content = dto.Content;
        section.Subtitle = dto.Subtitle;
        section.AdditionalData = dto.AdditionalData;
        section.DisplayOrder = dto.DisplayOrder;
        section.IsActive = dto.IsActive;
        section.UpdatedAt = DateTime.UtcNow;

        if (!dto.Id.HasValue || dto.Id.Value == 0)
        {
            _context.ContentSections.Add(section);
        }

        await _context.SaveChangesAsync();
        return section.Id;
    }

    public async Task<bool> DeleteSectionAsync(int id)
    {
        var section = await _context.ContentSections.FindAsync(id);
        if (section == null)
            return false;

        _context.ContentSections.Remove(section);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleSectionActiveAsync(int id)
    {
        var section = await _context.ContentSections.FindAsync(id);
        if (section == null)
            return false;

        section.IsActive = !section.IsActive;
        section.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}

