using System.Globalization;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <inheritdoc/>
public interface IDiscordCommandContext : ICommandContext
{
    /// <summary>
    ///     Gets the bot client instance.
    /// </summary>
    DiscordBotBase Bot { get; }

    /// <summary>
    ///     Gets the locale of the guild.
    /// </summary>
    /// <returns>
    ///     The locale of the guild or <see langword="null"/>.
    /// </returns>
    CultureInfo? GuildLocale { get; }

    /// <summary>
    ///     Gets the ID of the guild of this command execution.
    /// </summary>
    /// <remarks>
    ///     Returns the ID of the guild or <see langword="null"/> in private channels.
    /// </remarks>
    Snowflake? GuildId { get; }

    /// <summary>
    ///     Gets the ID of the channel of this command execution.
    /// </summary>
    Snowflake ChannelId { get; }

    /// <summary>
    ///     Gets the user who prompted this command execution.
    /// </summary>
    IUser Author { get; }

    /// <summary>
    ///     Gets the ID of the user who prompted this command execution.
    /// </summary>
    Snowflake AuthorId => Author.Id;
}
