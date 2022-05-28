namespace Disqord.Bot.Commands.Application;

public class MessageCommandAttribute : ApplicationCommandAttribute
{
    public MessageCommandAttribute(string alias)
        : base(alias)
    { }

    protected override ApplicationCommandType GetCommandType()
    {
        return ApplicationCommandType.Message;
    }
}
