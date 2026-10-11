"""Generate nicnegah-seed.sql: the NicNegah (نیک نگاه) pages, nav and settings for a CoderSight database.

Usage: python3 gen_seed.py [out.sql] [--blocks-json blocks.json]
The optional blocks JSON (page slug -> list of {blockType, data, styles}) is for previewing the pages.
"""
import json
import os
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
U = "/uploads/nicnegah-"  # the app serves /uploads/<file> only, so files stay flat

# Night Ink palette
BG, SURFACE, BORDER, LINE = "#08191F", "#0E252D", "#1C3D47", "#16323B"
INK, MUTED, CYAN, CYAN_LIGHT, DEEP = "#EEF7F8", "#93AEB5", "#00FFEA", "#7CFFF3", "#04252C"


def j(o):
    return json.dumps(o, ensure_ascii=False, separators=(",", ":"))


def style(bg="transparent", **kw):
    """Block wrapper style. Blocks bring their own vertical padding, so the wrapper adds none."""
    s = {"backgroundColor": bg, "paddingTop": "0", "paddingBottom": "0", "paddingLeft": "0", "paddingRight": "0"}
    s.update(kw)
    return s


# Values filled in from the variables at the top of the script.
TOKENS = ["ADDRESS", "HOURS", "PHONE", "ORDER_EMAIL", "TELEGRAM", "INSTAGRAM", "RECIPIENT_EMAIL", "MAP_EMBED_URL"]
ADDRESS, HOURS, PHONE, ORDER_EMAIL, TELEGRAM, INSTA, EMAIL, MAP = ("{{%s}}" % t for t in TOKENS)
# What the preview shows for each token (the same placeholders the SQL variables start with).
TOKEN_DEFAULTS = {"ADDRESS": "[نشانیِ کامل چاپخانه]", "HOURS": "[شنبه تا پنجشنبه]: [۹ تا ۱۸]", "PHONE": "[شماره‌ی تماس]",
                  "ORDER_EMAIL": "[ایمیلِ ارسالِ فایل]", "TELEGRAM": "[آیدیِ تلگرام]", "INSTAGRAM": "[آیدیِ اینستاگرام]",
                  "RECIPIENT_EMAIL": "[ایمیلِ دریافتِ پیام‌ها]", "MAP_EMBED_URL": ""}


def nested(block_type, data):
    return {"blockType": block_type, "data": j(data)}


def cta(title="فایلتان آماده است؟",
        desc="فایل یا ایده‌تان را بفرستید؛ قیمت و زمانِ تحویل را همان روز اعلام می‌کنیم."):
    # No backgroundColor: the block then uses the primary colour (cyan) as a flat band.
    return ("CallToAction", {"title": title, "description": desc, "buttonText": "استعلام قیمت", "buttonLink": "/fa/contact"}, style())


def page_heading(title, subtitle):
    return ("Heading", {"text": title, "subtitle": subtitle, "level": "h1", "alignment": "center"},
            style(SURFACE, paddingTop="2rem", paddingBottom="2rem", borderColor=LINE, borderWidth="0 0 1px 0"))


SERVICES = [
    ("business-cards", "کارت ویزیت", "سلفون مات و براق، طلاکوب، لبه‌گرد و کاغذهای فانتزی."),
    ("brochure", "بروشور و کاتالوگ", "تک‌برگ، دولت و سه‌لت؛ کاتالوگ با صحافیِ منگنه یا چسب گرم."),
    ("banner", "بنر و استند", "بنرِ فلکسی، رول‌آپ و استندِ نمایشگاهی در هر اندازه."),
    ("labels", "لیبل و استیکر", "برش‌خورده در هر شکل، روی کاغذ، PVC و شفاف."),
    ("packaging", "جعبه و بسته‌بندی", "جعبه‌ی محصول، ساک دستی و بسته‌بندیِ تیراژِ کم."),
    ("invitations", "کارت دعوت و سربرگ", "کارت عروسی و مراسم، سربرگ، پاکت و فاکتورِ اختصاصی."),
]

