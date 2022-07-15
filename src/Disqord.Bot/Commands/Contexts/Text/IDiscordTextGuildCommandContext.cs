namespace Disqord.Bot.Commands.Text;

public interface IDiscordTextGuildCommandContext : IDiscordTextCommandContext, IDiscordGuildCommandContext
{
    IGuildChannel Channel { get; }
}
