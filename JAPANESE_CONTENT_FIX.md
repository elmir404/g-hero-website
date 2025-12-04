# Fix: Japanese Content Not Found

## Problem

When viewing the site in Japanese (`/ja/...`), content is not showing because:
- **Next.js uses**: `'ja'` (ISO 639-1 standard)
- **Database might have**: `'jp'` (old code)

## Solution

### Option 1: Update Existing Database Records (Recommended)

If you have existing content with `'jp'` language code, update them to `'ja'`:

**Using SQL:**
```sql
UPDATE ContentSections 
SET LanguageCode = 'ja' 
WHERE LanguageCode = 'jp';
```

**Or via Admin Panel:**
1. Go to `/admin/content`
2. Filter by language (if you see "JP" option)
3. Edit each section and change language from "jp" to "ja"
4. Save

### Option 2: Create New Content with 'ja' Code

1. Go to `/admin/content`
2. Click "+ New Section"
3. Set Language to "日本語" (which uses 'ja')
4. Create all necessary sections for Japanese

### Option 3: Restart Web API (Auto-Seed)

The Web API now seeds content with `'ja'` code on startup. If your database is empty or you want to re-seed:

1. **Stop the Web API** (if running)
2. **Delete existing content** (optional, via admin or SQL)
3. **Restart Web API** - it will automatically seed content with correct `'ja'` code

## Verification

After fixing, verify:

1. **Check database:**
   ```sql
   SELECT LanguageCode, COUNT(*) 
   FROM ContentSections 
   GROUP BY LanguageCode;
   ```
   Should show: `en`, `az`, `ja` (not `jp`)

2. **Test API:**
   ```bash
   curl http://localhost:5000/api/home?lang=ja
   ```
   Should return Japanese content

3. **Test Next.js:**
   - Go to `http://localhost:3000/ja`
   - Content should display in Japanese

## What Was Fixed

1. ✅ **Web API seeding** now uses `'ja'` instead of `'jp'`
2. ✅ **Next.js page** now fetches content from API using locale
3. ✅ **Components** updated to accept API data with fallback to static translations

## Next Steps

1. **Restart Web API** to apply the new seeding logic
2. **Update existing database records** from `'jp'` to `'ja'` (if any)
3. **Test the Japanese page** at `http://localhost:3000/ja`

The content should now display correctly for Japanese!

