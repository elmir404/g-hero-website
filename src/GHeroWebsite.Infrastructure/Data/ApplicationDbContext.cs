using GHeroWebsite.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GHeroWebsite.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Contact> Contacts { get; set; }
    public DbSet<ContentSection> ContentSections { get; set; }
    public DbSet<HomeHeroSection> HomeHeroSections { get; set; }
    public DbSet<HomeService> HomeServices { get; set; }
    public DbSet<HomeTestimonial> HomeTestimonials { get; set; }
    public DbSet<HomeFaq> HomeFaqs { get; set; }
    public DbSet<HomeContactInfo> HomeContactInfos { get; set; }
    public DbSet<SiteSettings> SiteSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Contact entity configuration
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Subject).HasMaxLength(500);
            entity.Property(e => e.Company).HasMaxLength(200);
            entity.Property(e => e.Message).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => e.CreatedAt);
        });

        // ApplicationUser configuration
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        // ContentSection entity configuration
        modelBuilder.Entity<ContentSection>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SectionKey).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LanguageCode).IsRequired().HasMaxLength(10).HasDefaultValue("en");
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.Subtitle).HasMaxLength(1000);
            entity.Property(e => e.Content).HasColumnType("nvarchar(max)");
            entity.Property(e => e.AdditionalData).HasColumnType("nvarchar(max)");
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.HasIndex(e => new { e.SectionKey, e.LanguageCode });
            entity.HasIndex(e => e.IsActive);
        });

        // HomeHeroSection entity configuration
        modelBuilder.Entity<HomeHeroSection>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LanguageCode).IsRequired().HasMaxLength(10).HasDefaultValue("en");
            entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Subtitle).HasMaxLength(1000);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.PrimaryButtonText).HasMaxLength(100);
            entity.Property(e => e.PrimaryButtonUrl).HasMaxLength(500);
            entity.Property(e => e.SecondaryButtonText).HasMaxLength(100);
            entity.Property(e => e.SecondaryButtonUrl).HasMaxLength(500);
            entity.Property(e => e.BackgroundImageUrl).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.HasIndex(e => new { e.LanguageCode, e.IsActive });
        });

        // HomeService entity configuration
        modelBuilder.Entity<HomeService>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LanguageCode).IsRequired().HasMaxLength(10).HasDefaultValue("en");
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.IconName).HasMaxLength(100);
            entity.Property(e => e.IconUrl).HasMaxLength(500);
            entity.Property(e => e.ImageUrl).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.HasIndex(e => new { e.LanguageCode, e.IsActive });
        });

        // HomeTestimonial entity configuration
        modelBuilder.Entity<HomeTestimonial>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LanguageCode).IsRequired().HasMaxLength(10).HasDefaultValue("en");
            entity.Property(e => e.AuthorName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.AuthorTitle).HasMaxLength(200);
            entity.Property(e => e.AuthorCompany).HasMaxLength(200);
            entity.Property(e => e.AuthorImageUrl).HasMaxLength(1000);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.HasIndex(e => new { e.LanguageCode, e.IsActive });
        });

        // HomeFaq entity configuration
        modelBuilder.Entity<HomeFaq>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LanguageCode).IsRequired().HasMaxLength(10).HasDefaultValue("en");
            entity.Property(e => e.Question).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Answer).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.HasIndex(e => new { e.LanguageCode, e.IsActive });
        });

        // HomeContactInfo entity configuration
        modelBuilder.Entity<HomeContactInfo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LanguageCode).IsRequired().HasMaxLength(10).HasDefaultValue("en");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Website).HasMaxLength(500);
            entity.Property(e => e.FacebookUrl).HasMaxLength(500);
            entity.Property(e => e.TwitterUrl).HasMaxLength(500);
            entity.Property(e => e.LinkedInUrl).HasMaxLength(500);
            entity.Property(e => e.InstagramUrl).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.HasIndex(e => new { e.LanguageCode, e.IsActive });
        });

        // SiteSettings entity configuration (single row table)
        modelBuilder.Entity<SiteSettings>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.WorkingHours).HasMaxLength(200);
            entity.Property(e => e.WhatsAppUrl).HasMaxLength(500);
            entity.Property(e => e.InstagramUrl).HasMaxLength(500);
            entity.Property(e => e.FacebookUrl).HasMaxLength(500);
            entity.Property(e => e.LinkedInUrl).HasMaxLength(500);
            entity.Property(e => e.MapEmbedUrl).HasMaxLength(1000);
            entity.Property(e => e.UpdatedAt).IsRequired();
            
            // Ensure only one row exists
            entity.HasIndex(e => e.Id).IsUnique();
        });
    }
}

