namespace CoderSight.Core.Blocks.Data;

[CmsBlock("Faq", Icon = "help-circle", Description = "Collapsible Q&A pairs", Category = "Interactive")]
public class FaqBlockData : IBlockData
{
    public string? SectionTitle { get; set; }
    public List<FaqItem> Items { get; set; } = [];
    public string GetDisplayName() => SectionTitle ?? "FAQ";
}

public class FaqItem
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
}

[CmsBlock("Tabs", Icon = "bookmark", Description = "Tabbed content sections", Category = "Interactive")]
public class TabsBlockData : IBlockData
{
    public List<TabItem> Tabs { get; set; } = [];
    public string GetDisplayName() => $"Tabs ({Tabs.Count})";
}

public class TabItem
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Icon { get; set; }
}

[CmsBlock("Timeline", Icon = "timeline", Description = "Vertical event history", Category = "Interactive")]
public class TimelineBlockData : IBlockData
{
    public string? SectionTitle { get; set; }
    public List<TimelineEvent> Events { get; set; } = [];
    public string GetDisplayName() => SectionTitle ?? "Timeline";
}

public class TimelineEvent
{
    public string Date { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
}

[CmsBlock("ProgressSteps", Icon = "progress", Description = "Multi-step process indicator", Category = "Interactive")]
public class ProgressStepsBlockData : IBlockData
{
    public string? SectionTitle { get; set; }
    public List<ProgressStep> Steps { get; set; } = [];
    public string GetDisplayName() => SectionTitle ?? "Steps";
}

public class ProgressStep
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
}

[CmsBlock("MapEmbed", Icon = "map-pin", Description = "Google Maps iframe embed", Category = "Interactive")]
public class MapEmbedBlockData : IBlockData
{
    public string EmbedUrl { get; set; } = string.Empty;
    public int Height { get; set; } = 400;
    public string GetDisplayName() => "Map";
}

[CmsBlock("ContactForm", Icon = "mail", Description = "Configurable contact form", Category = "Interactive")]
public class ContactFormBlockData : IBlockData
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string RecipientEmail { get; set; } = string.Empty;
    public string SubmitButtonText { get; set; } = "Send message";
    public List<FormField> Fields { get; set; } = [];
    public string GetDisplayName() => Title ?? "Contact form";
}

public class FormField
{
    public string Label { get; set; } = string.Empty;
    public string Type { get; set; } = "text";
    public bool Required { get; set; }
    public string? Placeholder { get; set; }
}

[CmsBlock("Newsletter", Icon = "mail-forward", Description = "Email capture form", Category = "Interactive")]
public class NewsletterBlockData : IBlockData
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ButtonText { get; set; } = "Subscribe";
    public string? Placeholder { get; set; }
    public string GetDisplayName() => Title;
}
