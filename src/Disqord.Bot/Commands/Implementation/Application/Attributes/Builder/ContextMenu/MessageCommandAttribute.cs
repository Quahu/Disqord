namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Marks the decorated method as a message context menu command.
/// </summary>
/// <remarks>
///     The method must have a single <see cref="IMessage"/> parameter.
/// </remarks>
public class MessageCommandAttribute : ApplicationCommandAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="MessageCommandAttribute"/> with the specified alias.
    /// </summary>
    /// <param name="alias"> The alias of the command. </param>
    public MessageCommandAttribute(string alias)
        : base(alias)
    { }

    /// <inheritdoc/>
    protected override ApplicationCommandType GetCommandType()
    {
        return ApplicationCommandType.Message;
    }
}
