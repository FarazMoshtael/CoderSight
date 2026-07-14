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

    public string ToInlineStyle()
    {
        var parts = new List<string>
        {
            $"background-color:{BackgroundColor}",
            $"color:{TextColor}",
            $"padding:{PaddingTop} {PaddingRight} {PaddingBottom} {PaddingLeft}",
            $"margin-top:{MarginTop}",
            $"margin-bottom:{MarginBottom}",
            $"border-radius:{BorderRadius}",
            $"border:{BorderWidth} solid {BorderColor}",
            $"max-width:{MaxWidth}"
        };
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
