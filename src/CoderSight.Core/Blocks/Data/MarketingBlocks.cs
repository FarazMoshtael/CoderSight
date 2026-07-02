namespace CoderSight.Core.Blocks.Data;

[CmsBlock("CallToAction", Icon = "click", Description = "Highlighted CTA section with button", Category = "Marketing")]
public class CallToActionBlockData : IBlockData
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ButtonText { get; set; } = string.Empty;
    public string ButtonLink { get; set; } = string.Empty;
    public string? BackgroundColor { get; set; }
    public string GetDisplayName() => Title;
}

[CmsBlock("FeaturesList", Icon = "list-check", Description = "Icon + title + description grid", Category = "Marketing")]
public class FeaturesListBlockData : IBlockData
{
    public string? SectionTitle { get; set; }
    public List<FeatureItem> Features { get; set; } = [];
    public int Columns { get; set; } = 3;
    public string GetDisplayName() => SectionTitle ?? "Features";
}

public class FeatureItem
{
    public string Icon { get; set; } = "star";
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

[CmsBlock("PricingTable", Icon = "credit-card", Description = "Tiered plan comparison", Category = "Marketing")]
public class PricingTableBlockData : IBlockData
{
    public string? SectionTitle { get; set; }
    public List<PricingPlan> Plans { get; set; } = [];
    public string GetDisplayName() => SectionTitle ?? "Pricing";
}

public class PricingPlan
{
    public string Name { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
    public string Period { get; set; } = "/month";
    public string? Description { get; set; }
    public List<string> Features { get; set; } = [];
    public string? ButtonText { get; set; }
    public string? ButtonLink { get; set; }
    public bool IsHighlighted { get; set; }
}

[CmsBlock("Stats", Icon = "chart-bar", Description = "Animated number highlights", Category = "Marketing")]
public class StatsBlockData : IBlockData
{
    public List<StatItem> Stats { get; set; } = [];
    public string? BackgroundColor { get; set; }
    public string GetDisplayName() => $"Stats ({Stats.Count} items)";
}

public class StatItem
{
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string? Icon { get; set; }
}

[CmsBlock("LogoCloud", Icon = "brand-instagram", Description = "Partner/client logo row", Category = "Marketing")]
public class LogoCloudBlockData : IBlockData
{
    public string? Title { get; set; }
    public List<LogoItem> Logos { get; set; } = [];
    public string GetDisplayName() => Title ?? "Logo cloud";
}

public class LogoItem
{
    public string ImageUrl { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public string? Link { get; set; }
}

[CmsBlock("Banner", Icon = "ad-2", Description = "Dismissable announcement bar", Category = "Marketing")]
public class BannerBlockData : IBlockData
{
    public string Message { get; set; } = string.Empty;
    public string? LinkText { get; set; }
    public string? LinkUrl { get; set; }
    public string Style { get; set; } = "info";
    public bool Dismissable { get; set; } = true;
    public string GetDisplayName() => Message.Length > 40 ? Message[..40] + "..." : Message;
}
