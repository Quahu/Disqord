using System;

namespace Disqord.Gateway
{
    public class ThreadDeletedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild the thread channel was deleted in.
        /// </summary>
        public Snowflake GuildId => Thread.GuildId;

        /// <summary>
        ///     Gets the ID of the deleted thread.
        /// </summary>
        public Snowflake ThreadId => Thread.Id;

        /// <summary>
        ///     Gets the deleted thread.
        /// </summary>
        public IThreadChannel Thread { get; }

        public ThreadDeletedEventArgs(IThreadChannel thread)
        {
            if (thread == null)
                throw new ArgumentNullException(nameof(thread));

            Thread = thread;
        }
    }
}
