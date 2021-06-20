using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Disqord
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class LocalMessageBaseExtensions
    {
        public static TLocalMessage WithContent<TLocalMessage>(this TLocalMessage message, string content)
            where TLocalMessage : LocalMessageBase
        {
            message.Content = content;
            return message;
        }

        public static TLocalMessage WithIsTextToSpeech<TLocalMessage>(this TLocalMessage message, bool isTextToSpeech = true)
            where TLocalMessage : LocalMessageBase
        {
            message.IsTextToSpeech = isTextToSpeech;
            return message;
        }

        public static TLocalMessage WithEmbeds<TLocalMessage>(this TLocalMessage message, params LocalEmbed[] embeds)
            where TLocalMessage : LocalMessageBase
            => message.WithEmbeds(embeds as IEnumerable<LocalEmbed>);

        public static TLocalMessage WithEmbeds<TLocalMessage>(this TLocalMessage message, IEnumerable<LocalEmbed> embeds)
            where TLocalMessage : LocalMessageBase
        {
            if (embeds == null)
                throw new ArgumentNullException(nameof(embeds));

            message._embeds.Clear();
            message._embeds.AddRange(embeds);
            return message;
        }

        public static TLocalMessage AddEmbed<TLocalMessage>(this TLocalMessage message, LocalEmbed embed)
            where TLocalMessage : LocalMessageBase
        {
            if (embed == null)
                throw new ArgumentNullException(nameof(embed));

            message._embeds.Add(embed);
            return message;
        }

        public static TLocalMessage WithAllowedMentions<TLocalMessage>(this TLocalMessage message, LocalAllowedMentions allowedMentions)
            where TLocalMessage : LocalMessageBase
        {
            message.AllowedMentions = allowedMentions;
            return message;
        }

        public static TLocalMessage WithAttachments<TLocalMessage>(this TLocalMessage message, params LocalAttachment[] attachments)
            where TLocalMessage : LocalMessageBase
            => message.WithAttachments(attachments as IEnumerable<LocalAttachment>);

        public static TLocalMessage WithAttachments<TLocalMessage>(this TLocalMessage message, IEnumerable<LocalAttachment> attachments)
            where TLocalMessage : LocalMessageBase
        {
            if (attachments == null)
                throw new ArgumentNullException(nameof(attachments));

            message._attachments.Clear();
            message._attachments.AddRange(attachments);
            return message;
        }

        public static TLocalMessage AddAttachment<TLocalMessage>(this TLocalMessage message, LocalAttachment attachment)
            where TLocalMessage : LocalMessageBase
        {
            if (attachment == null)
                throw new ArgumentNullException(nameof(attachment));

            message._attachments.Add(attachment);
            return message;
        }

        public static TLocalMessage WithComponents<TLocalMessage>(this TLocalMessage message, params LocalRowComponent[] components)
            where TLocalMessage : LocalMessageBase
            => message.WithComponents(components as IEnumerable<LocalRowComponent>);

        public static TLocalMessage WithComponents<TLocalMessage>(this TLocalMessage message, IEnumerable<LocalRowComponent> components)
            where TLocalMessage : LocalMessageBase
        {
            if (components == null)
                throw new ArgumentNullException(nameof(components));

            message._components.Clear();
            message._components.AddRange(components);
            return message;
        }

        public static TLocalMessage AddComponent<TLocalMessage>(this TLocalMessage message, LocalRowComponent component)
            where TLocalMessage : LocalMessageBase
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            message._components.Add(component);
            return message;
        }
    }
}
