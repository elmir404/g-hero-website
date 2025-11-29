# Logo Setup Guide

## Logo Location

The G-Hero logo is located in: `public/assets/images/logo.svg`

## Replacing the Logo

To replace the placeholder logo with your actual logo:

1. **Recommended formats:**
   - `.svg` (Scalable Vector Graphics) - Best for quality and scalability
   - `.png` - For raster images with transparency
   - `.webp` - Modern format with good compression
   - `.jpg` - For photos (not recommended for logos)

2. **File naming:**
   - Keep the file as `logo.svg` OR
   - Update the path in `components/Navigation.tsx` to match your filename

3. **Size recommendations:**
   - Width: 150-200px for standard displays
   - Height: Should maintain aspect ratio
   - For SVG: Use vector format (infinite scalability)

4. **Logo dimensions:**
   - Current setup: 150x50px (3:1 ratio)
   - The logo is automatically resized in the navbar to `h-8 lg:h-10` (32px mobile, 40px desktop)

## Current Implementation

The logo is used in:
- `components/Navigation.tsx` - Main navigation bar (sticky header)

## How to Update

1. Replace `public/assets/images/logo.svg` with your logo file
2. If using a different filename or format, update the path in `components/Navigation.tsx`:
   ```tsx
   <Image
     src="/assets/images/your-logo.png"  // Update this path
     alt="G-Hero Company Logo"
     width={150}
     height={50}
     className="h-8 lg:h-10 w-auto"
     priority
   />
   ```
3. Adjust `width` and `height` props to match your logo's aspect ratio

## Multiple Logo Formats

For better browser compatibility, you can add multiple formats:
- `logo.svg` (primary)
- `logo.png` (fallback)
- `logo.webp` (optimized)

Update the component to use a `<picture>` element if you want to support multiple formats.

## Tips

- **SVG is recommended** for logos as it scales perfectly at any size
- Keep the logo file size small (< 50KB) for fast loading
- Ensure the logo looks good on both light backgrounds (current navbar has white/transparent background)
- Test the logo at different screen sizes to ensure readability

