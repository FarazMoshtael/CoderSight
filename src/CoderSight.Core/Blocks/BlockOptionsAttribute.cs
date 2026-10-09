namespace CoderSight.Core.Blocks;

/// <summary>
/// Limits a string block property to a fixed set of values (a layout or style variant).
/// The block editor shows a picker instead of a text box. The first value is the default.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public class BlockOptionsAttribute : Attribute
{
    public string[] Values { get; }

    public BlockOptionsAttribute(params string[] values)
    {
        Values = values;
    }
}