pages = [{
    "var": "@HomeId", "title": "خانه", "slug": "",
    "metaTitle": "نیک نگاه | چاپ دیجیتال",
    "metaDesc": "نیک نگاه، چاپ دیجیتال: کارت ویزیت، بروشور، بنر، لیبل و بسته‌بندی با رنگِ دقیق و تحویلِ سریع.",
    "blocks": [
        ("Hero", {
            "layout": "centered",
            "eyebrow": "چاپ دیجیتال نیک نگاه",
            "title": "چاپی که به چشم می‌آید",
            "subtitle": "از کارت ویزیت و بروشور تا بنر و بسته‌بندی. فایلتان را بفرستید، نمونه‌ی رنگ را ببینید و کارِ چاپ‌شده را سرِ وقت تحویل بگیرید.",
            "buttonText": "استعلام قیمت", "buttonLink": "/fa/contact",
            "backgroundColor": BG, "backgroundImageUrl": f"{U}hero-bg.jpg",
        }, style()),
        ("Stats", {"backgroundColor": SURFACE, "stats": [
            {"value": "[۱۲]+", "label": "سال تجربه"},
            {"value": "[۴۸] ساعت", "label": "میانگینِ زمانِ تحویل"},
            {"value": "[۳۰۰۰]+", "label": "سفارشِ چاپ‌شده"},
            {"value": "۶", "label": "گروهِ خدمات"},
        ]}, style()),
        ("ProductShowcase", {
            "sectionTitle": "خدماتِ چاپ", "sectionSubtitle": "هر چیزی که باید روی کاغذ خوب دیده شود",
            "layout": "cards", "columns": 3, "animation": "fade-up",
            "products": [{"title": t, "description": d, "imageUrl": f"{U}service-{k}.jpg",
                          "linkText": "استعلام قیمت", "linkUrl": "/fa/contact"} for k, t, d in SERVICES]}, style()),
        ("ProgressSteps", {"sectionTitle": "سفارش در چهار قدم", "steps": [
            {"title": "فایل را بفرستید", "description": "PDF، AI یا تصویرِ باکیفیت؛ یا ایده‌تان را بگویید."},
            {"title": "قیمت و نمونه", "description": "قیمت را اعلام می‌کنیم و نمونه‌ی رنگ را تأیید می‌کنید."},
            {"title": "چاپ و تکمیل", "description": "چاپ، برش، سلفون و صحافی."},
            {"title": "تحویل", "description": "حضوری، با پیک یا پست."},
        ]}, style()),
        ("Heading", {"text": "نمونه‌کارها", "subtitle": "بخشی از کارهایی که اخیراً چاپ کرده‌ایم",
                     "level": "h2", "alignment": "center"}, style(SURFACE, paddingTop="1.5rem")),
        ("ImageGallery", {"layout": "grid", "columns": 3, "images": [
            {"imageUrl": f"{U}sample-sheets.jpg", "altText": "برگه‌های چاپ‌شده با علائمِ برش"},
            {"imageUrl": f"{U}sample-swatches.jpg", "altText": "بادبزنِ رنگ برای انتخابِ دقیقِ رنگ"},
            {"imageUrl": f"{U}sample-halftone.jpg", "altText": "ترامِ چاپ با رنگ‌های CMYK"},
        ]}, style(SURFACE, paddingBottom="2rem")),
        ("FeaturesList", {"sectionTitle": "چرا نیک نگاه؟", "columns": 4, "features": [
            {"icon": "bolt", "title": "تحویلِ سریع", "description": "بیشترِ سفارش‌ها در [۲۴ تا ۷۲] ساعتِ کاری آماده می‌شوند."},
            {"icon": "shield", "title": "رنگِ دقیق", "description": "پیش از چاپ، نمونه‌ی رنگ را می‌بینید و تأیید می‌کنید."},
            {"icon": "pencil", "title": "طراحی و آماده‌سازی", "description": "اگر فایلِ آماده ندارید، طراحی و آماده‌سازیِ چاپ با ما."},
            {"icon": "truck", "title": "ارسال به سراسر کشور", "description": "تحویلِ حضوری یا ارسال با پیک و پست."},
        ]}, style()),
        cta(),
    ],
}]


