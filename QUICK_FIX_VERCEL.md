# Quick Fix: App Not Opening in Vercel

## Most Common Issue: Missing Environment Variable

**90% of deployment issues are caused by missing `NEXT_PUBLIC_API_URL`**

### Quick Fix Steps:

1. **Go to Vercel Dashboard**
   - Open your project
   - Click "Settings" → "Environment Variables"

2. **Add Environment Variable**
   - **Key**: `NEXT_PUBLIC_API_URL`
   - **Value**: `https://tesseract2024-001-site10.rtempurl.com/api`
   - **Environment**: Select ALL (Production, Preview, Development)
   - Click "Save"

3. **Redeploy**
   - Go to "Deployments" tab
   - Click "..." on latest deployment
   - Click "Redeploy"
   - OR push a new commit to trigger redeploy

4. **Wait for Deployment**
   - Check build logs for errors
   - Once deployed, test the site

## Other Quick Checks:

### Check Build Logs
1. Vercel Dashboard → Deployments → Latest
2. Click "Build Logs"
3. Look for red errors
4. Common errors:
   - TypeScript errors → Fix in code
   - Missing dependencies → Check package.json
   - Build timeout → Check vercel.json settings

### Check Function Logs
1. Vercel Dashboard → Deployments → Latest
2. Click "Functions" tab
3. Check for runtime errors
4. Common errors:
   - API connection failed → Check environment variable
   - CORS errors → Check API CORS settings

### Test API Directly
```bash
# Test if your API is accessible
curl https://tesseract2024-001-site10.rtempurl.com/api/home?lang=en
```

If this fails, your API might be down or the URL is wrong.

## Still Not Working?

1. **Check Browser Console**
   - Open your deployed site
   - Press F12
   - Check Console tab for errors
   - Check Network tab for failed requests

2. **Verify Environment Variable**
   - In browser console, type: `process.env.NEXT_PUBLIC_API_URL`
   - Should show your API URL
   - If `undefined`, environment variable is not set correctly

3. **Check vercel.json**
   - Ensure no syntax errors
   - Remove any invalid properties (like `env` or `build` with environment variables)

4. **Test Build Locally**
   ```bash
   npm run build
   npm start
   ```
   If this fails, fix errors before deploying.

## Emergency Fallback

If the app still won't open, the page will now show content using locale translations even if the API fails. The app should at least display something instead of a blank page.

