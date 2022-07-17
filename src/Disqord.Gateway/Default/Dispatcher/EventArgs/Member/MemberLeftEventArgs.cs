using System;

namespace Disqord.Gateway;

public class MemberLeftEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild which the user left.
    /// </summary>
    public Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the ID of the member that left.
    /// </summary>
    public Snowflake MemberId => User.Id;

    /// <summary>
    ///     Gets the guild that the user left.
    /// </summary>
    /// <returns>
    ///     <see langword="null"/> if the guild was not cached.
    /// </returns>
    public CachedGuild? Guild { get; }

    /// <summary>
    ///     Gets the user that left.
    /// </summary>
    public IUser User { get; }

    public MemberLeftEventArgs(
        Snowflake guildId,
        CachedGuild? guild,
        IUser user)
    {
        GuildId = guildId;
        Guild = guild;
        User = user;
    }
}
