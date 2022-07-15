namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Marks the decorated method as a slash command.
/// </summary>
public class SlashCommandAttribute : ApplicationCommandAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="SlashCommandAttribute"/> with the specified alias.
    /// </summary>
    /// <param name="alias"> The alias of the command. </param>
    public SlashCommandAttribute(string alias)
        : base(alias)
    { }

    /// <inheritdoc/>
    protected override ApplicationCommandType GetCommandType()
    {
        return ApplicationCommandType.Slash;
    }
}
