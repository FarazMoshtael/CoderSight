namespace CoderSight.Core.Blocks;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class CmsBlockAttribute : Attribute
{
    public string Name { get; }
    public string Icon { get; set; } = "layout";
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = "Content";

    public CmsBlockAttribute(string name)
    {
        Name = name;
    }
}
