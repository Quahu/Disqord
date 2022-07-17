using System;

namespace Disqord.Gateway;

public class GuildUnavailableEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild that became unavailable.
    /// </summary>
    public Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the guild that became unavailable.
    ///     Returns <see langword="null"/> if the guild was not cached.
    /// </summary>
    public CachedGuild? Guild { get; }

    public GuildUnavailableEventArgs(
        Snowflake guildId,
        CachedGuild? guild)
    {
        GuildId = guildId;
        Guild = guild;
    }
}
