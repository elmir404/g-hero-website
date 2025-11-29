# Assets Folder

This folder contains static assets for the G-Hero Company website.

## Structure

```
public/
└── assets/
    └── images/
        ├── logo.svg          # Main company logo (used in navigation)
        └── .gitkeep          # Keeps empty folder in git
```

## Logo

The main logo file is located at: `public/assets/images/logo.svg`

### Current Logo
- **Format**: SVG (Scalable Vector Graphics)
- **Location**: `public/assets/images/logo.svg`
- **Usage**: Navigation bar (navbar)
- **Display size**: 
  - Mobile: 32px height
  - Desktop: 40px height
  - Width: Auto (maintains aspect ratio)

### Replacing the Logo

1. Replace `logo.svg` with your actual logo file
2. Supported formats: `.svg`, `.png`, `.jpg`, `.webp`
3. If using a different filename, update `components/Navigation.tsx`
4. Recommended: Use SVG format for best quality and scalability

### Logo Guidelines

- **Aspect ratio**: 3:1 recommended (e.g., 150x50px)
- **File size**: Keep under 50KB for fast loading
- **Color**: Should work on white/light backgrounds
- **Transparency**: Supported (PNG/SVG with transparency)

## Adding More Assets

You can add other images, icons, or files to this folder structure:

```
public/assets/
├── images/
│   ├── logo.svg
│   ├── hero-image.jpg
│   └── icons/
├── fonts/          # Custom fonts (if needed)
└── documents/      # PDFs, documents (if needed)
```

## Next.js Static Files

Files in the `public` folder are served statically by Next.js and can be referenced with paths starting from `/`.

Example:
- File: `public/assets/images/logo.svg`
- URL: `/assets/images/logo.svg`
- Usage in code: `<Image src="/assets/images/logo.svg" ... />`