def service_list(title, sub, items, bg="transparent"):
    return ("ProductShowcase", {"sectionTitle": title, "sectionSubtitle": sub, "layout": "list", "columns": 2,
                                "products": [{"title": t, "description": d} for t, d in items]}, style(bg))


pages.append({
    "var": "@ServicesId", "title": "خدمات", "slug": "services",
    "metaTitle": "خدماتِ چاپ | نیک نگاه",
    "metaDesc": "خدماتِ چاپ دیجیتال نیک نگاه: کارت ویزیت، بروشور و کاتالوگ، بنر و استند، لیبل و بسته‌بندی، کارت دعوت و سربرگ.",
    "blocks": [
        page_heading("خدماتِ چاپ", "از یک بسته کارت ویزیت تا سفارشِ نمایشگاهی؛ برای هر کار، کاغذ و پوششِ مناسبش را پیشنهاد می‌دهیم."),
        service_list("کارت ویزیت", "۱۰۰۰ عدد در هر سفارش؛ تیراژِ کمتر را هم بپرسید", [
            ("سلفون مات یا براق", "۳۰۰ گرم، یک‌رو یا دورو."),
            ("طلاکوب و نقره‌کوب", "طلاکوبِ داغ روی سلفون مات."),
            ("یووی موضعی", "براقیِ برجسته روی بخشی از طرح."),
            ("لبه‌گرد و برشِ فرم", "گوشه‌های گرد یا برش در شکلِ دلخواه."),
            ("کاغذهای فانتزی", "کتان، کرافت و مقواهای رنگی."),
            ("کارت ویزیت شفاف", "روی PVC شفاف یا مات."),
        ]),
        service_list("تبلیغاتِ چاپی", "برای معرفیِ کسب‌وکار، نمایشگاه و رویداد", [
            ("بروشور", "A4 و A5، تک‌برگ، دولت یا سه‌لت."),
            ("کاتالوگ", "از ۸ صفحه، صحافیِ منگنه یا چسب گرم."),
            ("تراکت", "A5 و A6، گلاسه، برای پخشِ گسترده."),
            ("پوستر", "A3 تا ۷۰×۱۰۰، روی گلاسه یا مات."),
        ], SURFACE),
        service_list("بنر و نمایشگاه", "چاپِ عریض برای فضای باز و داخلی", [
            ("بنرِ فلکسی", "هر اندازه، با دوخت و پرچ."),
            ("رول‌آپ", "۸۰×۲۰۰ و ۱۰۰×۲۰۰، با پایه و کیف."),
            ("استندِ X", "سبک و تاشو برای پیشخوان و نمایشگاه."),
            ("شاسی و فوم‌برد", "تابلوی سبک برای دیوار و ویترین."),
        ]),
        service_list("لیبل و بسته‌بندی", "برای محصولی که در قفسه دیده شود", [
            ("لیبلِ کاغذی و PVC", "رول یا برگ، برش‌خورده در هر شکل."),
            ("استیکرِ شفاف", "برای شیشه، ظرف و بسته‌بندی."),
            ("جعبه‌ی محصول", "تیراژِ کم، طراحیِ قالب با ما."),
            ("ساک دستیِ کاغذی", "با چاپِ لوگو، کرافت یا سفید."),
        ], SURFACE),
        service_list("اداری و مراسم", "اوراقِ اداری و کارت‌های مناسبتی", [
            ("سربرگ و پاکت", "تحریرِ ۸۰ و ۱۰۰ گرم."),
            ("فاکتور و قبض", "تک‌برگ یا دسته‌ای، با شماره‌گذاری."),
            ("کارت دعوت", "عروسی، مراسم و رویدادهای کاری."),
            ("تقویم و سررسید", "رومیزی و دیواری، با طرحِ اختصاصی."),
        ]),
        ("CardGrid", {"sectionTitle": "فایلِ آماده برای چاپ", "columns": 4, "layout": "inline", "cards": [
            {"icon": "article", "title": "PDF یا AI", "description": "با فونت‌های تبدیل‌شده به منحنی."},
            {"icon": "sparkles", "title": "۳۰۰ dpi", "description": "تصاویر با وضوحِ اندازه‌ی واقعیِ چاپ."},
            {"icon": "category", "title": "رنگِ CMYK", "description": "نه RGB؛ تا رنگ‌ها جابه‌جا نشوند."},
            {"icon": "settings", "title": "۳ میلی‌متر حاشیه", "description": "از هر طرف، برای برشِ تمیز."},
        ]}, style(SURFACE, borderColor=LINE, borderWidth="1px 0")),
        cta("فایل ندارید؟", "طراحی و آماده‌سازیِ فایلِ چاپ را هم انجام می‌دهیم. ایده‌تان را بگویید."),
    ],
})

