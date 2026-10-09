"""Generate roastino-seed.sql: the Roastino (رستینو) pages, nav and settings for a CoderSight database.

Usage: python3 gen_seed.py [out.sql] [--blocks-json blocks.json]
The optional blocks JSON (page slug -> list of {blockType, data, styles}) is for previewing the pages.
"""
import json
import os
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
U = "/uploads/roastino-"  # the app serves /uploads/<file> only, so files stay flat

# Warm Cream palette
INK, MUTED, CREAM, SURFACE, SAND, BORDER = "#2B1A14", "#6B5448", "#FBF4EA", "#FFFDF9", "#F3E6D3", "#E6D5BF"
RED, RED_DARK, ROAST = "#C8161F", "#A50F18", "#8A5A3B"


def j(o):
    return json.dumps(o, ensure_ascii=False, separators=(",", ":"))


def style(bg="transparent", **kw):
    """Block wrapper style. Blocks bring their own vertical padding, so the wrapper adds none."""
    s = {"backgroundColor": bg, "paddingTop": "0", "paddingBottom": "0", "paddingLeft": "0", "paddingRight": "0"}
    s.update(kw)
    return s


def nested(block_type, data):
    return {"blockType": block_type, "data": j(data)}


# Values filled in from the variables at the top of the script.
TOKENS = ["ADDRESS", "HOURS", "PHONE", "INSTAGRAM", "RECIPIENT_EMAIL", "MAP_EMBED_URL"]
ADDRESS, HOURS, PHONE, INSTA, EMAIL, MAP = ("{{%s}}" % t for t in TOKENS)

pages = [
    {
        "var": "@HomeId", "title": "خانه", "slug": "",
        "metaTitle": "رستینو | کافه‌بار و فروشگاه قهوه",
        "metaDesc": "رستینو، کافه‌بار و فروشگاه قهوه. قهوه را پشتِ بار برایتان دم می‌کنیم و دانه‌ی قهوه را برای خانه می‌فروشیم.",
        "blocks": [
            ("Hero", {
                "layout": "split",
                "eyebrow": "کافه‌بار و فروشگاه قهوه",
                "title": "یک فنجانِ خوب،\nهمین‌جا یا در خانه",
                "subtitle": "در رستینو قهوه را پشتِ بار و جلوی چشمتان دم می‌کنیم و دانه‌ی قهوه را هم برای خانه می‌فروشیم. بیایید و بنشینید، یا قهوه‌تان را با خودتان ببرید.",
                "buttonText": "سری به ما بزنید", "buttonLink": "/fa/contact",
                "imageUrl": f"{U}logo.png",
            }, style()),
            ("CardGrid", {"sectionTitle": "دو کار، با یک علاقه", "columns": 2, "layout": "stacked", "cards": [
                {"icon": "coffee", "title": "کافه‌بار",
                 "description": "اسپرسو و قهوه‌ی دمی، پشتِ بار و جلوی چشم شما. چند صندلی برای نشستن و نوشیدنِ یک فنجان با حوصله.",
                 "linkText": "منوی قهوه", "linkUrl": "/fa/menu"},
                {"icon": "shopping-bag", "title": "فروشِ قهوه",
                 "description": "دانه‌ی قهوه برای خانه و محلِ کار. اگر نمی‌دانید کدام را بردارید، در انتخاب کمکتان می‌کنیم.",
                 "linkText": "دانه‌ها", "linkUrl": "/fa/menu"},
            ]}, style(SURFACE, borderColor=BORDER, borderWidth="1px 0")),
            ("Heading", {"text": "داستانِ یک کافه‌ی کوچک", "level": "h2", "alignment": "center"}, style(paddingTop="2rem")),
            ("RichText", {"content": (
                "<p>رستینو با یک فکرِ ساده شروع شد: قهوه‌ی خوب باید در دسترس باشد، چه در فنجانی که پشتِ بار برایتان دم می‌کنیم، چه در کیسه‌ای که با خودتان به خانه می‌برید.</p>"
                "<p>جا کم است، اما برای یک فنجانِ خوب و یک گفت‌وگوی کوتاه، همیشه جا هست.</p>")},
             style(textAlign="center", fontSize="1.125rem")),
            ("ImageGallery", {"layout": "grid", "columns": 3, "images": [
                {"imageUrl": f"{U}gallery-bar.jpg", "altText": "بارِ قهوه‌ی رستینو"},
                {"imageUrl": f"{U}gallery-beans.jpg", "altText": "قفسه‌ی دانه‌های قهوه"},
                {"imageUrl": f"{U}gallery-space.jpg", "altText": "فضای کافه"},
            ]}, style(paddingBottom="2rem")),
            ("CardGrid", {"sectionTitle": "سری به ما بزنید", "columns": 3, "layout": "stacked", "cards": [
                {"icon": "map-pin", "title": "نشانی", "description": ADDRESS, "linkText": "مسیریابی", "linkUrl": "/fa/contact"},
                {"icon": "clock", "title": "ساعتِ کار", "description": HOURS},
                {"icon": "phone", "title": "تماس", "description": PHONE, "linkText": "صفحه‌ی تماس با ما", "linkUrl": "/fa/contact"},
            ]}, style(SAND)),
            ("Newsletter", {"style": "brand", "title": "قهوه‌ی تازه که رسید، خبرتان می‌کنیم",
                            "description": "ماهی یکی‌دو ایمیل؛ فقط دانه‌های تازه و خبرهای کافه.",
                            "buttonText": "عضویت", "placeholder": "ایمیلِ شما"}, style(paddingTop="2rem", paddingBottom="2rem")),
        ],
    },
]


