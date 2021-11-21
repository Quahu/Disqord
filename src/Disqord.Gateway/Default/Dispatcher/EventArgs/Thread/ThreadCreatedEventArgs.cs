using System;
using Qommon;

namespace Disqord.Gateway
{
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

        public ThreadCreatedEventArgs(
            IThreadChannel thread)
        {
            Guard.IsNotNull(thread);

            Thread = thread;
        }
    }
}
