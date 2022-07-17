using System;

namespace Disqord.Gateway;

public class MemberUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the member was updated.
    /// </summary>
    public Snowflake GuildId => NewMember.GuildId;

    /// <summary>
    ///     Gets the ID of the updated member.
    /// </summary>
    public Snowflake MemberId => NewMember.Id;

    /// <summary>
    ///     Gets the member in the state before the update occurred.
    ///     Returns <see langword="null"/> if the member was not cached.
    /// </summary>
    public CachedMember? OldMember { get; }

    /// <summary>
    ///     Gets the updated member.
    /// </summary>
    public IMember NewMember { get; }

    public MemberUpdatedEventArgs(
        CachedMember? oldMember,
        IMember newMember)
    {
        OldMember = oldMember;
        NewMember = newMember;
    }
}
