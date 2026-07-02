using System.Reflection;
using System.Text.Json;

namespace CoderSight.Core.Blocks;

public class BlockRegistry
{
    private readonly Dictionary<string, BlockRegistration> _blocks = new(StringComparer.OrdinalIgnoreCase);
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public void DiscoverBlocks(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var dataTypes = assembly.GetTypes()
                .Where(t => t is { IsAbstract: false, IsInterface: false }
                    && typeof(IBlockData).IsAssignableFrom(t)
                    && t.GetCustomAttribute<CmsBlockAttribute>() is not null);

            foreach (var dataType in dataTypes)
            {
                var attr = dataType.GetCustomAttribute<CmsBlockAttribute>()!;
                _blocks[attr.Name] = new BlockRegistration
                {
                    Name = attr.Name,
                    Icon = attr.Icon,
                    Description = attr.Description,
                    Category = attr.Category,
                    DataType = dataType
                };
            }
        }
    }

    public void RegisterComponentTypes(Assembly webAssembly)
    {
        var componentTypes = webAssembly.GetTypes()
            .Where(t => t.Namespace?.Contains("Blocks") == true);

        foreach (var (name, registration) in _blocks)
        {
            var expectedComponentName = $"{name}Block";
            var expectedEditorName = $"{name}BlockEditor";

            registration.ComponentType = componentTypes
                .FirstOrDefault(t => t.Name.Equals(expectedComponentName, StringComparison.OrdinalIgnoreCase))
                ?? typeof(object);

            registration.EditorComponentType = componentTypes
                .FirstOrDefault(t => t.Name.Equals(expectedEditorName, StringComparison.OrdinalIgnoreCase))
                ?? typeof(object);
        }
    }

    public BlockRegistration? Get(string blockType) =>
        _blocks.TryGetValue(blockType, out var reg) ? reg : null;

    public IReadOnlyDictionary<string, BlockRegistration> GetAll() => _blocks;

    public IEnumerable<BlockRegistration> GetByCategory(string category) =>
        _blocks.Values.Where(b => b.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<string> GetCategories() =>
        _blocks.Values.Select(b => b.Category).Distinct(StringComparer.OrdinalIgnoreCase);

    public IBlockData? DeserializeBlockData(string blockType, string json)
    {
        var registration = Get(blockType);
        if (registration is null) return null;
        return JsonSerializer.Deserialize(json, registration.DataType, JsonOptions) as IBlockData;
    }

    public Dictionary<string, object?> DeserializeAsParameters(string blockType, string json)
    {
        var data = DeserializeBlockData(blockType, json);
        return new Dictionary<string, object?> { ["Data"] = data };
    }
}
