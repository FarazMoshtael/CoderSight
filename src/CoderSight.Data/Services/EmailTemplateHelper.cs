using System.Net;

namespace CoderSight.Data.Services;

public static class EmailTemplateHelper
{
    public static string Wrap(string siteName, string? logoUrl, string? siteUrl, string bodyHtml, string? footerHtml = null)
    {
        var logoBlock = BuildLogoBlock(siteName, logoUrl, siteUrl);
        var footer = footerHtml ?? $"<p>Sent by {Encode(siteName)}</p>";

        return $"""
        <!DOCTYPE html>
        <html>
        <body style="margin:0;padding:0;background:#f3f4f6;font-family:Arial,sans-serif;">
          <div style="max-width:600px;margin:0 auto;padding:20px;">
            <div style="background:#ffffff;border-radius:12px;padding:32px;margin-top:20px;">
              {logoBlock}
              {bodyHtml}
            </div>
            <div style="text-align:center;padding:20px;font-size:12px;color:#9ca3af;">
              {footer}
            </div>
          </div>
        </body>
        </html>
        """;
    }

    private static string BuildLogoBlock(string siteName, string? logoUrl, string? siteUrl)
    {
        var safeName = Encode(siteName);

        string inner;
        if (!string.IsNullOrEmpty(logoUrl))
        {
            var fullLogoUrl = MakeAbsolute(logoUrl, siteUrl);
            inner = $"""<img src="{fullLogoUrl}" alt="{safeName}" style="max-height:48px;max-width:200px;width:auto;" /><div style="margin-top:8px;font-size:16px;font-weight:bold;color:#020817;">{safeName}</div>""";
        }
        else
        {
            inner = $"""<span style="font-size:20px;font-weight:bold;color:#020817;">{safeName}</span>""";
        }

        return $"""<div style="text-align:center;margin-bottom:24px;">{inner}</div>""";
    }

    public static string MakeAbsolute(string url, string? siteUrl)
    {
        if (url.StartsWith("http://") || url.StartsWith("https://"))
            return url;
        var baseUrl = siteUrl?.TrimEnd('/') ?? "";
        return baseUrl + (url.StartsWith('/') ? url : "/" + url);
    }

    public static string Encode(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";
}