def drinks(title, sub, items):
    return ("ProductShowcase", {
        "sectionTitle": title, "sectionSubtitle": sub, "layout": "list", "columns": 2, "animation": "fade-up",
        "products": [{"title": t, "description": d, "badge": b, "price": ""} for t, d, b in items]}, style())


pages.append({
    "var": "@MenuId", "title": "منوی قهوه", "slug": "menu",
    "metaTitle": "منوی قهوه | رستینو",
    "metaDesc": "منوی نوشیدنی‌های بارِ رستینو و دانه‌های قهوه برای خانه.",
    "blocks": [
        ("Heading", {"text": "منوی قهوه", "subtitle": "نوشیدنی‌هایی که پشتِ بار آماده می‌کنیم و دانه‌هایی که برای خانه می‌فروشیم.",
                     "level": "h1", "alignment": "center"}, style(SAND, paddingTop="2rem", paddingBottom="2rem")),
        drinks("بر پایه‌ی اسپرسو", "تک یا دوبل، با شیر یا بدون شیر", [
            ("اسپرسو", "یک یا دو شات، غلیظ و پرکرما.", ""),
            ("آمریکانو", "اسپرسو با آبِ داغ؛ سبک‌تر و بلندتر.", ""),
            ("کاپوچینو", "اسپرسو، شیرِ داغ و فومِ پرحجم.", ""),
            ("لاته", "اسپرسو با شیرِ داغِ بیشتر و لایه‌ی نازکِ فوم.", ""),
            ("فلت‌وایت", "دو شات با شیرِ مخملی؛ قهوه پررنگ‌تر از لاته.", "پیشنهادِ باریستا"),
            ("کورتادو", "اسپرسو و کمی شیر؛ کوچک و پرقدرت.", ""),
            ("ماکیاتو", "اسپرسو با یک قاشق فومِ شیر.", ""),
            ("موکا", "اسپرسو، شکلاتِ داغ و شیر.", ""),
        ]),
        drinks("قهوه‌ی دمی", "دم‌آوریِ دستی، با دانه‌ای که خودتان انتخاب می‌کنید", [
            ("V60", "روشن و شفاف؛ مناسبِ دانه‌های میوه‌ای.", ""),
            ("کمکس", "برای دو نفر، با طعمی تمیز و ملایم.", ""),
            ("فرنچ‌پرس", "پرتن و غلیظ، با بافتی سنگین‌تر.", ""),
            ("ایروپرس", "سریع و متعادل، بینِ اسپرسو و دمی.", ""),
            ("قهوه‌ی ترک", "آسیابِ خیلی نرم، روی حرارتِ ملایم.", ""),
        ]),
        drinks("نوشیدنی‌های سرد", "برای روزهای گرم، و هر وقت که دلتان بخواهد", [
            ("آیس‌آمریکانو", "دو شات اسپرسو روی آبِ سرد و یخ.", ""),
            ("آیس‌لاته", "اسپرسو روی شیرِ سرد و یخ.", ""),
            ("کلدبرو", "قهوه‌ای که ساعت‌ها در آبِ سرد دم کشیده؛ نرم و کم‌تلخی.", ""),
            ("اسپرسو تونیک", "اسپرسو روی تونیک و یخ.", ""),
            ("آفوگاتو", "یک اسکوپ بستنیِ وانیلی زیرِ شاتِ داغِ اسپرسو.", ""),
        ]),
        ("ProductShowcase", {
            "sectionTitle": "دانه‌ی قهوه برای خانه", "sectionSubtitle": "قهوه‌ای را که پشتِ بار دوست داشتید، به خانه ببرید",
            "layout": "cards", "columns": 3, "animation": "fade-up", "products": [
                {"title": "ترکیبِ اسپرسو", "description": "برای دستگاهِ اسپرسو و موکاپات.", "imageUrl": f"{U}beans-espresso.jpg", "price": ""},
                {"title": "عربیکای تک‌خاستگاه", "description": "برای V60 و کمکس.", "imageUrl": f"{U}beans-single-origin.jpg", "price": ""},
                {"title": "قهوه‌ی ترک", "description": "آسیابِ خیلی نرم، آماده‌ی دم.", "imageUrl": f"{U}beans-turkish.jpg", "price": ""},
            ]}, style(SAND, marginTop="3rem")),
        ("RichText", {"content": "<p>قیمت‌ها به تومان است. برای موجودیِ دانه‌ها و نوشیدنیِ روز، از باریستا بپرسید.</p>"},
         style(textAlign="center")),
    ],
})

