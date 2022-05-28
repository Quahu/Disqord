namespace Disqord.Bot.Commands.Application;

public class UserCommandAttribute : ApplicationCommandAttribute
{
    public UserCommandAttribute(string alias)
        : base(alias)
    { }

    protected override ApplicationCommandType GetCommandType()
    {
        return ApplicationCommandType.User;
    }
}
