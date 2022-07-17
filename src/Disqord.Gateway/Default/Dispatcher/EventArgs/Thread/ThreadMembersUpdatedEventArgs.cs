using System;
using System.Collections.Generic;

namespace Disqord.Gateway;

public class ThreadMembersUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the update occurred.
    /// </summary>
    public Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the ID of the thread that the update occurred for.
    /// </summary>
    public Snowflake ThreadId { get; }

    /// <summary>
    ///     Gets the cached thread the the update occurred for.
    ///     Returns <see langword="null"/> if the thread was not cached.
    /// </summary>
    public CachedThreadChannel? Thread { get; }

    /// <summary>
    ///     Gets the approximate member count of the thread.
    /// </summary>
    /// <remarks>
    ///     This is not always a reliable property as Discord stops counting rather quickly.
    /// </remarks>
    public int MemberCount { get; }

    /// <summary>
    ///     Gets the thread members added to the thread keyed by their IDs.
    /// </summary>
    public IReadOnlyDictionary<Snowflake, IThreadMember> AddedMembers { get; }

    /// <summary>
    ///     Gets the IDs of the thread members removed from the thread.
    /// </summary>
    public IReadOnlyList<Snowflake> RemovedMemberIds { get; }

    public ThreadMembersUpdatedEventArgs(
        Snowflake guildId,
        Snowflake threadId,
        CachedThreadChannel? thread,
        int memberCount,
        IReadOnlyDictionary<Snowflake, IThreadMember> addedMembers,
        IReadOnlyList<Snowflake> removedMemberIds)
    {
        GuildId = guildId;
        ThreadId = threadId;
        Thread = thread;
        MemberCount = memberCount;
        AddedMembers = addedMembers;
        RemovedMemberIds = removedMemberIds;
    }
}
