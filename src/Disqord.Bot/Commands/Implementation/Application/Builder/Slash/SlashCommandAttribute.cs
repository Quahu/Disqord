namespace Disqord.Bot.Commands.Application;

public class SlashCommandAttribute : ApplicationCommandAttribute
{
    public SlashCommandAttribute(string alias)
        : base(alias)
    { }

    /// <inheritdoc/>
    protected override ApplicationCommandType GetCommandType()
    {
        return ApplicationCommandType.Slash;
    }
}
