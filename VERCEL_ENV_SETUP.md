# Vercel Environment Variables Setup

This guide explains how to set up environment variables for your G-Hero Website project on Vercel.

## Required Environment Variables

### 1. `NEXT_PUBLIC_API_URL`
- **Description**: The base URL for your ASP.NET Core Web API
- **Development**: `http://localhost:5000/api`
- **Production**: `https://your-api-domain.com/api`
- **Example**: `https://api.g-hero-company.com/api`

## Setting Environment Variables in Vercel

### Option 1: Via Vercel Dashboard (Recommended)

1. Go to your project on [Vercel Dashboard](https://vercel.com/dashboard)
2. Navigate to **Settings** → **Environment Variables**
3. Add each environment variable:
   - **Key**: `NEXT_PUBLIC_API_URL`
   - **Value**: Your API URL (e.g., `https://api.g-hero-company.com/api`)
   - **Environment**: Select all (Production, Preview, Development)
4. Click **Save**

### Option 2: Via Vercel CLI

```bash
# Install Vercel CLI if not already installed
npm i -g vercel

# Login to Vercel
vercel login

# Link your project (if not already linked)
vercel link

# Set environment variable for production
vercel env add NEXT_PUBLIC_API_URL production

# Set environment variable for preview
vercel env add NEXT_PUBLIC_API_URL preview

# Set environment variable for development
vercel env add NEXT_PUBLIC_API_URL development
```

### Option 3: Using Vercel CLI with .env file

Create a `.env.production` file:
```env
NEXT_PUBLIC_API_URL=https://api.g-hero-company.com/api
```

Then push to Vercel:
```bash
vercel env pull .env.production
```

## Environment-Specific Values

You can set different values for different environments:

- **Production**: Your production API URL
- **Preview**: Your staging/preview API URL (or same as production)
- **Development**: `http://localhost:5000/api` (for local development)

## Important Notes

1. **Public Variables**: Variables prefixed with `NEXT_PUBLIC_` are exposed to the browser. Only use this prefix for variables that are safe to expose.

2. **Redeploy Required**: After adding or updating environment variables, you need to redeploy your application for changes to take effect.

3. **API CORS**: Make sure your ASP.NET Core Web API has CORS configured to allow requests from your Vercel domain:
   ```csharp
   // In Program.cs
   builder.Services.AddCors(options =>
   {
       options.AddPolicy("AllowVercel", policy =>
       {
           policy.WithOrigins("https://your-vercel-domain.vercel.app")
                 .AllowAnyHeader()
                 .AllowAnyMethod()
                 .AllowCredentials();
       });
   });
   ```

4. **Local Development**: For local development, create a `.env.local` file:
   ```env
   NEXT_PUBLIC_API_URL=http://localhost:5000/api
   ```
   This file is already in `.gitignore` and won't be committed.

## Verifying Environment Variables

After deployment, you can verify your environment variables are set correctly by:

1. Checking the Vercel dashboard under Settings → Environment Variables
2. Viewing the build logs during deployment
3. Using the browser console to check `process.env.NEXT_PUBLIC_API_URL` (only for public variables)

## Troubleshooting

### Environment variable not working?
- Make sure you've redeployed after adding the variable
- Check that the variable name matches exactly (case-sensitive)
- Verify the variable is set for the correct environment (Production/Preview/Development)
- For `NEXT_PUBLIC_` variables, ensure they're set before the build process

### API calls failing?
- Verify CORS is configured correctly on your API
- Check that the API URL is correct and accessible
- Ensure the API is running and accessible from the internet (for production)

