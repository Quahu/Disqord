namespace Disqord.Extensions.Interactivity.Menus;

public class SelectionAttribute : ComponentAttribute
{
    public SelectionComponentType Type { get; init; }

    public ChannelType[]? ChannelTypes { get; init; }

    public string? Placeholder { get; init; }

    public int MinimumSelectedOptions { get; init; } = -1;

    public int MaximumSelectedOptions { get; init; } = -1;
}
