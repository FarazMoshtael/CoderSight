using System.Globalization;
using System.Text.RegularExpressions;

namespace CoderSight.Core.Theming;

/// <summary>
/// Default brand palette and helpers that turn colours into the CSS custom properties
/// (<c>--cs-*</c>) rendered from <see cref="Entities.SiteSettings"/>.
/// </summary>
public static partial class ThemeColors
{
    public const string Primary = "#2563EB";
    public const string PrimaryHover = "#1D4ED8";
    public const string OnPrimary = "#FFFFFF";
    public const string Ink = "#020817";
    public const string Dark = "#1A2233";
    public const string Muted = "#64748B";
    public const string FontFamily = "Sora";
    public const string Surface = "#FFFFFF";
    public const string Border = "#E5E7EB";
    public const string Radius = "0.75rem";

    /// <summary>
    /// Maps a default brand hex value stored in block data or block styles to its theme variable,
    /// so content saved before theming existed follows the current brand. Other values pass through.
    /// </summary>
    public static string Resolve(string? color) => color?.Trim().ToUpperInvariant() switch
    {
        Primary => "rgb(var(--cs-primary))",
        PrimaryHover => "rgb(var(--cs-primary-hover))",
        Ink => "rgb(var(--cs-ink))",
        Dark => "rgb(var(--cs-dark))",
        Muted => "rgb(var(--cs-muted))",
        _ => color ?? string.Empty
    };

    /// <summary>
    /// Converts <c>#rgb</c> or <c>#rrggbb</c> to space-separated channels (<c>37 99 235</c>) for
    /// Tailwind's <c>rgb(var(--x) / alpha)</c> form. Invalid input falls back to <paramref name="fallback"/>.
    /// </summary>
    public static string ToRgbChannels(string? hex, string fallback)
    {
        if (!TryParseHex(hex, out var r, out var g, out var b) && !TryParseHex(fallback, out r, out g, out b))
            return "0 0 0";
        return $"{r} {g} {b}";
    }

    /// <summary>Keeps letters, digits and spaces so a font name is safe in CSS and a Google Fonts URL.</summary>
    public static string SanitizeFontFamily(string? name)
    {
        var cleaned = FontNameRegex().Replace(name ?? string.Empty, "").Trim();
        return cleaned.Length == 0 ? FontFamily : cleaned;
    }

    private static bool TryParseHex(string? hex, out int r, out int g, out int b)
    {
        r = g = b = 0;
        var value = hex?.Trim().TrimStart('#') ?? string.Empty;
        if (value.Length == 3)
            value = string.Concat(value[0], value[0], value[1], value[1], value[2], value[2]);
        if (value.Length != 6 || !int.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var rgb))
            return false;
        r = (rgb >> 16) & 0xFF;
        g = (rgb >> 8) & 0xFF;
        b = rgb & 0xFF;
        return true;
    }

    /// <summary>
    /// Like <see cref="SanitizeFontFamily"/>, but an empty name stays empty (meaning "use the body font").
    /// </summary>
    public static string SanitizeOptionalFontFamily(string? name) =>
        FontNameRegex().Replace(name ?? string.Empty, "").Trim();

    /// <summary>Accepts a single CSS length such as <c>0.75rem</c>, <c>12px</c> or <c>0</c>; anything else gets the default.</summary>
    public static string SanitizeRadius(string? value)
    {
        var v = value?.Trim() ?? string.Empty;
        return RadiusRegex().IsMatch(v) ? v : Radius;
    }

    [GeneratedRegex("[^A-Za-z0-9 ]")]
    private static partial Regex FontNameRegex();

    [GeneratedRegex(@"^(0|\d{1,3}(\.\d{1,3})?(px|rem|em))$")]
    private static partial Regex RadiusRegex();
}
