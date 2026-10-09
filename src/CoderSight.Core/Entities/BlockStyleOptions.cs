using System.Text.Json;
using CoderSight.Core.Theming;

namespace CoderSight.Core.Entities;

public class BlockStyleOptions
{
    public string BackgroundColor { get; set; } = "transparent";
    public string TextColor { get; set; } = "#020817";
    public string PaddingTop { get; set; } = "2rem";
    public string PaddingBottom { get; set; } = "2rem";
    public string PaddingLeft { get; set; } = "1rem";
    public string PaddingRight { get; set; } = "1rem";
    public string MarginTop { get; set; } = "0";
    public string MarginBottom { get; set; } = "0";
    public string BorderRadius { get; set; } = "0";
    public string BorderColor { get; set; } = "transparent";
    public string BorderWidth { get; set; } = "0";
    public string MaxWidth { get; set; } = "100%";
    public string TextAlign { get; set; } = "";
    public string FontSize { get; set; } = "";
    public string BackgroundImageUrl { get; set; } = "";
    public string BackgroundSize { get; set; } = "cover";
    public string BackgroundPosition { get; set; } = "center";
    public string CustomCssClass { get; set; } = "";
    public bool FullWidth { get; set; } = true;

    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>Deserializes stored style JSON, returning null when nothing is stored.</summary>
    public static BlockStyleOptions? TryParse(string? json)
    {
        if (string.IsNullOrWhiteSpace(json) || json == "{}")
            return null;
        try
        {
            return JsonSerializer.Deserialize<BlockStyleOptions>(json, JsonOptions);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>Deserializes stored style JSON, falling back to defaults.</summary>
    public static BlockStyleOptions FromJson(string? json) => TryParse(json) ?? new BlockStyleOptions();

    public string ToJson() => JsonSerializer.Serialize(this, JsonOptions);

    public string ToInlineStyle()
    {
        var parts = new List<string>
        {
            $"background-color:{ThemeColors.Resolve(BackgroundColor)}",
            $"padding:{PaddingTop} {PaddingRight} {PaddingBottom} {PaddingLeft}",
            $"margin-top:{MarginTop}",
            $"margin-bottom:{MarginBottom}",
            $"border-radius:{BorderRadius}",
            // Emitted per-property (rather than the `border` shorthand) so per-side
            // widths such as "0 0 1px 0" work — used by the navbar's bottom rule.
            $"border-width:{BorderWidth}",
            "border-style:solid",
            $"border-color:{ThemeColors.Resolve(BorderColor)}",
            $"max-width:{MaxWidth}"
        };
        // The default text colour is left out so blocks inherit the brand's ink colour from the page.
        if (!string.IsNullOrWhiteSpace(TextColor) && !TextColor.Equals(ThemeColors.Ink, StringComparison.OrdinalIgnoreCase))
            parts.Add($"color:{ThemeColors.Resolve(TextColor)}");
        if (MaxWidth != "100%")
        {
            parts.Add("margin-left:auto");
            parts.Add("margin-right:auto");
        }
        if (!string.IsNullOrEmpty(TextAlign))
            parts.Add($"text-align:{TextAlign}");
        if (!string.IsNullOrEmpty(FontSize))
            parts.Add($"font-size:{FontSize}");
        if (!string.IsNullOrEmpty(BackgroundImageUrl))
        {
            parts.Add($"background-image:url('{BackgroundImageUrl}')");
            parts.Add($"background-size:{BackgroundSize}");
            parts.Add($"background-position:{BackgroundPosition}");
            parts.Add("background-repeat:no-repeat");
        }
        return string.Join(";", parts);
    }
}
