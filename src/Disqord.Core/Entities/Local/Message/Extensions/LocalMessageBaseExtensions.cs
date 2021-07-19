using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Disqord
{
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

        public static TMessage WithEmbeds<TMessage>(this TMessage message, params LocalEmbed[] embeds)
            where TMessage : LocalMessageBase
            => message.WithEmbeds(embeds as IEnumerable<LocalEmbed>);

        public static TMessage WithEmbeds<TMessage>(this TMessage message, IEnumerable<LocalEmbed> embeds)
            where TMessage : LocalMessageBase
        {
            if (embeds == null)
                throw new ArgumentNullException(nameof(embeds));

            message._embeds.Clear();
            message._embeds.AddRange(embeds);
            return message;
        }

        public static TMessage AddEmbed<TMessage>(this TMessage message, LocalEmbed embed)
            where TMessage : LocalMessageBase
        {
            if (embed == null)
                throw new ArgumentNullException(nameof(embed));

            message._embeds.Add(embed);
            return message;
        }

        public static TMessage WithAllowedMentions<TMessage>(this TMessage message, LocalAllowedMentions allowedMentions)
            where TMessage : LocalMessageBase
        {
            message.AllowedMentions = allowedMentions;
            return message;
        }

        public static TMessage WithAttachments<TMessage>(this TMessage message, params LocalAttachment[] attachments)
            where TMessage : LocalMessageBase
            => message.WithAttachments(attachments as IEnumerable<LocalAttachment>);

        public static TMessage WithAttachments<TMessage>(this TMessage message, IEnumerable<LocalAttachment> attachments)
            where TMessage : LocalMessageBase
        {
            if (attachments == null)
                throw new ArgumentNullException(nameof(attachments));

            message._attachments.Clear();
            message._attachments.AddRange(attachments);
            return message;
        }

        public static TMessage AddAttachment<TMessage>(this TMessage message, LocalAttachment attachment)
            where TMessage : LocalMessageBase
        {
            if (attachment == null)
                throw new ArgumentNullException(nameof(attachment));

            message._attachments.Add(attachment);
            return message;
        }

        public static TMessage WithComponents<TMessage>(this TMessage message, params LocalRowComponent[] components)
            where TMessage : LocalMessageBase
            => message.WithComponents(components as IEnumerable<LocalRowComponent>);

        public static TMessage WithComponents<TMessage>(this TMessage message, IEnumerable<LocalRowComponent> components)
            where TMessage : LocalMessageBase
        {
            if (components == null)
                throw new ArgumentNullException(nameof(components));

            message._components.Clear();
            message._components.AddRange(components);
            return message;
        }

        public static TMessage AddComponent<TMessage>(this TMessage message, LocalRowComponent component)
            where TMessage : LocalMessageBase
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            message._components.Add(component);
            return message;
        }

        public static TMessage WithStickerIds<TMessage>(this TMessage message, params Snowflake[] stickerIds)
            where TMessage : LocalMessageBase
            => message.WithStickerIds(stickerIds as IEnumerable<Snowflake>);

        public static TMessage WithStickerIds<TMessage>(this TMessage message, IEnumerable<Snowflake> stickerIds)
            where TMessage : LocalMessageBase
        {
            if (stickerIds == null)
                throw new ArgumentNullException(nameof(stickerIds));

            message._stickerIds.Clear();
            message._stickerIds.AddRange(stickerIds);
            return message;
        }

        public static TMessage AddStickerId<TMessage>(this TMessage message, Snowflake stickerId)
            where TMessage : LocalMessageBase
        {
            message._stickerIds.Add(stickerId);
            return message;
        }
    }
}
