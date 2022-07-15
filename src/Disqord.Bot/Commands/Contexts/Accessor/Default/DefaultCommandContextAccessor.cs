namespace Disqord.Bot.Commands.Default;

/// <inheritdoc/>
public class DefaultCommandContextAccessor : ICommandContextAccessor
{
    /// <inheritdoc/>
    public IDiscordCommandContext Context { get; set; } = null!;
}
