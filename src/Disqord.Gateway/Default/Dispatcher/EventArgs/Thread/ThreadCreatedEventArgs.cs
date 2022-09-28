using System;
using Qommon;

namespace Disqord.Gateway;

public class ThreadCreatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild the thread channel was created in.
    /// </summary>
    public Snowflake GuildId => Thread.GuildId;

    /// <summary>
    ///     Gets the ID of the created thread.
    /// </summary>
    public Snowflake ThreadId => Thread.Id;

    /// <summary>
    ///     Gets the created thread.
    /// </summary>
    public IThreadChannel Thread { get; }

    /// <summary>
    ///     Gets whether the thread was created or the bot joined the existing thread.
    /// </summary>
    /// <returns>
    ///     <see langword="true"/> if the thread was created; <see langword="false"/> if the bot joined the thread.
    /// </returns>
    public bool IsThreadCreation { get; }

    public ThreadCreatedEventArgs(
        IThreadChannel thread,
        bool isThreadCreation)
    {
        Guard.IsNotNull(thread);

        Thread = thread;
        IsThreadCreation = isThreadCreation;
    }
}
