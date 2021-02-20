using System;
using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents a non-system message.
    /// </summary>
    public interface IUserMessage : IMessage
    {
        /// <summary>
        ///     Gets the edit date of this message.
        /// </summary>
        DateTimeOffset? EditedAt { get; }

        /// <summary>
        ///     Gets the webhook ID of this message.
        ///     Returns a valid value if the message was sent by a webhook.
        /// </summary>
        Snowflake? WebhookId { get; }

        /// <summary>
        ///     Gets whether this message is text-to-speech.
        /// </summary>
        bool IsTextToSpeech { get; }

        /// <summary>
        ///     Gets the optional nonce of this message.
        /// </summary>
        Optional<string> Nonce { get; }

        /// <summary>
        ///     Gets whether this message is pinned.
        /// </summary>
        bool IsPinned { get; }

        /// <summary>
        ///     Gets whether this message mentions everyone.
        /// </summary>
        bool MentionsEveryone { get; }

        /// <summary>
        ///     Gets the mentioned role IDs of this message.
        /// </summary>
        IReadOnlyList<Snowflake> MentionedRoleIds { get; }

        /// <summary>
        ///     Gets the attachments of this message.
        /// </summary>
        IReadOnlyList<Attachment> Attachments { get; }

        /// <summary>
        ///     Gets the embeds of this message.
        /// </summary>
        IReadOnlyList<Embed> Embeds { get; }

        /// <summary>
        ///     Gets the activity tied to this message.
        /// </summary>
        MessageActivity Activity { get; }

        /// <summary>
        ///     Gets the application tied to this message.
        /// </summary>
        MessageApplication Application { get; }

        /// <summary>
        ///     Gets the reference tied to this message.
        /// </summary>
        MessageReference Reference { get; }

        /// <summary>
        ///     Gets the <see cref="MessageFlag"/> of this message.
        /// </summary>
        MessageFlag Flags { get; }

        /// <summary>
        ///     Gets the stickers sent with this message.
        /// </summary>
        IReadOnlyList<Sticker> Stickers { get; }

        /// <summary>
        ///     Gets the optional referenced message present in replies.
        ///     If the message is a reply but this property has no value,
        ///     the backend did not attempt to fetch the message that was being replied to,
        ///     so its state is unknown. If the property has a value but the value is null, the referenced message was deleted.
        /// </summary>
        Optional<IUserMessage> ReferencedMessage { get; }
    }
}
