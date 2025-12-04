# Vercel Deployment Troubleshooting Guide

## Common Issues and Solutions

### 1. App Not Opening / Blank Page

**Symptoms:**
- Page loads but shows blank screen
- Browser console shows errors
- Build succeeds but app doesn't render

**Solutions:**

#### A. Check Build Logs
1. Go to Vercel Dashboard → Your Project → Deployments
2. Click on the latest deployment
3. Check the "Build Logs" tab for errors
4. Look for TypeScript errors, missing dependencies, or build failures

#### B. Check Runtime Logs
1. In Vercel Dashboard → Deployments → Latest
2. Click "Functions" tab
3. Check for runtime errors
4. Look for API connection errors or unhandled exceptions

#### C. Check Browser Console
1. Open your deployed site
2. Press F12 to open DevTools
3. Check Console tab for JavaScript errors
4. Check Network tab for failed API requests

### 2. API Connection Errors

**Symptoms:**
- Page shows "Error: Failed to load content"
- Network tab shows 404 or CORS errors
- API calls failing

**Solutions:**

#### A. Set Environment Variable
1. Go to Vercel Dashboard → Settings → Environment Variables
2. Add: `NEXT_PUBLIC_API_URL` = `https://tesseract2024-001-site10.rtempurl.com/api`
3. Select all environments (Production, Preview, Development)
4. **Redeploy** after adding variables

#### B. Verify API is Accessible
```bash
# Test if API is accessible
curl https://tesseract2024-001-site10.rtempurl.com/api/home?lang=en
```

#### C. Check CORS Configuration
- Ensure your API allows requests from your Vercel domain
- Check API CORS settings in `Program.cs`

### 3. Build Errors

**Common Build Errors:**

#### A. TypeScript Errors
```bash
# Test build locally first
npm run build
```
Fix any TypeScript errors before deploying.

#### B. Missing Dependencies
```bash
# Ensure all dependencies are in package.json
npm install
npm run build
```

#### C. Image Domain Not Configured
If you see image errors, ensure `next.config.js` has:
```js
images: {
  remotePatterns: [
    {
      protocol: 'https',
      hostname: 'images.unsplash.com',
    },
  ],
}
```

### 4. Routing Issues

**Symptoms:**
- 404 errors
- Redirect loops
- Locale routing not working

**Solutions:**

#### A. Check Middleware
- Ensure `middleware.ts` is in the root directory
- Check that locale routing is configured correctly

#### B. Check Default Locale
- Verify `i18n/config.ts` has correct `defaultLocale`
- Ensure default locale matches your content

### 5. Environment Variables Not Working

**Symptoms:**
- API calls go to wrong URL
- `process.env.NEXT_PUBLIC_API_URL` is undefined

**Solutions:**

#### A. Verify Variable Name
- Must start with `NEXT_PUBLIC_` for client-side access
- Case-sensitive: `NEXT_PUBLIC_API_URL` not `next_public_api_url`

#### B. Redeploy After Changes
- Environment variables require a new deployment
- Go to Deployments → Click "Redeploy"

#### C. Check Variable Scope
- Set for all environments: Production, Preview, Development
- Or set specific values for each environment

### 6. Page Crashes on Load

**Symptoms:**
- Page loads then immediately shows error
- Console shows unhandled promise rejection

**Solutions:**

#### A. Improve Error Handling
- Check `app/[locale]/page.tsx` error handling
- Ensure API failures don't crash the page

#### B. Add Fallback Content
- Show default content if API fails
- Use locale translations as fallback

## Quick Diagnostic Steps

### Step 1: Check Build Status
```bash
# Build locally to catch errors
npm run build
```

### Step 2: Check Vercel Logs
1. Vercel Dashboard → Project → Deployments
2. Click latest deployment
3. Check "Build Logs" and "Function Logs"

### Step 3: Test API Connection
```bash
# Test API endpoint
curl https://tesseract2024-001-site10.rtempurl.com/api/home?lang=en
```

### Step 4: Check Environment Variables
1. Vercel Dashboard → Settings → Environment Variables
2. Verify `NEXT_PUBLIC_API_URL` is set
3. Check it's set for the correct environment

### Step 5: Check Browser Console
1. Open deployed site
2. F12 → Console tab
3. Look for errors
4. F12 → Network tab
5. Check failed requests

## Debugging Checklist

- [ ] Build succeeds locally (`npm run build`)
- [ ] Environment variable `NEXT_PUBLIC_API_URL` is set in Vercel
- [ ] Environment variable is set for all environments
- [ ] Redeployed after setting environment variables
- [ ] API is accessible and returns data
- [ ] CORS is configured on API for Vercel domain
- [ ] No TypeScript errors in build logs
- [ ] No runtime errors in function logs
- [ ] Browser console shows no JavaScript errors
- [ ] Network tab shows successful API calls

## Getting More Help

1. **Check Vercel Status**: https://vercel-status.com
2. **Vercel Documentation**: https://vercel.com/docs
3. **Next.js Documentation**: https://nextjs.org/docs
4. **Check Build Logs**: Vercel Dashboard → Deployments → Build Logs

## Common Fixes

### Fix 1: Add Error Boundary
Create `app/error.tsx` to catch errors gracefully.

### Fix 2: Improve API Error Handling
Update `app/[locale]/page.tsx` to handle API failures better.

### Fix 3: Add Loading States
Show proper loading indicators while data is fetching.

### Fix 4: Add Fallback Content
Use locale translations if API data is unavailable.

