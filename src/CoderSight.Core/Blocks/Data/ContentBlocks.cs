namespace CoderSight.Core.Blocks.Data;

[CmsBlock("Hero", Icon = "layout", Description = "Full-width banner with title, subtitle, and CTA", Category = "Content")]
public class HeroBlockData : IBlockData
{
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string? ButtonText { get; set; }
    public string? ButtonLink { get; set; }
    public string? BackgroundImageUrl { get; set; }
    public string? BackgroundColor { get; set; }
    public string GetDisplayName() => Title;
}

[CmsBlock("RichText", Icon = "align-left", Description = "WYSIWYG rich text content", Category = "Content")]
public class RichTextBlockData : IBlockData
{
    public string Content { get; set; } = string.Empty;
    public string GetDisplayName() => "Rich text";
}

[CmsBlock("Heading", Icon = "heading", Description = "Section title with optional subtitle", Category = "Content")]
public class HeadingBlockData : IBlockData
{
    public string Text { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string Level { get; set; } = "h2";
    public string Alignment { get; set; } = "center";
    public string GetDisplayName() => Text;
}

[CmsBlock("Blockquote", Icon = "quote", Description = "Highlighted quote with attribution", Category = "Content")]
public class BlockquoteBlockData : IBlockData
{
    public string Quote { get; set; } = string.Empty;
    public string? Attribution { get; set; }
    public string GetDisplayName() => Quote.Length > 40 ? Quote[..40] + "..." : Quote;
}

[CmsBlock("CodeSnippet", Icon = "code", Description = "Syntax-highlighted code block", Category = "Content")]
public class CodeSnippetBlockData : IBlockData
{
    public string Code { get; set; } = string.Empty;
    public string Language { get; set; } = "csharp";
    public string? FileName { get; set; }
    public string GetDisplayName() => FileName ?? "Code snippet";
}

[CmsBlock("Spacer", Icon = "separator", Description = "Adjustable spacing or divider line", Category = "Content")]
public class SpacerBlockData : IBlockData
{
    public int Height { get; set; } = 40;
    public bool ShowDivider { get; set; }
    public string GetDisplayName() => ShowDivider ? "Divider" : $"Spacer ({Height}px)";
}
