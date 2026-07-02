using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoderSight.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePageDesigns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Delete existing blocks for Home, About, Contact pages (identified by LocalizationGroupId)
            migrationBuilder.Sql(@"
                DELETE FROM [PageBlocks]
                WHERE [PageId] IN (
                    SELECT [Id] FROM [Pages]
                    WHERE [LocalizationGroupId] IN (
                        'c0000000-0000-0000-0000-000000000001',  -- Home
                        'c0000000-0000-0000-0000-000000000002',  -- About
                        'c0000000-0000-0000-0000-000000000004'   -- Contact
                    )
                )
            ");

            // ──── HOME (EN) ────
            InsertBlocks(migrationBuilder, "''", "en", new[]
            {
                (0, "Slider", @"{""IntervalMs"":5000,""ShowArrows"":true,""ShowDots"":true,""Slides"":[{""ImageUrl"":"""",""Title"":""Build Better Software"",""Subtitle"":""Practical tutorials, expert insights, and hands-on guides for modern developers."",""ButtonText"":""Get Started"",""ButtonLink"":""/en/blog""},{""ImageUrl"":"""",""Title"":""Learn from Industry Experts"",""Subtitle"":""Deep-dive articles written by experienced engineers and technical leaders."",""ButtonText"":""Browse Articles"",""ButtonLink"":""/en/blog""},{""ImageUrl"":"""",""Title"":""Join the Community"",""Subtitle"":""Connect with 50,000+ developers sharing knowledge and growing together."",""ButtonText"":""Sign Up Free"",""ButtonLink"":""/register""}]}"),
                (1, "Stats", @"{""BackgroundColor"":""#020817"",""Stats"":[{""Value"":""500+"",""Label"":""Articles"",""Icon"":""article""},{""Value"":""50K+"",""Label"":""Monthly Readers"",""Icon"":""users""},{""Value"":""100+"",""Label"":""Contributors"",""Icon"":""pencil""},{""Value"":""20+"",""Label"":""Topics"",""Icon"":""category""}]}"),
                (2, "FeaturesList", @"{""SectionTitle"":""Why CoderSight?"",""Columns"":3,""Features"":[{""Icon"":""code"",""Title"":""In-Depth Tutorials"",""Description"":""Step-by-step guides on modern development practices and tools.""},{""Icon"":""bulb"",""Title"":""Expert Insights"",""Description"":""Learn from experienced developers and industry leaders.""},{""Icon"":""rocket"",""Title"":""Stay Current"",""Description"":""Keep up with the latest trends, frameworks, and technologies.""}]}"),
                (3, "CardCarousel", @"{""SectionTitle"":""Latest Articles"",""SectionSubtitle"":""Fresh content from our contributors"",""AutoPlay"":true,""AutoPlayInterval"":4,""Cards"":[{""Title"":""Getting Started with Blazor"",""Description"":""A beginner's guide to building web apps with Blazor Server."",""Badge"":""Tutorial"",""ImageUrl"":null,""LinkUrl"":""/en/blog/getting-started"",""LinkText"":""Read more""},{""Title"":""10 Tips for Code Reviews"",""Description"":""Improve your team's code quality with proven strategies."",""Badge"":""Best Practice"",""ImageUrl"":null,""LinkUrl"":""/en/blog/top-10-tips"",""LinkText"":""Read more""},{""Title"":""EF Core Performance"",""Description"":""Optimize your Entity Framework queries for production."",""Badge"":""Guide"",""ImageUrl"":null,""LinkUrl"":""/en/blog"",""LinkText"":""Read more""},{""Title"":""REST API Design"",""Description"":""Design clean, maintainable REST APIs with ASP.NET Core."",""Badge"":""Tutorial"",""ImageUrl"":null,""LinkUrl"":""/en/blog"",""LinkText"":""Read more""}]}"),
                (4, "Reviews", @"{""SectionTitle"":""What Developers Say"",""Reviews"":[{""ReviewerName"":""Sarah Johnson, Full-Stack Developer"",""Rating"":5,""Text"":""CoderSight helped me level up my skills. The tutorials are practical and well-structured."",""Date"":null},{""ReviewerName"":""Michael Torres, Senior Engineer at TechCorp"",""Rating"":5,""Text"":""The best resource for staying current with .NET and web development trends."",""Date"":null},{""ReviewerName"":""Emily Zhang, Software Engineer"",""Rating"":5,""Text"":""Clear explanations with real-world examples. Exactly what I needed for my career growth."",""Date"":null}]}"),
                (5, "ProgressSteps", @"{""SectionTitle"":""How It Works"",""Steps"":[{""Title"":""Sign Up"",""Description"":""Create your free account"",""Icon"":""user-plus""},{""Title"":""Browse"",""Description"":""Explore tutorials by topic"",""Icon"":""search""},{""Title"":""Learn"",""Description"":""Follow hands-on guides"",""Icon"":""book""},{""Title"":""Build"",""Description"":""Apply skills to real projects"",""Icon"":""hammer""}]}"),
                (6, "CallToAction", @"{""Title"":""Ready to level up?"",""Description"":""Join thousands of developers who trust CoderSight for their learning journey."",""ButtonText"":""Browse Articles"",""ButtonLink"":""/en/blog"",""BackgroundColor"":""#020817""}"),
                (7, "Newsletter", @"{""Title"":""Stay in the loop"",""Description"":""Get the latest articles and tips delivered to your inbox."",""ButtonText"":""Subscribe"",""Placeholder"":""Enter your email""}")
            });

            // ──── HOME (FA) ────
            InsertBlocks(migrationBuilder, "''", "fa", new[]
            {
                (0, "Slider", "{\"IntervalMs\":5000,\"ShowArrows\":true,\"ShowDots\":true,\"Slides\":[{\"ImageUrl\":\"\",\"Title\":\"نرم‌افزار بهتر بسازید\",\"Subtitle\":\"آموزش‌های عملی، بینش‌های متخصصان و راهنماهای کاربردی برای توسعه‌دهندگان مدرن.\",\"ButtonText\":\"شروع کنید\",\"ButtonLink\":\"/fa/بلاگ\"},{\"ImageUrl\":\"\",\"Title\":\"از متخصصان صنعت بیاموزید\",\"Subtitle\":\"مقالات عمیق نوشته‌شده توسط مهندسان باتجربه و رهبران فنی.\",\"ButtonText\":\"مرور مقالات\",\"ButtonLink\":\"/fa/بلاگ\"},{\"ImageUrl\":\"\",\"Title\":\"به جامعه بپیوندید\",\"Subtitle\":\"با بیش از ۵۰ هزار توسعه‌دهنده در اشتراک دانش و رشد مشترک همراه شوید.\",\"ButtonText\":\"ثبت‌نام رایگان\",\"ButtonLink\":\"/register\"}]}"),
                (1, "Stats", "{\"BackgroundColor\":\"#020817\",\"Stats\":[{\"Value\":\"+۵۰۰\",\"Label\":\"مقاله\",\"Icon\":\"article\"},{\"Value\":\"+۵۰ هزار\",\"Label\":\"خواننده ماهانه\",\"Icon\":\"users\"},{\"Value\":\"+۱۰۰\",\"Label\":\"مشارکت‌کننده\",\"Icon\":\"pencil\"},{\"Value\":\"+۲۰\",\"Label\":\"موضوع\",\"Icon\":\"category\"}]}"),
                (2, "FeaturesList", "{\"SectionTitle\":\"چرا کدرسایت؟\",\"Columns\":3,\"Features\":[{\"Icon\":\"code\",\"Title\":\"آموزش‌های عمیق\",\"Description\":\"راهنماهای گام‌به‌گام درباره شیوه‌ها و ابزارهای مدرن توسعه.\"},{\"Icon\":\"bulb\",\"Title\":\"بینش‌های متخصصان\",\"Description\":\"از توسعه‌دهندگان باتجربه و رهبران صنعت بیاموزید.\"},{\"Icon\":\"rocket\",\"Title\":\"به‌روز بمانید\",\"Description\":\"با آخرین روندها، فریمورک‌ها و فناوری‌ها همراه باشید.\"}]}"),
                (3, "CardCarousel", "{\"SectionTitle\":\"آخرین مقالات\",\"SectionSubtitle\":\"محتوای تازه از مشارکت‌کنندگان ما\",\"AutoPlay\":true,\"AutoPlayInterval\":4,\"Cards\":[{\"Title\":\"شروع کار با بلیزور\",\"Description\":\"راهنمای مبتدیان برای ساخت وب‌اپ با بلیزور سرور.\",\"Badge\":\"آموزش\",\"ImageUrl\":null,\"LinkUrl\":\"/fa/بلاگ/شروع-کار\",\"LinkText\":\"ادامه مطلب\"},{\"Title\":\"۱۰ نکته برای بررسی کد\",\"Description\":\"کیفیت کد تیم خود را با استراتژی‌های اثبات‌شده بهبود دهید.\",\"Badge\":\"بهترین شیوه\",\"ImageUrl\":null,\"LinkUrl\":\"/fa/بلاگ/۱۰-نکته\",\"LinkText\":\"ادامه مطلب\"},{\"Title\":\"عملکرد EF Core\",\"Description\":\"کوئری‌های Entity Framework خود را برای تولید بهینه کنید.\",\"Badge\":\"راهنما\",\"ImageUrl\":null,\"LinkUrl\":\"/fa/بلاگ\",\"LinkText\":\"ادامه مطلب\"},{\"Title\":\"طراحی REST API\",\"Description\":\"APIهای REST تمیز و قابل نگهداری با ASP.NET Core طراحی کنید.\",\"Badge\":\"آموزش\",\"ImageUrl\":null,\"LinkUrl\":\"/fa/بلاگ\",\"LinkText\":\"ادامه مطلب\"}]}"),
                (4, "Reviews", "{\"SectionTitle\":\"نظر توسعه‌دهندگان\",\"Reviews\":[{\"ReviewerName\":\"سارا جانسون، توسعه‌دهنده فول‌استک\",\"Rating\":5,\"Text\":\"کدرسایت به من کمک کرد مهارت‌هایم را ارتقا دهم. آموزش‌ها عملی و خوب ساختاربندی شده‌اند.\",\"Date\":null},{\"ReviewerName\":\"مایکل تورس، مهندس ارشد در تک‌کورپ\",\"Rating\":5,\"Text\":\"بهترین منبع برای به‌روز ماندن با روندهای .NET و توسعه وب.\",\"Date\":null},{\"ReviewerName\":\"امیلی ژانگ، مهندس نرم‌افزار\",\"Rating\":5,\"Text\":\"توضیحات واضح با مثال‌های واقعی. دقیقاً آنچه برای رشد حرفه‌ای‌ام نیاز داشتم.\",\"Date\":null}]}"),
                (5, "ProgressSteps", "{\"SectionTitle\":\"چگونه کار می‌کند\",\"Steps\":[{\"Title\":\"ثبت‌نام\",\"Description\":\"حساب رایگان خود را بسازید\",\"Icon\":\"user-plus\"},{\"Title\":\"مرور\",\"Description\":\"آموزش‌ها را بر اساس موضوع کاوش کنید\",\"Icon\":\"search\"},{\"Title\":\"یادگیری\",\"Description\":\"راهنماهای عملی را دنبال کنید\",\"Icon\":\"book\"},{\"Title\":\"ساخت\",\"Description\":\"مهارت‌ها را در پروژه‌های واقعی به کار ببرید\",\"Icon\":\"hammer\"}]}"),
                (6, "CallToAction", "{\"Title\":\"آماده ارتقا هستید؟\",\"Description\":\"به هزاران توسعه‌دهنده‌ای بپیوندید که به کدرسایت برای مسیر یادگیری‌شان اعتماد دارند.\",\"ButtonText\":\"مرور مقالات\",\"ButtonLink\":\"/fa/بلاگ\",\"BackgroundColor\":\"#020817\"}"),
                (7, "Newsletter", "{\"Title\":\"در جریان باشید\",\"Description\":\"آخرین مقالات و نکات را در صندوق ورودی خود دریافت کنید.\",\"ButtonText\":\"اشتراک\",\"Placeholder\":\"ایمیل خود را وارد کنید\"}")
            });

            // ──── ABOUT (EN) ────
            InsertBlocks(migrationBuilder, "'about'", "en", new[]
            {
                (0, "Hero", @"{""Title"":""About CoderSight"",""Subtitle"":""Empowering developers with knowledge since 2024."",""BackgroundColor"":""#020817""}"),
                (1, "RichText", @"{""Content"":""<h2>Our Mission</h2><p>CoderSight was founded with a simple goal: to make high-quality developer education accessible to everyone. We believe that great software starts with great developers, and great developers never stop learning.</p><p>Our team of experienced engineers and technical writers creates content that bridges the gap between theory and practice, helping developers at every level build better software.</p>""}"),
                (2, "Heading", @"{""Text"":""What Drives Us"",""Level"":""h2"",""Alignment"":""center""}"),
                (3, "Tabs", @"{""Tabs"":[{""Title"":""Our Mission"",""Content"":""<h3>Democratize Developer Education</h3><p>We make high-quality, practical development knowledge freely accessible. Every tutorial is written to bridge theory and real-world application, regardless of your experience level.</p>"",""Icon"":""target""},{""Title"":""Our Vision"",""Content"":""<h3>A World of Confident Builders</h3><p>We envision a future where every developer has the resources and community to build with confidence — turning ideas into production-ready software.</p>"",""Icon"":""eye""},{""Title"":""Our Values"",""Content"":""<h3>Clarity · Community · Craft</h3><p>We value clear communication over jargon, community growth over competition, and software craftsmanship over shortcuts. Every piece of content reflects these principles.</p>"",""Icon"":""heart""}]}"),
                (4, "Timeline", @"{""SectionTitle"":""Our Journey"",""Events"":[{""Date"":""2024"",""Title"":""Founded"",""Description"":""CoderSight launched with a focus on practical developer tutorials."",""Icon"":""rocket""},{""Date"":""2024"",""Title"":""100 Articles"",""Description"":""Reached our first major content milestone."",""Icon"":""article""},{""Date"":""2025"",""Title"":""Community Growth"",""Description"":""Crossed 50,000 monthly active readers."",""Icon"":""users""},{""Date"":""2026"",""Title"":""Platform Expansion"",""Description"":""Launched interactive tutorials and code playgrounds."",""Icon"":""code""}]}"),
                (5, "Team", @"{""SectionTitle"":""Meet the Team"",""Columns"":4,""Members"":[{""Name"":""Alex Chen"",""Role"":""Founder & Lead Engineer"",""Bio"":""15 years of full-stack experience."",""ImageUrl"":null,""LinkedInUrl"":""#"",""TwitterUrl"":""#""},{""Name"":""Maria Rodriguez"",""Role"":""Head of Content"",""Bio"":""Technical writer and educator."",""ImageUrl"":null,""LinkedInUrl"":""#"",""TwitterUrl"":""#""},{""Name"":""James Kim"",""Role"":""Senior Developer"",""Bio"":""Open-source contributor and speaker."",""ImageUrl"":null,""LinkedInUrl"":""#"",""TwitterUrl"":""#""},{""Name"":""Priya Sharma"",""Role"":""Community Manager"",""Bio"":""Building developer communities."",""ImageUrl"":null,""LinkedInUrl"":""#"",""TwitterUrl"":""#""}]}"),
                (6, "Stats", @"{""BackgroundColor"":""#020817"",""Stats"":[{""Value"":""2024"",""Label"":""Founded"",""Icon"":""calendar""},{""Value"":""15+"",""Label"":""Team Members"",""Icon"":""users""},{""Value"":""30+"",""Label"":""Countries Reached"",""Icon"":""world""},{""Value"":""4.8/5"",""Label"":""Reader Rating"",""Icon"":""star""}]}"),
                (7, "CallToAction", @"{""Title"":""Want to join our team?"",""Description"":""We're always looking for passionate developers and writers to join CoderSight."",""ButtonText"":""View Open Positions"",""ButtonLink"":""/en/contact"",""BackgroundColor"":""#020817""}")
            });

            // ──── ABOUT (FA) ────
            InsertBlocks(migrationBuilder, "N'درباره-ما'", "fa", new[]
            {
                (0, "Hero", "{\"Title\":\"درباره کدرسایت\",\"Subtitle\":\"توانمندسازی توسعه‌دهندگان با دانش از سال ۲۰۲۴.\",\"BackgroundColor\":\"#020817\"}"),
                (1, "RichText", "{\"Content\":\"<h2>مأموریت ما</h2><p>کدرسایت با یک هدف ساده تأسیس شد: دسترسی همگان به آموزش باکیفیت توسعه‌دهندگی. ما معتقدیم نرم‌افزار عالی با توسعه‌دهندگان عالی شروع می‌شود و توسعه‌دهندگان عالی هرگز از یادگیری دست نمی‌کشند.</p><p>تیم ما از مهندسان باتجربه و نویسندگان فنی محتوایی می‌سازد که شکاف بین تئوری و عمل را پر می‌کند و به توسعه‌دهندگان در هر سطحی کمک می‌کند نرم‌افزار بهتری بسازند.</p>\"}"),
                (2, "Heading", "{\"Text\":\"چه چیزی ما را هدایت می‌کند\",\"Level\":\"h2\",\"Alignment\":\"center\"}"),
                (3, "Tabs", "{\"Tabs\":[{\"Title\":\"مأموریت ما\",\"Content\":\"<h3>دموکراتیزه کردن آموزش توسعه‌دهندگی</h3><p>ما دانش توسعه عملی و باکیفیت را آزادانه در دسترس قرار می‌دهیم. هر آموزش برای پل زدن بین تئوری و کاربرد واقعی نوشته شده است، صرف‌نظر از سطح تجربه شما.</p>\",\"Icon\":\"target\"},{\"Title\":\"چشم‌انداز ما\",\"Content\":\"<h3>جهانی از سازندگان با اعتماد به نفس</h3><p>ما آینده‌ای را تصور می‌کنیم که هر توسعه‌دهنده منابع و جامعه لازم برای ساختن با اطمینان را داشته باشد — تبدیل ایده‌ها به نرم‌افزار آماده تولید.</p>\",\"Icon\":\"eye\"},{\"Title\":\"ارزش‌های ما\",\"Content\":\"<h3>وضوح · جامعه · صنعت</h3><p>ما ارتباط واضح را بر اصطلاحات فنی، رشد جامعه را بر رقابت، و صنعت‌گری نرم‌افزار را بر میان‌بُرها ارجح می‌دانیم. هر قطعه محتوا این اصول را منعکس می‌کند.</p>\",\"Icon\":\"heart\"}]}"),
                (4, "Timeline", "{\"SectionTitle\":\"مسیر ما\",\"Events\":[{\"Date\":\"۲۰۲۴\",\"Title\":\"تأسیس\",\"Description\":\"کدرسایت با تمرکز بر آموزش‌های عملی توسعه‌دهندگی راه‌اندازی شد.\",\"Icon\":\"rocket\"},{\"Date\":\"۲۰۲۴\",\"Title\":\"۱۰۰ مقاله\",\"Description\":\"به اولین نقطه عطف محتوایی رسیدیم.\",\"Icon\":\"article\"},{\"Date\":\"۲۰۲۵\",\"Title\":\"رشد جامعه\",\"Description\":\"از ۵۰ هزار خواننده فعال ماهانه عبور کردیم.\",\"Icon\":\"users\"},{\"Date\":\"۲۰۲۶\",\"Title\":\"گسترش پلتفرم\",\"Description\":\"آموزش‌های تعاملی و محیط‌های کدنویسی راه‌اندازی شد.\",\"Icon\":\"code\"}]}"),
                (5, "Team", "{\"SectionTitle\":\"تیم ما\",\"Columns\":4,\"Members\":[{\"Name\":\"الکس چن\",\"Role\":\"بنیان‌گذار و مهندس ارشد\",\"Bio\":\"۱۵ سال تجربه فول‌استک.\",\"ImageUrl\":null,\"LinkedInUrl\":\"#\",\"TwitterUrl\":\"#\"},{\"Name\":\"ماریا رودریگز\",\"Role\":\"مدیر محتوا\",\"Bio\":\"نویسنده فنی و آموزگار.\",\"ImageUrl\":null,\"LinkedInUrl\":\"#\",\"TwitterUrl\":\"#\"},{\"Name\":\"جیمز کیم\",\"Role\":\"توسعه‌دهنده ارشد\",\"Bio\":\"مشارکت‌کننده متن‌باز و سخنران.\",\"ImageUrl\":null,\"LinkedInUrl\":\"#\",\"TwitterUrl\":\"#\"},{\"Name\":\"پریا شارما\",\"Role\":\"مدیر جامعه\",\"Bio\":\"ساخت جوامع توسعه‌دهندگی.\",\"ImageUrl\":null,\"LinkedInUrl\":\"#\",\"TwitterUrl\":\"#\"}]}"),
                (6, "Stats", "{\"BackgroundColor\":\"#020817\",\"Stats\":[{\"Value\":\"۲۰۲۴\",\"Label\":\"تأسیس\",\"Icon\":\"calendar\"},{\"Value\":\"+۱۵\",\"Label\":\"عضو تیم\",\"Icon\":\"users\"},{\"Value\":\"+۳۰\",\"Label\":\"کشور\",\"Icon\":\"world\"},{\"Value\":\"۴.۸/۵\",\"Label\":\"امتیاز خوانندگان\",\"Icon\":\"star\"}]}"),
                (7, "CallToAction", "{\"Title\":\"می‌خواهید به تیم ما بپیوندید؟\",\"Description\":\"ما همیشه به دنبال توسعه‌دهندگان و نویسندگان پرشور برای پیوستن به کدرسایت هستیم.\",\"ButtonText\":\"مشاهده موقعیت‌های شغلی\",\"ButtonLink\":\"/fa/تماس\",\"BackgroundColor\":\"#020817\"}")
            });

            // ──── CONTACT (EN) ────
            InsertBlocks(migrationBuilder, "'contact'", "en", new[]
            {
                (0, "Hero", @"{""Title"":""Get in Touch"",""Subtitle"":""We'd love to hear from you. Reach out anytime."",""BackgroundColor"":""#020817""}"),
                (1, "FeaturesList", @"{""SectionTitle"":""Contact Information"",""Columns"":3,""Features"":[{""Icon"":""mail"",""Title"":""Email Us"",""Description"":""hello@codersight.com — We reply within 24 hours""},{""Icon"":""map-pin"",""Title"":""Location"",""Description"":""San Francisco, CA — United States""},{""Icon"":""clock"",""Title"":""Office Hours"",""Description"":""Mon - Fri, 9am - 5pm — Pacific Standard Time""}]}"),
                (2, "ContactForm", @"{""Title"":""Send us a message"",""Description"":""Fill out the form and we'll get back to you shortly."",""RecipientEmail"":""hello@codersight.com"",""SubmitButtonText"":""Send Message"",""Fields"":[{""Label"":""Your Name"",""Type"":""text"",""Required"":true,""Placeholder"":""John Doe""},{""Label"":""Email Address"",""Type"":""email"",""Required"":true,""Placeholder"":""john@example.com""},{""Label"":""Subject"",""Type"":""text"",""Required"":true,""Placeholder"":""General Inquiry""},{""Label"":""Message"",""Type"":""textarea"",""Required"":true,""Placeholder"":""Tell us how we can help...""}]}"),
                (3, "MapEmbed", @"{""EmbedUrl"":""https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d100939.98555098464!2d-122.507640204439!3d37.757631337886!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x80859a6d00690021%3A0x4a501367f076adff!2sSan+Francisco!5e0!3m2!1sen!2sus!4v1"",""Height"":400}"),
                (4, "Faq", @"{""SectionTitle"":""Common Questions"",""Items"":[{""Question"":""How quickly do you respond?"",""Answer"":""We typically respond to all inquiries within 24 hours during business days. For urgent matters, please mention it in the subject line.""},{""Question"":""Do you offer partnership opportunities?"",""Answer"":""Yes! We're always open to collaborations with developers, companies, and content creators. Reach out with your proposal and we'll explore how we can work together.""},{""Question"":""Can I contribute articles to CoderSight?"",""Answer"":""Absolutely! We welcome guest contributions from experienced developers. Send us a brief pitch with your topic idea and writing samples.""}]}")
            });

            // ──── CONTACT (FA) ────
            InsertBlocks(migrationBuilder, "N'تماس'", "fa", new[]
            {
                (0, "Hero", "{\"Title\":\"تماس با ما\",\"Subtitle\":\"خوشحال می‌شویم از شما بشنویم. هر زمان با ما تماس بگیرید.\",\"BackgroundColor\":\"#020817\"}"),
                (1, "FeaturesList", "{\"SectionTitle\":\"اطلاعات تماس\",\"Columns\":3,\"Features\":[{\"Icon\":\"mail\",\"Title\":\"ایمیل\",\"Description\":\"hello@codersight.com — ظرف ۲۴ ساعت پاسخ می‌دهیم\"},{\"Icon\":\"map-pin\",\"Title\":\"مکان\",\"Description\":\"سانفرانسیسکو، کالیفرنیا — ایالات متحده\"},{\"Icon\":\"clock\",\"Title\":\"ساعات کاری\",\"Description\":\"دوشنبه تا جمعه، ۹ صبح تا ۵ عصر — وقت اقیانوس آرام\"}]}"),
                (2, "ContactForm", "{\"Title\":\"برای ما پیام بفرستید\",\"Description\":\"فرم را پر کنید و ما به زودی پاسخ خواهیم داد.\",\"RecipientEmail\":\"hello@codersight.com\",\"SubmitButtonText\":\"ارسال پیام\",\"Fields\":[{\"Label\":\"نام شما\",\"Type\":\"text\",\"Required\":true,\"Placeholder\":\"نام و نام خانوادگی\"},{\"Label\":\"آدرس ایمیل\",\"Type\":\"email\",\"Required\":true,\"Placeholder\":\"email@example.com\"},{\"Label\":\"موضوع\",\"Type\":\"text\",\"Required\":true,\"Placeholder\":\"درخواست عمومی\"},{\"Label\":\"پیام\",\"Type\":\"textarea\",\"Required\":true,\"Placeholder\":\"بگویید چگونه می‌توانیم کمک کنیم...\"}]}"),
                (3, "MapEmbed", "{\"EmbedUrl\":\"https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d100939.98555098464!2d-122.507640204439!3d37.757631337886!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x80859a6d00690021%3A0x4a501367f076adff!2sSan+Francisco!5e0!3m2!1sen!2sus!4v1\",\"Height\":400}"),
                (4, "Faq", "{\"SectionTitle\":\"سؤالات متداول\",\"Items\":[{\"Question\":\"چقدر سریع پاسخ می‌دهید؟\",\"Answer\":\"ما معمولاً ظرف ۲۴ ساعت در روزهای کاری به تمام درخواست‌ها پاسخ می‌دهیم. برای موارد فوری، لطفاً در موضوع ذکر کنید.\"},{\"Question\":\"آیا فرصت‌های همکاری ارائه می‌دهید؟\",\"Answer\":\"بله! ما همیشه آماده همکاری با توسعه‌دهندگان، شرکت‌ها و تولیدکنندگان محتوا هستیم. پیشنهاد خود را ارسال کنید و ما بررسی خواهیم کرد.\"},{\"Question\":\"آیا می‌توانم مقاله برای کدرسایت بنویسم؟\",\"Answer\":\"قطعاً! ما از مشارکت مهمان توسعه‌دهندگان باتجربه استقبال می‌کنیم. یک خلاصه کوتاه از موضوع و نمونه نوشته‌هایتان ارسال کنید.\"}]}")
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Down migration not supported for data-only changes — restore from backup if needed
        }

        private static void InsertBlocks(MigrationBuilder migrationBuilder, string slugLiteral, string culture, (int order, string blockType, string json)[] blocks)
        {
            foreach (var (order, blockType, json) in blocks)
            {
                var escapedJson = json.Replace("'", "''");
                migrationBuilder.Sql($@"
                    INSERT INTO [PageBlocks] ([Id], [PageId], [BlockType], [SortOrder], [Data])
                    SELECT NEWID(), [Id], '{blockType}', {order}, N'{escapedJson}'
                    FROM [Pages]
                    WHERE [Slug] = {slugLiteral} AND [Culture] = '{culture}'
                ");
            }
        }
    }
}
