# Architecture Overview

## Layered Architecture

The project follows a clean, layered architecture pattern with clear separation of concerns:

```
┌─────────────────────────────────────┐
│         Web Layer (MVC)             │
│  Controllers, Views, Static Files   │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│      Application Layer               │
│  Services, DTOs, Business Logic     │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│      Domain Layer                    │
│  Entities (Contact, User)           │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│   Infrastructure Layer               │
│  EF Core, DbContext, Repositories   │
└─────────────────────────────────────┘
```

## Layer Responsibilities

### Domain Layer (`GHeroWebsite.Domain`)
**Purpose:** Contains core business entities and domain logic.

**Components:**
- `Entities/Contact.cs` - Contact form submission entity
- `Entities/User.cs` - User/Admin entity

**Rules:**
- No dependencies on other layers
- Pure domain models with business logic
- No infrastructure concerns

### Infrastructure Layer (`GHeroWebsite.Infrastructure`)
**Purpose:** Handles data access and external service integrations.

**Components:**
- `Data/ApplicationDbContext.cs` - EF Core DbContext
- Entity configurations
- Database migrations

**Responsibilities:**
- Database access via EF Core
- Entity-to-database mapping
- Migration management

### Application Layer (`GHeroWebsite.Application`)
**Purpose:** Contains business logic and application services.

**Components:**
- `DTOs/ContactDto.cs` - Data transfer objects
- `Services/IContactService.cs` - Contact service interface
- `Services/ContactService.cs` - Contact business logic
- `Services/IUserService.cs` - User service interface
- `Services/UserService.cs` - User authentication logic

**Responsibilities:**
- Business logic implementation
- Data transformation (Entity ↔ DTO)
- Service orchestration
- Validation rules

### Web Layer (`GHeroWebsite.Web`)
**Purpose:** Handles HTTP requests, UI rendering, and user interaction.

**Components:**
- `Controllers/` - MVC controllers
  - `HomeController.cs` - Public pages
  - `ContactController.cs` - Contact form handling
  - `AccountController.cs` - Authentication
  - `Admin/DashboardController.cs` - Admin dashboard
  - `Admin/ContactMessagesController.cs` - Message management
- `Views/` - Razor views
- `wwwroot/` - Static files (CSS, JS, images)

**Responsibilities:**
- HTTP request handling
- View rendering
- Model binding and validation
- Authentication/Authorization
- Static file serving

## Data Flow

### Contact Form Submission Flow

```
1. User submits form
   ↓
2. ContactController.Submit()
   - Model binding (ContactDto)
   - Model validation
   ↓
3. ContactService.CreateContactAsync()
   - Transform DTO → Entity
   - Business logic
   ↓
4. ApplicationDbContext
   - Save to database
   ↓
5. Response to user
   - Success/Error message
   - Redirect
```

### Admin Authentication Flow

```
1. User submits login form
   ↓
2. AccountController.Login()
   - Validate credentials
   ↓
3. UserService.AuthenticateAsync()
   - Hash password comparison
   - Update last login
   ↓
4. Create Claims Principal
   - Set authentication cookie
   ↓
5. Redirect to admin dashboard
```

## Dependency Injection

Services are registered in `Program.cs`:

```csharp
// Infrastructure
builder.Services.AddDbContext<ApplicationDbContext>(...);

// Application Services
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IUserService, UserService>();
```

**Lifetime:**
- `Scoped` - One instance per HTTP request (services)
- `Singleton` - One instance for application lifetime
- `Transient` - New instance every time

## Authentication & Authorization

### Authentication Scheme
- **Type:** Cookie-based authentication
- **Scheme Name:** "CookieAuth"
- **Configuration:** In `Program.cs`

### Authorization
- **Attribute:** `[Authorize]` on admin controllers
- **Protection:** All `/Admin/*` routes require authentication
- **Login Path:** `/Account/Login`

### Password Security
- **Current:** SHA256 hashing (simple, upgrade for production)
- **Recommended:** bcrypt, Argon2, or PBKDF2

## Database Design

### Contact Table
```sql
CREATE TABLE Contacts (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(200) NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    Company NVARCHAR(200) NULL,
    Message NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    IsRead BIT NOT NULL DEFAULT 0,
    ReadAt DATETIME2 NULL
);

CREATE INDEX IX_Contacts_Email ON Contacts(Email);
CREATE INDEX IX_Contacts_CreatedAt ON Contacts(CreatedAt);
```

### User Table
```sql
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    FullName NVARCHAR(200) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL,
    LastLoginAt DATETIME2 NULL,
    Role NVARCHAR(50) NOT NULL DEFAULT 'Admin'
);
```

## Extensibility

### Adding New Entities

1. **Create Entity** in `GHeroWebsite.Domain/Entities/`
   ```csharp
   public class NewEntity
   {
       public int Id { get; set; }
       // Properties...
   }
   ```

2. **Add DbSet** in `ApplicationDbContext`
   ```csharp
   public DbSet<NewEntity> NewEntities { get; set; }
   ```

3. **Configure** in `OnModelCreating` (if needed)

4. **Create Migration**
   ```bash
   dotnet ef migrations add AddNewEntity
   dotnet ef database update
   ```

5. **Create Service** in Application layer (if needed)

6. **Create Controller** in Web layer

7. **Create Views** for UI

### Adding New Features

**Example: Email Notifications**

1. Create `IEmailService` in Application layer
2. Implement `EmailService` in Infrastructure layer (or separate service)
3. Inject into `ContactService`
4. Call after saving contact

**Example: Logging**

1. Add Serilog/NLog package
2. Configure in `Program.cs`
3. Inject `ILogger<T>` into services

## Best Practices

1. **Separation of Concerns** - Each layer has a single responsibility
2. **Dependency Inversion** - Depend on abstractions (interfaces)
3. **Single Responsibility** - Each class does one thing
4. **DRY** - Don't repeat yourself
5. **SOLID Principles** - Follow SOLID design principles
6. **Error Handling** - Use try-catch and proper logging
7. **Validation** - Validate at multiple layers (client, server, database)
8. **Security** - Always validate input, use parameterized queries (EF Core does this)

## Testing Strategy

### Unit Tests
- Test Application layer services
- Mock dependencies (DbContext, etc.)

### Integration Tests
- Test database operations
- Test controller actions

### E2E Tests
- Test full user workflows
- Test authentication flows

## Performance Considerations

1. **Database Queries** - Use `.AsNoTracking()` for read-only queries
2. **Caching** - Consider caching for frequently accessed data
3. **Pagination** - Implement pagination for large datasets
4. **Async/Await** - Use async methods for I/O operations
5. **Connection Pooling** - EF Core handles this automatically

## Security Checklist

- [x] Input validation
- [x] CSRF protection (AntiForgeryToken)
- [x] Authentication required for admin routes
- [x] Password hashing
- [ ] HTTPS enforcement (configure in production)
- [ ] SQL injection prevention (EF Core handles this)
- [ ] XSS prevention (Razor automatically encodes)
- [ ] Secure cookie settings
- [ ] Rate limiting (consider adding)
- [ ] Content Security Policy (consider adding)

