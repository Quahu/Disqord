using System;
using System.Collections.Generic;

namespace Disqord.Gateway;

public class ThreadsSynchronizedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the threads were synchronized.
    /// </summary>
    public Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the synchronized threads keyed by the IDs of their parent channels.
    /// </summary>
    public IReadOnlyDictionary<Snowflake, IReadOnlyList<IThreadChannel>> Threads { get; }

    /// <summary>
    ///     Gets the threads that were removed from the cache due to the synchronization.
    /// </summary>
    public IReadOnlyDictionary<Snowflake, IReadOnlyList<CachedThreadChannel>> UncachedThreads { get; }

    public ThreadsSynchronizedEventArgs(
        Snowflake guildId,
        IReadOnlyDictionary<Snowflake, IReadOnlyList<IThreadChannel>> threads,
        IReadOnlyDictionary<Snowflake, IReadOnlyList<CachedThreadChannel>> uncachedThreads)
    {
        GuildId = guildId;
        Threads = threads;
        UncachedThreads = uncachedThreads;
    }
}