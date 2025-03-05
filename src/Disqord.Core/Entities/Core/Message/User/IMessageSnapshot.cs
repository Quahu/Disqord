using System;
using System.Collections.Generic;

namespace Disqord;

public interface IMessageSnapshot : IEntity
{
    /// <summary>
    ///     Gets the <see cref="UserMessageType"/> of this message snapshot.
    /// </summary>
    UserMessageType Type { get; }

    /// <summary>
    ///     Gets the content of this message snapshot.
    /// </summary>
    string Content { get; }

    /// <summary>
    ///     Gets the mentioned users of this message snapshot.
    /// </summary>
    IReadOnlyList<IUser> MentionedUsers { get; }

    /// <summary>
    ///     Gets the mentioned role IDs of this message snapshot.
    /// </summary>
    IReadOnlyList<Snowflake> MentionedRoleIds { get; }

    /// <summary>
    ///     Gets the attachments of this message snapshot.
    /// </summary>
    IReadOnlyList<IAttachment> Attachments { get; }

    /// <summary>
    ///     Gets the embeds of this message snapshot.
    /// </summary>
    IReadOnlyList<IEmbed> Embeds { get; }

    /// <summary>
    ///     Gets the timestamp of when the snapshotted message was created.
    /// </summary>
    DateTimeOffset Timestamp { get; }

    /// <summary>
    ///     Gets the edit date of this message snapshot.
    /// </summary>
    DateTimeOffset? EditedAt { get; }

    /// <summary>
    ///     Gets the flags of this message snapshot.
    /// </summary>
    MessageFlags Flags { get; }

    /// <summary>
    ///     Gets the stickers sent with this message snapshot.
    /// </summary>
    IReadOnlyList<IMessageSticker> Stickers { get; }

    /// <summary>
    ///     Gets the components of this message snapshot.
    /// </summary>
    IReadOnlyList<IRowComponent> Components { get; }
}
