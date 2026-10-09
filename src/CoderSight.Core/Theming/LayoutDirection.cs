using System.Globalization;

namespace CoderSight.Core.Theming;

/// <summary>
/// Direction options for the navbar and footer, and the page-level right-to-left check.
/// </summary>
public static class LayoutDirection
{
    public const string Auto = "auto";
    public const string Ltr = "ltr";
    public const string Rtl = "rtl";
    public const string Center = "center";

    public static readonly (string Value, string Label)[] Options =
    [
        (Auto, "Auto (page language)"),
        (Ltr, "Left to right"),
        (Rtl, "Right to left"),
        (Center, "Center")
    ];

    // Hosts without ICU data (globalization-invariant mode) report every culture as left-to-right,
    // so the right-to-left languages are also matched by name.
    private static readonly HashSet<string> RtlLanguages = new(StringComparer.OrdinalIgnoreCase)
    {
        "fa", "ar", "he", "ur", "ps", "ckb", "dv", "yi"
    };

    public static bool IsRtl(CultureInfo culture) =>
        culture.TextInfo.IsRightToLeft || RtlLanguages.Contains(culture.TwoLetterISOLanguageName);

    public static bool IsRtl(string? cultureName) =>
        !string.IsNullOrEmpty(cultureName) && RtlLanguages.Contains(cultureName.Split('-')[0]);

    /// <summary>The <c>dir</c> attribute for a forced direction, or null to inherit the page's.</summary>
    public static string? DirAttribute(string? direction) => Normalize(direction) switch
    {
        Ltr => Ltr,
        Rtl => Rtl,
        _ => null
    };

    public static bool IsCenter(string? direction) => Normalize(direction) == Center;

    public static string Normalize(string? direction) =>
        direction?.Trim().ToLowerInvariant() switch
        {
            Ltr => Ltr,
            Rtl => Rtl,
            Center => Center,
            _ => Auto
        };
}
