namespace Disqord.Bot.Commands.Components;

/// <summary>
///     Marks the decorated method as a button component command.
/// </summary>
public class ButtonCommandAttribute : ComponentCommandAttribute
{
    /// <inheritdoc/>
    /// <summary>
    ///     Instantiates a new <see cref="ButtonCommandAttribute"/> with the specified pattern.
    /// </summary>
    public ButtonCommandAttribute(string pattern)
        : base(pattern)
    { }

    /// <inheritdoc />
    protected override ComponentCommandType GetCommandType()
    {
        return ComponentCommandType.Button;
    }
}
