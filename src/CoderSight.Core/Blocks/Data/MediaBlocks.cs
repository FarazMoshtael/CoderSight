namespace CoderSight.Core.Blocks.Data;

[CmsBlock("Image", Icon = "photo", Description = "Single image with optional caption", Category = "Media")]
public class ImageBlockData : IBlockData
{
    public string ImageUrl { get; set; } = string.Empty;
    public string? AltText { get; set; }
    public string? Caption { get; set; }
    public string Size { get; set; } = "full";
    public string GetDisplayName() => AltText ?? "Image";
}

[CmsBlock("ImageGallery", Icon = "grid-dots", Description = "Grid or carousel of images", Category = "Media")]
public class ImageGalleryBlockData : IBlockData
{
    public List<GalleryImage> Images { get; set; } = [];
    public int Columns { get; set; } = 3;
    public string Layout { get; set; } = "grid";
    public string GetDisplayName() => $"Gallery ({Images.Count} images)";
}

public class GalleryImage
{
    public string ImageUrl { get; set; } = string.Empty;
    public string? AltText { get; set; }
    public string? Caption { get; set; }
}

[CmsBlock("VideoEmbed", Icon = "player-play", Description = "YouTube, Vimeo, or uploaded video", Category = "Media")]
public class VideoEmbedBlockData : IBlockData
{
    public string VideoUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? Caption { get; set; }
    public bool AutoPlay { get; set; }
    public string GetDisplayName() => Caption ?? "Video";
}

[CmsBlock("Slider", Icon = "carousel-horizontal", Description = "Auto-rotating image slides", Category = "Media")]
public class SliderBlockData : IBlockData
{
    public List<SliderSlide> Slides { get; set; } = [];
    public int IntervalMs { get; set; } = 5000;
    public bool ShowArrows { get; set; } = true;
    public bool ShowDots { get; set; } = true;
    public string GetDisplayName() => $"Slider ({Slides.Count} slides)";
}

public class SliderSlide
{
    public string ImageUrl { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public string? ButtonText { get; set; }
    public string? ButtonLink { get; set; }
}
