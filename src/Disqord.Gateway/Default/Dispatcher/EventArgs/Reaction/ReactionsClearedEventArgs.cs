using System;
using System.Collections.Generic;
using Qommon;

namespace Disqord.Gateway
{
    public class ReactionsClearedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild in which the reactions were cleared.
        ///     Returns <see langword="null"/> if it was removed in a private channel.
        /// </summary>
        public Snowflake? GuildId { get; }

        /// <summary>
        ///     Gets the ID of the channel in which the reactions were cleared.
        /// </summary>
        public Snowflake ChannelId { get; }

        /// <summary>
        ///     Gets the ID of the message to which the reactions were cleared.
        /// </summary>
        public Snowflake MessageId { get; }

        /// <summary>
        ///     Gets the message from which the reactions were cleared.
        ///     Returns <see langword="null"/> if the message was not cached.
        /// </summary>
        public CachedUserMessage Message { get; }

        /// <summary>
        ///     Gets the emoji that was cleared.
        ///     Returns <see langword="null"/> if all emojis were cleared and not just a single one.
        /// </summary>
        public IEmoji Emoji { get; }

        /// <summary>
        ///     Gets the reactions in the state they were prior to this event.
        ///     Returns a value only if the <see cref="Message"/> is not <see langword="null"/> and the message had reaction data available.
        /// </summary>
        public Optional<IReadOnlyDictionary<IEmoji, IMessageReaction>> OldReactions { get; }

        public ReactionsClearedEventArgs(
            Snowflake? guildId,
            Snowflake channelId,
            Snowflake messageId,
            CachedUserMessage message,
            IEmoji emoji,
            Optional<IReadOnlyDictionary<IEmoji, IMessageReaction>> oldReactions)
        {
            GuildId = guildId;
            ChannelId = channelId;
            MessageId = messageId;
            Message = message;
            Emoji = emoji;
            OldReactions = oldReactions;
        }
    }
}
