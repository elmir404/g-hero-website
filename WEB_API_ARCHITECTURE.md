# Web API + Next.js Architecture Guide

## Overview

The project has been refactored into a **clean separation**:
- **Backend**: ASP.NET Core Web API (pure JSON endpoints)
- **Frontend**: Next.js (React components, pages, routing)

## Project Structure

```
GHeroWebsite/
├── src/
│   ├── GHeroWebsite.Domain/          # Entities
│   │   └── Entities/
│   │       ├── Contact.cs
│   │       ├── ContentSection.cs
│   │       ├── HomeHeroSection.cs
│   │       ├── HomeService.cs
│   │       ├── HomeTestimonial.cs
│   │       ├── HomeFaq.cs
│   │       ├── HomeContactInfo.cs
│   │       └── ApplicationUser.cs (Identity)
│   ├── GHeroWebsite.Infrastructure/   # EF Core, DbContext
│   │   └── Data/
│   │       └── ApplicationDbContext.cs
│   ├── GHeroWebsite.Application/     # Business Logic
│   │   ├── DTOs/
│   │   │   ├── ContactRequestDto.cs
│   │   │   ├── ContactResponseDto.cs
│   │   │   ├── HomePageDto.cs
│   │   │   ├── LoginRequestDto.cs
│   │   │   └── LoginResponseDto.cs
│   │   └── Services/
│   │       ├── ContactService.cs
│   │       ├── ContentService.cs
│   │       ├── HomePageService.cs
│   │       └── AuthService.cs
│   └── GHeroWebsite.WebApi/           # API Controllers
│       ├── Controllers/
│       │   ├── HomeController.cs
│       │   ├── ContactController.cs
│       │   ├── AuthController.cs
│       │   └── Admin/
│       │       ├── ContactMessagesController.cs
│       │       └── ContentController.cs
│       └── Program.cs
└── (Next.js project root)
    ├── lib/
    │   └── api-client.ts
    ├── pages/
    │   └── api/
    │       └── home.ts
    ├── components/
    │   └── Contact.tsx (updated)
    └── app/
        └── page.tsx (updated)
```

## Web API Endpoints

### Public Endpoints

#### Home Page
- `GET /api/home?lang=en` - Get complete home page data
- `GET /api/home/hero?lang=en` - Get hero section
- `GET /api/home/services?lang=en` - Get services list
- `GET /api/home/testimonials?lang=en` - Get testimonials
- `GET /api/home/faqs?lang=en` - Get FAQs
- `GET /api/home/contact-info?lang=en` - Get contact information

#### Contact
- `POST /api/contact` - Submit contact form
  ```json
  {
    "name": "John Doe",
    "email": "john@example.com",
    "subject": "Inquiry",
    "company": "Company Name",
    "message": "Message content"
  }
  ```

#### Authentication
- `POST /api/auth/login` - Login and get JWT token
  ```json
  {
    "username": "admin",
    "password": "Admin123!"
  }
  ```
  Returns:
  ```json
  {
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "username": "admin",
    "email": "admin@g-hero-company.com",
    "fullName": "Administrator",
    "roles": ["Admin"],
    "expiresAt": "2024-12-09T10:00:00Z"
  }
  ```

- `GET /api/auth/me` - Get current user (requires auth)

### Admin Endpoints (Protected - Requires JWT Token)

#### Contact Messages
- `GET /api/admin/contact?page=1&pageSize=20` - List all messages (paginated)
- `GET /api/admin/contact/unread` - Get unread messages
- `GET /api/admin/contact/{id}` - Get message details
- `PATCH /api/admin/contact/{id}/read` - Mark as read
- `PATCH /api/admin/contact/{id}/unread` - Mark as unread
- `DELETE /api/admin/contact/{id}` - Delete message

#### Content Management
- `GET /api/admin/content?lang=en` - List all content sections
- `GET /api/admin/content/{id}` - Get section by ID
- `POST /api/admin/content` - Create new section
- `PUT /api/admin/content/{id}` - Update section
- `DELETE /api/admin/content/{id}` - Delete section
- `PATCH /api/admin/content/{id}/toggle-active` - Toggle active status

## Authentication

### JWT Configuration

The API uses JWT Bearer authentication. Configure in `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLongForJWTTokenGeneration!",
    "Issuer": "GHeroWebsite",
    "Audience": "GHeroWebsite"
  }
}
```

### Using JWT Token

1. **Login** to get token:
   ```typescript
   const response = await apiClient.login('admin', 'Admin123!');
   // Token is automatically stored in apiClient
   ```

2. **Include token** in requests:
   ```typescript
   // apiClient automatically adds: Authorization: Bearer {token}
   const messages = await apiClient.getContactMessages();
   ```

## Next.js Integration

### API Client Setup

1. **Create API client** (`lib/api-client.ts`):
   - Handles all API calls
   - Manages JWT token storage
   - Provides typed methods

2. **Environment variable**:
   ```env
   NEXT_PUBLIC_API_URL=http://localhost:5000/api
   ```

### Using in Components

```typescript
import apiClient from '@/lib/api-client';

// In component
const [homeData, setHomeData] = useState(null);

useEffect(() => {
  async function loadData() {
    const data = await apiClient.getHomePage('en');
    setHomeData(data);
  }
  loadData();
}, []);
```

### Server-Side Rendering (SSR)

For Next.js App Router (server components):

```typescript
// app/page.tsx
export default async function HomePage({ 
  searchParams 
}: { 
  searchParams: { lang?: string } 
}) {
  const lang = searchParams.lang || 'en';
  const homeData = await fetch(`${process.env.API_URL}/home?lang=${lang}`)
    .then(res => res.json());
  
  return <HomePageClient data={homeData} />;
}
```

## Database Entities

### Contact
- `Id`, `Name`, `Email`, `Subject`, `Company`, `Message`, `CreatedAt`, `IsRead`, `ReadAt`

### ContentSection
- `Id`, `SectionKey`, `LanguageCode`, `Title`, `Content`, `Subtitle`, `AdditionalData`, `DisplayOrder`, `IsActive`

### HomeHeroSection
- `Id`, `LanguageCode`, `Title`, `Subtitle`, `Description`, `PrimaryButtonText`, `PrimaryButtonUrl`, `SecondaryButtonText`, `SecondaryButtonUrl`, `BackgroundImageUrl`

### HomeService
- `Id`, `LanguageCode`, `Title`, `Description`, `IconName`, `IconUrl`, `ImageUrl`, `DisplayOrder`

### HomeTestimonial
- `Id`, `LanguageCode`, `AuthorName`, `AuthorTitle`, `AuthorCompany`, `AuthorImageUrl`, `Content`, `Rating`

### HomeFaq
- `Id`, `LanguageCode`, `Question`, `Answer`, `DisplayOrder`

### HomeContactInfo
- `Id`, `LanguageCode`, `Address`, `Phone`, `Email`, `Website`, `FacebookUrl`, `TwitterUrl`, `LinkedInUrl`, `InstagramUrl`

## Running the Projects

### 1. Start Web API

```bash
cd src/GHeroWebsite.WebApi
dotnet run
```

API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `http://localhost:5000/swagger` (in development)

### 2. Start Next.js

```bash
npm run dev
```

Next.js will be available at:
- `http://localhost:3000`

### 3. Configure CORS

The Web API is configured to allow requests from:
- `http://localhost:3000`
- `http://localhost:3001`

Update `Program.cs` if you need different origins.

## Next Steps

1. **Update Next.js components** to use API client
2. **Create admin pages** in Next.js
3. **Add more homepage section entities** as needed
4. **Implement image upload** for sections
5. **Add caching** for better performance

