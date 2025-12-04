# Troubleshooting: Contact Messages Not Showing

## Common Issues and Solutions

### 1. Authentication Issue

**Problem**: JWT token is missing or expired.

**Check**:
- Open browser DevTools → Application → Local Storage
- Look for `auth_token` key
- If missing or expired, you need to login again

**Solution**:
1. Go to `/admin/login`
2. Login with `admin` / `Admin123!`
3. Token will be saved automatically
4. Try accessing `/admin/contacts` again

### 2. API Not Running

**Check**:
- Is the Web API running? Should be at `http://localhost:5000`
- Check browser console for network errors
- Check if API endpoint is accessible: `http://localhost:5000/api/admin/contact`

**Solution**:
```bash
cd src/GHeroWebsite.WebApi
dotnet run
```

### 3. CORS Error

**Check**:
- Browser console shows CORS error
- Network tab shows preflight request failed

**Solution**:
- Ensure Web API CORS is configured for `http://localhost:3000`
- Check `Program.cs` CORS configuration

### 4. No Messages in Database

**Check**:
- Database might be empty
- No contact form submissions yet

**Solution**:
1. Submit a test message via the contact form on the website
2. Or check database directly:
   ```sql
   SELECT * FROM Contacts;
   ```

### 5. Wrong API URL

**Check**:
- `.env.local` file has correct `NEXT_PUBLIC_API_URL`
- Should be: `http://localhost:5000/api`

**Solution**:
1. Check `.env.local` file
2. Restart Next.js dev server after changing

### 6. Authorization Error (403/401)

**Problem**: User doesn't have Admin role or token is invalid.

**Check**:
- Browser console for 401/403 errors
- Network tab shows unauthorized response

**Solution**:
1. Logout and login again
2. Verify admin user exists in database
3. Check user has "Admin" role

## Debugging Steps

### Step 1: Check Browser Console
Open DevTools (F12) → Console tab
- Look for JavaScript errors
- Look for API call errors

### Step 2: Check Network Tab
Open DevTools → Network tab
- Find the request to `/api/admin/contact`
- Check:
  - Status code (should be 200)
  - Request headers (should include `Authorization: Bearer ...`)
  - Response body (should contain `data` array)

### Step 3: Test API Directly

**Using curl**:
```bash
# First, get token by logging in
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin123!"}'

# Then use the token
curl http://localhost:5000/api/admin/contact \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

**Using Swagger UI**:
1. Go to `http://localhost:5000/swagger`
2. Click "Authorize" button
3. Enter: `Bearer YOUR_TOKEN`
4. Try the `/api/admin/contact` endpoint

### Step 4: Check Database

```sql
-- Check if contacts exist
SELECT COUNT(*) FROM Contacts;

-- Check if admin user exists
SELECT * FROM AspNetUsers WHERE UserName = 'admin';

-- Check if admin has Admin role
SELECT u.UserName, r.Name 
FROM AspNetUsers u
JOIN AspNetUserRoles ur ON u.Id = ur.UserId
JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.UserName = 'admin';
```

## Quick Fix Checklist

- [ ] Web API is running (`http://localhost:5000`)
- [ ] Next.js is running (`http://localhost:3000`)
- [ ] Logged in as admin (`/admin/login`)
- [ ] Token exists in localStorage
- [ ] `.env.local` has correct API URL
- [ ] Database has contact messages
- [ ] No CORS errors in console
- [ ] Network requests return 200 status

## Still Not Working?

1. **Check browser console** for specific error messages
2. **Check Network tab** to see the actual API response
3. **Verify token** is being sent in Authorization header
4. **Test API directly** using Swagger or curl
5. **Check database** to ensure contacts exist

