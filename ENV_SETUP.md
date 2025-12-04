# Environment Variables Setup

## Required Environment Variables

For the Next.js frontend to connect to the .NET Web API, you need to set up environment variables.

## Quick Setup

1. **Create `.env.local` file** in the root of your Next.js project (same level as `package.json`)

2. **Add the following variable:**

```env
NEXT_PUBLIC_API_URL=http://localhost:5000/api
```

## File Structure

```
g-hero-website/
├── .env.local          # Your local environment variables (not committed to git)
├── env.example         # Example file (committed to git)
├── package.json
└── ...
```

## Environment Variables

### `NEXT_PUBLIC_API_URL`

- **Description**: Base URL for the .NET Web API
- **Development**: `http://localhost:5000/api`
- **Production**: `https://your-api-domain.com/api`
- **Required**: Yes
- **Note**: The `NEXT_PUBLIC_` prefix makes it available in the browser

## Setup Instructions

### Option 1: Create `.env.local` manually

1. Create a file named `.env.local` in the project root
2. Add the content:
   ```env
   NEXT_PUBLIC_API_URL=http://localhost:5000/api
   ```
3. Save the file
4. Restart your Next.js dev server

### Option 2: Copy from example

```bash
# Copy the example file
cp env.example .env.local

# Edit .env.local with your values
# (or it will use the defaults from env.example)
```

## Development vs Production

### Development
```env
NEXT_PUBLIC_API_URL=http://localhost:5000/api
```

### Production
```env
NEXT_PUBLIC_API_URL=https://api.yourdomain.com/api
```

## Important Notes

1. **`.env.local` is gitignored** - Your local environment variables won't be committed
2. **Restart required** - After changing `.env.local`, restart your Next.js dev server
3. **`NEXT_PUBLIC_` prefix** - Only variables with this prefix are exposed to the browser
4. **No quotes needed** - Don't wrap values in quotes unless the value itself contains spaces

## Verification

To verify your environment variables are loaded:

1. Check the browser console - the API client should use the correct URL
2. Check Network tab - API calls should go to the URL you specified
3. If using default (`http://localhost:5000/api`), it will work without `.env.local` but it's better to be explicit

## Troubleshooting

### API calls failing?

1. Check that `.env.local` exists in the project root
2. Verify `NEXT_PUBLIC_API_URL` is set correctly
3. Make sure the Web API is running on the specified port
4. Restart Next.js dev server after changing `.env.local`
5. Check browser console for errors

### CORS errors?

- Ensure the Web API CORS is configured for your Next.js origin
- Check `src/GHeroWebsite.WebApi/Program.cs` CORS configuration

## Example `.env.local`

```env
# Web API Configuration
NEXT_PUBLIC_API_URL=http://localhost:5000/api

# Optional: Add other variables as needed
# NODE_ENV=development
```

