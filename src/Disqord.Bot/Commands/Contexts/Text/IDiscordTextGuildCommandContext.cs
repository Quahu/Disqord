namespace Disqord.Bot.Commands.Text;

public interface IDiscordTextGuildCommandContext : IDiscordTextCommandContext, IDiscordGuildCommandContext
{
    /// <summary>
    ///     Gets the channel in which the context message was received.
    /// </summary>
    /// <returns>
    ///     The channel or <see langword="null"/> if the channel was not cached.
    /// </returns>
    IMessageGuildChannel? Channel { get; }
}
