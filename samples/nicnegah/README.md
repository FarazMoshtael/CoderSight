# NicNegah (نیک نگاه) sample brand

A Persian, right-to-left setup for a digital printing shop, built only from CoderSight blocks.
It creates four pages (home, services, about, contact) in the Night Ink look: a dark page, the logo's
bright cyan as the primary colour, Vazirmatn text and a halftone hero.

## What's here

| File | Purpose |
|---|---|
| `nicnegah-seed.sql` | One script that sets the site settings, theme, navigation, media rows and the four pages. |
| `gen_seed.py` | Generates `nicnegah-seed.sql`. Edit page content here and re-run `python3 gen_seed.py`. |
| `gen_images.py` | Draws the placeholder images in `uploads/`. You only need it to redraw them. |
| `uploads/` | 23 files, all named `nicnegah-*`: logo, favicon, share image, hero background, service and product images, and three samples. |

## Requirements

The seed uses theme settings and block options from the `AddBrandTheme` and
`AddThemeSurfaceAndHeadingFont` migrations. Run the app once, or `dotnet ef database update`,
before running the script.

## Setup

1. Copy the files in `uploads/` into the web app's `wwwroot/uploads/` folder. The app serves uploads only from `/uploads/<file>`, so they must sit directly in that folder, not in a subfolder.
2. Open `nicnegah-seed.sql` and fill in the variables at the top:
   - `@Address`, `@Hours`, `@Phone`, `@OrderEmail` (shown on the contact page for sending print files), `@Telegram`, `@Instagram`
   - `@RecipientEmail` (where contact form messages go)
   - `@MapEmbedUrl` (Google Maps > Share > Embed a map > the `src` URL; leave empty to hide the map)
   - `@InstagramUrl`, `@TelegramUrl` (footer links), `@SiteUrl`
   - `@ArchiveOtherPages` (1 archives the demo pages; nothing is deleted)
3. Run the script against the CoderSight database (SQL Server). It runs in one transaction and can be re-run.
4. Restart the app or wait for the settings cache to expire, then open `/fa/`.

The site is set to Persian only. The `/fa/` URL segment is what switches pages to RTL, so multilingual
mode stays on with `fa` as the default and only culture; the language toggle hides itself in that case.

## Images

Everything in `uploads/` except `nicnegah-logo.png` is a drawn placeholder. Replace a file with a real
photo under the same name and the pages pick it up; no SQL change is needed. The hero background is
`nicnegah-hero-bg.jpg`. Service and product images are 800×600, samples 1200×800.

## Placeholders to replace

Shown in [brackets] on the pages:

- Home and About: the stats (years, average delivery time, orders) and the delivery window in "why us".
- About: the years and milestones in the history timeline.
- Contact: everything from the variables above.

## Known gaps

- The contact form sends only name, email, subject and message, so the phone number goes in the message, and print files are sent by email or Telegram.
- Contact form and newsletter status messages ("Sending...") are still in English.
- Footer social links are labelled in English (Instagram, Telegram).
