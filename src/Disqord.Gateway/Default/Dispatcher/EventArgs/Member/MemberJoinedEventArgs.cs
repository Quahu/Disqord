using System;

namespace Disqord.Gateway;

public class MemberJoinedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild which the member joined.
    /// </summary>
    public Snowflake GuildId => Member.GuildId;

    /// <summary>
    ///     Gets the ID of the member that joined.
    /// </summary>
    public Snowflake MemberId => Member.Id;

    /// <summary>
    ///     Gets the guild that the member joined.
    /// </summary>
    /// <returns>
    ///     <see langword="null"/> if the guild was not cached.
    /// </returns>
    public CachedGuild? Guild { get; }

    /// <summary>
    ///     Gets the member that joined.
    /// </summary>
    public IMember Member { get; }

    public MemberJoinedEventArgs(
        CachedGuild? guild,
        IMember member)
    {
        Guild = guild;
        Member = member;
    }
}
