namespace Disqord.Bot.Commands.Components;

/// <summary>
///     Marks the decorated method as a modal submission command.
/// </summary>
public class ModalCommandAttribute : ComponentCommandAttribute
{
    /// <inheritdoc/>
    /// <summary>
    ///     Instantiates a new <see cref="ModalCommandAttribute"/> with the specified pattern.
    /// </summary>
    public ModalCommandAttribute(string pattern)
        : base(pattern)
    { }

    /// <inheritdoc />
    protected override ComponentCommandType GetCommandType()
    {
        return ComponentCommandType.Modal;
    }
}
