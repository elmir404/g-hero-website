# G-Hero Company Website

A modern, responsive, multilingual website for G-Hero Company built with Next.js, TypeScript, and Tailwind CSS.

## Features

- 🌐 **Multilingual Support**: Japanese, English, and Azerbaijani
- 🎨 **Modern Design**: Clean, professional design with sakura pink and teal color scheme
- 📱 **Fully Responsive**: Optimized for desktop, tablet, and mobile devices
- ⚡ **Fast Performance**: Built with Next.js 14 for optimal performance
- 🎯 **SEO Optimized**: Proper metadata and semantic HTML

## Technology Stack

- **Framework**: Next.js 14 (App Router)
- **Language**: TypeScript
- **Styling**: Tailwind CSS
- **Fonts**: Noto Sans (supports Japanese, English, and Azerbaijani)

## Getting Started

### Prerequisites

- Node.js 18+ 
- npm or yarn

### Installation

1. Install dependencies:
```bash
npm install
```

2. Run the development server:
```bash
npm run dev
```

3. Open [http://localhost:3000](http://localhost:3000) in your browser.

The site will automatically redirect to the default locale (Japanese) at `/ja`.

## Project Structure

```
g-hero-website/
├── app/
│   ├── [locale]/          # Locale-based routing
│   │   ├── layout.tsx     # Locale layout
│   │   └── page.tsx       # Home page
│   ├── globals.css        # Global styles
│   └── layout.tsx         # Root layout
├── components/            # React components
│   ├── Navigation.tsx
│   ├── Hero.tsx
│   ├── Philosophy.tsx
│   ├── Mission.tsx
│   ├── Vision.tsx
│   ├── CoreValues.tsx
│   ├── BusinessDomains.tsx
│   ├── About.tsx
│   ├── Contact.tsx
│   └── Footer.tsx
├── contexts/              # React contexts
│   └── LocaleContext.tsx  # i18n context
├── i18n/                  # Internationalization
│   ├── config.ts          # Locale configuration
│   └── messages/          # Translation files
│       ├── ja.json
│       ├── en.json
│       └── az.json
├── middleware.ts          # Locale routing middleware
└── package.json
```

## Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run start` - Start production server
- `npm run lint` - Run ESLint

## Language Support

The website supports three languages:

- **Japanese (JA)**: `/ja` - Default locale
- **English (EN)**: `/en`
- **Azerbaijani (AZ)**: `/az`

Users can switch languages using the language switcher in the navigation bar.

## Customization

### Colors

The color scheme can be customized in `tailwind.config.ts`:

- **Sakura Pink**: Primary accent color
- **Teal**: Secondary accent color

### Content

All multilingual content is stored in JSON files in `i18n/messages/`. Edit these files to update the website content.

## Deployment

The site can be deployed to any platform that supports Next.js:

- Vercel (recommended)
- Netlify
- AWS Amplify
- Any Node.js hosting platform

For Vercel deployment:

```bash
npm install -g vercel
vercel
```

## License

© 2024 G-Hero Company. All rights reserved.