pages.append({
    "var": "@ContactId", "title": "تماس با ما", "slug": "contact",
    "metaTitle": "تماس با ما | رستینو",
    "metaDesc": "نشانی، ساعتِ کار و راه‌های تماس با رستینو.",
    "blocks": [
        ("Heading", {"text": "تماس با ما",
                     "subtitle": "سؤالی درباره‌ی قهوه دارید یا می‌خواهید دانه‌ای را برایتان کنار بگذاریم؟ پیام بدهید، یا بهتر از آن، سری به کافه بزنید.",
                     "level": "h1", "alignment": "center"}, style(SAND, paddingTop="2rem", paddingBottom="2rem")),
        ("Columns", {"columnCount": 2, "gap": "2rem", "columns": [
            {"width": "38%", "blocks": [nested("CardGrid", {"columns": 1, "layout": "inline", "cards": [
                {"icon": "map-pin", "title": "نشانی", "description": ADDRESS},
                {"icon": "clock", "title": "ساعتِ کار", "description": HOURS},
                {"icon": "phone", "title": "تلفن", "description": PHONE},
                {"icon": "brand-instagram", "title": "اینستاگرام", "description": INSTA},
            ]})]},
            {"width": "60%", "blocks": [nested("ContactForm", {
                "title": "پیام بفرستید", "description": "پیامتان را بنویسید؛ به‌زودی پاسخ می‌دهیم.",
                "recipientEmail": EMAIL, "submitButtonText": "ارسالِ پیام",
                "fields": [
                    {"label": "نام", "name": "name", "type": "text", "required": True, "placeholder": "نامِ شما"},
                    {"label": "ایمیل", "name": "email", "type": "email", "required": True, "placeholder": "email@example.com"},
                    {"label": "موضوع", "name": "subject", "type": "text", "required": False, "placeholder": "مثلاً خریدِ دانه‌ی قهوه"},
                    {"label": "پیام", "name": "message", "type": "textarea", "required": True, "placeholder": "پیامتان را بنویسید"},
                ]})]},
        ]}, style()),
        ("MapEmbed", {"embedUrl": MAP, "height": 420}, style(paddingBottom="3rem")),
    ],
})

