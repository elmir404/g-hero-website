using GHeroWebsite.Application.DTOs;
using GHeroWebsite.Application.Services;
using GHeroWebsite.Domain.Entities;
using GHeroWebsite.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "G-Hero Website API", Version = "v1" });
    
    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001", "https://localhost:5001", "https://g-hero.jp")
              .AllowAnyHeader() // This includes Authorization header
              .AllowAnyMethod() // This includes GET, POST, etc.
              .AllowCredentials() // Important for cookies, but also needed for Authorization header in some cases
              .WithExposedHeaders("Content-Disposition"); // Expose any additional headers if needed
    });
});

// Configure Entity Framework
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=(localdb)\\mssqllocaldb;Database=GHeroWebsiteDb;Trusted_Connection=True;MultipleActiveResultSets=true";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "GHeroWebsite";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "GHeroWebsite";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(5), // Allow 5 minutes clock skew
        RoleClaimType = ClaimTypes.Role,
        NameClaimType = ClaimTypes.Name
    };
    
    // Configure token extraction - handle both Authorization header and query string
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Log what we're receiving
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            var authHeader = context.Request.Headers["Authorization"].ToString();
            var path = context.Request.Path;
            
            logger.LogInformation("JWT MessageReceived - Path: {Path}, HasAuthHeader: {HasAuth}, AuthHeader: {AuthHeader}", 
                path, !string.IsNullOrEmpty(authHeader), string.IsNullOrEmpty(authHeader) ? "None" : authHeader.Substring(0, Math.Min(50, authHeader.Length)) + "...");
            
            // Handle token extraction manually if needed
            if (!string.IsNullOrEmpty(authHeader))
            {
                // Check if it starts with "Bearer "
                if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    // Extract token after "Bearer "
                    var token = authHeader.Substring("Bearer ".Length).Trim();
                    context.Token = token;
                    logger.LogInformation("Token extracted from Bearer header");
                }
                else
                {
                    // Token is sent without "Bearer " prefix - use the entire header value
                    context.Token = authHeader.Trim();
                    logger.LogInformation("Token extracted from header without Bearer prefix");
                }
            }
            
            // Also check query string as fallback (optional, for debugging)
            var queryToken = context.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(queryToken) && string.IsNullOrEmpty(context.Token))
            {
                context.Token = queryToken;
                logger.LogInformation("Token found in query string");
            }
            
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(context.Exception, 
                "JWT Authentication failed - Path: {Path}, Error: {Error}", 
                context.Request.Path, 
                context.Exception?.Message);
            
            // Log more details about the exception
            if (context.Exception is SecurityTokenExpiredException)
            {
                logger.LogError("Token expired");
            }
            else if (context.Exception is SecurityTokenInvalidSignatureException)
            {
                logger.LogError("Token signature invalid");
            }
            else if (context.Exception is SecurityTokenInvalidAudienceException)
            {
                logger.LogError("Token audience invalid");
            }
            else if (context.Exception is SecurityTokenInvalidIssuerException)
            {
                logger.LogError("Token issuer invalid");
            }
            
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            var userName = context.Principal?.Identity?.Name;
            var roles = context.Principal?.FindAll(ClaimTypes.Role)?.Select(c => c.Value).ToList() ?? new List<string>();
            logger.LogInformation("JWT Token validated successfully - User: {User}, Roles: {Roles}", 
                userName, string.Join(", ", roles));
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            var authHeader = context.Request.Headers["Authorization"].ToString();
            logger.LogWarning("JWT Challenge issued - Path: {Path}, Error: {Error}, ErrorDescription: {ErrorDescription}, HasAuthHeader: {HasAuth}", 
                context.Request.Path,
                context.Error, 
                context.ErrorDescription,
                !string.IsNullOrEmpty(authHeader));
            
            // Ensure we return 401 with proper challenge
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            var errorResponse = JsonSerializer.Serialize(new { error = "Unauthorized", message = "Authentication required" });
            return context.Response.WriteAsync(errorResponse);
        }
    };
});

builder.Services.AddAuthorization();

// Register application services
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IHomePageService, HomePageService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISiteSettingsService, SiteSettingsService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwaggerUI();
app.UseHttpsRedirection();

// IMPORTANT: CORS must come before Authentication and Authorization
app.UseCors("AllowNextJs");

// Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Ensure database is created and seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Apply migrations instead of EnsureCreated to handle schema updates
    try
    {
        dbContext.Database.Migrate();
    }
    catch
    {
        // If migrations fail, try EnsureCreated as fallback
        dbContext.Database.EnsureCreated();
    }
    
    // Seed roles
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var adminRoleExists = await roleManager.RoleExistsAsync("Admin");
    if (!adminRoleExists)
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
    
    // Seed admin user if it doesn't exist
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    
    var adminExists = await userService.GetUserByUsernameAsync("gheroadmin");
    if (adminExists == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = "gheroadmin",
            Email = "admin@g-hero.jp",
            FullName = "Adminstrator",
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true
        };
        
        var result = await userManager.CreateAsync(adminUser, "Ghero345!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
    
    // Seed initial content sections if they don't exist
    var contentService = scope.ServiceProvider.GetRequiredService<IContentService>();
    await SeedInitialContentAsync(contentService);
}

