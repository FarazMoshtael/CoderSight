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

    // Site background
    public string SiteBackgroundColor { get; set; } = "#F9FAFB";

    // Navbar styles
    public string NavBackgroundColor { get; set; } = "#FFFFFF";
    public string NavTextColor { get; set; } = "#374151";
    public string NavBorderColor { get; set; } = "#E5E7EB";
    public string NavLogoTextColor { get; set; } = "#020817";
    public string NavHoverColor { get; set; } = "#2563EB";
    public string NavHeight { get; set; } = "4rem";
    public string NavLogoHeight { get; set; } = "2rem";
    public string NavLogoFontSize { get; set; } = "1.25rem";

    // Footer styles
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

    // SMTP / Email
    public string? SmtpHost { get; set; }
    public int SmtpPort { get; set; } = 587;
    public string? SmtpUsername { get; set; }
    public string? SmtpPassword { get; set; }
    public string? SmtpFromEmail { get; set; }
    public string? SmtpFromName { get; set; }
    public bool SmtpUseSsl { get; set; } = true;
}