MEDIA = [("logo.png", "image/png", "لوگوی رستینو"), ("favicon.png", "image/png", "آیکونِ رستینو"),
         ("og-image.png", "image/png", "رستینو"), ("gallery-bar.jpg", "image/jpeg", "بارِ قهوه‌ی رستینو"),
         ("gallery-beans.jpg", "image/jpeg", "قفسه‌ی دانه‌های قهوه"), ("gallery-space.jpg", "image/jpeg", "فضای کافه"),
         ("beans-espresso.jpg", "image/jpeg", "ترکیبِ اسپرسو"), ("beans-single-origin.jpg", "image/jpeg", "عربیکای تک‌خاستگاه"),
         ("beans-turkish.jpg", "image/jpeg", "قهوه‌ی ترک")]


def sq(s):
    return "N'" + s.replace("'", "''") + "'"


def tokenized(s):
    """T-SQL expression for a JSON literal, with {{TOKEN}} swapped for the matching JSON-escaped variable.
    {{N_TOKEN}} marks a token inside a nested block's JSON string, which needs escaping twice."""
    expr = sq(s)
    for t in TOKENS:
        var = "@" + "".join(w.capitalize() for w in t.split("_")) + "Json"
        if "{{%s}}" % t in s:
            expr = f"REPLACE({expr}, N'{{{{{t}}}}}', {var})"
        if "{{N_%s}}" % t in s:
            expr = f"REPLACE({expr}, N'{{{{N_{t}}}}}', {var}2)"
    return expr


def block_rows(page):
    rows = []
    for bt, data, st in page["blocks"]:
        if bt == "Columns":
            for col in data["columns"]:
                for nb in col["blocks"]:
                    for t in TOKENS:
                        nb["data"] = nb["data"].replace("{{%s}}" % t, "{{N_%s}}" % t)
        rows.append((bt, j(data), j(st)))
    return rows


