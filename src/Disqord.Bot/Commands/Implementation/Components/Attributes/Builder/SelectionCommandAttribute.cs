namespace Disqord.Bot.Commands.Components;

/// <summary>
///     Marks the decorated method as a selection component command.
/// </summary>
public class SelectionCommandAttribute : ComponentCommandAttribute
{
    /// <inheritdoc/>
    /// <summary>
    ///     Instantiates a new <see cref="SelectionCommandAttribute"/> with the specified pattern.
    /// </summary>
    public SelectionCommandAttribute(string pattern)
        : base(pattern)
    { }

    /// <inheritdoc />
    protected override ComponentCommandType GetCommandType()
    {
        return ComponentCommandType.Selection;
    }
}
