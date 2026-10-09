namespace CoderSight.Core.Entities;

public class SiteSettings
{
    public Guid Id { get; set; }
    public string SiteName { get; set; } = "CoderSight";
    public string? SiteUrl { get; set; }
    public string DefaultCulture { get; set; } = "en";
    public string SupportedCultures { get; set; } = "en,fa";
    public string? LogoUrl { get; set; }
    public string? FaviconUrl { get; set; }
    public string? OgImageUrl { get; set; }
    public string? DefaultMetaDescription { get; set; }
    public string? FooterText { get; set; }
    public string? GoogleAnalyticsId { get; set; }
    public bool EnableMultilingual { get; set; } = true;
    public bool EnableUserRegistration { get; set; } = true;
    public bool EnableBlogComments { get; set; } = true;
    public bool EnableUserBlogSubmissions { get; set; } = false;

    // Brand theme (rendered as --cs-* CSS variables used by blocks)
    public string ThemePrimaryColor { get; set; } = "#2563EB";
    public string ThemePrimaryHoverColor { get; set; } = "#1D4ED8";
    public string ThemeOnPrimaryColor { get; set; } = "#FFFFFF";
    public string ThemeInkColor { get; set; } = "#020817";
    public string ThemeDarkColor { get; set; } = "#1A2233";
    public string ThemeMutedColor { get; set; } = "#64748B";
    public string ThemeFontFamily { get; set; } = "Sora";
    public string ThemeHeadingFontFamily { get; set; } = string.Empty;
    public string ThemeSurfaceColor { get; set; } = "#FFFFFF";
    public string ThemeBorderColor { get; set; } = "#E5E7EB";
    public string ThemeRadius { get; set; } = "0.75rem";

    // Site background
    public string SiteBackgroundColor { get; set; } = "#F9FAFB";

    // Navbar styles
    /// <summary>Serialized <see cref="BlockStyleOptions"/> for the navbar container.
    /// Empty until first edited, in which case <see cref="ResolveNavStyle"/> derives it from the colors below.</summary>
    public string NavStyles { get; set; } = "";
    /// <summary>Navbar item direction: auto (follows the page language), ltr, rtl or center. See <see cref="Theming.LayoutDirection"/>.</summary>
    public string NavDirection { get; set; } = "auto";
    public string NavBackgroundColor { get; set; } = "#FFFFFF";
    public string NavTextColor { get; set; } = "#374151";
    public string NavBorderColor { get; set; } = "#E5E7EB";
    public string NavLogoTextColor { get; set; } = "#020817";
    public string NavHoverColor { get; set; } = "#2563EB";
    public string NavHeight { get; set; } = "4rem";
    public string NavLogoHeight { get; set; } = "2rem";
    public string NavLogoFontSize { get; set; } = "1.25rem";

    // Footer styles
    /// <summary>Serialized <see cref="BlockStyleOptions"/> for the footer container.
    /// Empty until first edited, in which case <see cref="ResolveFooterStyle"/> derives it from the colors below.</summary>
    public string FooterStyles { get; set; } = "";
    /// <summary>Footer direction: auto (follows the page language), ltr, rtl or center.</summary>
    public string FooterDirection { get; set; } = "auto";
    public string FooterBackgroundColor { get; set; } = "#111827";
    public string FooterTextColor { get; set; } = "#9CA3AF";
    public string FooterHeadingColor { get; set; } = "#FFFFFF";
    public string FooterLinkHoverColor { get; set; } = "#FFFFFF";
    public string FooterBorderColor { get; set; } = "#1F2937";

    // Social Links
    public string? SocialGitHub { get; set; }
    public string? SocialTwitter { get; set; }
    public string? SocialLinkedIn { get; set; }
    public string? SocialInstagram { get; set; }
    public string? SocialYouTube { get; set; }
    public string? SocialTelegram { get; set; }

    // Cloudflare Turnstile
    public string? TurnstileSiteKey { get; set; }
    public string? TurnstileSecretKey { get; set; }

    // SMTP / Email
    public string? SmtpHost { get; set; }
    public int SmtpPort { get; set; } = 587;
    public string? SmtpUsername { get; set; }
    public string? SmtpPassword { get; set; }
    public string? SmtpFromEmail { get; set; }
    public string? SmtpFromName { get; set; }
    public bool SmtpUseSsl { get; set; } = true;

    /// <summary>The navbar container style — stored options when present, otherwise built from the legacy color fields.</summary>
    public BlockStyleOptions ResolveNavStyle() =>
        BlockStyleOptions.TryParse(NavStyles) ?? new BlockStyleOptions
        {
            BackgroundColor = NavBackgroundColor,
            TextColor = NavTextColor,
            BorderColor = NavBorderColor,
            BorderWidth = "0 0 1px 0",
            PaddingTop = "0",
            PaddingBottom = "0",
            PaddingLeft = "0",
            PaddingRight = "0"
        };

    /// <summary>The footer container style — stored options when present, otherwise built from the legacy color fields.</summary>
    public BlockStyleOptions ResolveFooterStyle() =>
        BlockStyleOptions.TryParse(FooterStyles) ?? new BlockStyleOptions
        {
            BackgroundColor = FooterBackgroundColor,
            TextColor = FooterTextColor,
            PaddingTop = "0",
            PaddingBottom = "0",
            PaddingLeft = "0",
            PaddingRight = "0"
        };
}
