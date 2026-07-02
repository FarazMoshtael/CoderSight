namespace CoderSight.Core.Blocks;

public class BlockRegistration
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = "layout";
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = "Content";
    public Type DataType { get; set; } = null!;
    public Type ComponentType { get; set; } = null!;
    public Type EditorComponentType { get; set; } = null!;
}
