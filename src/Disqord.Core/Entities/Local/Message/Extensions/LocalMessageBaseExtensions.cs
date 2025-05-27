using System.Collections.Generic;
using System.ComponentModel;
using Qommon;

namespace Disqord;

/// <summary>
///     Defines <see cref="LocalMessageBase"/> extensions.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalMessageBaseExtensions
{
    public static TMessage WithContent<TMessage>(this TMessage message, string content)
        where TMessage : LocalMessageBase
    {
        message.Content = content;
        return message;
    }

    public static TMessage WithIsTextToSpeech<TMessage>(this TMessage message, bool isTextToSpeech = true)
        where TMessage : LocalMessageBase
    {
        message.IsTextToSpeech = isTextToSpeech;
        return message;
    }

    public static TMessage AddEmbed<TMessage>(this TMessage message, LocalEmbed embed)
        where TMessage : LocalMessageBase
    {
        Guard.IsNotNull(embed);

        if (message.Embeds.Add(embed, out var list))
            message.Embeds = new(list);

        return message;
    }

    public static TMessage WithEmbeds<TMessage>(this TMessage message, IEnumerable<LocalEmbed> embeds)
        where TMessage : LocalMessageBase
    {
        Guard.IsNotNull(embeds);

        if (message.Embeds.With(embeds, out var list))
            message.Embeds = new(list);

        return message;
    }

    public static TMessage WithEmbeds<TMessage>(this TMessage message, params LocalEmbed[] embeds)
        where TMessage : LocalMessageBase
    {
        return message.WithEmbeds(embeds as IEnumerable<LocalEmbed>);
    }

    public static TMessage WithFlags<TMessage>(this TMessage message, MessageFlags flags)
        where TMessage : LocalMessageBase
    {
        message.Flags = flags;
        return message;
    }

    public static TMessage WithAllowedMentions<TMessage>(this TMessage message, LocalAllowedMentions allowedMentions)
        where TMessage : LocalMessageBase
    {
        message.AllowedMentions = allowedMentions;
        return message;
    }

    public static TMessage AddAttachment<TMessage>(this TMessage message, LocalAttachment attachment)
        where TMessage : LocalMessageBase
    {
        Guard.IsNotNull(attachment);

        if (message.Attachments.Add(attachment, out var list))
            message.Attachments = new(list);

        return message;
    }

    public static TMessage WithAttachments<TMessage>(this TMessage message, IEnumerable<LocalAttachment> attachments)
        where TMessage : LocalMessageBase
    {
        Guard.IsNotNull(attachments);

        if (message.Attachments.With(attachments, out var list))
            message.Attachments = new(list);

        return message;
    }

    public static TMessage WithAttachments<TMessage>(this TMessage message, params LocalAttachment[] attachments)
        where TMessage : LocalMessageBase
    {
        return message.WithAttachments(attachments as IEnumerable<LocalAttachment>);
    }

    public static TMessage AddStickerId<TMessage>(this TMessage message, Snowflake stickerId)
        where TMessage : LocalMessageBase
    {
        if (message.StickerIds.Add(stickerId, out var list))
            message.StickerIds = new(list);

        return message;
    }

    public static TMessage WithStickerIds<TMessage>(this TMessage message, IEnumerable<Snowflake> stickerIds)
        where TMessage : LocalMessageBase
    {
        Guard.IsNotNull(stickerIds);

        if (message.StickerIds.With(stickerIds, out var list))
            message.StickerIds = new(list);

        return message;
    }

    public static TMessage WithStickerIds<TMessage>(this TMessage message, params Snowflake[] stickerIds)
        where TMessage : LocalMessageBase
    {
        return message.WithStickerIds(stickerIds as IEnumerable<Snowflake>);
    }

    public static TMessage WithPoll<TMessage>(this TMessage message, LocalPoll poll)
        where TMessage : LocalMessageBase
    {
        message.Poll = poll;
        return message;
    }
}