pages.append({
    "var": "@AboutId", "title": "درباره ما", "slug": "about",
    "metaTitle": "درباره ما | نیک نگاه",
    "metaDesc": "نیک نگاه، چاپخانه‌ی دیجیتال: دقت در رنگ، خوش‌قولی در تحویل و مشاوره در انتخابِ کاغذ و پوشش.",
    "blocks": [
        ("Hero", {
            "layout": "split",
            "title": "هر چاپ،\nبا یک نگاهِ دقیق",
            "subtitle": "نیک نگاه چاپخانه‌ای دیجیتال است برای کسب‌وکارها و آدم‌هایی که می‌خواهند کارشان روی کاغذ همان‌طور دیده شود که روی صفحه طراحی کرده‌اند.",
            "imageUrl": f"{U}logo.png",
        }, style()),
        ("RichText", {"content": (
            "<p>اسمِ ما از همین‌جا آمده: نگاهِ نیک. پیش از چاپ، فایل را نگاه می‌کنیم؛ رنگ، وضوح و حاشیه‌ی برش را. "
            "اگر چیزی درست نیست، پیش از آن‌که کاغذ و وقتِ شما هدر برود، خبرتان می‌کنیم. "
            "بعد نمونه‌ی رنگ را می‌بینید و تا تأیید نکنید، چاپِ نهایی شروع نمی‌شود.</p>")},
         style(textAlign="center", fontSize="1.125rem")),
        ("Stats", {"backgroundColor": SURFACE, "stats": [
            {"value": "[۱۲]+", "label": "سال تجربه"},
            {"value": "[۴۸] ساعت", "label": "میانگینِ زمانِ تحویل"},
            {"value": "[۳۰۰۰]+", "label": "سفارشِ چاپ‌شده"},
            {"value": "۶", "label": "گروهِ خدمات"},
        ]}, style()),
        ("FeaturesList", {"sectionTitle": "آنچه برایمان مهم است", "columns": 3, "features": [
            {"icon": "shield", "title": "دقت در رنگ", "description": "کالیبره‌کردنِ دستگاه‌ها و نمونه‌ی رنگ پیش از هر چاپ."},
            {"icon": "clock", "title": "خوش‌قولی", "description": "زمانِ تحویل را از اول می‌گوییم و به آن پایبندیم."},
            {"icon": "message-circle", "title": "مشاوره‌ی صادقانه", "description": "کاغذ و پوششی را پیشنهاد می‌دهیم که به کارِ شما بیاید، نه گران‌ترین را."},
        ]}, style()),
        ("Timeline", {"sectionTitle": "مسیرِ ما", "events": [
            {"date": "[۱۳۹۲]", "title": "شروع با یک دستگاه", "description": "چاپِ کارت ویزیت و تراکت برای کسب‌وکارهای محله."},
            {"date": "[۱۳۹۷]", "title": "چاپِ عریض", "description": "اضافه‌شدنِ بنر، رول‌آپ و چاپِ نمایشگاهی."},
            {"date": "[۱۴۰۲]", "title": "لیبل و بسته‌بندی", "description": "برشِ دیجیتال برای لیبل، استیکر و جعبه‌ی تیراژِ کم."},
            {"date": "[۱۴۰۵]", "title": "سفارشِ آنلاین", "description": "ارسالِ فایل و استعلامِ قیمت از راهِ دور."},
        ]}, style(SURFACE)),
        cta("کارِ بعدی‌تان را با ما چاپ کنید", "فایل یا ایده‌تان را بفرستید؛ قیمت و زمانِ تحویل را همان روز اعلام می‌کنیم."),
    ],
})