def build_sql():
    sizes = {f: os.path.getsize(os.path.join(HERE, "uploads", "roastino-" + f)) for f, _, _ in MEDIA
             if os.path.exists(os.path.join(HERE, "uploads", "roastino-" + f))}
    L = [f"""/* =====================================================================
   Roastino (رستینو) for CoderSight  (SQL Server 2016+)
   Pages: /fa/ (home), /fa/menu, /fa/contact, plus nav, theme and settings.
   Generated by samples/roastino/gen_seed.py; edit that file, not this one.

   Needs the block variants and theme columns from the AddBrandTheme and
   AddThemeSurfaceAndHeadingFont migrations.

   Before running:
     1. Start the app once against the database so migrations and the
        default seed have run.
     2. Copy the files in samples/roastino/uploads into
        src/CoderSight.Web/wwwroot/uploads/.
     3. Fill in the variables below.
   Safe to re-run: it replaces the three Roastino pages and the nav menu.
   ===================================================================== */
SET NOCOUNT ON;
SET XACT_ABORT ON;

-- ---------- Fill these in ----------
DECLARE @Address        nvarchar(400)  = N'[نشانیِ کامل کافه]';
DECLARE @Hours          nvarchar(400)  = N'[روزهای کاری]: [ساعت باز و بسته شدن]';
DECLARE @Phone          nvarchar(100)  = N'[شماره‌ی تماس]';
DECLARE @Instagram      nvarchar(100)  = N'[آیدیِ اینستاگرام]';
DECLARE @InstagramUrl   nvarchar(400)  = NULL;   -- e.g. N'https://instagram.com/roastino'
DECLARE @RecipientEmail nvarchar(256)  = N'[ایمیلِ دریافتِ پیام‌ها]';
DECLARE @MapEmbedUrl    nvarchar(1000) = N'';    -- Google Maps > Share > Embed a map > the src="..." URL
DECLARE @SiteUrl        nvarchar(400)  = NULL;   -- e.g. N'https://roastino.ir'
DECLARE @ArchiveOtherPages bit = 1;              -- 1 = hide the CoderSight demo pages (Status = Archived; nothing is deleted)

-- JSON-escaped copies (spliced into block JSON), and double-escaped ones for blocks nested in Columns
DECLARE @AddressJson nvarchar(800) = STRING_ESCAPE(@Address, 'json');
DECLARE @HoursJson nvarchar(800) = STRING_ESCAPE(@Hours, 'json');
DECLARE @PhoneJson nvarchar(200) = STRING_ESCAPE(@Phone, 'json');
DECLARE @InstagramJson nvarchar(200) = STRING_ESCAPE(@Instagram, 'json');
DECLARE @RecipientEmailJson nvarchar(512) = STRING_ESCAPE(@RecipientEmail, 'json');
DECLARE @MapEmbedUrlJson nvarchar(2000) = STRING_ESCAPE(@MapEmbedUrl, 'json');
DECLARE @AddressJson2 nvarchar(1600) = STRING_ESCAPE(@AddressJson, 'json');
DECLARE @HoursJson2 nvarchar(1600) = STRING_ESCAPE(@HoursJson, 'json');
DECLARE @PhoneJson2 nvarchar(400) = STRING_ESCAPE(@PhoneJson, 'json');
DECLARE @InstagramJson2 nvarchar(400) = STRING_ESCAPE(@InstagramJson, 'json');
DECLARE @RecipientEmailJson2 nvarchar(1024) = STRING_ESCAPE(@RecipientEmailJson, 'json');
DECLARE @MapEmbedUrlJson2 nvarchar(4000) = STRING_ESCAPE(@MapEmbedUrlJson, 'json');

BEGIN TRANSACTION;

-- ---------- Site settings and brand theme ----------
UPDATE SiteSettings SET
    SiteName = N'رستینو',
    SiteUrl = COALESCE(@SiteUrl, SiteUrl),
    DefaultCulture = 'fa',
    SupportedCultures = 'fa',
    EnableMultilingual = 1,          -- keep on: the /fa/ URL segment is what makes pages right-to-left
    EnableUserRegistration = 0,      -- hides Sign In / Sign Up
    EnableBlogComments = 0,
    EnableUserBlogSubmissions = 0,
    LogoUrl = N'{U}logo.png',
    FaviconUrl = N'{U}favicon.png',
    OgImageUrl = N'{U}og-image.png',
    DefaultMetaDescription = N'رستینو، کافه‌بار و فروشگاه قهوه.',
    FooterText = N'© ۱۴۰۵ رستینو · کافه‌بار و فروشگاه قهوه',
    SocialInstagram = @InstagramUrl,
    SiteBackgroundColor = '{CREAM}',
    NavBackgroundColor = '{CREAM}', NavTextColor = '{INK}', NavBorderColor = '{BORDER}',
    NavLogoTextColor = '{INK}', NavHoverColor = '{RED}',
    NavHeight = '4.5rem', NavLogoHeight = '3rem', NavLogoFontSize = '1.6rem',
    FooterBackgroundColor = '{INK}', FooterTextColor = '{BORDER}', FooterHeadingColor = '#FFFFFF',
    FooterLinkHoverColor = '#FFFFFF', FooterBorderColor = '#47322A',
    ThemePrimaryColor = '{RED}', ThemePrimaryHoverColor = '{RED_DARK}', ThemeOnPrimaryColor = '#FFFFFF',
    ThemeInkColor = '{INK}', ThemeDarkColor = '{ROAST}', ThemeMutedColor = '{MUTED}',
    ThemeSurfaceColor = '{SURFACE}', ThemeBorderColor = '{BORDER}', ThemeRadius = '1.25rem',
    ThemeFontFamily = 'Vazirmatn', ThemeHeadingFontFamily = 'Noto Naskh Arabic';

-- ---------- Navigation ----------
DELETE FROM NavMenuItems WHERE ParentId IS NOT NULL;
DELETE FROM NavMenuItems;
INSERT INTO NavMenuItems (Id, ParentId, Label, LabelFa, Url, OpenInNewTab, SortOrder, IsVisible) VALUES
    (NEWID(), NULL, N'Home',    N'خانه',       N'',        0, 0, 1),
    (NEWID(), NULL, N'Menu',    N'منوی قهوه',  N'menu',    0, 1, 1),
    (NEWID(), NULL, N'Contact', N'تماس با ما', N'contact', 0, 2, 1);

-- ---------- Media library ----------
DELETE FROM Media WHERE Url LIKE N'{U}%';
INSERT INTO Media (Id, FileName, Url, ContentType, SizeBytes, AltText, UploadedAt) VALUES"""]
    L.append(",\n".join(f"    (NEWID(), N'roastino-{f}', N'{U}{f}', '{ct}', {sizes.get(f, 0)}, {sq(alt)}, SYSUTCDATETIME())"
                        for f, ct, alt in MEDIA) + ";")
    L.append("""
-- ---------- Pages ----------
DELETE FROM Pages WHERE Culture = 'fa' AND Slug IN (N'', N'menu', N'contact');   -- their blocks cascade

IF @ArchiveOtherPages = 1
    UPDATE Pages SET Status = 'Archived', UpdatedAt = SYSUTCDATETIME()
    WHERE NOT (Culture = 'fa' AND Slug IN (N'', N'menu', N'contact'));
""")
    L.append("DECLARE " + ", ".join(f"{p['var']} uniqueidentifier = NEWID()" for p in pages) + ";\n")
    for p in pages:
        L.append(f"-- {p['title']}  (/fa/{p['slug']})")
        L.append("INSERT INTO Pages (Id, Title, Slug, Status, MetaTitle, MetaDescription, Culture, LocalizationGroupId, CreatedAt, UpdatedAt, PublishedAt)")
        L.append(f"VALUES ({p['var']}, {sq(p['title'])}, N'{p['slug']}', 'Published', {sq(p['metaTitle'])}, {sq(p['metaDesc'])}, "
                 "'fa', NULL, SYSUTCDATETIME(), SYSUTCDATETIME(), SYSUTCDATETIME());")
        L.append("INSERT INTO PageBlocks (Id, PageId, BlockType, SortOrder, Data, Styles) VALUES")
        L.append(",\n".join(f"    (NEWID(), {p['var']}, N'{bt}', {i},\n     {tokenized(data)},\n     N'{st}')"
                            for i, (bt, data, st) in enumerate(block_rows(p))) + ";\n")
    L.append("""COMMIT TRANSACTION;

SELECT p.Slug, p.Title, p.Status, COUNT(b.Id) AS Blocks
FROM Pages p LEFT JOIN PageBlocks b ON b.PageId = p.Id
WHERE p.Culture = 'fa' AND p.Slug IN (N'', N'menu', N'contact')
GROUP BY p.Slug, p.Title, p.Status;
""")
    return "\n".join(L)


if __name__ == "__main__":
    args = sys.argv[1:]
    blocks_json = None
    if "--blocks-json" in args:
        i = args.index("--blocks-json")
        blocks_json = args[i + 1]
        del args[i:i + 2]
    out = args[0] if args else os.path.join(HERE, "roastino-seed.sql")
    sql = build_sql()
    with open(out, "w", encoding="utf-8") as f:
        f.write(sql)
    print("wrote", out)
    if blocks_json:
        with open(blocks_json, "w", encoding="utf-8") as f:
            json.dump({p["slug"] or "home": [{"blockType": bt, "data": d, "styles": s} for bt, d, s in block_rows(p)]
                       for p in pages}, f, ensure_ascii=False, indent=1)
        print("wrote", blocks_json)
