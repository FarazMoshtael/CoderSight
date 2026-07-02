using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace CoderSight.Web;

public class UrlSegmentCultureProvider : IRequestCultureProvider
{
    private static readonly HashSet<string> SupportedCultures = new(StringComparer.OrdinalIgnoreCase) { "en", "fa" };

    public Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var path = httpContext.Request.Path.Value ?? "";
        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length > 0 && SupportedCultures.Contains(segments[0]))
        {
            var culture = segments[0].ToLowerInvariant();
            return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(culture, culture));
        }

        return Task.FromResult<ProviderCultureResult?>(null);
    }
}
