# Vercel Deployment Checklist

## ✅ Fixed Issues

1. **Layout Params** - Updated to async params (Next.js 14 requirement)
2. **Logo Image** - Replaced with text logo to avoid missing file errors
3. **API Client** - Added localStorage error handling
4. **Error Handling** - App now renders even if API fails
5. **vercel.json** - Created minimal configuration

## 🔧 Required Steps Before Deployment

### 1. Set Environment Variable in Vercel

**CRITICAL**: This is the #1 cause of deployment failures.

1. Go to [Vercel Dashboard](https://vercel.com/dashboard)
2. Select your project
3. Go to **Settings** → **Environment Variables**
4. Add:
   - **Key**: `NEXT_PUBLIC_API_URL`
   - **Value**: `https://tesseract2024-001-site10.rtempurl.com/api`
   - **Environment**: Select ALL (Production, Preview, Development)
5. Click **Save**

### 2. Verify Build Works Locally

```bash
# Install dependencies
npm install

# Test build
npm run build

# Test production build
npm start
```

If build fails locally, fix errors before deploying.

### 3. Check for Missing Files

Ensure these files exist:
- ✅ `package.json`
- ✅ `next.config.js`
- ✅ `tailwind.config.ts`
- ✅ `postcss.config.js`
- ✅ `tsconfig.json`
- ✅ `vercel.json` (just created)
- ✅ `app/layout.tsx`
- ✅ `app/page.tsx`
- ✅ `app/[locale]/layout.tsx`
- ✅ `app/[locale]/page.tsx`
- ✅ `middleware.ts`

### 4. Deploy to Vercel

**Option A: Via Git (Recommended)**
1. Push your code to GitHub/GitLab/Bitbucket
2. Connect repository to Vercel
3. Vercel will auto-deploy

**Option B: Via Vercel CLI**
```bash
npm install -g vercel
vercel
```

## 🐛 Troubleshooting

### Issue: "Site can't be reached" / "Connection closed"

**Possible Causes:**

1. **Missing Environment Variable** (90% of cases)
   - Solution: Set `NEXT_PUBLIC_API_URL` in Vercel Dashboard
   - Redeploy after setting

2. **Build Failure**
   - Check Vercel Dashboard → Deployments → Build Logs
   - Look for TypeScript errors, missing dependencies
   - Fix errors and redeploy

3. **Runtime Error**
   - Check Vercel Dashboard → Deployments → Function Logs
   - Look for unhandled exceptions
   - Check browser console (F12)

4. **API Connection Failed**
   - Verify API is accessible: `curl https://tesseract2024-001-site10.rtempurl.com/api/home?lang=en`
   - Check CORS settings on API
   - Verify API URL in environment variable

### Issue: Blank Page

**Solution:**
- The app now has fallback content using locale translations
- Even if API fails, the page should render
- Check browser console for specific errors

### Issue: Build Timeout

**Solution:**
- Check `vercel.json` function timeout settings
- Optimize build process
- Remove unnecessary dependencies

## 📋 Pre-Deployment Checklist

- [ ] Environment variable `NEXT_PUBLIC_API_URL` is set in Vercel
- [ ] `npm run build` succeeds locally
- [ ] `npm start` works locally
- [ ] No TypeScript errors
- [ ] No missing dependencies
- [ ] API is accessible and returns data
- [ ] Code is pushed to Git repository (if using Git deployment)

## 🚀 After Deployment

1. **Test the Site**
   - Visit your Vercel URL
   - Check all pages load
   - Test language switcher
   - Test contact form

2. **Check Logs**
   - Vercel Dashboard → Deployments → Latest
   - Review Build Logs
   - Review Function Logs

3. **Monitor Errors**
   - Check browser console
   - Check Network tab for failed requests
   - Monitor Vercel Analytics

## 📞 Still Having Issues?

1. **Check Vercel Status**: https://vercel-status.com
2. **Review Build Logs**: Detailed error messages
3. **Test API Directly**: Verify API is working
4. **Check Environment Variables**: Ensure they're set correctly
5. **Review Code Changes**: Ensure all fixes are committed

## 🔍 Quick Diagnostic Commands

```bash
# Test build locally
npm run build

# Test production server locally
npm start

# Check for TypeScript errors
npx tsc --noEmit

# Check for linting errors
npm run lint

# Test API connection
curl https://tesseract2024-001-site10.rtempurl.com/api/home?lang=en
```