pages.append({
    "var": "@ContactId", "title": "تماس با ما", "slug": "contact",
    "metaTitle": "تماس با ما | نیک نگاه",
    "metaDesc": "نشانی، ساعتِ کار و راه‌های تماس با نیک نگاه؛ برای استعلامِ قیمت و ارسالِ فایل.",
    "blocks": [
        page_heading("تماس با ما", "برای استعلامِ قیمت، نوعِ کار، تیراژ و اندازه را بنویسید. فایل را می‌توانید با ایمیل یا تلگرام بفرستید."),
        ("Columns", {"columnCount": 2, "gap": "2rem", "columns": [
            {"width": "38%", "blocks": [nested("CardGrid", {"columns": 1, "layout": "inline", "cards": [
                {"icon": "map-pin", "title": "نشانی", "description": ADDRESS},
                {"icon": "clock", "title": "ساعتِ کار", "description": HOURS},
                {"icon": "phone", "title": "تلفن", "description": PHONE},
                {"icon": "mail", "title": "ارسالِ فایل با ایمیل", "description": ORDER_EMAIL},
                {"icon": "brand-telegram", "title": "تلگرام", "description": TELEGRAM},
                {"icon": "brand-instagram", "title": "اینستاگرام", "description": INSTA},
            ]})]},
            {"width": "60%", "blocks": [nested("ContactForm", {
                "title": "استعلامِ قیمت", "description": "مشخصاتِ کار را بنویسید؛ در ساعتِ کاری پاسخ می‌دهیم.",
                "recipientEmail": EMAIL, "submitButtonText": "ارسالِ درخواست",
                "fields": [
                    {"label": "نام", "name": "name", "type": "text", "required": True, "placeholder": "نام یا نامِ شرکت"},
                    {"label": "ایمیل", "name": "email", "type": "email", "required": True, "placeholder": "email@example.com"},
                    {"label": "نوعِ کار", "name": "subject", "type": "text", "required": False, "placeholder": "مثلاً کارت ویزیت طلاکوب"},
                    {"label": "توضیحات", "name": "message", "type": "textarea", "required": True,
                     "placeholder": "تیراژ، اندازه، کاغذ، زمانِ موردنیاز و شماره‌ی تماس"},
                ]})]},
        ]}, style()),
        ("MapEmbed", {"embedUrl": MAP, "height": 420}, style(paddingBottom="3rem")),
    ],
})

NAV = [("Home", "خانه", "", 1), ("Services", "خدمات", "services", 1), ("About", "درباره ما", "about", 1),
       ("Contact", "تماس با ما", "contact", 1)]

MEDIA = [("logo.png", "image/png", "لوگوی نیک نگاه"), ("favicon.png", "image/png", "آیکونِ نیک نگاه"),
         ("og-image.png", "image/png", "نیک نگاه، چاپ دیجیتال"), ("hero-bg.jpg", "image/jpeg", "پس‌زمینه‌ی ترامِ چاپ"),
         ("service-business-cards.jpg", "image/jpeg", "کارت ویزیت"), ("service-brochure.jpg", "image/jpeg", "بروشور و کاتالوگ"),
         ("service-banner.jpg", "image/jpeg", "بنر و استند"), ("service-labels.jpg", "image/jpeg", "لیبل و استیکر"),
         ("service-packaging.jpg", "image/jpeg", "جعبه و بسته‌بندی"), ("service-invitations.jpg", "image/jpeg", "کارت دعوت و سربرگ"),
         ("sample-sheets.jpg", "image/jpeg", "برگه‌های چاپ‌شده با علائمِ برش"),
         ("sample-swatches.jpg", "image/jpeg", "بادبزنِ رنگ"), ("sample-halftone.jpg", "image/jpeg", "ترامِ چاپ با رنگ‌های CMYK")]


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


