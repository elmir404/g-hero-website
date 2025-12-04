# Multilingual Content Guide

## Overview

The website now supports **3 languages** in the UI:
- 🇬🇧 **English (en)** - Default language
- 🇦🇿 **Azerbaijani (az)** - Azərbaycan dili
- 🇯🇵 **Japanese (jp)** - 日本語

All content can be managed separately for each language through the admin panel.

## How It Works

### Language Selection

Users can switch languages using the language switcher in the navigation bar:
- **EN** - English
- **AZ** - Azerbaijani  
- **日本語** - Japanese

The selected language is saved in a cookie and persists across page visits.

### Content Storage

Each content section is stored with a `LanguageCode` field:
- Same `SectionKey` can exist for multiple languages
- Example: `hero.title` exists 3 times (en, az, jp)
- Each language version is independent

### Database Structure

The `ContentSections` table now includes:
- `SectionKey` - Content identifier (e.g., "hero.title")
- `LanguageCode` - Language code ("en", "az", "jp")
- `Title`, `Content`, `Subtitle`, `AdditionalData` - Language-specific content

## Admin Panel

### Managing Multilingual Content

1. **Go to Content Management**: `/Admin/Content`
2. **Filter by Language**: Use the language filter buttons at the top
3. **Edit Content**: Click "Edit" on any section
4. **Select Language**: Choose language from dropdown when creating/editing

### Creating Content for All Languages

For each content section, you need to create 3 versions:

1. **English version**:
   - Section Key: `hero.title`
   - Language: English
   - Content: "G-Hero Company"

2. **Azerbaijani version**:
   - Section Key: `hero.title`
   - Language: Azerbaijani
   - Content: "G-Hero Company" (or translated)

3. **Japanese version**:
   - Section Key: `hero.title`
   - Language: Japanese
   - Content: "G-Hero Company" (or translated)

### Language Filter in Admin

The admin panel shows a language filter:
- Click **EN**, **AZ**, or **日本語** to filter content by language
- Only sections for the selected language are displayed
- Makes it easy to manage content for each language separately

## Initial Content

On first run, the application automatically seeds content for all 3 languages:
- ✅ English (en) - Complete
- ✅ Azerbaijani (az) - Complete
- ✅ Japanese (jp) - Complete

All major sections are pre-populated:
- Hero section
- Philosophy
- Mission
- Vision
- Core Values
- Business Domains
- About

## Language Switching

### For Users

1. Click language button in navigation (EN/AZ/日本語)
2. Page reloads with content in selected language
3. Language preference is saved in cookie
4. Next visit uses saved language

### For Developers

Language is determined in this order:
1. Query parameter (`?lang=en`)
2. Cookie (`Language`)
3. Default (`en`)

## Technical Details

### HomeController

```csharp
public async Task<IActionResult> Index(string? lang = null)
{
    var languageCode = lang ?? Request.Cookies["Language"] ?? "en";
    // Validate and load content for language
    var contentDict = await _contentService.GetSectionsByLanguageAsync(languageCode);
    ViewBag.ContentSections = contentDict;
    ViewBag.CurrentLanguage = languageCode;
    return View();
}
```

### ContentService

```csharp
public async Task<Dictionary<string, ContentSection>> GetSectionsByLanguageAsync(string languageCode)
{
    var sections = await _context.ContentSections
        .Where(s => s.IsActive && s.LanguageCode == languageCode)
        .ToListAsync();
    return sections.ToDictionary(s => s.SectionKey, s => s);
}
```

## Best Practices

### 1. Complete Translations

Ensure all sections have translations for all 3 languages:
- If a section is missing for a language, it won't display
- Use fallback content in views if needed

### 2. Consistent Section Keys

Use the same `SectionKey` across all languages:
- ✅ `hero.title` (en, az, jp)
- ❌ `hero.title.en`, `hero.title.az` (don't do this)

### 3. Language-Specific Content

Some content may need language-specific formatting:
- Japanese: May need different line breaks
- Azerbaijani: Right-to-left considerations (if needed)
- English: Standard formatting

### 4. JSON Arrays

For sections with lists (Vision, Values, Domains):
- Each language has its own JSON array
- Format remains the same, content is translated

## Adding New Languages

To add a new language (e.g., Russian):

1. **Update Language List**:
   - `HomeController.cs` - Add to validation array
   - `ContentController.cs` - Add to `AvailableLanguages`
   - `_Layout.cshtml` - Add language button

2. **Update Database**:
   - Add content sections for new language
   - Use same `SectionKey` with new `LanguageCode`

3. **Update Seeding**:
   - Add translations to `SeedInitialContentAsync` in `Program.cs`

## Troubleshooting

### Content Not Showing in Selected Language

- Check if content exists for that language in database
- Verify `LanguageCode` matches exactly ("en", "az", "jp")
- Check if section is marked as `IsActive = true`

### Language Not Persisting

- Check browser cookie settings
- Verify cookie is being set (check browser DevTools)
- Cookie expires after 1 year

### Missing Translations

- Use admin panel to add missing translations
- Or update seeding code to include all languages

## Example: Editing Hero Title

1. Go to `/Admin/Content?lang=en`
2. Find "hero.title" section
3. Click "Edit"
4. Update title for English
5. Save

Repeat for other languages:
- `/Admin/Content?lang=az` - Edit Azerbaijani version
- `/Admin/Content?lang=jp` - Edit Japanese version

## URL Structure

- Default: `/` (uses cookie or defaults to English)
- English: `/?lang=en`
- Azerbaijani: `/?lang=az`
- Japanese: `/?lang=jp`

Language preference is saved, so users don't need to select every time.

## Future Enhancements

Potential improvements:
- Auto-detect browser language
- Language-specific URLs (`/en/`, `/az/`, `/jp/`)
- Translation management interface
- Bulk translation import/export
- Translation status indicators (complete/incomplete)

