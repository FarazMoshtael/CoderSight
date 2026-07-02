namespace CoderSight.Core.Entities;

public class NavMenuItem
{
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public string Label { get; set; } = string.Empty;
    public string? LabelFa { get; set; }
    public string? Url { get; set; }
    public bool OpenInNewTab { get; set; }
    public int SortOrder { get; set; }
    public bool IsVisible { get; set; } = true;

    public NavMenuItem? Parent { get; set; }
    public List<NavMenuItem> Children { get; set; } = [];
}
