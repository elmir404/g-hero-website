using GHeroWebsite.Application.DTOs;
using GHeroWebsite.Application.Services;
using GHeroWebsite.Domain.Entities;
using GHeroWebsite.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure Entity Framework
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=(localdb)\\mssqllocaldb;Database=GHeroWebsiteDb;Trusted_Connection=True;MultipleActiveResultSets=true";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false; // Set to true in production with email confirmation

})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Use SameAsRequest for development, RequireHttps for production
});

// Register application services
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IContentService, ContentService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

// Ensure database is created and seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
    
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
    
    var adminExists = await userService.GetUserByUsernameAsync("admin");
    if (adminExists == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@g-hero-company.com",
            FullName = "Administrator",
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true
        };
        
        var result = await userManager.CreateAsync(adminUser, "Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
    
    // Seed initial content sections if they don't exist
    var contentService = scope.ServiceProvider.GetRequiredService<IContentService>();
    await SeedInitialContentAsync(contentService);
}

static async Task SeedInitialContentAsync(IContentService contentService)
{
    // English content
    var enSections = new[]
    {
        new { Key = "hero.title", Lang = "en", Title = "G-Hero Company", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 1 },
        new { Key = "hero.tagline", Lang = "en", Title = (string?)null, Content = "Japanese Quality to the World, Global Potential to Japan", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 2 },
        new { Key = "hero.description", Lang = "en", Title = (string?)null, Content = "G-Hero Company is a global IT & solutions bridge connecting Japan and the world, dedicated to creating new value through innovative services.", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 3 },
        new { Key = "philosophy.title", Lang = "en", Title = "Philosophy", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 10 },
        new { Key = "philosophy.content", Lang = "en", Title = (string?)null, Content = "G-Hero Company Co., Ltd. was founded with the mission of creating new value in the international community and with the aim of becoming a bridge that delivers \"Japanese quality to the world, and the world's potential to Japan\". Through our diverse business domains, we contribute to the growth of our customers, partner companies, and local communities, striving to grow as a sustainable and trusted global company.", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 11 },
        new { Key = "mission.title", Lang = "en", Title = "Mission", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 20 },
        new { Key = "mission.content", Lang = "en", Title = (string?)null, Content = "To become a \"bridge\" that connects countries, people, businesses, and opportunities. Based on reliable quality and sincere service, we create new value and future in the global market.", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 21 },
        new { Key = "vision.title", Lang = "en", Title = "Vision", Content = (string?)null, Subtitle = (string?)null, AdditionalData = "[\"Grow into a leading diversified global corporate group representing Asia.\",\"Promote Japan's technology, culture, and services to the world and maximize their value.\",\"Connect Japanese and overseas companies, investors, and talent to create the best environment for both sides.\",\"Remain an innovative and trusted partner in the ever-changing global market.\"]", Order = 30 },
        new { Key = "values.title", Lang = "en", Title = "Core Values", Content = (string?)null, Subtitle = (string?)null, AdditionalData = "[\"Integrity|We conduct our business with transparency and integrity, in full compliance with laws and regulations.\",\"Quality|We expand \\\"Japanese standards\\\" globally and deliver top quality in every business we operate.\",\"Innovation|We go beyond existing frameworks to constantly create new value and business models.\",\"Global Mindset|We leverage multicultural understanding and global awareness to enable cross-border collaboration.\",\"Customer Success|We believe our growth comes from the success of our customers.\"]", Order = 40 },
        new { Key = "domains.title", Lang = "en", Title = "Business Domains", Content = (string?)null, Subtitle = "Our core business domains include:", AdditionalData = "[\"Attraction of domestic and overseas investors and legal consulting\",\"Wholesale, retail, import and export of food products\",\"E-commerce (EC site operation)\",\"Logistics planning and operations\",\"Export sales of Japanese cosmetics\",\"Travel and tourism services\",\"Automobile sales, leasing, and rental\",\"Real estate sales, brokerage, and management\",\"Overseas study support and education services\",\"International exchange and dispatch of IT engineers and specialists\",\"Hotel and accommodation booking and operations\",\"Private lodging (minpaku) and simplified lodging business\",\"Planning and operation of restaurants (cafes and restaurants)\",\"Import and sales of alcoholic beverages\",\"All related and ancillary businesses\"]", Order = 50 },
        new { Key = "about.title", Lang = "en", Title = "About G-Hero Company", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 60 },
        new { Key = "about.content", Lang = "en", Title = (string?)null, Content = "G-Hero Company is a Japan-based global IT & multi-business bridge company. We combine international perspectives with Japanese quality standards to deliver optimal solutions to our clients and partners.", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 61 }
    };

    // Azerbaijani content
    var azSections = new[]
    {
        new { Key = "hero.title", Lang = "az", Title = "G-Hero Company", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 1 },
        new { Key = "hero.tagline", Lang = "az", Title = (string?)null, Content = "Yapon keyfiyyətini dünyaya, dünyanın imkanlarını Yaponiyaya", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 2 },
        new { Key = "hero.description", Lang = "az", Title = (string?)null, Content = "G-Hero Company Yaponiyanı dünya ilə birləşdirən qlobal IT və həllər körpüsüdür, innovativ xidmətlər vasitəsilə yeni dəyər yaratmağa həsr olunmuşdur.", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 3 },
        new { Key = "philosophy.title", Lang = "az", Title = "Fəlsəfə", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 10 },
        new { Key = "philosophy.content", Lang = "az", Title = (string?)null, Content = "G-Hero Company beynəlxalq cəmiyyətdə yeni dəyər yaratmağı öz missiyası hesab edir və \"Yapon keyfiyyətini dünyaya, dünyanın imkanlarını Yaponiyaya\" çatdıran körpü olmaq məqsədi ilə təsis edilmişdir. Müxtəlif biznes sahələrimiz vasitəsilə müştərilərimizin, tərəfdaş şirkətlərin və yerli icmaların inkişafına töhfə verir, davamlı və etibarlı qlobal şirkət kimi böyüməyə çalışırıq.", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 11 },
        new { Key = "mission.title", Lang = "az", Title = "Missiya", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 20 },
        new { Key = "mission.content", Lang = "az", Title = (string?)null, Content = "Ölkələri, insanları, biznesləri və imkanları birləşdirən bir \"körpü\" olmaq. Yüksək keyfiyyətə və səmimi əməkdaşlığa əsaslanaraq beynəlxalq bazarda yeni dəyərlər və gələcək imkanlar yaradırıq.", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 21 },
        new { Key = "vision.title", Lang = "az", Title = "Vizyon", Content = (string?)null, Subtitle = (string?)null, AdditionalData = "[\"Asiyanı təmsil edən çoxşaxəli qlobal korporativ qrupa çevrilmək.\",\"Yapon texnologiyasını, mədəniyyətini və xidmətlərini dünyaya tanıtmaq və onların dəyərini maksimuma çatdırmaq.\",\"Yaponiya və xarici ölkələrin şirkətlərini, investorlarını və peşəkar kadrlarını birləşdirərək hər iki tərəf üçün optimal mühit yaratmaq.\",\"Daim dəyişən beynəlxalq bazarda yenilikçi və etibarlı tərəfdaş olaraq qalmaq.\"]", Order = 30 },
        new { Key = "values.title", Lang = "az", Title = "Əsas dəyərlər", Content = (string?)null, Subtitle = (string?)null, AdditionalData = "[\"Dürüstlük (Integrity)|Biznesimizi qanunlara tam riayət etməklə, şəffaflıq və dürüstlüyü hər şeydən üstün tutaraq həyata keçiririk.\",\"Keyfiyyət (Quality)|\\\"Yapon standartı\\\"nı qlobal miqyasda tətbiq edir və fəaliyyət göstərdiyimiz bütün sahələrdə ən yüksək keyfiyyəti təqdim edirik.\",\"İnnovasiya (Innovation)|Mövcud çərçivələri aşaraq daim yeni dəyərlər və biznes modelləri yaradırıq.\",\"Qlobal düşüncə tərzi (Global Mindset)|Multikultural anlayış və beynəlxalq baxış bucağından istifadə edərək sərhədləri aşan əməkdaşlıqlar qururuq.\",\"Müştəri dəyəri (Customer Success)|Müştərilərimizin uğurunu şirkətimizin inkişafının əsas mənbəyi hesab edirik.\"]", Order = 40 },
        new { Key = "domains.title", Lang = "az", Title = "Biznes sahələri", Content = (string?)null, Subtitle = "Şirkətimizin əsas fəaliyyət sahələri:", AdditionalData = "[\"Yerli və xarici investorların cəlbi və hüquqi konsaltinq\",\"Qida məhsullarının topdan və pərakəndə satışı, idxal və ixracı\",\"Elektron kommersiya (EC saytların idarə edilməsi)\",\"Logistika xidmətlərinin planlaşdırılması və idarə edilməsi\",\"Yapon istehsalı kosmetikanın ixracı və satışı\",\"Səyahət və turizm xidmətləri\",\"Avtomobillərin alqı-satqısı, lizinqi və icarəsi\",\"Daşınmaz əmlakın alqı-satqısı, vasitəçiliyi və idarə edilməsi\",\"Xaricdə təhsil və təhsil xidmətləri\",\"IT mühəndislərinin və ixtisaslı kadrların beynəlxalq mübadiləsi və yerləşdirilməsi\",\"Otel və yaşayış məkanlarının rezervasiyası və idarə olunması\",\"Yaşayış üçün icarə (minpaku) və sadələşdirilmiş yerləşdirmə fəaliyyəti\",\"İctimai iaşə obyektlərinin (kafe və restoranlar) konsepsiyası və idarə olunması\",\"Spirtli içkilərin idxalı və satışı\",\"Yuxarıdakı sahələrlə əlaqəli bütün yardımçı fəaliyyətlər\"]", Order = 50 },
        new { Key = "about.title", Lang = "az", Title = "G-Hero Company haqqında", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 60 },
        new { Key = "about.content", Lang = "az", Title = (string?)null, Content = "G-Hero Company Yaponiyada yerləşən qlobal IT və çoxşaxəli biznes körpüsü şirkətidir. Beynəlxalq perspektivləri Yapon keyfiyyət standartları ilə birləşdirərək müştərilərimizə və tərəfdaşlarımıza optimal həllər təqdim edirik.", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 61 }
    };

    // Japanese content
    var jpSections = new[]
    {
        new { Key = "hero.title", Lang = "jp", Title = "G-Hero Company", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 1 },
        new { Key = "hero.tagline", Lang = "jp", Title = (string?)null, Content = "日本の品質を世界へ、世界の可能性を日本へ", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 2 },
        new { Key = "hero.description", Lang = "jp", Title = (string?)null, Content = "G-Hero Companyは、日本と世界をつなぐグローバルIT・ソリューションの架け橋として、新たな価値創造を目指す企業です。", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 3 },
        new { Key = "philosophy.title", Lang = "jp", Title = "企業理念", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 10 },
        new { Key = "philosophy.content", Lang = "jp", Title = (string?)null, Content = "株式会社G-Hero Companyは、国際社会における新たな価値創造を使命とし、「日本の品質を世界へ、世界の可能性を日本へ」届ける架け橋となることを目的として設立されました。多様な事業領域を通じて、お客様・パートナー企業・地域社会の発展に寄与し、持続可能で信頼されるグローバル企業としての成長を目指します。", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 11 },
        new { Key = "mission.title", Lang = "jp", Title = "ミッション", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 20 },
        new { Key = "mission.content", Lang = "jp", Title = (string?)null, Content = "国と国、人と人、ビジネスと機会をつなぐ \"橋（Bridge）\" となる。確かな品質と誠実な対応を基盤に、国際市場における新たな価値と未来を創出します。", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 21 },
        new { Key = "vision.title", Lang = "jp", Title = "ビジョン", Content = (string?)null, Subtitle = (string?)null, AdditionalData = "[\"アジアを代表する多角的グローバル企業グループへ成長すること。\",\"日本の技術・文化・サービスを世界に発信し、その価値を最大化すること。\",\"日本と海外の企業・投資家・人材を結びつけ、双方にとって最適な環境を創ること。\",\"常に変化し続ける国際市場において、革新的で信頼されるパートナーであり続けること。\"]", Order = 30 },
        new { Key = "values.title", Lang = "jp", Title = "コアバリュー", Content = (string?)null, Subtitle = (string?)null, AdditionalData = "[\"誠実（Integrity）|法令遵守はもちろん、透明性と誠実さを最優先にビジネスを遂行します。\",\"品質（Quality）|「日本基準」をグローバルに展開し、すべての事業で最高品質を提供します。\",\"革新（Innovation）|既存の枠を超え、常に新しい価値とビジネスモデルを創造します。\",\"国際性（Global Mindset）|多文化理解と国際感覚を活かし、国境を越えたビジネス連携を実現します。\",\"顧客価値（Customer Success）|顧客の成功こそが、当社の成長の源泉であると考えます。\"]", Order = 40 },
        new { Key = "domains.title", Lang = "jp", Title = "事業内容", Content = (string?)null, Subtitle = "当社は以下の事業を中心に、多角的なサービスを展開しております：", AdditionalData = "[\"海外・国内投資家誘致および法務コンサルティング\",\"食品の卸売・小売・輸出入事業\",\"電子商取引（ECサイト運営）\",\"物流サービスの企画・運営\",\"日本製化粧品の輸出販売\",\"旅行業・観光案内サービス\",\"自動車の売買・リース・レンタル業\",\"不動産売買・仲介・管理業\",\"海外留学支援・教育関連業務\",\"ITエンジニア・専門人材の国際交流・派遣\",\"ホテル・宿泊施設の予約・運営\",\"住宅宿泊事業（民泊）および簡易宿所営業\",\"飲食店（カフェ・レストラン）の企画・運営\",\"酒類の輸入・販売業\",\"上記事業に付帯関連する一切の業務\"]", Order = 50 },
        new { Key = "about.title", Lang = "jp", Title = "会社情報", Content = (string?)null, Subtitle = (string?)null, AdditionalData = (string?)null, Order = 60 },
        new { Key = "about.content", Lang = "jp", Title = (string?)null, Content = "G-Hero Companyは、日本を拠点とするグローバルIT・多角ビジネスブリッジ企業です。国際的な視点と日本の品質基準を組み合わせ、お客様とパートナーに最適なソリューションを提供しています。", Subtitle = (string?)null, AdditionalData = (string?)null, Order = 61 }
    };

    var allSections = enSections.Concat(azSections).Concat(jpSections);

    foreach (var section in allSections)
    {
        var existing = await contentService.GetSectionByKeyAsync(section.Key, section.Lang);
        if (existing == null)
        {
            var dto = new ContentSectionDto
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

app.Run();