app.Run();

static async Task SeedInitialContentAsync(IContentService contentService)
{
    // English content
    var enSections = new[]
    {
        new { Key = "philosophy.title", Lang = "en", Title = "Philosophy", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 10 },
        new { Key = "philosophy.content", Lang = "en", Title = (string?)null, Content = "G-Hero Company Co., Ltd. was founded with the mission of creating new value in the international community and with the aim of becoming a bridge that delivers \"Japanese quality to the world, and the world's potential to Japan\". Through our diverse business domains, we contribute to the growth of our customers, partner companies, and local communities, striving to grow as a sustainable and trusted global company.", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 11 },
        new { Key = "mission.title", Lang = "en", Title = "Mission", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 20 },
        new { Key = "mission.content", Lang = "en", Title = (string?)null, Content = "To become a \"bridge\" that connects countries, people, businesses, and opportunities. Based on reliable quality and sincere service, we create new value and future in the global market.", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 21 },
        new { Key = "about.title", Lang = "en", Title = "About G-Hero Company", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 60 },
        new { Key = "about.content", Lang = "en", Title = (string?)null, Content = "G-Hero Company is a Japan-based global IT & multi-business bridge company. We combine international perspectives with Japanese quality standards to deliver optimal solutions to our clients and partners.", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 61 }
    };

    // Azerbaijani content
    var azSections = new[]
    {
        new { Key = "philosophy.title", Lang = "az", Title = "Fəlsəfə", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 10 },
        new { Key = "philosophy.content", Lang = "az", Title = (string?)null, Content = "G-Hero Company beynəlxalq cəmiyyətdə yeni dəyər yaratmağı öz missiyası hesab edir və \"Yapon keyfiyyətini dünyaya, dünyanın imkanlarını Yaponiyaya\" çatdıran körpü olmaq məqsədi ilə təsis edilmişdir.", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 11 },
        new { Key = "mission.title", Lang = "az", Title = "Missiya", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 20 },
        new { Key = "mission.content", Lang = "az", Title = (string?)null, Content = "Ölkələri, insanları, biznesləri və imkanları birləşdirən bir \"körpü\" olmaq.", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 21 },
        new { Key = "about.title", Lang = "az", Title = "G-Hero Company haqqında", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 60 },
        new { Key = "about.content", Lang = "az", Title = (string?)null, Content = "G-Hero Company Yaponiyada yerləşən qlobal IT və çoxşaxəli biznes körpüsü şirkətidir.", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 61 }
    };

    // Japanese content - Using "ja" (ISO 639-1) instead of "jp"
    var jaSections = new[]
    {
        new { Key = "philosophy.title", Lang = "ja", Title = "企業理念", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 10 },
        new { Key = "philosophy.content", Lang = "ja", Title = (string?)null, Content = "株式会社G-Hero Companyは、国際社会における新たな価値創造を使命とし、「日本の品質を世界へ、世界の可能性を日本へ」届ける架け橋となることを目的として設立されました。多様な事業領域を通じて、お客様・パートナー企業・地域社会の発展に寄与し、持続可能で信頼されるグローバル企業としての成長を目指します。", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 11 },
        new { Key = "mission.title", Lang = "ja", Title = "ミッション", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 20 },
        new { Key = "mission.content", Lang = "ja", Title = (string?)null, Content = "国と国、人と人、ビジネスと機会をつなぐ \"橋（Bridge）\" となる。確かな品質と誠実な対応を基盤に、国際市場における新たな価値と未来を創出します。", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 21 },
        new { Key = "about.title", Lang = "ja", Title = "会社情報", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 60 },
        new { Key = "about.content", Lang = "ja", Title = (string?)null, Content = "G-Hero Companyは、日本を拠点とするグローバルIT・多角ビジネスブリッジ企業です。国際的な視点と日本の品質基準を組み合わせ、お客様とパートナーに最適なソリューションを提供しています。", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 61 }
    };

    var allSections = enSections.Concat(azSections).Concat(jaSections);

    foreach (var section in allSections)
    {
        // Check if section already exists
        var existing = await contentService.GetSectionByKeyAsync(section.Key, section.Lang);
        if (existing == null)
        {
            var dto = new GHeroWebsite.Application.DTOs.ContentSectionDto
            {
                SectionKey = section.Key,
                LanguageCode = section.Lang,
                Title = section.Title,
                Content = section.Content,
                Subtitle = section.Subtitle,
                AdditionalData = section.AdditionalData,
                DisplayOrder = section.Order,
                IsActive = true
            };
            await contentService.CreateOrUpdateSectionAsync(dto);
        }
    }
}

