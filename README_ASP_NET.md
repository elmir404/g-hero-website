# G-Hero Website - ASP.NET Core MVC

This is the ASP.NET Core MVC version of the G-Hero Company website, converted from the original Next.js project.

## Project Structure

```
GHeroWebsite/
├── src/
│   ├── GHeroWebsite.Domain/          # Domain entities
│   │   └── Entities/
│   │       ├── Contact.cs
│   │       └── User.cs
│   ├── GHeroWebsite.Infrastructure/   # Data access layer
│   │   └── Data/
│   │       └── ApplicationDbContext.cs
│   ├── GHeroWebsite.Application/      # Business logic layer
│   │   ├── DTOs/
│   │   │   └── ContactDto.cs
│   │   └── Services/
│   │       ├── IContactService.cs
│   │       ├── ContactService.cs
│   │       ├── IUserService.cs
│   │       └── UserService.cs
│   └── GHeroWebsite.Web/              # MVC presentation layer
│       ├── Controllers/
│       │   ├── HomeController.cs
│       │   ├── ContactController.cs
│       │   ├── AccountController.cs
│       │   └── Admin/
│       │       ├── DashboardController.cs
│       │       └── ContactMessagesController.cs
│       ├── Views/
│       │   ├── Home/
│       │   ├── Contact/
│       │   ├── Account/
│       │   └── Admin/
│       ├── wwwroot/
│       │   ├── css/
│       │   ├── js/
│       │   └── assets/
│       └── Program.cs
└── GHeroWebsite.sln
```

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB is fine for development)
- Visual Studio 2022 or VS Code

### Step 1: Restore Dependencies
```bash
dotnet restore
```

### Step 2: Update Connection String
Edit `src/GHeroWebsite.Web/appsettings.json` and update the connection string if needed:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GHeroWebsiteDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Step 3: Create Database and Run Migrations
```bash
cd src/GHeroWebsite.Web
dotnet ef migrations add InitialCreate --project ../GHeroWebsite.Infrastructure
dotnet ef database update
```

Or, the application will automatically create the database on first run (using `EnsureCreated()`).

### Step 4: Run the Application
```bash
cd src/GHeroWebsite.Web
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.

## Default Admin Credentials

- **Username:** `admin`
- **Password:** `Admin123!`

**Important:** Change these credentials in production!

## Features

### Public Pages
- **Home Page** - Main landing page with all sections (Hero, Philosophy, Mission, Vision, Core Values, Business Domains, About, Contact)
- **Contact Form** - Submit contact messages with validation

### Admin Panel
- **Login** - Simple cookie-based authentication
- **Dashboard** - Overview of contact messages with statistics
- **Contact Messages** - List all messages
- **Message Details** - View, mark as read/unread, and delete messages

## Architecture

### Domain Layer
Contains the core entities:
- `Contact` - Contact form submissions
- `User` - Admin users

### Infrastructure Layer
- `ApplicationDbContext` - EF Core DbContext
- Database configuration and migrations

### Application Layer
- DTOs for data transfer
- Services for business logic:
  - `ContactService` - Contact message management
  - `UserService` - User authentication and management

### Web Layer
- MVC Controllers
- Razor Views
- Static assets (CSS, JS, images)

## Database Schema

### Contact Table
- `Id` (int, PK)
- `Name` (string, required, max 200)
- `Email` (string, required, max 200)
- `Company` (string, optional, max 200)
- `Message` (string, required)
- `CreatedAt` (DateTime, UTC)
- `IsRead` (bool)
- `ReadAt` (DateTime?, nullable)

### User Table
- `Id` (int, PK)
- `Username` (string, unique, max 100)
- `Email` (string, unique, max 200)
- `PasswordHash` (string, max 500)
- `FullName` (string, optional, max 200)
- `IsActive` (bool)
- `CreatedAt` (DateTime, UTC)
- `LastLoginAt` (DateTime?, nullable)
- `Role` (string, max 50)

## Adding More Entities

To add new entities:

1. Create entity in `GHeroWebsite.Domain/Entities/`
2. Add DbSet to `ApplicationDbContext`
3. Configure in `OnModelCreating` if needed
4. Create migration: `dotnet ef migrations add AddNewEntity`
5. Update database: `dotnet ef database update`

## UI Styling

The project uses Tailwind CSS via CDN to maintain the same visual design as the Next.js version. Custom colors (sakura and teal) are defined inline in the layout.

## Security Notes

- Passwords are hashed using SHA256 (consider upgrading to bcrypt/Argon2 for production)
- Authentication uses cookie-based sessions
- Admin routes are protected with `[Authorize]` attribute
- CSRF protection via AntiForgeryToken

## Next Steps

1. **Production Database**: Set up proper SQL Server connection string
2. **Password Hashing**: Upgrade to bcrypt or Argon2
3. **Email Notifications**: Add email service for contact form submissions
4. **Logging**: Configure proper logging (Serilog, NLog, etc.)
5. **Error Handling**: Add global error handling middleware
6. **Environment Variables**: Move sensitive config to environment variables or Azure Key Vault

