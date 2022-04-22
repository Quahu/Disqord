using System.Collections.Generic;
using Disqord.Models;
using Qommon;

namespace Disqord
{
    /// <summary>
    ///     Represents a message.
    /// </summary>
    public interface IMessage : ISnowflakeEntity, IChannelEntity, IJsonUpdatable<MessageJsonModel>
    {
        /// <summary>
        ///     Gets the author of this message.
        /// </summary>
        IUser Author { get; }

        /// <summary>
        ///     Gets the content of this message.
        /// </summary>
        string Content { get; }

        /// <summary>
        ///     Gets the mentioned users of this message.
        /// </summary>
        IReadOnlyList<IUser> MentionedUsers { get; }

        /// <summary>
        ///     Gets the reactions of this message.
        /// </summary>
        Optional<IReadOnlyDictionary<IEmoji, IMessageReaction>> Reactions { get; }

        /// <summary>
        ///     Gets the flags of this message.
        /// </summary>
        MessageFlag Flags { get; }
    }
}
