# ASP.NET Core MVC Setup Guide

This guide will walk you through setting up and running the ASP.NET Core MVC version of the G-Hero website.

## Prerequisites

1. **.NET 8 SDK** - Download from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
2. **SQL Server** - SQL Server Express or LocalDB (included with Visual Studio)
3. **IDE** - Visual Studio 2022, VS Code, or Rider

## Step-by-Step Setup

### 1. Verify .NET SDK Installation

Open a terminal and run:
```bash
dotnet --version
```

You should see version 8.0.x or higher.

### 2. Restore NuGet Packages

Navigate to the solution directory and restore packages:
```bash
cd C:\Users\oilin\g-hero-website
dotnet restore
```

### 3. Copy Logo Assets

Copy the logo files from the original Next.js project:
```bash
# Windows PowerShell
Copy-Item -Path "public\assets\images\logo.webp" -Destination "src\GHeroWebsite.Web\wwwroot\assets\images\logo.webp"
Copy-Item -Path "public\assets\images\logo.svg" -Destination "src\GHeroWebsite.Web\wwwroot\assets\images\logo.svg" -ErrorAction SilentlyContinue
```

Or manually copy:
- `public/assets/images/logo.webp` → `src/GHeroWebsite.Web/wwwroot/assets/images/logo.webp`
- `public/assets/images/logo.svg` → `src/GHeroWebsite.Web/wwwroot/assets/images/logo.svg` (if it exists)

### 4. Configure Database Connection

Edit `src/GHeroWebsite.Web/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GHeroWebsiteDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

For SQL Server Express, use:
```json
"Server=localhost\\SQLEXPRESS;Database=GHeroWebsiteDb;Trusted_Connection=True;MultipleActiveResultSets=true"
```

### 5. Install EF Core Tools (if not already installed)

```bash
dotnet tool install --global dotnet-ef
```

### 6. Create Database Migration

```bash
cd src/GHeroWebsite.Web
dotnet ef migrations add InitialCreate --project ../GHeroWebsite.Infrastructure
```

### 7. Apply Migration to Database

```bash
dotnet ef database update --project ../GHeroWebsite.Infrastructure
```

**Note:** Alternatively, the application will automatically create the database on first run using `EnsureCreated()`, but migrations are the recommended approach for production.

### 8. Run the Application

```bash
cd src/GHeroWebsite.Web
dotnet run
```

Or from Visual Studio:
- Open `GHeroWebsite.sln`
- Set `GHeroWebsite.Web` as the startup project
- Press F5 to run

The application will be available at:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`

### 9. Access the Application

- **Public Site:** Navigate to `https://localhost:5001`
- **Admin Login:** Navigate to `https://localhost:5001/Account/Login`
  - Username: `admin`
  - Password: `Admin123!`

## Project Structure Overview

```
GHeroWebsite/
├── src/
│   ├── GHeroWebsite.Domain/          # Domain entities (Contact, User)
│   ├── GHeroWebsite.Infrastructure/ # EF Core, DbContext, data access
│   ├── GHeroWebsite.Application/     # Business logic, services, DTOs
│   └── GHeroWebsite.Web/             # MVC controllers, views, static files
└── GHeroWebsite.sln                  # Solution file
```

## Key Features

### Public Pages
- **Home Page** (`/`) - All sections: Hero, Philosophy, Mission, Vision, Core Values, Business Domains, About, Contact
- **Contact Form** - Validated form that saves to database

### Admin Panel
- **Dashboard** (`/Dashboard`) - Overview with statistics
- **Contact Messages** (`/Admin/ContactMessages`) - List all messages
- **Message Details** (`/Admin/ContactMessages/Details/{id}`) - View and manage individual messages

### Authentication
- Cookie-based authentication
- Simple password hashing (SHA256 - upgrade for production)
- Protected admin routes with `[Authorize]` attribute

## Troubleshooting

### Database Connection Issues

**Error:** "Cannot open database"
- Ensure SQL Server LocalDB is installed
- Check connection string in `appsettings.json`
- Try creating the database manually:
  ```sql
  CREATE DATABASE GHeroWebsiteDb;
  ```

### Migration Issues

**Error:** "No migrations found"
- Ensure you're in the `src/GHeroWebsite.Web` directory
- Check that `GHeroWebsite.Infrastructure` project references are correct

### Port Already in Use

**Error:** "Address already in use"
- Change ports in `Properties/launchSettings.json` or
- Kill the process using the port:
  ```bash
  # Windows
  netstat -ano | findstr :5000
  taskkill /PID <PID> /F
  ```

### Logo Not Displaying

- Ensure logo files are copied to `wwwroot/assets/images/`
- Check file paths in `_Layout.cshtml`
- Verify static file serving is enabled (it is by default)

## Next Steps

1. **Change Default Admin Password** - Update the seeded admin user password
2. **Configure Production Database** - Set up proper SQL Server connection
3. **Add Email Notifications** - Send emails when contact form is submitted
4. **Improve Password Security** - Upgrade from SHA256 to bcrypt/Argon2
5. **Add Logging** - Configure Serilog or NLog
6. **Environment Configuration** - Use environment variables for sensitive data

## Development Tips

- Use `dotnet watch run` for hot reload during development
- Check `appsettings.Development.json` for development-specific settings
- Use SQL Server Management Studio (SSMS) to inspect the database
- Enable detailed error pages in development (already configured)

## Production Deployment

1. Update connection string to production database
2. Change admin password
3. Upgrade password hashing
4. Configure HTTPS
5. Set up proper logging
6. Use environment variables for secrets
7. Configure CORS if needed
8. Set up CI/CD pipeline

