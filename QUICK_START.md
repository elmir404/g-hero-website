# Quick Start Guide

## 🚀 Get Started in 5 Minutes

### 1. Prerequisites Check
```bash
dotnet --version  # Should be 8.0.x or higher
```

### 2. Restore & Build
```bash
dotnet restore GHeroWebsite.sln
dotnet build GHeroWebsite.sln
```

### 3. Copy Logo
Copy the logo file:
```powershell
Copy-Item "public\assets\images\logo.webp" "src\GHeroWebsite.Web\wwwroot\assets\images\logo.webp"
```

### 4. Run the Application
```bash
cd src/GHeroWebsite.Web
dotnet run
```

### 5. Access the Application
- **Website:** http://localhost:5000 or https://localhost:5001
- **Admin Login:** http://localhost:5000/Account/Login
  - Username: `admin`
  - Password: `Admin123!`

## 📁 What Was Created

### Solution Structure
```
GHeroWebsite.sln
└── src/
    ├── GHeroWebsite.Domain/          # Entities (Contact, User)
    ├── GHeroWebsite.Infrastructure/  # EF Core, DbContext
    ├── GHeroWebsite.Application/     # Services, DTOs
    └── GHeroWebsite.Web/             # MVC Controllers & Views
```

### Key Files

**Domain Layer:**
- `Entities/Contact.cs` - Contact form entity
- `Entities/User.cs` - User/Admin entity

**Infrastructure:**
- `Data/ApplicationDbContext.cs` - EF Core DbContext

**Application:**
- `Services/ContactService.cs` - Contact business logic
- `Services/UserService.cs` - Authentication logic
- `DTOs/ContactDto.cs` - Contact form DTO

**Web:**
- `Controllers/HomeController.cs` - Public pages
- `Controllers/ContactController.cs` - Contact form handler
- `Controllers/AccountController.cs` - Login/logout
- `Controllers/Admin/DashboardController.cs` - Admin dashboard
- `Controllers/Admin/ContactMessagesController.cs` - Message management
- `Views/Home/Index.cshtml` - Main page with all sections
- `Views/Account/Login.cshtml` - Login page
- `Views/Admin/` - Admin panel views

## 🎯 Features Implemented

✅ **Layered Architecture** - Clean separation (Domain, Infrastructure, Application, Web)  
✅ **Entity Framework Core** - Code-First with migrations support  
✅ **Contact Form** - Validated form that saves to database  
✅ **Authentication** - Cookie-based login system  
✅ **Admin Panel** - Manage contact messages (view, mark read/unread, delete)  
✅ **UI/UX** - Tailwind CSS matching original Next.js design  
✅ **Database** - Auto-created on first run (or use migrations)

## 🔧 Configuration

### Database Connection
Edit `src/GHeroWebsite.Web/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GHeroWebsiteDb;..."
  }
}
```

### Default Admin User
Created automatically on first run:
- Username: `admin`
- Password: `Admin123!`
- Email: `admin@g-hero-company.com`

**⚠️ Change this password in production!**

## 📝 Next Steps

1. **Copy Logo** - Copy `logo.webp` to `wwwroot/assets/images/`
2. **Test Contact Form** - Submit a test message
3. **Login to Admin** - Test the admin panel
4. **Review Code** - Check the architecture documentation
5. **Customize** - Modify views, add features, etc.

## 🐛 Troubleshooting

**Build Errors?**
- Run `dotnet restore` first
- Check .NET 8 SDK is installed

**Database Errors?**
- Ensure SQL Server LocalDB is installed
- Check connection string in `appsettings.json`

**Logo Not Showing?**
- Verify logo file is in `wwwroot/assets/images/`
- Check browser console for 404 errors

## 📚 Documentation

- **SETUP_GUIDE.md** - Detailed setup instructions
- **ARCHITECTURE.md** - Architecture overview
- **README_ASP_NET.md** - Complete project documentation

## 🎨 UI Notes

The UI uses:
- **Tailwind CSS** (via CDN) - Same styling as Next.js version
- **Custom Colors** - Sakura (pink) and Teal color schemes
- **Responsive Design** - Mobile-friendly layout
- **Smooth Scrolling** - Anchor links with smooth scroll

## 🔐 Security Notes

- Passwords are hashed (SHA256 - upgrade for production)
- CSRF protection enabled
- Admin routes protected with `[Authorize]`
- Input validation on all forms

## 🚢 Production Checklist

Before deploying:
- [ ] Change admin password
- [ ] Update connection string to production database
- [ ] Upgrade password hashing (bcrypt/Argon2)
- [ ] Configure HTTPS
- [ ] Set up proper logging
- [ ] Use environment variables for secrets
- [ ] Test all functionality
- [ ] Review security settings

---

**Need Help?** Check the detailed documentation files or review the code comments.

