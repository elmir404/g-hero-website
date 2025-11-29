# Quick Start Guide

## Installation

1. **Install dependencies:**
   ```bash
   npm install
   ```

2. **Run the development server:**
   ```bash
   npm run dev
   ```

3. **Open your browser:**
   Navigate to [http://localhost:3000](http://localhost:3000)
   
   The site will automatically redirect to `/ja` (Japanese - default locale).

## Language Routes

- **Japanese**: http://localhost:3000/ja
- **English**: http://localhost:3000/en
- **Azerbaijani**: http://localhost:3000/az

## Features Overview

✅ **Multilingual Support** - Switch between JA, EN, AZ using the language switcher in the navigation bar

✅ **Responsive Design** - Fully responsive across desktop, tablet, and mobile devices

✅ **Modern UI** - Clean, professional design with sakura pink and teal color scheme

✅ **All Sections Included**:
- Hero section with company tagline and CTAs
- Philosophy section
- Mission section
- Vision section (4 items as cards)
- Core Values section (5 value cards with icons)
- Business Domains section (15 services in a grid)
- About section
- Contact section with form
- Footer

✅ **Smooth Navigation** - Sticky navigation bar with smooth scrolling to sections

## Project Structure

```
g-hero-website/
├── app/
│   ├── [locale]/          # Locale-based routing
│   │   ├── layout.tsx     # Server component layout
│   │   ├── layout-client.tsx  # Client component wrapper
│   │   └── page.tsx       # Home page
│   ├── globals.css        # Global styles
│   ├── layout.tsx         # Root layout (html/body)
│   └── page.tsx           # Root redirect page
├── components/            # React components
│   ├── Navigation.tsx     # Top navigation with language switcher
│   ├── Hero.tsx
│   ├── Philosophy.tsx
│   ├── Mission.tsx
│   ├── Vision.tsx
│   ├── CoreValues.tsx
│   ├── BusinessDomains.tsx
│   ├── About.tsx
│   ├── Contact.tsx
│   └── Footer.tsx
├── contexts/
│   └── LocaleContext.tsx  # i18n context provider
├── i18n/
│   ├── config.ts          # Locale configuration
│   └── messages/          # Translation files
│       ├── ja.json        # Japanese
│       ├── en.json        # English
│       └── az.json        # Azerbaijani
└── middleware.ts          # Locale routing middleware
```

## Customization

### Colors

Edit `tailwind.config.ts` to customize the color scheme:
- `sakura.*` - Primary accent (soft pink)
- `teal.*` - Secondary accent

### Content

All content is stored in JSON files:
- `i18n/messages/ja.json` - Japanese content
- `i18n/messages/en.json` - English content
- `i18n/messages/az.json` - Azerbaijani content

Edit these files to update website content.

### Fonts

The site uses Noto Sans fonts (supports Japanese, English, and Azerbaijani) imported in `app/globals.css`.

## Build for Production

```bash
npm run build
npm start
```

## Deployment

This site can be deployed to:
- **Vercel** (recommended for Next.js)
- **Netlify**
- **AWS Amplify**
- Any Node.js hosting platform

For Vercel:
```bash
npm install -g vercel
vercel
```

## Troubleshooting

### Port already in use
Change the port:
```bash
npm run dev -- -p 3001
```

### Styles not loading
Make sure Tailwind CSS is properly configured and rebuild:
```bash
npm run build
```

### Language switcher not working
Clear browser cache and restart the dev server.

## Support

For issues or questions, please check the main README.md file.

