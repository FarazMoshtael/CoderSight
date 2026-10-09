using CoderSight.Core.Services;
using Microsoft.AspNetCore.Localization;

namespace CoderSight.Web;

/// <summary>
/// Falls back to the site's default language from Site Settings for URLs without a culture segment
/// (login, register, profile, author pages), so a Persian-only site doesn't render them in English and left-to-right.
/// </summary>
public class SiteDefaultCultureProvider : IRequestCultureProvider
{
    // The admin area stays in English, and Blazor's own endpoints keep the culture they had before.
    private static readonly string[] ExcludedPrefixes = ["/admin", "/api", "/_blazor", "/_framework", "/_content"];

    public async Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var path = httpContext.Request.Path;
        if (ExcludedPrefixes.Any(prefix => path.StartsWithSegments(prefix, StringComparison.OrdinalIgnoreCase)))
            return null;

        var settingsService = httpContext.RequestServices.GetService<ISiteSettingsService>();
        if (settingsService is null) return null;

        var settings = await settingsService.GetAsync();
        var culture = settings?.DefaultCulture?.Trim();
        return string.IsNullOrEmpty(culture) ? null : new ProviderCultureResult(culture, culture);
    }
}
