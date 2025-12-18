-- Migration: Add SiteSettings table
-- Run this SQL script on your database to add the SiteSettings table

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[SiteSettings] (
        [Id] int IDENTITY(1,1) NOT NULL,
        [Phone] nvarchar(50) NULL,
        [Email] nvarchar(200) NULL,
        [Address] nvarchar(500) NULL,
        [WorkingHours] nvarchar(200) NULL,
        [WhatsAppUrl] nvarchar(500) NULL,
        [InstagramUrl] nvarchar(500) NULL,
        [FacebookUrl] nvarchar(500) NULL,
        [LinkedInUrl] nvarchar(500) NULL,
        [MapEmbedUrl] nvarchar(1000) NULL,
        [UpdatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_SiteSettings] PRIMARY KEY ([Id])
    );

    CREATE UNIQUE INDEX [IX_SiteSettings_Id] ON [dbo].[SiteSettings] ([Id]);
END
GO

