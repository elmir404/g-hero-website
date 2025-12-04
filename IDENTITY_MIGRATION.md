# ASP.NET Core Identity Migration Guide

## Overview

The authentication system has been upgraded from a simple custom implementation to **ASP.NET Core Identity**, which provides enterprise-grade security features.

## What Changed

### ✅ Security Improvements

1. **Password Hashing**: Now uses PBKDF2 (industry standard) instead of SHA256
2. **Password Requirements**: Enforced strong password policy
   - Minimum 8 characters
   - Requires uppercase, lowercase, digit, and special character
3. **Account Lockout**: Automatic lockout after 5 failed login attempts (15 minutes)
4. **Role-Based Authorization**: Built-in role management system
5. **Token Providers**: Support for password reset, email confirmation tokens
6. **Secure Cookies**: HttpOnly, Secure cookie settings

### ✅ Architecture Changes

1. **ApplicationUser**: Now inherits from `IdentityUser` (replaces custom `User` entity)
2. **DbContext**: Now inherits from `IdentityDbContext<ApplicationUser>`
3. **UserManager & SignInManager**: Using Identity's built-in managers
4. **Roles**: Using Identity's role system instead of custom role field

## New Features

### Password Security
- **PBKDF2 hashing** with salt (much more secure than SHA256)
- **Password complexity** requirements enforced
- **Password history** support (can be enabled)

### Account Protection
- **Automatic lockout** after failed attempts
- **Lockout duration**: 15 minutes
- **Max failed attempts**: 5

### Role Management
- Built-in **role system** (`Admin` role created automatically)
- Easy to add more roles (Editor, Viewer, etc.)
- Role-based authorization with `[Authorize(Roles = "Admin")]`

## Database Changes

### New Tables (Created by Identity)
- `AspNetUsers` - User accounts
- `AspNetRoles` - Roles
- `AspNetUserRoles` - User-Role mapping
- `AspNetUserClaims` - User claims
- `AspNetUserLogins` - External login providers
- `AspNetUserTokens` - Tokens (password reset, etc.)
- `AspNetRoleClaims` - Role claims

### Removed
- Old `Users` table (replaced by `AspNetUsers`)

## Migration Steps

### 1. Delete Old Database (Development Only)

If you have an existing database with the old `Users` table:

```bash
# Option 1: Delete the database
# In SQL Server Management Studio or via command line

# Option 2: Let the app recreate it
# The app will automatically recreate the database on first run
```

### 2. Run the Application

The application will automatically:
- Create Identity tables
- Seed the `Admin` role
- Create the default admin user

```bash
cd src/GHeroWebsite.Web
dotnet run
```

### 3. Default Admin Credentials

- **Username**: `admin`
- **Password**: `Admin123!`

**⚠️ IMPORTANT**: Change this password immediately in production!

## Using Identity Features

### Creating New Users

```csharp
var user = new ApplicationUser
{
    UserName = "newuser",
    Email = "user@example.com",
    FullName = "New User",
    EmailConfirmed = true
};

var result = await _userManager.CreateAsync(user, "SecurePassword123!");
if (result.Succeeded)
{
    await _userManager.AddToRoleAsync(user, "Admin");
}
```

### Checking User Roles

```csharp
if (User.IsInRole("Admin"))
{
    // Admin-only code
}
```

### Password Validation

Identity automatically validates passwords based on the configured requirements:
- Minimum length: 8 characters
- Requires: uppercase, lowercase, digit, special character

### Account Lockout

After 5 failed login attempts, the account is locked for 15 minutes. The lockout is automatic and handled by Identity.

## Configuration

### Password Requirements

Edit `Program.cs` to change password requirements:

```csharp
options.Password.RequireDigit = true;
options.Password.RequireLowercase = true;
options.Password.RequireUppercase = true;
options.Password.RequireNonAlphanumeric = true;
options.Password.RequiredLength = 8;
options.Password.RequiredUniqueChars = 1;
```

### Lockout Settings

```csharp
options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
options.Lockout.MaxFailedAccessAttempts = 5;
options.Lockout.AllowedForNewUsers = true;
```

### Cookie Settings

```csharp
options.ExpireTimeSpan = TimeSpan.FromDays(7);
options.SlidingExpiration = true;
options.Cookie.HttpOnly = true;
options.Cookie.SecurePolicy = CookieSecurePolicy.RequireHttps; // For production
```

## Security Best Practices

### Production Checklist

- [ ] Change default admin password
- [ ] Enable HTTPS and set `CookieSecurePolicy.RequireHttps`
- [ ] Enable email confirmation (`RequireConfirmedEmail = true`)
- [ ] Set up email service for password reset
- [ ] Configure proper CORS if needed
- [ ] Review and adjust password requirements
- [ ] Set up logging for security events
- [ ] Enable two-factor authentication (2FA) if needed

### Password Reset (Future Enhancement)

To add password reset functionality:

1. Add email service
2. Configure token providers
3. Create password reset pages
4. Send reset links via email

## Troubleshooting

### "Invalid username or password" After Migration

- The old database had different password hashes
- Delete the database and let it recreate
- Or manually create a new user with Identity

### Account Locked Out

- Wait 15 minutes, or
- Reset via database (development only):
  ```sql
  UPDATE AspNetUsers 
  SET LockoutEnd = NULL, AccessFailedCount = 0 
  WHERE UserName = 'admin'
  ```

### Password Requirements Not Met

Ensure password has:
- At least 8 characters
- One uppercase letter
- One lowercase letter
- One digit
- One special character (!@#$%^&*)

## Benefits Over Custom Authentication

1. **Security**: Industry-standard password hashing (PBKDF2)
2. **Features**: Built-in lockout, password reset, 2FA support
3. **Maintenance**: Microsoft-maintained, regularly updated
4. **Extensibility**: Easy to add external providers (Google, Facebook, etc.)
5. **Best Practices**: Follows OWASP guidelines
6. **Testing**: Well-tested framework used by millions

## Next Steps

1. **Email Confirmation**: Set up email service for account verification
2. **Password Reset**: Implement forgot password functionality
3. **Two-Factor Authentication**: Add 2FA for enhanced security
4. **External Logins**: Add Google, Microsoft, etc. login options
5. **User Management UI**: Create admin interface for user management
6. **Audit Logging**: Track user actions and security events

## References

- [ASP.NET Core Identity Documentation](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [Identity Configuration](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration)
- [Password Hashing in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing)

