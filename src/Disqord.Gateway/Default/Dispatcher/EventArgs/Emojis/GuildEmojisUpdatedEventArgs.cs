using System;
using System.Collections.Generic;

namespace Disqord.Gateway
{
    public class GuildEmojisUpdatedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild the emojis were updated in.
        /// </summary>
        public Snowflake GuildId { get; }

        /// <summary>
        ///     Gets the guild the emojis were updated in.
        ///     Returns <see langword="null"/> if the guild was not cached.
        /// </summary>
        public CachedGuild Guild { get; }

        /// <summary>
        ///     Gets the emojis in the state before the update occurred.
        ///     Returns <see langword="null"/> if the guild was not cached.
        /// </summary>
        public IReadOnlyDictionary<Snowflake, IGuildEmoji> OldEmojis { get; }

        /// <summary>
        ///     Gets the updated emojis.
        /// </summary>
        public IReadOnlyDictionary<Snowflake, IGuildEmoji> NewEmojis { get; }

        public GuildEmojisUpdatedEventArgs(
            Snowflake guildId,
            CachedGuild guild,
            IReadOnlyDictionary<Snowflake, IGuildEmoji> oldEmojis,
            IReadOnlyDictionary<Snowflake, IGuildEmoji> newEmojis)
        {
            GuildId = guildId;
            Guild = guild;
            OldEmojis = oldEmojis;
            NewEmojis = newEmojis;
        }
    }
}
