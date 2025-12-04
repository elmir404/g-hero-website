# Debug: Contact Messages Not Showing

## Quick Debugging Steps

### 1. Open Browser Console
1. Go to `/admin/contacts`
2. Press F12 to open DevTools
3. Check the **Console** tab for errors
4. Check the **Network** tab for API requests

### 2. Check Network Request
In the Network tab:
1. Look for request to `/api/admin/contact`
2. Click on it to see:
   - **Status Code**: Should be 200 (not 401, 403, or 500)
   - **Request Headers**: Should include `Authorization: Bearer ...`
   - **Response**: Should have `data` array

### 3. Check Authentication
1. Open DevTools → Application → Local Storage
2. Look for `auth_token` key
3. If missing, you need to login again

### 4. Test API Directly

**Option A: Using Browser Console**
```javascript
// Paste this in browser console on /admin/contacts page
const token = localStorage.getItem('auth_token');
fetch('http://localhost:5000/api/admin/contact?page=1&pageSize=20', {
  headers: {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  }
})
.then(r => r.json())
.then(data => console.log('API Response:', data))
.catch(err => console.error('Error:', err));
```

**Option B: Using Swagger**
1. Go to `http://localhost:5000/swagger`
2. Click "Authorize" button
3. Enter: `Bearer YOUR_TOKEN_HERE`
4. Try the `GET /api/admin/contact` endpoint

### 5. Check Database

**If using SQL Server:**
```sql
-- Check if contacts table exists
SELECT * FROM Contacts;

-- Check count
SELECT COUNT(*) as TotalContacts FROM Contacts;

-- Check if any contacts exist
SELECT TOP 10 * FROM Contacts ORDER BY CreatedAt DESC;
```

### 6. Common Issues

#### Issue: 401 Unauthorized
**Cause**: Token expired or invalid
**Fix**: 
- Logout and login again
- Check token in localStorage

#### Issue: 403 Forbidden
**Cause**: User doesn't have Admin role
**Fix**:
- Verify admin user exists
- Check user has "Admin" role in database

#### Issue: Empty Array `[]`
**Cause**: Database is empty (no contact messages)
**Fix**:
- Submit a test message via contact form
- Or manually insert test data

#### Issue: CORS Error
**Cause**: API CORS not configured correctly
**Fix**:
- Check `Program.cs` CORS configuration
- Ensure `http://localhost:3000` is allowed

#### Issue: Network Error
**Cause**: API not running or wrong URL
**Fix**:
- Verify API is running at `http://localhost:5000`
- Check `.env.local` has correct `NEXT_PUBLIC_API_URL`

## Enable Debug Mode

Uncomment the `TestApi` component in `app/admin/contacts/page.tsx` to see detailed console logs:

```tsx
// Uncomment this line:
import TestApi from './test-api';

// And add this in the return:
<TestApi />
```

This will log all API calls and responses to the console.

## Expected Response Format

The API should return:
```json
{
  "data": [
    {
      "id": 1,
      "name": "John Doe",
      "email": "john@example.com",
      "subject": "Inquiry",
      "company": "Company Name",
      "message": "Message content",
      "createdAt": "2024-12-02T10:00:00Z",
      "isRead": false,
      "readAt": null
    }
  ],
  "total": 1,
  "page": 1,
  "pageSize": 20,
  "totalPages": 1
}
```

## Still Not Working?

1. **Share the browser console errors**
2. **Share the Network tab details** for the `/api/admin/contact` request
3. **Check if Web API is running** and accessible
4. **Verify you're logged in** as admin

