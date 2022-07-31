namespace Disqord.Bot.Commands;

/// <inheritdoc/>
public interface IDiscordGuildCommandContext : IDiscordCommandContext
{
    /// <summary>
    ///     Gets the ID of the guild of this command execution.
    /// </summary>
    new Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the member who prompted this command execution.
    /// </summary>
    new IMember Author { get; }
}
