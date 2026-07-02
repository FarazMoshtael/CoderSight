namespace CoderSight.Core.Blocks.Data;

[CmsBlock("Columns", Icon = "columns", Description = "2-4 column layout with nested blocks", Category = "Layout")]
public class ColumnsBlockData : IBlockData
{
    public int ColumnCount { get; set; } = 2;
    public List<ColumnContent> Columns { get; set; } = [];
    public string Gap { get; set; } = "1.5rem";
    public string GetDisplayName() => $"Columns ({ColumnCount})";
}

public class ColumnContent
{
    public List<NestedBlock> Blocks { get; set; } = [];
    public string? Width { get; set; }
}

public class NestedBlock
{
    public string BlockType { get; set; } = string.Empty;
    public string Data { get; set; } = "{}";
}

[CmsBlock("CardGrid", Icon = "cards", Description = "Repeating card layout", Category = "Layout")]
public class CardGridBlockData : IBlockData
{
    public string? SectionTitle { get; set; }
    public List<CardItem> Cards { get; set; } = [];
    public int Columns { get; set; } = 3;
    public string GetDisplayName() => SectionTitle ?? "Card grid";
}

public class CardItem
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? Icon { get; set; }
    public string? LinkUrl { get; set; }
    public string? LinkText { get; set; }
}

[CmsBlock("CardCarousel", Icon = "carousel-horizontal", Description = "Horizontally scrollable card carousel with auto-play", Category = "Layout")]
public class CardCarouselBlockData : IBlockData
{
    public string? SectionTitle { get; set; }
    public string? SectionSubtitle { get; set; }
    public List<CarouselCard> Cards { get; set; } = [];
    public bool AutoPlay { get; set; } = true;
    public int AutoPlayInterval { get; set; } = 4;
    public string GetDisplayName() => SectionTitle ?? $"Carousel ({Cards.Count} cards)";
}

public class CarouselCard
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? LinkUrl { get; set; }
    public string? LinkText { get; set; }
    public string? Badge { get; set; }
}

[CmsBlock("ProductShowcase", Icon = "shopping-cart", Description = "Animated product cards with price and rating", Category = "Layout")]
public class ProductShowcaseBlockData : IBlockData
{
    public string? SectionTitle { get; set; }
    public string? SectionSubtitle { get; set; }
    public List<ProductCard> Products { get; set; } = [];
    public int Columns { get; set; } = 3;
    public string Animation { get; set; } = "fade-up";
    public string GetDisplayName() => SectionTitle ?? $"Products ({Products.Count})";
}

public class ProductCard
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? Price { get; set; }
    public string? OriginalPrice { get; set; }
    public string? Badge { get; set; }
    public double Rating { get; set; }
    public string? LinkUrl { get; set; }
    public string? LinkText { get; set; }
}

[CmsBlock("HtmlEmbed", Icon = "code", Description = "Raw HTML, iframe, or widget embed", Category = "Layout")]
public class HtmlEmbedBlockData : IBlockData
{
    public string Html { get; set; } = string.Empty;
    public string GetDisplayName() => "HTML embed";
}
