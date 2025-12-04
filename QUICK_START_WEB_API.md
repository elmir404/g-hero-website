# Quick Start: Web API + Next.js

## Prerequisites
- .NET 8 SDK
- Node.js 18+ and npm
- SQL Server (LocalDB or SQL Server Express)

## Step 1: Build and Run Web API

```bash
# Restore packages
dotnet restore GHeroWebsite.sln

# Build solution
dotnet build GHeroWebsite.sln

# Run Web API
cd src/GHeroWebsite.WebApi
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `http://localhost:5000/swagger` (in development)

## Step 2: Database Setup

The database will be created automatically on first run. Default admin credentials:
- Username: `admin`
- Password: `Admin123!`

## Step 3: Configure Next.js

1. **Set environment variable** (`.env.local`):
```env
NEXT_PUBLIC_API_URL=http://localhost:5000/api
```

2. **Install dependencies** (if not already done):
```bash
npm install
```

3. **Start Next.js**:
```bash
npm run dev
```

Next.js will be available at `http://localhost:3000`

## Step 4: Test the API

### Using Swagger UI
1. Navigate to `http://localhost:5000/swagger`
2. Try the endpoints:
   - `GET /api/home` - Get home page data
   - `POST /api/contact` - Submit contact form
   - `POST /api/auth/login` - Login (use `admin` / `Admin123!`)

### Using curl

```bash
# Get home page
curl http://localhost:5000/api/home?lang=en

# Submit contact form
curl -X POST http://localhost:5000/api/contact \
  -H "Content-Type: application/json" \
  -d '{"name":"John Doe","email":"john@example.com","message":"Hello"}'

# Login
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin123!"}'
```

## Step 5: Update Next.js Components

1. **Use API client** in your components:
```typescript
import apiClient from '@/lib/api-client';

// In component
const data = await apiClient.getHomePage('en');
```

2. **Update Contact form** to use `apiClient.submitContact()`

3. **Create admin pages** using the example files:
   - `pages/admin/login.tsx.example`
   - `app/page.tsx.example`

## Troubleshooting

### CORS Issues
If you see CORS errors, ensure:
- Web API CORS is configured for `http://localhost:3000`
- `NEXT_PUBLIC_API_URL` is set correctly

### Database Connection
If database errors occur:
- Ensure SQL Server LocalDB is installed
- Check connection string in `appsettings.json`
- Run migrations manually: `dotnet ef database update` (from Infrastructure project)

### JWT Token Issues
- Ensure JWT key in `appsettings.json` is at least 32 characters
- Check token expiration (default: 7 days)
- Verify token is included in Authorization header

## Next Steps

1. Review `WEB_API_ARCHITECTURE.md` for architecture details
2. Check `API_ENDPOINTS_REFERENCE.md` for all endpoints
3. See `NEXTJS_INTEGRATION.md` for Next.js integration examples

