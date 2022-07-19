namespace Disqord.Extensions.Interactivity.Menus;

/// <summary>
///     Marks a method as a <see cref="ButtonViewComponent"/>.
/// </summary>
public class ButtonAttribute : ButtonBaseAttribute
{
    public LocalButtonComponentStyle Style { get; init; } = LocalButtonComponentStyle.Primary;
}