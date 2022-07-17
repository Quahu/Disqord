using System;

namespace Disqord.Gateway;

public class GuildUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the updated guild.
    /// </summary>
    public Snowflake GuildId => NewGuild.Id;

    /// <summary>
    ///     Gets the guild in the state before the update occurred.
    ///     Returns <see langword="null"/> if the guild was not cached.
    /// </summary>
    public CachedGuild? OldGuild { get; }

    /// <summary>
    ///     Gets the updated guild.
    /// </summary>
    public IGuild NewGuild { get; }

    public GuildUpdatedEventArgs(
        CachedGuild? oldGuild,
        IGuild newGuild)
    {
        OldGuild = oldGuild;
        NewGuild = newGuild;
    }
}
