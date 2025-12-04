# Dynamic Content Management Guide

## Overview

The home page is now fully dynamic! All content can be edited from the admin panel without touching code. The system uses a flexible `ContentSection` entity that stores content by unique keys.

## How It Works

### Content Sections

Each piece of content on the home page is stored as a `ContentSection` with:
- **SectionKey** - Unique identifier (e.g., "hero.title", "philosophy.content")
- **Title** - Section title (optional)
- **Content** - Main content (supports HTML)
- **Subtitle** - Subtitle text (optional)
- **AdditionalData** - JSON for structured data (lists, arrays)
- **DisplayOrder** - Order for sorting
- **IsActive** - Show/hide toggle

### Accessing Content Management

1. Login to admin panel: `/Account/Login`
2. Go to Content Management: `/Admin/Content`
3. Edit existing sections or create new ones

## Section Keys Reference

### Hero Section
- `hero.title` - Main hero title
- `hero.tagline` - Hero tagline/subtitle
- `hero.description` - Hero description paragraph

### Philosophy Section
- `philosophy.title` - Section title
- `philosophy.content` - Main content (HTML supported)

### Mission Section
- `mission.title` - Section title
- `mission.content` - Main content (HTML supported)

### Vision Section
- `vision.title` - Section title
- `vision.items` - JSON array of vision items (stored in AdditionalData)
  ```json
  ["Item 1", "Item 2", "Item 3"]
  ```

### Core Values Section
- `values.title` - Section title
- `values.items` - JSON array of value items (stored in AdditionalData)
  Format: `["Name|Description", "Name2|Description2"]`
  ```json
  ["Integrity|We conduct our business...", "Quality|We expand Japanese standards..."]
  ```

### Business Domains Section
- `domains.title` - Section title
- `domains.subtitle` - Section subtitle
- `domains.items` - JSON array of domain items (stored in AdditionalData)
  ```json
  ["Domain 1", "Domain 2", "Domain 3"]
  ```

### About Section
- `about.title` - Section title
- `about.content` - Main content (HTML supported)

## Editing Content

### Simple Text Content

1. Go to `/Admin/Content`
2. Find the section (e.g., "hero.title")
3. Click "Edit"
4. Update the Title or Content field
5. Click "Update Section"

### Structured Data (Lists/Arrays)

For sections that use lists (Vision, Values, Domains):

1. Edit the section (e.g., "vision.items")
2. In the "Additional Data (JSON)" field, enter a JSON array:
   ```json
   ["Item 1", "Item 2", "Item 3"]
   ```
3. For Values, use pipe-separated format:
   ```json
   ["Name|Description", "Name2|Description2"]
   ```

### HTML Content

The `Content` field supports HTML. You can use:
- `<p>`, `<strong>`, `<em>` tags
- Line breaks with `<br>`
- Links with `<a href="...">`

Example:
```html
<p>This is a <strong>bold</strong> paragraph with a <a href="/contact">link</a>.</p>
```

## Creating New Sections

1. Go to `/Admin/Content`
2. Click "+ Add New Section"
3. Enter:
   - **Section Key** - Unique identifier (e.g., "custom.section")
   - **Title** - Section title
   - **Content** - Main content
   - **Display Order** - Lower numbers appear first
   - **Active** - Check to make visible
4. Click "Create Section"

## Using Custom Sections in Views

To use a custom section in a view:

```csharp
// In controller
var section = await _contentService.GetSectionByKeyAsync("custom.section");
ViewBag.CustomSection = section;
```

```razor
@* In view *@
@if (ViewBag.CustomSection != null)
{
    <h2>@ViewBag.CustomSection.Title</h2>
    @Html.Raw(ViewBag.CustomSection.Content)
}
```

## Fallback Content

The home page includes fallback content. If a section doesn't exist in the database, it will show default content. This ensures the page always displays something.

## Activating/Deactivating Sections

- **Active** sections appear on the website
- **Inactive** sections are hidden but preserved in the database
- Toggle status from the Content Management list

## Best Practices

1. **Backup Before Major Changes** - Export database or take notes
2. **Test Changes** - Preview on the website after editing
3. **Use Consistent Keys** - Follow the pattern: `section.property`
4. **Validate JSON** - When using AdditionalData, ensure valid JSON
5. **HTML Safety** - Be careful with HTML in content fields
6. **Display Order** - Use increments of 10 (10, 20, 30) for easy reordering

## Troubleshooting

### Content Not Updating
- Check if section is marked as "Active"
- Clear browser cache
- Verify section key matches exactly

### JSON Errors
- Validate JSON syntax using a JSON validator
- Ensure proper escaping of quotes
- Check for trailing commas

### HTML Not Rendering
- Use `@Html.Raw()` in views for HTML content
- Ensure HTML is properly formatted
- Check for unclosed tags

## Example: Editing Hero Section

1. Login: `/Account/Login`
2. Navigate: `/Admin/Content`
3. Find: "hero.title" section
4. Click: "Edit"
5. Update: Change Title to "New Company Name"
6. Save: Click "Update Section"
7. View: Check homepage to see changes

## Example: Adding Vision Item

1. Find: "vision.items" section
2. Edit: Click "Edit"
3. Additional Data field contains:
   ```json
   ["Item 1", "Item 2", "Item 3"]
   ```
4. Add new item:
   ```json
   ["Item 1", "Item 2", "Item 3", "New Item 4"]
   ```
5. Save: Click "Update Section"

## Database Structure

Content is stored in the `ContentSections` table:
- All content is preserved
- Changes are tracked with `UpdatedAt` timestamp
- Soft delete via `IsActive` flag

## Future Enhancements

Potential improvements:
- Rich text editor (WYSIWYG)
- Image uploads for sections
- Multi-language support
- Content versioning/history
- Preview before publish
- Scheduled publishing

