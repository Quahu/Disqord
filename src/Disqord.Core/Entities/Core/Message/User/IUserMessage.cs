using System;
using System.Collections.Generic;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents a non-system message.
/// </summary>
public interface IUserMessage : IMessage
{
    /// <summary>
    ///     Gets the <see cref="UserMessageType"/> of this message.
    /// </summary>
    UserMessageType Type { get; }

    /// <summary>
    ///     Gets the edit date of this message.
    /// </summary>
    DateTimeOffset? EditedAt { get; }

    /// <summary>
    ///     Gets the ID of the webhook of this message.
    /// </summary>
    /// <returns>
    ///     The ID of the webhook or <see langword="null"/> if the message was not sent by a webhook.
    /// </returns>
    Snowflake? WebhookId { get; }

    /// <summary>
    ///     Gets whether this message is text-to-speech.
    /// </summary>
    bool IsTextToSpeech { get; }

    /// <summary>
    ///     Gets the optional nonce of this message.
    /// </summary>
    /// <remarks>
    ///     This can be set when sending a message (<see cref="LocalMessage.Nonce"/>)
    ///     which lets you identify it is the message you sent.
    /// </remarks>
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
    IReadOnlyList<IAttachment> Attachments { get; }

    /// <summary>
    ///     Gets the embeds of this message.
    /// </summary>
    IReadOnlyList<IEmbed> Embeds { get; }

    /// <summary>
    ///     Gets the activity tied to this message.
    /// </summary>
    IMessageActivity? Activity { get; }

    /// <summary>
    ///     Gets the application tied to this message.
    /// </summary>
    IMessageApplication? Application { get; }

    /// <summary>
    ///     Gets the ID of the application of this message.
    /// </summary>
    /// <returns>
    ///     The ID of the application or <see langword="null"/> if the message is not a response to an interaction.
    /// </returns>
    Snowflake? ApplicationId { get; }

    /// <summary>
    ///     Gets the reference tied to this message.
    /// </summary>
    IMessageReference? Reference { get; }

    /// <summary>
    ///     Gets the optional referenced message present in replies.
    /// </summary>
    /// <remarks>
    ///     If the message is a reply but this property has no value,
    ///     the Discord backend did not attempt to fetch the message that was being replied to, so its state is unknown.
    ///     If the property has a value but the value is <see langword="null"/>, the referenced message was deleted.
    /// </remarks>
    Optional<IUserMessage?> ReferencedMessage { get; }

    /// <summary>
    ///     Gets the interaction tied to this message.
    /// </summary>
    IMessageInteraction? Interaction { get; }

    /// <summary>
    ///     Gets the components of this message.
    /// </summary>
    IReadOnlyList<IRowComponent> Components { get; }

    /// <summary>
    ///     Gets the stickers sent with this message.
    /// </summary>
    IReadOnlyList<IMessageSticker> Stickers { get; }
}
