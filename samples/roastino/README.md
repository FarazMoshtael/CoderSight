# Roastino (رستینو) sample brand

A Persian, right-to-left setup for a small coffee bar and bean shop, built only from CoderSight blocks.
It creates three pages (home, coffee menu, contact) in the Warm Cream look: cream background,
Noto Naskh Arabic headings, Vazirmatn body text and a red accent.

## What's here

| File | Purpose |
|---|---|
| `roastino-seed.sql` | One script that sets the site settings, theme, navigation, media rows and the three pages. |
| `gen_seed.py` | Generates `roastino-seed.sql`. Edit page content here and re-run `python3 gen_seed.py`. |
| `uploads/` | Logo, favicon, OG image, three gallery images and three bean images, all named `roastino-*`. |

## Requirements

The seed uses theme settings and block options that ship with this branch, so the database must be on
the latest migrations (`AddBrandTheme` and `AddThemeSurfaceAndHeadingFont`). Run the app once, or
`dotnet ef database update`, before running the script.

## Setup

1. Copy the files in `uploads/` into the web app's `wwwroot/uploads/` folder. The app serves uploads only from `/uploads/<file>`, so they must sit directly in that folder, not in a subfolder.
2. Open `roastino-seed.sql` and fill in the variables at the top:
   - `@Address`, `@Hours`, `@Phone`, `@Instagram`, `@InstagramUrl`
   - `@RecipientEmail` (where contact form messages go)
   - `@MapEmbedUrl` (Google Maps > Share > Embed a map > the `src` URL; leave empty to hide the map)
   - `@SiteUrl`
   - `@ArchiveOtherPages` (1 archives the demo pages; nothing is deleted)
3. Run the script against the CoderSight database (SQL Server). It runs in one transaction and can be re-run.
4. Restart the app or wait for the settings cache to expire, then open `/fa/`.

The site is set to Persian only. The `/fa/` URL segment is what switches pages to RTL, so multilingual
mode stays on with `fa` as the default and only culture; the language toggle hides itself in that case.

## Placeholders to replace

- The three gallery images and three bean images are drawn placeholders. Replace the files with real photos (same names) or change them in the admin.
- Menu prices are empty. Add them per item in the admin (Menu page > product blocks), or in `gen_seed.py`.
- Address, hours, phone, Instagram and email come from the variables above.

## Known gaps

- Rich text blocks have no typography styling, so the story paragraph and the price note are plain text.
- The menu page has no jump links to its sections.
- Newsletter and contact form status messages ("Sending...", "Subscribing...") are still in English.
