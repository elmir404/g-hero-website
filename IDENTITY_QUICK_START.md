# Identity Quick Start

## What's New

✅ **ASP.NET Core Identity** - Enterprise-grade authentication  
✅ **Secure Password Hashing** - PBKDF2 instead of SHA256  
✅ **Account Lockout** - Protection against brute force  
✅ **Role-Based Access** - Built-in role management  
✅ **Strong Passwords** - Enforced complexity requirements  

## First Run

1. **Delete old database** (if exists):
   - The old `Users` table is incompatible
   - Let the app create a new database

2. **Run the application**:
   ```bash
   cd src/GHeroWebsite.Web
   dotnet run
   ```

3. **Login**:
   - URL: `/Account/Login`
   - Username: `admin`
   - Password: `Admin123!`

## Password Requirements

Your password must have:
- ✅ At least 8 characters
- ✅ One uppercase letter (A-Z)
- ✅ One lowercase letter (a-z)
- ✅ One digit (0-9)
- ✅ One special character (!@#$%^&*)

## Account Lockout

- **5 failed attempts** = Account locked
- **Lockout duration**: 15 minutes
- Automatically unlocks after time expires

## Creating New Users

Currently done via code. Future: Admin UI for user management.

## Security Features

- 🔒 PBKDF2 password hashing
- 🔒 Automatic account lockout
- 🔒 Secure cookie settings
- 🔒 Role-based authorization
- 🔒 Password complexity enforcement

## Need Help?

See `IDENTITY_MIGRATION.md` for detailed documentation.

