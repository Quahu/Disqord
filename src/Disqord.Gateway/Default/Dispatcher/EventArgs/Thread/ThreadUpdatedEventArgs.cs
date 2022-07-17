using System;

namespace Disqord.Gateway;

public class ThreadUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild the thread channel was updated in.
    /// </summary>
    public Snowflake GuildId => NewThread.GuildId;

    /// <summary>
    ///     Gets the ID of the updated thread.
    /// </summary>
    public Snowflake ThreadId => NewThread.Id;

    /// <summary>
    ///     Gets the thread in the state before the update occurred.
    ///     Returns <see langword="null"/> if the thread was not cached.
    /// </summary>
    public CachedThreadChannel? OldThread { get; }

    /// <summary>
    ///     Gets the updated thread.
    /// </summary>
    public IThreadChannel NewThread { get; }

    public ThreadUpdatedEventArgs(
        CachedThreadChannel? oldThread,
        IThreadChannel newThread)
    {
        OldThread = oldThread;
        NewThread = newThread;
    }
}
