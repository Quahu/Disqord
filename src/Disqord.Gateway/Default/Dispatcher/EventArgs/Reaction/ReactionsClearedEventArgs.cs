using System;
using System.Collections.Generic;

namespace Disqord.Gateway
{
    public class ReactionsClearedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the channel in which the reactions were cleared.
        /// </summary>
        public Snowflake ChannelId { get; }

        /// <summary>
        ///     Gets the ID of the message to which the reactions were cleared.
        /// </summary>
        public Snowflake MessageId { get; }

        /// <summary>
        ///     Gets the message from swhich the reactions were cleared.
        ///     Returns <see langword="null"/> if the message was not cached.
        /// </summary>
        public CachedUserMessage Message { get; }

        /// <summary>
        ///     Gets the ID of the guild in which the reactions were cleared.
        ///     Returns <see langword="null"/> if it was removed in a private channel.
        /// </summary>
        public Snowflake? GuildId { get; }

        /// <summary>
        ///     Gets the emoji that was cleared.
        ///     Returns <see langword="null"/> if all emojis were cleared and not just a single one.
        /// </summary>
        public IEmoji Emoji { get; }

        /// <summary>
        ///     Gets the reactions in the state they were prior to this event.
        ///     Returns a value only if the <see cref="Message"/> is not <see langword="null"/> and the message had reaction data available.
        /// </summary>
        public Optional<IReadOnlyDictionary<IEmoji, MessageReaction>> OldReactions { get; }

        public ReactionsClearedEventArgs(
            Snowflake channelId, 
            Snowflake messageId, 
            CachedUserMessage message, 
            Snowflake? guildId, 
            IEmoji emoji, 
            Optional<IReadOnlyDictionary<IEmoji, MessageReaction>> oldReactions)
        {
            ChannelId = channelId;
            MessageId = messageId;
            Message = message;
            GuildId = guildId;
            Emoji = emoji;
            OldReactions = oldReactions;
        }
    }
}
