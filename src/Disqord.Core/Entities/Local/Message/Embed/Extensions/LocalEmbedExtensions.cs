using System;
using System.Collections.Generic;

namespace Disqord
{
    public static class LocalEmbedExtensions
    {
        public static LocalEmbed WithTitle(this LocalEmbed embed, string title)
        {
            embed.Title = title;
            return embed;
        }

        public static LocalEmbed WithDescription(this LocalEmbed embed, string description)
        {
            embed.Description = description;
            return embed;
        }

        public static LocalEmbed WithUrl(this LocalEmbed embed, string url)
        {
            embed.Url = url;
            return embed;
        }

        public static LocalEmbed WithImageUrl(this LocalEmbed embed, string imageUrl)
        {
            embed.ImageUrl = imageUrl;
            return embed;
        }

        public static LocalEmbed WithThumbnailUrl(this LocalEmbed embed, string thumbnailUrl)
        {
            embed.ThumbnailUrl = thumbnailUrl;
            return embed;
        }

        public static LocalEmbed WithTimestamp(this LocalEmbed embed, DateTimeOffset? timestamp)
        {
            embed.Timestamp = timestamp;
            return embed;
        }

        public static LocalEmbed WithColor(this LocalEmbed embed, Color? color)
        {
            embed.Color = color;
            return embed;
        }

        public static LocalEmbed WithFooter(this LocalEmbed embed, string text, string iconUrl = null)
        {
            embed.Footer = new LocalEmbedFooter
            {
                Text = text,
                IconUrl = iconUrl
            };
            return embed;
        }

        public static LocalEmbed WithFooter(this LocalEmbed embed, LocalEmbedFooter footer)
        {
            embed.Footer = footer;
            return embed;
        }

        public static LocalEmbed WithAuthor(this LocalEmbed embed, string name, string iconUrl = null, string url = null)
        {
            embed.Author = new LocalEmbedAuthor
            {
                Name = name,
                IconUrl = iconUrl,
                Url = url
            };
            return embed;
        }

        public static LocalEmbed WithAuthor(this LocalEmbed embed, LocalEmbedAuthor author)
        {
            embed.Author = author;
            return embed;
        }

        public static LocalEmbed WithAuthor(this LocalEmbed embed, IUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            embed.Author = new LocalEmbedAuthor
            {
                Name = user.Tag,
                IconUrl = user.GetAvatarUrl(size: 128)
            };
            return embed;
        }

        public static LocalEmbed WithFields(this LocalEmbed embed, params LocalEmbedField[] fields)
            => embed.WithFields(fields as IEnumerable<LocalEmbedField>);

        public static LocalEmbed WithFields(this LocalEmbed embed, IEnumerable<LocalEmbedField> fields)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));

            embed._fields.Clear();
            embed._fields.AddRange(fields);
            return embed;
        }

        public static LocalEmbed AddField(this LocalEmbed embed, string name, string value, bool isInline = false)
        {
            embed._fields.Add(new LocalEmbedField
            {
                Name = name,
                Value = value,
                IsInline = isInline
            });
            return embed;
        }

        public static LocalEmbed AddField(this LocalEmbed embed, string name, object value, bool isInline = false)
        {
            embed._fields.Add(new LocalEmbedField
            {
                Name = name,
                Value = value?.ToString(),
                IsInline = isInline
            });
            return embed;
        }

        public static LocalEmbed AddField(this LocalEmbed embed, LocalEmbedField field)
        {
            embed._fields.Add(field);
            return embed;
        }

        public static LocalEmbed AddBlankField(this LocalEmbed embed, bool isInline = false)
            => embed.AddField("\u200b", "\u200b", isInline);
    }
}
