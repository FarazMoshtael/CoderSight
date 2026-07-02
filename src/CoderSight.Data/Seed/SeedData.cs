using System.Text.Json;
using CoderSight.Core.Entities;
using CoderSight.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoderSight.Data.Seed;

public static class SeedData
{
    private static readonly JsonSerializerOptions Json = new() { WriteIndented = false };

    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CmsDbContext>();
        await db.Database.MigrateAsync();

        await SeedRolesAndAdminAsync(scope.ServiceProvider);

        if (await db.Pages.AnyAsync()) return;

        await SeedCategoriesAndTagsAsync(db);
        await SeedPagesAsync(db);
        await SeedBlogPostsAsync(db);
    }

    private static async Task SeedRolesAndAdminAsync(IServiceProvider sp)
    {
        var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[] { "Admin", "Editor" })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        if (await userManager.FindByEmailAsync("admin@codersight.com") is null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin@codersight.com",
                Email = "admin@codersight.com",
                DisplayName = "Admin",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, "Admin@123456");
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }

    private static readonly Guid CatTutorialsEn = Guid.Parse("a0000000-0000-0000-0000-000000000001");
    private static readonly Guid CatBestPracticesEn = Guid.Parse("a0000000-0000-0000-0000-000000000002");
    private static readonly Guid CatNewsEn = Guid.Parse("a0000000-0000-0000-0000-000000000003");
    private static readonly Guid CatTutorialsFa = Guid.Parse("a0000000-0000-0000-0000-000000000011");
    private static readonly Guid CatBestPracticesFa = Guid.Parse("a0000000-0000-0000-0000-000000000012");
    private static readonly Guid CatNewsFa = Guid.Parse("a0000000-0000-0000-0000-000000000013");

    private static readonly Guid TagGettingStartedEn = Guid.Parse("b0000000-0000-0000-0000-000000000001");
    private static readonly Guid TagBeginnerEn = Guid.Parse("b0000000-0000-0000-0000-000000000002");
    private static readonly Guid TagCodeReviewEn = Guid.Parse("b0000000-0000-0000-0000-000000000003");
    private static readonly Guid TagProductivityEn = Guid.Parse("b0000000-0000-0000-0000-000000000004");
    private static readonly Guid TagTipsEn = Guid.Parse("b0000000-0000-0000-0000-000000000005");
    private static readonly Guid TagAnnouncementsEn = Guid.Parse("b0000000-0000-0000-0000-000000000006");

    private static readonly Guid LocHome = Guid.Parse("c0000000-0000-0000-0000-000000000001");
    private static readonly Guid LocAbout = Guid.Parse("c0000000-0000-0000-0000-000000000002");
    private static readonly Guid LocServices = Guid.Parse("c0000000-0000-0000-0000-000000000003");
    private static readonly Guid LocContact = Guid.Parse("c0000000-0000-0000-0000-000000000004");
    private static readonly Guid LocBlog = Guid.Parse("c0000000-0000-0000-0000-000000000005");
    private static readonly Guid LocPost1 = Guid.Parse("c0000000-0000-0000-0000-000000000006");
    private static readonly Guid LocPost2 = Guid.Parse("c0000000-0000-0000-0000-000000000007");
    private static readonly Guid LocCatTutorials = Guid.Parse("c0000000-0000-0000-0000-000000000101");
    private static readonly Guid LocCatBestPractices = Guid.Parse("c0000000-0000-0000-0000-000000000102");
    private static readonly Guid LocCatNews = Guid.Parse("c0000000-0000-0000-0000-000000000103");

    private static async Task SeedCategoriesAndTagsAsync(CmsDbContext db)
    {
        db.Categories.AddRange(
            new Category { Id = CatTutorialsEn, Name = "Tutorials", Slug = "tutorials", Culture = "en", LocalizationGroupId = LocCatTutorials, Description = "Step-by-step guides and tutorials" },
            new Category { Id = CatBestPracticesEn, Name = "Best Practices", Slug = "best-practices", Culture = "en", LocalizationGroupId = LocCatBestPractices, Description = "Industry best practices and standards" },
            new Category { Id = CatNewsEn, Name = "News", Slug = "news", Culture = "en", LocalizationGroupId = LocCatNews, Description = "Latest news and updates" },
            new Category { Id = CatTutorialsFa, Name = "آموزش‌ها", Slug = "آموزش‌ها", Culture = "fa", LocalizationGroupId = LocCatTutorials, Description = "راهنماها و آموزش‌های گام‌به‌گام" },
            new Category { Id = CatBestPracticesFa, Name = "بهترین شیوه‌ها", Slug = "بهترین-شیوه‌ها", Culture = "fa", LocalizationGroupId = LocCatBestPractices, Description = "بهترین شیوه‌ها و استانداردهای صنعت" },
            new Category { Id = CatNewsFa, Name = "اخبار", Slug = "اخبار", Culture = "fa", LocalizationGroupId = LocCatNews, Description = "آخرین اخبار و به‌روزرسانی‌ها" }
        );

        db.Tags.AddRange(
            new Tag { Id = TagGettingStartedEn, Name = "Getting Started", Slug = "getting-started", Culture = "en" },
            new Tag { Id = TagBeginnerEn, Name = "Beginner", Slug = "beginner", Culture = "en" },
            new Tag { Id = TagCodeReviewEn, Name = "Code Review", Slug = "code-review", Culture = "en" },
            new Tag { Id = TagProductivityEn, Name = "Productivity", Slug = "productivity", Culture = "en" },
            new Tag { Id = TagTipsEn, Name = "Tips", Slug = "tips", Culture = "en" },
            new Tag { Id = TagAnnouncementsEn, Name = "Announcements", Slug = "announcements", Culture = "en" }
        );

        await db.SaveChangesAsync();
    }

    private static async Task SeedPagesAsync(CmsDbContext db)
    {
        // ──── HOME (EN) ────
        db.Pages.Add(CreatePage("Home", "", "en", LocHome, [
            Block("Slider", 0, new { IntervalMs = 5000, ShowArrows = true, ShowDots = true, Slides = new[] {
                new { ImageUrl = "", Title = "Build Better Software", Subtitle = "Practical tutorials, expert insights, and hands-on guides for modern developers.", ButtonText = "Get Started", ButtonLink = "/en/blog" },
                new { ImageUrl = "", Title = "Learn from Industry Experts", Subtitle = "Deep-dive articles written by experienced engineers and technical leaders.", ButtonText = "Browse Articles", ButtonLink = "/en/blog" },
                new { ImageUrl = "", Title = "Join the Community", Subtitle = "Connect with 50,000+ developers sharing knowledge and growing together.", ButtonText = "Sign Up Free", ButtonLink = "/register" }
            }}),
            Block("Stats", 1, new { BackgroundColor = "#020817", Stats = new[] {
                new { Value = "500+", Label = "Articles", Icon = "article" },
                new { Value = "50K+", Label = "Monthly Readers", Icon = "users" },
                new { Value = "100+", Label = "Contributors", Icon = "pencil" },
                new { Value = "20+", Label = "Topics", Icon = "category" }
            }}),
            Block("FeaturesList", 2, new { SectionTitle = "Why CoderSight?", Columns = 3, Features = new[] {
                new { Icon = "code", Title = "In-Depth Tutorials", Description = "Step-by-step guides on modern development practices and tools." },
                new { Icon = "bulb", Title = "Expert Insights", Description = "Learn from experienced developers and industry leaders." },
                new { Icon = "rocket", Title = "Stay Current", Description = "Keep up with the latest trends, frameworks, and technologies." }
            }}),
            Block("CardCarousel", 3, new { SectionTitle = "Latest Articles", SectionSubtitle = "Fresh content from our contributors", AutoPlay = true, AutoPlayInterval = 4, Cards = new[] {
                new { Title = "Getting Started with Blazor", Description = "A beginner's guide to building web apps with Blazor Server.", Badge = "Tutorial", ImageUrl = (string?)null, LinkUrl = "/en/blog/getting-started", LinkText = "Read more" },
                new { Title = "10 Tips for Code Reviews", Description = "Improve your team's code quality with proven strategies.", Badge = "Best Practice", ImageUrl = (string?)null, LinkUrl = "/en/blog/top-10-tips", LinkText = "Read more" },
                new { Title = "EF Core Performance", Description = "Optimize your Entity Framework queries for production.", Badge = "Guide", ImageUrl = (string?)null, LinkUrl = "/en/blog", LinkText = "Read more" },
                new { Title = "REST API Design", Description = "Design clean, maintainable REST APIs with ASP.NET Core.", Badge = "Tutorial", ImageUrl = (string?)null, LinkUrl = "/en/blog", LinkText = "Read more" }
            }}),
            Block("Reviews", 4, new { SectionTitle = "What Developers Say", Reviews = new[] {
                new { ReviewerName = "Sarah Johnson, Full-Stack Developer", Rating = 5, Text = "CoderSight helped me level up my skills. The tutorials are practical and well-structured.", Date = (string?)null },
                new { ReviewerName = "Michael Torres, Senior Engineer at TechCorp", Rating = 5, Text = "The best resource for staying current with .NET and web development trends.", Date = (string?)null },
                new { ReviewerName = "Emily Zhang, Software Engineer", Rating = 5, Text = "Clear explanations with real-world examples. Exactly what I needed for my career growth.", Date = (string?)null }
            }}),
            Block("ProgressSteps", 5, new { SectionTitle = "How It Works", Steps = new[] {
                new { Title = "Sign Up", Description = "Create your free account", Icon = "user-plus" },
                new { Title = "Browse", Description = "Explore tutorials by topic", Icon = "search" },
                new { Title = "Learn", Description = "Follow hands-on guides", Icon = "book" },
                new { Title = "Build", Description = "Apply skills to real projects", Icon = "hammer" }
            }}),
            Block("CallToAction", 6, new { Title = "Ready to level up?", Description = "Join thousands of developers who trust CoderSight for their learning journey.", ButtonText = "Browse Articles", ButtonLink = "/en/blog", BackgroundColor = "#020817" }),
            Block("Newsletter", 7, new { Title = "Stay in the loop", Description = "Get the latest articles and tips delivered to your inbox.", ButtonText = "Subscribe", Placeholder = "Enter your email" })
        ]));

        // ──── HOME (FA) ────
        db.Pages.Add(CreatePage("خانه", "", "fa", LocHome, [
            Block("Slider", 0, new { IntervalMs = 5000, ShowArrows = true, ShowDots = true, Slides = new[] {
                new { ImageUrl = "", Title = "نرم‌افزار بهتر بسازید", Subtitle = "آموزش‌های عملی، بینش‌های متخصصان و راهنماهای کاربردی برای توسعه‌دهندگان مدرن.", ButtonText = "شروع کنید", ButtonLink = "/fa/بلاگ" },
                new { ImageUrl = "", Title = "از متخصصان صنعت بیاموزید", Subtitle = "مقالات عمیق نوشته‌شده توسط مهندسان باتجربه و رهبران فنی.", ButtonText = "مرور مقالات", ButtonLink = "/fa/بلاگ" },
                new { ImageUrl = "", Title = "به جامعه بپیوندید", Subtitle = "با بیش از ۵۰ هزار توسعه‌دهنده در اشتراک دانش و رشد مشترک همراه شوید.", ButtonText = "ثبت‌نام رایگان", ButtonLink = "/register" }
            }}),
            Block("Stats", 1, new { BackgroundColor = "#020817", Stats = new[] {
                new { Value = "+۵۰۰", Label = "مقاله", Icon = "article" },
                new { Value = "+۵۰ هزار", Label = "خواننده ماهانه", Icon = "users" },
                new { Value = "+۱۰۰", Label = "مشارکت‌کننده", Icon = "pencil" },
                new { Value = "+۲۰", Label = "موضوع", Icon = "category" }
            }}),
            Block("FeaturesList", 2, new { SectionTitle = "چرا کدرسایت؟", Columns = 3, Features = new[] {
                new { Icon = "code", Title = "آموزش‌های عمیق", Description = "راهنماهای گام‌به‌گام درباره شیوه‌ها و ابزارهای مدرن توسعه." },
                new { Icon = "bulb", Title = "بینش‌های متخصصان", Description = "از توسعه‌دهندگان باتجربه و رهبران صنعت بیاموزید." },
                new { Icon = "rocket", Title = "به‌روز بمانید", Description = "با آخرین روندها، فریمورک‌ها و فناوری‌ها همراه باشید." }
            }}),
            Block("CardCarousel", 3, new { SectionTitle = "آخرین مقالات", SectionSubtitle = "محتوای تازه از مشارکت‌کنندگان ما", AutoPlay = true, AutoPlayInterval = 4, Cards = new[] {
                new { Title = "شروع کار با بلیزور", Description = "راهنمای مبتدیان برای ساخت وب‌اپ با بلیزور سرور.", Badge = "آموزش", ImageUrl = (string?)null, LinkUrl = "/fa/بلاگ/شروع-کار", LinkText = "ادامه مطلب" },
                new { Title = "۱۰ نکته برای بررسی کد", Description = "کیفیت کد تیم خود را با استراتژی‌های اثبات‌شده بهبود دهید.", Badge = "بهترین شیوه", ImageUrl = (string?)null, LinkUrl = "/fa/بلاگ/۱۰-نکته", LinkText = "ادامه مطلب" },
                new { Title = "عملکرد EF Core", Description = "کوئری‌های Entity Framework خود را برای تولید بهینه کنید.", Badge = "راهنما", ImageUrl = (string?)null, LinkUrl = "/fa/بلاگ", LinkText = "ادامه مطلب" },
                new { Title = "طراحی REST API", Description = "APIهای REST تمیز و قابل نگهداری با ASP.NET Core طراحی کنید.", Badge = "آموزش", ImageUrl = (string?)null, LinkUrl = "/fa/بلاگ", LinkText = "ادامه مطلب" }
            }}),
            Block("Reviews", 4, new { SectionTitle = "نظر توسعه‌دهندگان", Reviews = new[] {
                new { ReviewerName = "سارا جانسون، توسعه‌دهنده فول‌استک", Rating = 5, Text = "کدرسایت به من کمک کرد مهارت‌هایم را ارتقا دهم. آموزش‌ها عملی و خوب ساختاربندی شده‌اند.", Date = (string?)null },
                new { ReviewerName = "مایکل تورس، مهندس ارشد در تک‌کورپ", Rating = 5, Text = "بهترین منبع برای به‌روز ماندن با روندهای .NET و توسعه وب.", Date = (string?)null },
                new { ReviewerName = "امیلی ژانگ، مهندس نرم‌افزار", Rating = 5, Text = "توضیحات واضح با مثال‌های واقعی. دقیقاً آنچه برای رشد حرفه‌ای‌ام نیاز داشتم.", Date = (string?)null }
            }}),
            Block("ProgressSteps", 5, new { SectionTitle = "چگونه کار می‌کند", Steps = new[] {
                new { Title = "ثبت‌نام", Description = "حساب رایگان خود را بسازید", Icon = "user-plus" },
                new { Title = "مرور", Description = "آموزش‌ها را بر اساس موضوع کاوش کنید", Icon = "search" },
                new { Title = "یادگیری", Description = "راهنماهای عملی را دنبال کنید", Icon = "book" },
                new { Title = "ساخت", Description = "مهارت‌ها را در پروژه‌های واقعی به کار ببرید", Icon = "hammer" }
            }}),
            Block("CallToAction", 6, new { Title = "آماده ارتقا هستید؟", Description = "به هزاران توسعه‌دهنده‌ای بپیوندید که به کدرسایت برای مسیر یادگیری‌شان اعتماد دارند.", ButtonText = "مرور مقالات", ButtonLink = "/fa/بلاگ", BackgroundColor = "#020817" }),
            Block("Newsletter", 7, new { Title = "در جریان باشید", Description = "آخرین مقالات و نکات را در صندوق ورودی خود دریافت کنید.", ButtonText = "اشتراک", Placeholder = "ایمیل خود را وارد کنید" })
        ]));

        // ──── ABOUT (EN) ────
        db.Pages.Add(CreatePage("About Us", "about", "en", LocAbout, [
            Block("Hero", 0, new { Title = "About CoderSight", Subtitle = "Empowering developers with knowledge since 2024.", BackgroundColor = "#020817" }),
            Block("RichText", 1, new { Content = "<h2>Our Mission</h2><p>CoderSight was founded with a simple goal: to make high-quality developer education accessible to everyone. We believe that great software starts with great developers, and great developers never stop learning.</p><p>Our team of experienced engineers and technical writers creates content that bridges the gap between theory and practice, helping developers at every level build better software.</p>" }),
            Block("Heading", 2, new { Text = "What Drives Us", Level = "h2", Alignment = "center" }),
            Block("Tabs", 3, new { Tabs = new[] {
                new { Title = "Our Mission", Content = "<h3>Democratize Developer Education</h3><p>We make high-quality, practical development knowledge freely accessible. Every tutorial is written to bridge theory and real-world application, regardless of your experience level.</p>", Icon = "target" },
                new { Title = "Our Vision", Content = "<h3>A World of Confident Builders</h3><p>We envision a future where every developer has the resources and community to build with confidence — turning ideas into production-ready software.</p>", Icon = "eye" },
                new { Title = "Our Values", Content = "<h3>Clarity · Community · Craft</h3><p>We value clear communication over jargon, community growth over competition, and software craftsmanship over shortcuts. Every piece of content reflects these principles.</p>", Icon = "heart" }
            }}),
            Block("Timeline", 4, new { SectionTitle = "Our Journey", Events = new[] {
                new { Date = "2024", Title = "Founded", Description = "CoderSight launched with a focus on practical developer tutorials.", Icon = "rocket" },
                new { Date = "2024", Title = "100 Articles", Description = "Reached our first major content milestone.", Icon = "article" },
                new { Date = "2025", Title = "Community Growth", Description = "Crossed 50,000 monthly active readers.", Icon = "users" },
                new { Date = "2026", Title = "Platform Expansion", Description = "Launched interactive tutorials and code playgrounds.", Icon = "code" }
            }}),
            Block("Team", 5, new { SectionTitle = "Meet the Team", Columns = 4, Members = new[] {
                new { Name = "Alex Chen", Role = "Founder & Lead Engineer", Bio = "15 years of full-stack experience.", ImageUrl = (string?)null, LinkedInUrl = "#", TwitterUrl = "#" },
                new { Name = "Maria Rodriguez", Role = "Head of Content", Bio = "Technical writer and educator.", ImageUrl = (string?)null, LinkedInUrl = "#", TwitterUrl = "#" },
                new { Name = "James Kim", Role = "Senior Developer", Bio = "Open-source contributor and speaker.", ImageUrl = (string?)null, LinkedInUrl = "#", TwitterUrl = "#" },
                new { Name = "Priya Sharma", Role = "Community Manager", Bio = "Building developer communities.", ImageUrl = (string?)null, LinkedInUrl = "#", TwitterUrl = "#" }
            }}),
            Block("Stats", 6, new { BackgroundColor = "#020817", Stats = new[] {
                new { Value = "2024", Label = "Founded", Icon = "calendar" },
                new { Value = "15+", Label = "Team Members", Icon = "users" },
                new { Value = "30+", Label = "Countries Reached", Icon = "world" },
                new { Value = "4.8/5", Label = "Reader Rating", Icon = "star" }
            }}),
            Block("CallToAction", 7, new { Title = "Want to join our team?", Description = "We're always looking for passionate developers and writers to join CoderSight.", ButtonText = "View Open Positions", ButtonLink = "/en/contact", BackgroundColor = "#020817" })
        ]));

        // ──── ABOUT (FA) ────
        db.Pages.Add(CreatePage("درباره ما", "درباره-ما", "fa", LocAbout, [
            Block("Hero", 0, new { Title = "درباره کدرسایت", Subtitle = "توانمندسازی توسعه‌دهندگان با دانش از سال ۲۰۲۴.", BackgroundColor = "#020817" }),
            Block("RichText", 1, new { Content = "<h2>مأموریت ما</h2><p>کدرسایت با یک هدف ساده تأسیس شد: دسترسی همگان به آموزش باکیفیت توسعه‌دهندگی. ما معتقدیم نرم‌افزار عالی با توسعه‌دهندگان عالی شروع می‌شود و توسعه‌دهندگان عالی هرگز از یادگیری دست نمی‌کشند.</p><p>تیم ما از مهندسان باتجربه و نویسندگان فنی محتوایی می‌سازد که شکاف بین تئوری و عمل را پر می‌کند و به توسعه‌دهندگان در هر سطحی کمک می‌کند نرم‌افزار بهتری بسازند.</p>" }),
            Block("Heading", 2, new { Text = "چه چیزی ما را هدایت می‌کند", Level = "h2", Alignment = "center" }),
            Block("Tabs", 3, new { Tabs = new[] {
                new { Title = "مأموریت ما", Content = "<h3>دموکراتیزه کردن آموزش توسعه‌دهندگی</h3><p>ما دانش توسعه عملی و باکیفیت را آزادانه در دسترس قرار می‌دهیم. هر آموزش برای پل زدن بین تئوری و کاربرد واقعی نوشته شده است، صرف‌نظر از سطح تجربه شما.</p>", Icon = "target" },
                new { Title = "چشم‌انداز ما", Content = "<h3>جهانی از سازندگان با اعتماد به نفس</h3><p>ما آینده‌ای را تصور می‌کنیم که هر توسعه‌دهنده منابع و جامعه لازم برای ساختن با اطمینان را داشته باشد — تبدیل ایده‌ها به نرم‌افزار آماده تولید.</p>", Icon = "eye" },
                new { Title = "ارزش‌های ما", Content = "<h3>وضوح · جامعه · صنعت</h3><p>ما ارتباط واضح را بر اصطلاحات فنی، رشد جامعه را بر رقابت، و صنعت‌گری نرم‌افزار را بر میان‌بُرها ارجح می‌دانیم. هر قطعه محتوا این اصول را منعکس می‌کند.</p>", Icon = "heart" }
            }}),
            Block("Timeline", 4, new { SectionTitle = "مسیر ما", Events = new[] {
                new { Date = "۲۰۲۴", Title = "تأسیس", Description = "کدرسایت با تمرکز بر آموزش‌های عملی توسعه‌دهندگی راه‌اندازی شد.", Icon = "rocket" },
                new { Date = "۲۰۲۴", Title = "۱۰۰ مقاله", Description = "به اولین نقطه عطف محتوایی رسیدیم.", Icon = "article" },
                new { Date = "۲۰۲۵", Title = "رشد جامعه", Description = "از ۵۰ هزار خواننده فعال ماهانه عبور کردیم.", Icon = "users" },
                new { Date = "۲۰۲۶", Title = "گسترش پلتفرم", Description = "آموزش‌های تعاملی و محیط‌های کدنویسی راه‌اندازی شد.", Icon = "code" }
            }}),
            Block("Team", 5, new { SectionTitle = "تیم ما", Columns = 4, Members = new[] {
                new { Name = "الکس چن", Role = "بنیان‌گذار و مهندس ارشد", Bio = "۱۵ سال تجربه فول‌استک.", ImageUrl = (string?)null, LinkedInUrl = "#", TwitterUrl = "#" },
                new { Name = "ماریا رودریگز", Role = "مدیر محتوا", Bio = "نویسنده فنی و آموزگار.", ImageUrl = (string?)null, LinkedInUrl = "#", TwitterUrl = "#" },
                new { Name = "جیمز کیم", Role = "توسعه‌دهنده ارشد", Bio = "مشارکت‌کننده متن‌باز و سخنران.", ImageUrl = (string?)null, LinkedInUrl = "#", TwitterUrl = "#" },
                new { Name = "پریا شارما", Role = "مدیر جامعه", Bio = "ساخت جوامع توسعه‌دهندگی.", ImageUrl = (string?)null, LinkedInUrl = "#", TwitterUrl = "#" }
            }}),
            Block("Stats", 6, new { BackgroundColor = "#020817", Stats = new[] {
                new { Value = "۲۰۲۴", Label = "تأسیس", Icon = "calendar" },
                new { Value = "+۱۵", Label = "عضو تیم", Icon = "users" },
                new { Value = "+۳۰", Label = "کشور", Icon = "world" },
                new { Value = "۴.۸/۵", Label = "امتیاز خوانندگان", Icon = "star" }
            }}),
            Block("CallToAction", 7, new { Title = "می‌خواهید به تیم ما بپیوندید؟", Description = "ما همیشه به دنبال توسعه‌دهندگان و نویسندگان پرشور برای پیوستن به کدرسایت هستیم.", ButtonText = "مشاهده موقعیت‌های شغلی", ButtonLink = "/fa/تماس", BackgroundColor = "#020817" })
        ]));

        // ──── SERVICES (EN) ────
        db.Pages.Add(CreatePage("Services", "services", "en", LocServices, [
            Block("Heading", 0, new { Text = "Our Services", Subtitle = "Everything you need to grow as a developer", Level = "h1", Alignment = "center" }),
            Block("CardGrid", 1, new { Columns = 3, Cards = new[] {
                new { Title = "Technical Writing", Description = "Professional documentation and tutorial creation for your projects.", Icon = "pencil", LinkUrl = "#", LinkText = "Learn more" },
                new { Title = "Code Reviews", Description = "Expert code review services to improve quality and maintainability.", Icon = "code", LinkUrl = "#", LinkText = "Learn more" },
                new { Title = "Consulting", Description = "Architecture and technology consulting for your team.", Icon = "bulb", LinkUrl = "#", LinkText = "Learn more" },
                new { Title = "Workshops", Description = "Hands-on workshops for teams on modern development practices.", Icon = "users", LinkUrl = "#", LinkText = "Learn more" },
                new { Title = "Mentoring", Description = "One-on-one mentoring for developers at any career stage.", Icon = "heart", LinkUrl = "#", LinkText = "Learn more" },
                new { Title = "Open Source", Description = "Open source project support and contribution guidance.", Icon = "brand-github", LinkUrl = "#", LinkText = "Learn more" }
            }}),
            Block("PricingTable", 2, new { SectionTitle = "Plans", Plans = new[] {
                new { Name = "Free", Price = "$0", Period = "/month", Description = "For individual learners", Features = new[] { "Access to all articles", "Community forum", "Monthly newsletter" }, ButtonText = "Get Started", ButtonLink = "#", IsHighlighted = false },
                new { Name = "Pro", Price = "$19", Period = "/month", Description = "For serious developers", Features = new[] { "Everything in Free", "Interactive tutorials", "Code playgrounds", "Priority support" }, ButtonText = "Start Free Trial", ButtonLink = "#", IsHighlighted = true },
                new { Name = "Team", Price = "$49", Period = "/month", Description = "For development teams", Features = new[] { "Everything in Pro", "Team dashboard", "Custom workshops", "Dedicated support" }, ButtonText = "Contact Us", ButtonLink = "/en/contact", IsHighlighted = false }
            }}),
            Block("Faq", 3, new { SectionTitle = "Frequently Asked Questions", Items = new[] {
                new { Question = "How do I get started?", Answer = "Simply create a free account and start browsing our tutorial library." },
                new { Question = "Can I cancel my subscription?", Answer = "Yes, you can cancel anytime. No long-term commitments." },
                new { Question = "Do you offer team discounts?", Answer = "Yes! Contact us for custom team pricing." }
            }}),
            Block("CallToAction", 4, new { Title = "Not sure which plan is right for you?", Description = "Our team is happy to help you find the perfect fit.", ButtonText = "Contact Us", ButtonLink = "/en/contact" })
        ]));

        // ──── SERVICES (FA) ────
        db.Pages.Add(CreatePage("خدمات", "خدمات", "fa", LocServices, [
            Block("Heading", 0, new { Text = "خدمات ما", Subtitle = "همه آنچه برای رشد به‌عنوان یک توسعه‌دهنده نیاز دارید", Level = "h1", Alignment = "center" }),
            Block("CardGrid", 1, new { Columns = 3, Cards = new[] {
                new { Title = "نوشتن فنی", Description = "ایجاد مستندات و آموزش‌های حرفه‌ای برای پروژه‌های شما.", Icon = "pencil", LinkUrl = "#", LinkText = "بیشتر بدانید" },
                new { Title = "بررسی کد", Description = "خدمات بررسی کد متخصص برای بهبود کیفیت.", Icon = "code", LinkUrl = "#", LinkText = "بیشتر بدانید" },
                new { Title = "مشاوره", Description = "مشاوره معماری و فناوری برای تیم شما.", Icon = "bulb", LinkUrl = "#", LinkText = "بیشتر بدانید" }
            }}),
            Block("PricingTable", 2, new { SectionTitle = "پلن‌ها", Plans = new[] {
                new { Name = "رایگان", Price = "۰$", Period = "/ماه", Description = "برای یادگیرندگان", Features = new[] { "دسترسی به همه مقالات", "انجمن", "خبرنامه ماهانه" }, ButtonText = "شروع", ButtonLink = "#", IsHighlighted = false },
                new { Name = "حرفه‌ای", Price = "۱۹$", Period = "/ماه", Description = "برای توسعه‌دهندگان جدی", Features = new[] { "همه امکانات رایگان", "آموزش‌های تعاملی", "پشتیبانی اولویت‌دار" }, ButtonText = "شروع آزمایشی", ButtonLink = "#", IsHighlighted = true }
            }}),
            Block("Faq", 3, new { SectionTitle = "سؤالات متداول", Items = new[] {
                new { Question = "چگونه شروع کنم؟", Answer = "کافیست یک حساب رایگان بسازید و شروع به مرور کتابخانه آموزشی ما کنید." },
                new { Question = "آیا می‌توانم اشتراکم را لغو کنم؟", Answer = "بله، هر زمان بخواهید می‌توانید لغو کنید." }
            }}),
            Block("CallToAction", 4, new { Title = "مطمئن نیستید کدام پلن مناسب شماست؟", Description = "تیم ما خوشحال می‌شود به شما کمک کند.", ButtonText = "تماس با ما", ButtonLink = "/fa/تماس" })
        ]));

        // ──── CONTACT (EN) ────
        db.Pages.Add(CreatePage("Contact", "contact", "en", LocContact, [
            Block("Hero", 0, new { Title = "Get in Touch", Subtitle = "We'd love to hear from you. Reach out anytime.", BackgroundColor = "#020817" }),
            Block("FeaturesList", 1, new { SectionTitle = "Contact Information", Columns = 3, Features = new[] {
                new { Icon = "mail", Title = "Email Us", Description = "hello@codersight.com — We reply within 24 hours" },
                new { Icon = "map-pin", Title = "Location", Description = "San Francisco, CA — United States" },
                new { Icon = "clock", Title = "Office Hours", Description = "Mon - Fri, 9am - 5pm — Pacific Standard Time" }
            }}),
            Block("ContactForm", 2, new { Title = "Send us a message", Description = "Fill out the form and we'll get back to you shortly.", RecipientEmail = "hello@codersight.com", SubmitButtonText = "Send Message", Fields = new[] {
                new { Label = "Your Name", Type = "text", Required = true, Placeholder = "John Doe" },
                new { Label = "Email Address", Type = "email", Required = true, Placeholder = "john@example.com" },
                new { Label = "Subject", Type = "text", Required = true, Placeholder = "General Inquiry" },
                new { Label = "Message", Type = "textarea", Required = true, Placeholder = "Tell us how we can help..." }
            }}),
            Block("MapEmbed", 3, new { EmbedUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d100939.98555098464!2d-122.507640204439!3d37.757631337886!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x80859a6d00690021%3A0x4a501367f076adff!2sSan+Francisco!5e0!3m2!1sen!2sus!4v1", Height = 400 }),
            Block("Faq", 4, new { SectionTitle = "Common Questions", Items = new[] {
                new { Question = "How quickly do you respond?", Answer = "We typically respond to all inquiries within 24 hours during business days. For urgent matters, please mention it in the subject line." },
                new { Question = "Do you offer partnership opportunities?", Answer = "Yes! We're always open to collaborations with developers, companies, and content creators. Reach out with your proposal and we'll explore how we can work together." },
                new { Question = "Can I contribute articles to CoderSight?", Answer = "Absolutely! We welcome guest contributions from experienced developers. Send us a brief pitch with your topic idea and writing samples." }
            }})
        ]));

        // ──── CONTACT (FA) ────
        db.Pages.Add(CreatePage("تماس با ما", "تماس", "fa", LocContact, [
            Block("Hero", 0, new { Title = "تماس با ما", Subtitle = "خوشحال می‌شویم از شما بشنویم. هر زمان با ما تماس بگیرید.", BackgroundColor = "#020817" }),
            Block("FeaturesList", 1, new { SectionTitle = "اطلاعات تماس", Columns = 3, Features = new[] {
                new { Icon = "mail", Title = "ایمیل", Description = "hello@codersight.com — ظرف ۲۴ ساعت پاسخ می‌دهیم" },
                new { Icon = "map-pin", Title = "مکان", Description = "سانفرانسیسکو، کالیفرنیا — ایالات متحده" },
                new { Icon = "clock", Title = "ساعات کاری", Description = "دوشنبه تا جمعه، ۹ صبح تا ۵ عصر — وقت اقیانوس آرام" }
            }}),
            Block("ContactForm", 2, new { Title = "برای ما پیام بفرستید", Description = "فرم را پر کنید و ما به زودی پاسخ خواهیم داد.", RecipientEmail = "hello@codersight.com", SubmitButtonText = "ارسال پیام", Fields = new[] {
                new { Label = "نام شما", Type = "text", Required = true, Placeholder = "نام و نام خانوادگی" },
                new { Label = "آدرس ایمیل", Type = "email", Required = true, Placeholder = "email@example.com" },
                new { Label = "موضوع", Type = "text", Required = true, Placeholder = "درخواست عمومی" },
                new { Label = "پیام", Type = "textarea", Required = true, Placeholder = "بگویید چگونه می‌توانیم کمک کنیم..." }
            }}),
            Block("MapEmbed", 3, new { EmbedUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d100939.98555098464!2d-122.507640204439!3d37.757631337886!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x80859a6d00690021%3A0x4a501367f076adff!2sSan+Francisco!5e0!3m2!1sen!2sus!4v1", Height = 400 }),
            Block("Faq", 4, new { SectionTitle = "سؤالات متداول", Items = new[] {
                new { Question = "چقدر سریع پاسخ می‌دهید؟", Answer = "ما معمولاً ظرف ۲۴ ساعت در روزهای کاری به تمام درخواست‌ها پاسخ می‌دهیم. برای موارد فوری، لطفاً در موضوع ذکر کنید." },
                new { Question = "آیا فرصت‌های همکاری ارائه می‌دهید؟", Answer = "بله! ما همیشه آماده همکاری با توسعه‌دهندگان، شرکت‌ها و تولیدکنندگان محتوا هستیم. پیشنهاد خود را ارسال کنید و ما بررسی خواهیم کرد." },
                new { Question = "آیا می‌توانم مقاله برای کدرسایت بنویسم؟", Answer = "قطعاً! ما از مشارکت مهمان توسعه‌دهندگان باتجربه استقبال می‌کنیم. یک خلاصه کوتاه از موضوع و نمونه نوشته‌هایتان ارسال کنید." }
            }})
        ]));

        // ──── BLOG LISTING (EN) ────
        db.Pages.Add(CreatePage("Blog", "blog", "en", LocBlog, [
            Block("Heading", 0, new { Text = "Blog", Subtitle = "Latest articles, tutorials, and insights", Level = "h1", Alignment = "center" }),
            Block("PostList", 1, new { PostsPerPage = 9, Layout = "grid", ShowCategoryFilter = true, ShowSearch = true }),
            Block("Newsletter", 2, new { Title = "Never miss an article", Description = "Subscribe to get notified when we publish new content.", ButtonText = "Subscribe", Placeholder = "Your email address" })
        ]));

        // ──── BLOG LISTING (FA) ────
        db.Pages.Add(CreatePage("بلاگ", "بلاگ", "fa", LocBlog, [
            Block("Heading", 0, new { Text = "بلاگ", Subtitle = "آخرین مقالات، آموزش‌ها و بینش‌ها", Level = "h1", Alignment = "center" }),
            Block("PostList", 1, new { PostsPerPage = 9, Layout = "grid", ShowCategoryFilter = true, ShowSearch = true }),
            Block("Newsletter", 2, new { Title = "هیچ مقاله‌ای را از دست ندهید", Description = "مشترک شوید تا از انتشار محتوای جدید مطلع شوید.", ButtonText = "اشتراک", Placeholder = "آدرس ایمیل شما" })
        ]));

        await db.SaveChangesAsync();
    }

    private static async Task SeedBlogPostsAsync(CmsDbContext db)
    {
        // ──── BLOG POST 1 (EN) ────
        var post1PageEn = CreatePage("Getting Started with CoderSight", "blog/getting-started", "en", LocPost1, [
            Block("Hero", 0, new { Title = "Getting Started with CoderSight", Subtitle = "A beginner's guide to making the most of our platform.", BackgroundColor = "#1e293b" }),
            Block("TableOfContents", 1, new { Title = "In this article", MinHeadingLevel = 2, MaxHeadingLevel = 3 }),
            Block("RichText", 2, new { Content = "<h2>Introduction</h2><p>Welcome to CoderSight! Whether you're a seasoned developer or just starting your coding journey, this guide will help you navigate our platform and make the most of the resources available to you.</p><h2>Creating Your Account</h2><p>Getting started is simple. Head to our registration page and create a free account. You'll get immediate access to our entire library of articles and tutorials.</p><h2>Exploring Content</h2><p>Our content is organized by categories and tags, making it easy to find exactly what you're looking for. Use the search bar or browse by topic to discover articles that match your interests.</p><h2>Interactive Features</h2><p>Pro subscribers get access to interactive code playgrounds where you can experiment with the code examples from our tutorials directly in the browser.</p>" }),
            Block("CodeSnippet", 3, new { Code = "// Example: Your first C# program\nConsole.WriteLine(\"Hello, CoderSight!\");\n\n// Explore our tutorials for more examples\nvar topics = new[] { \"Blazor\", \"ASP.NET\", \"EF Core\" };\nforeach (var topic in topics)\n{\n    Console.WriteLine($\"Learn {topic} on CoderSight!\");\n}", Language = "csharp", FileName = "GettingStarted.cs" }),
            Block("RichText", 4, new { Content = "<h2>What's Next?</h2><p>Now that you're set up, we recommend starting with our most popular tutorials. Check out the related posts below for suggestions, or browse our full catalog.</p>" }),
            Block("AuthorBio", 5, new { Name = "Alex Chen", Bio = "Founder of CoderSight with 15 years of full-stack development experience. Passionate about developer education and open source.", GitHubUrl = "#", TwitterUrl = "#", LinkedInUrl = "#" }),
            Block("ShareButtons", 6, new { ShowTwitter = true, ShowLinkedIn = true, ShowFacebook = true, ShowCopyLink = true }),
            Block("RelatedPosts", 7, new { Title = "You might also like", MaxPosts = 3 })
        ]);
        db.Pages.Add(post1PageEn);

        // ──── BLOG POST 1 (FA) ────
        var post1PageFa = CreatePage("شروع کار با کدرسایت", "بلاگ/شروع-کار", "fa", LocPost1, [
            Block("Hero", 0, new { Title = "شروع کار با کدرسایت", Subtitle = "راهنمای مبتدیان برای استفاده حداکثری از پلتفرم ما.", BackgroundColor = "#1e293b" }),
            Block("TableOfContents", 1, new { Title = "در این مقاله", MinHeadingLevel = 2, MaxHeadingLevel = 3 }),
            Block("RichText", 2, new { Content = "<h2>مقدمه</h2><p>به کدرسایت خوش آمدید! چه یک توسعه‌دهنده باتجربه باشید و چه تازه سفر کدنویسی خود را شروع کرده‌اید، این راهنما به شما کمک می‌کند پلتفرم ما را بشناسید.</p><h2>ایجاد حساب کاربری</h2><p>شروع کار ساده است. به صفحه ثبت‌نام بروید و یک حساب رایگان بسازید.</p>" }),
            Block("AuthorBio", 3, new { Name = "الکس چن", Bio = "بنیان‌گذار کدرسایت با ۱۵ سال تجربه توسعه فول‌استک." }),
            Block("ShareButtons", 4, new { ShowTwitter = true, ShowLinkedIn = true, ShowFacebook = true, ShowCopyLink = true })
        ]);
        db.Pages.Add(post1PageFa);

        // ──── BLOG POST 2 (EN) ────
        var post2PageEn = CreatePage("10 Tips for Better Code Reviews", "blog/top-10-tips", "en", LocPost2, [
            Block("Hero", 0, new { Title = "10 Tips for Better Code Reviews", Subtitle = "Improve your team's code quality with these proven strategies.", BackgroundColor = "#0f172a" }),
            Block("RichText", 1, new { Content = "<h2>Why Code Reviews Matter</h2><p>Code reviews are one of the most effective ways to improve code quality, share knowledge, and catch bugs early. But not all code reviews are created equal. Here are our top 10 tips for making them more effective.</p><h2>1. Keep PRs Small</h2><p>Smaller pull requests are easier to review thoroughly. Aim for under 400 lines of changes when possible.</p><h2>2. Write Descriptive PR Descriptions</h2><p>Help reviewers understand the context. Explain what changed, why, and how to test it.</p><h2>3. Review Promptly</h2><p>Don't let PRs sit for days. Quick turnaround keeps momentum and reduces context switching.</p><h2>4. Focus on Logic, Not Style</h2><p>Use automated formatters and linters for style. Reserve human review for logic, architecture, and correctness.</p><h2>5. Ask Questions Instead of Making Demands</h2><p>\"What do you think about...\" is more productive than \"Change this to...\"</p>" }),
            Block("Image", 2, new { ImageUrl = "/images/placeholder-code-review.svg", AltText = "Code review process diagram", Caption = "A typical code review workflow" }),
            Block("Blockquote", 3, new { Quote = "The best code reviews are conversations, not inspections.", Attribution = "Unknown" }),
            Block("RichText", 4, new { Content = "<h2>6. Test the Changes</h2><p>Don't just read the code — pull the branch and test it.</p><h2>7. Praise Good Work</h2><p>Positive feedback encourages good practices.</p><h2>8. Use Checklists</h2><p>A simple checklist ensures consistency across reviews.</p><h2>9. Set Clear Expectations</h2><p>Define what \"approved\" means for your team.</p><h2>10. Learn From Every Review</h2><p>Both author and reviewer should grow from the process.</p>" }),
            Block("AuthorBio", 5, new { Name = "Maria Rodriguez", Bio = "Head of Content at CoderSight. Technical writer and educator with a passion for clear communication.", LinkedInUrl = "#", TwitterUrl = "#" }),
            Block("RelatedPosts", 6, new { Title = "Related Articles", MaxPosts = 3, FilterByCategory = "best-practices" })
        ]);
        db.Pages.Add(post2PageEn);

        // ──── BLOG POST 2 (FA) ────
        var post2PageFa = CreatePage("۱۰ نکته برای بررسی کد بهتر", "بلاگ/۱۰-نکته", "fa", LocPost2, [
            Block("Hero", 0, new { Title = "۱۰ نکته برای بررسی کد بهتر", Subtitle = "کیفیت کد تیم خود را با این استراتژی‌های اثبات‌شده بهبود دهید.", BackgroundColor = "#0f172a" }),
            Block("RichText", 1, new { Content = "<h2>چرا بررسی کد مهم است؟</h2><p>بررسی کد یکی از مؤثرترین راه‌ها برای بهبود کیفیت کد، اشتراک دانش و یافتن زودهنگام باگ‌هاست.</p><h2>۱. درخواست‌های کوچک بفرستید</h2><p>درخواست‌های کوچکتر راحت‌تر بررسی می‌شوند.</p><h2>۲. توضیحات توصیفی بنویسید</h2><p>به بررسی‌کنندگان کمک کنید زمینه را درک کنند.</p>" }),
            Block("Blockquote", 2, new { Quote = "بهترین بررسی‌های کد گفتگو هستند، نه بازرسی.", Attribution = "ناشناس" }),
            Block("AuthorBio", 3, new { Name = "ماریا رودریگز", Bio = "مدیر محتوا در کدرسایت. نویسنده فنی و آموزگار." }),
            Block("RelatedPosts", 4, new { Title = "مقالات مرتبط", MaxPosts = 3 })
        ]);
        db.Pages.Add(post2PageFa);

        await db.SaveChangesAsync();

        // Create BlogPost entries
        var now = DateTime.UtcNow;
        db.BlogPosts.AddRange(
            new BlogPost { Id = Guid.NewGuid(), PageId = post1PageEn.Id, Excerpt = "A beginner's guide to making the most of the CoderSight platform.", ReadTimeMinutes = 5, IsFeatured = true },
            new BlogPost { Id = Guid.NewGuid(), PageId = post1PageFa.Id, Excerpt = "راهنمای مبتدیان برای استفاده حداکثری از پلتفرم کدرسایت.", ReadTimeMinutes = 5, IsFeatured = true },
            new BlogPost { Id = Guid.NewGuid(), PageId = post2PageEn.Id, Excerpt = "Improve your team's code quality with these 10 proven code review strategies.", ReadTimeMinutes = 8, IsFeatured = false },
            new BlogPost { Id = Guid.NewGuid(), PageId = post2PageFa.Id, Excerpt = "کیفیت کد تیم خود را با این ۱۰ استراتژی اثبات‌شده بهبود دهید.", ReadTimeMinutes = 8, IsFeatured = false }
        );
        await db.SaveChangesAsync();

        // Link blog posts to categories and tags
        var blogPosts = await db.BlogPosts.Include(bp => bp.Page).ToListAsync();
        var post1En = blogPosts.First(bp => bp.Page.Slug == "blog/getting-started");
        var post2En = blogPosts.First(bp => bp.Page.Slug == "blog/top-10-tips");

        db.BlogPostCategories.AddRange(
            new BlogPostCategory { BlogPostId = post1En.Id, CategoryId = CatTutorialsEn },
            new BlogPostCategory { BlogPostId = post2En.Id, CategoryId = CatBestPracticesEn }
        );

        db.BlogPostTags.AddRange(
            new BlogPostTag { BlogPostId = post1En.Id, TagId = TagGettingStartedEn },
            new BlogPostTag { BlogPostId = post1En.Id, TagId = TagBeginnerEn },
            new BlogPostTag { BlogPostId = post2En.Id, TagId = TagCodeReviewEn },
            new BlogPostTag { BlogPostId = post2En.Id, TagId = TagProductivityEn },
            new BlogPostTag { BlogPostId = post2En.Id, TagId = TagTipsEn }
        );

        await db.SaveChangesAsync();
    }

    private static Page CreatePage(string title, string slug, string culture, Guid locGroupId, List<PageBlock> blocks)
    {
        var page = new Page
        {
            Id = Guid.NewGuid(),
            Title = title,
            Slug = slug,
            Culture = culture,
            Status = PageStatus.Published,
            PublishedAt = DateTime.UtcNow,
            LocalizationGroupId = locGroupId,
            MetaTitle = title,
            MetaDescription = $"{title} — CoderSight",
            Blocks = blocks
        };
        return page;
    }

    private static PageBlock Block(string type, int order, object data) => new()
    {
        Id = Guid.NewGuid(),
        BlockType = type,
        SortOrder = order,
        Data = JsonSerializer.Serialize(data, Json)
    };
}
