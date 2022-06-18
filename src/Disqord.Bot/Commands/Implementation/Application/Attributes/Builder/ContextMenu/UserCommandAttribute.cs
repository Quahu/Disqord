namespace Disqord.Bot.Commands.Application;

public class UserCommandAttribute : ApplicationCommandAttribute
{
    public UserCommandAttribute(string alias)
        : base(alias)
    { }

    /// <inheritdoc/>
    protected override ApplicationCommandType GetCommandType()
    {
        return ApplicationCommandType.User;
    }
}
