using System;
using System.Collections.Generic;
using Qommon;

namespace Disqord
{
    public static class LocalEmbedExtensions
    {
        public static TEmbed WithTitle<TEmbed>(this TEmbed embed, string title)
            where TEmbed : LocalEmbed
        {
            embed.Title = title;
            return embed;
        }

        public static TEmbed WithDescription<TEmbed>(this TEmbed embed, string description)
            where TEmbed : LocalEmbed
        {
            embed.Description = description;
            return embed;
        }

        public static TEmbed WithUrl<TEmbed>(this TEmbed embed, string url)
            where TEmbed : LocalEmbed
        {
            embed.Url = url;
            return embed;
        }

        public static TEmbed WithImageUrl<TEmbed>(this TEmbed embed, string imageUrl)
            where TEmbed : LocalEmbed
        {
            embed.ImageUrl = imageUrl;
            return embed;
        }

        public static TEmbed WithThumbnailUrl<TEmbed>(this TEmbed embed, string thumbnailUrl)
            where TEmbed : LocalEmbed
        {
            embed.ThumbnailUrl = thumbnailUrl;
            return embed;
        }

        public static TEmbed WithTimestamp<TEmbed>(this TEmbed embed, DateTimeOffset? timestamp)
            where TEmbed : LocalEmbed
        {
            embed.Timestamp = timestamp;
            return embed;
        }

        public static TEmbed WithColor<TEmbed>(this TEmbed embed, Color? color)
            where TEmbed : LocalEmbed
        {
            embed.Color = color;
            return embed;
        }

        public static TEmbed WithFooter<TEmbed>(this TEmbed embed, string text, string iconUrl = null)
            where TEmbed : LocalEmbed
        {
            embed.Footer = new LocalEmbedFooter
            {
                Text = text,
                IconUrl = iconUrl
            };
            return embed;
        }

        public static TEmbed WithFooter<TEmbed>(this TEmbed embed, LocalEmbedFooter footer)
            where TEmbed : LocalEmbed
        {
            embed.Footer = footer;
            return embed;
        }

        public static TEmbed WithAuthor<TEmbed>(this TEmbed embed, string name, string iconUrl = null, string url = null)
            where TEmbed : LocalEmbed
        {
            embed.Author = new LocalEmbedAuthor
            {
                Name = name,
                IconUrl = iconUrl,
                Url = url
            };
            return embed;
        }

        public static TEmbed WithAuthor<TEmbed>(this TEmbed embed, LocalEmbedAuthor author)
            where TEmbed : LocalEmbed
        {
            embed.Author = author;
            return embed;
        }

        public static TEmbed WithAuthor<TEmbed>(this TEmbed embed, IUser user)
            where TEmbed : LocalEmbed
        {
            Guard.IsNotNull(user);

            embed.Author = new LocalEmbedAuthor
            {
                Name = user.Tag,
                IconUrl = user.GetAvatarUrl(size: 128)
            };
            return embed;
        }

        public static TEmbed WithFields<TEmbed>(this TEmbed embed, params LocalEmbedField[] fields)
            where TEmbed : LocalEmbed
            => embed.WithFields(fields as IEnumerable<LocalEmbedField>);

        public static TEmbed WithFields<TEmbed>(this TEmbed embed, IEnumerable<LocalEmbedField> fields)
            where TEmbed : LocalEmbed
        {
            Guard.IsNotNull(fields);

            embed._fields.Clear();
            embed._fields.AddRange(fields);
            return embed;
        }

        public static TEmbed AddField<TEmbed>(this TEmbed embed, string name, string value, bool isInline = false)
            where TEmbed : LocalEmbed
        {
            embed._fields.Add(new LocalEmbedField
            {
                Name = name,
                Value = value,
                IsInline = isInline
            });
            return embed;
        }

        public static TEmbed AddField<TEmbed>(this TEmbed embed, string name, object value, bool isInline = false)
            where TEmbed : LocalEmbed
        {
            embed._fields.Add(new LocalEmbedField
            {
                Name = name,
                Value = value?.ToString(),
                IsInline = isInline
            });
            return embed;
        }

        public static TEmbed AddField<TEmbed>(this TEmbed embed, LocalEmbedField field)
            where TEmbed : LocalEmbed
        {
            embed._fields.Add(field);
            return embed;
        }

        public static TEmbed AddBlankField<TEmbed>(this TEmbed embed, bool isInline = false)
            where TEmbed : LocalEmbed
            => embed.AddField("\u200b", "\u200b", isInline);
    }
}
