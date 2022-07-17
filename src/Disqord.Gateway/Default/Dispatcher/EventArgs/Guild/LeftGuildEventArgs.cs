using System;

namespace Disqord.Gateway;

public class LeftGuildEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild the current user left.
    /// </summary>
    public Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the guild the current user left.
    ///     Returns <see langword="null"/> if the guild was not cached.
    /// </summary>
    public CachedGuild? Guild { get; }

    public LeftGuildEventArgs(
        Snowflake guildId,
        CachedGuild? guild)
    {
        GuildId = guildId;
        Guild = guild;
    }
}