def preview(s):
    """Block JSON with every token replaced by its placeholder, for rendering previews."""
    for t, v in TOKEN_DEFAULTS.items():
        s = s.replace("{{%s}}" % t, v).replace("{{N_%s}}" % t, v)
    return s


def build_sql():
    sizes = {f: os.path.getsize(os.path.join(HERE, "uploads", "nicnegah-" + f)) for f, _, _ in MEDIA
             if os.path.exists(os.path.join(HERE, "uploads", "nicnegah-" + f))}
    slugs = ", ".join(f"N'{p['slug']}'" for p in pages)
    L = [f"""/* =====================================================================
   NicNegah (نیک نگاه) for CoderSight  (SQL Server 2016+)
   Pages: /fa/ (home), /fa/services, /fa/about, /fa/contact, plus nav,
   theme and settings. Night Ink look.
   Generated by samples/nicnegah/gen_seed.py; edit that file, not this one.

   Before running:
     1. Start the app once against the database so migrations and the
        default seed have run.
     2. Copy the files in samples/nicnegah/uploads into
        src/CoderSight.Web/wwwroot/uploads/.
     3. Fill in the variables below.
   Safe to re-run: it replaces the four NicNegah pages and the nav menu.
   ===================================================================== */
SET NOCOUNT ON;
SET XACT_ABORT ON;

-- ---------- Fill these in ----------
DECLARE @Address        nvarchar(400)  = N'[نشانیِ کامل چاپخانه]';
DECLARE @Hours          nvarchar(400)  = N'[شنبه تا پنجشنبه]: [۹ تا ۱۸]';
DECLARE @Phone          nvarchar(100)  = N'[شماره‌ی تماس]';
DECLARE @OrderEmail     nvarchar(256)  = N'[ایمیلِ ارسالِ فایل]';   -- shown on the contact page for sending print files
DECLARE @Telegram       nvarchar(100)  = N'[آیدیِ تلگرام]';
DECLARE @Instagram      nvarchar(100)  = N'[آیدیِ اینستاگرام]';
DECLARE @RecipientEmail nvarchar(256)  = N'[ایمیلِ دریافتِ پیام‌ها]';  -- where contact form messages go
DECLARE @MapEmbedUrl    nvarchar(1000) = N'';    -- Google Maps > Share > Embed a map > the src="..." URL; empty hides the map
DECLARE @InstagramUrl   nvarchar(400) = NULL;   -- e.g. N'https://instagram.com/nicnegah'
DECLARE @TelegramUrl    nvarchar(400) = NULL;   -- e.g. N'https://t.me/nicnegah'
DECLARE @SiteUrl        nvarchar(400) = NULL;   -- e.g. N'https://nicnegah.ir'
DECLARE @ArchiveOtherPages bit = 1;             -- 1 = hide the CoderSight demo pages (Status = Archived; nothing is deleted)
"""] + ["-- JSON-escaped copies (spliced into block JSON), and double-escaped ones for blocks nested in Columns"] + [
        f"DECLARE @{''.join(w.capitalize() for w in t.split('_'))}Json nvarchar(2000) = STRING_ESCAPE(@{''.join(w.capitalize() for w in t.split('_'))}, 'json');"
        for t in TOKENS] + [
        f"DECLARE @{''.join(w.capitalize() for w in t.split('_'))}Json2 nvarchar(4000) = STRING_ESCAPE(@{''.join(w.capitalize() for w in t.split('_'))}Json, 'json');"
        for t in TOKENS] + [f"""

BEGIN TRANSACTION;

-- ---------- Site settings and brand theme ----------
UPDATE SiteSettings SET
    SiteName = N'نیک نگاه',
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
    DefaultMetaDescription = {sq(pages[0]['metaDesc'])},
    FooterText = N'© ۱۴۰۵ نیک نگاه · چاپ دیجیتال',
    SocialInstagram = @InstagramUrl,
    SocialTelegram = @TelegramUrl,
    SiteBackgroundColor = '{BG}',
    -- Empty NavStyles/FooterStyles make the navbar and footer use the colours below.
    NavStyles = N'', FooterStyles = N'',
    NavBackgroundColor = '{BG}', NavTextColor = '#D7E9EC', NavBorderColor = '{LINE}',
    NavLogoTextColor = '#FFFFFF', NavHoverColor = '{CYAN}',
    NavHeight = '4.5rem', NavLogoHeight = '2.75rem', NavLogoFontSize = '1.4rem',
    FooterBackgroundColor = '#050F13', FooterTextColor = '#8FAAB1', FooterHeadingColor = '#FFFFFF',
    FooterLinkHoverColor = '{CYAN}', FooterBorderColor = '{LINE}',
    ThemePrimaryColor = '{CYAN}', ThemePrimaryHoverColor = '{CYAN_LIGHT}', ThemeOnPrimaryColor = '{BG}',
    ThemeInkColor = '{INK}', ThemeDarkColor = '{DEEP}', ThemeMutedColor = '{MUTED}',
    ThemeSurfaceColor = '{SURFACE}', ThemeBorderColor = '{BORDER}', ThemeRadius = '0.75rem',
    ThemeFontFamily = 'Vazirmatn', ThemeHeadingFontFamily = '';

-- ---------- Navigation ----------
DELETE FROM NavMenuItems WHERE ParentId IS NOT NULL;
DELETE FROM NavMenuItems;
INSERT INTO NavMenuItems (Id, ParentId, Label, LabelFa, Url, OpenInNewTab, SortOrder, IsVisible) VALUES"""]
    L.append(",\n".join(f"    (NEWID(), NULL, N'{en}', {sq(fa)}, N'{url}', 0, {i}, {vis})"
                        for i, (en, fa, url, vis) in enumerate(NAV)) + ";")
    L.append(f"""
-- ---------- Media library ----------
DELETE FROM Media WHERE Url LIKE N'{U}%';
INSERT INTO Media (Id, FileName, Url, ContentType, SizeBytes, AltText, UploadedAt) VALUES""")
    L.append(",\n".join(f"    (NEWID(), N'nicnegah-{f}', N'{U}{f}', '{ct}', {sizes.get(f, 0)}, {sq(alt)}, SYSUTCDATETIME())"
                        for f, ct, alt in MEDIA) + ";")
    L.append(f"""
-- ---------- Pages ----------
DELETE FROM Pages WHERE Culture = 'fa' AND Slug IN ({slugs});   -- their blocks cascade

IF @ArchiveOtherPages = 1
    UPDATE Pages SET Status = 'Archived', UpdatedAt = SYSUTCDATETIME()
    WHERE NOT (Culture = 'fa' AND Slug IN ({slugs}));
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
    L.append(f"""COMMIT TRANSACTION;

SELECT p.Slug, p.Title, p.Status, COUNT(b.Id) AS Blocks
FROM Pages p LEFT JOIN PageBlocks b ON b.PageId = p.Id
WHERE p.Culture = 'fa' AND p.Slug IN ({slugs})
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
    out = args[0] if args else os.path.join(HERE, "nicnegah-seed.sql")
    with open(out, "w", encoding="utf-8") as f:
        f.write(build_sql())
    print("wrote", out)
    if blocks_json:
        with open(blocks_json, "w", encoding="utf-8") as f:
            json.dump({p["slug"] or "home": [{"blockType": bt, "data": preview(d), "styles": s} for bt, d, s in block_rows(p)]
                       for p in pages}, f, ensure_ascii=False, indent=1)
        print("wrote", blocks_json)
