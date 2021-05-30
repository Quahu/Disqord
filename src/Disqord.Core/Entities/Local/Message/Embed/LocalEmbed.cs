using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public sealed class LocalEmbed : ILocalConstruct
    {
        public const int MAX_FIELDS_AMOUNT = 25;

        public const int MAX_TITLE_LENGTH = 256;

        public const int MAX_DESCRIPTION_LENGTH = 2048;

        public const int MAX_LENGTH = 6000;

        public string Title
        {
            get => _title;
            set
            {
                if (value != null && value.Length > MAX_TITLE_LENGTH)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The embed's title must not be longer than {MAX_TITLE_LENGTH} characters.");

                _title = value;
            }
        }
        private string _title;

        public string Description
        {
            get => _description;
            set
            {
                if (value != null && value.Length > MAX_DESCRIPTION_LENGTH)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The embed's description must not be longer than {MAX_DESCRIPTION_LENGTH} characters.");

                _description = value;
            }
        }
        private string _description;

        public string Url { get; set; }

        public string ImageUrl { get; set; }

        public string ThumbnailUrl { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public Color? Color { get; set; }

        public LocalEmbedFooter Footer { get; set; }

        public LocalEmbedAuthor Author { get; set; }

        public IList<LocalEmbedField> Fields
        {
            get => _fields;
            set => WithFields(value);
        }
        private readonly IList<LocalEmbedField> _fields;

        public int Length
        {
            get
            {
                var titleLength = _title?.Length ?? 0;
                var descriptionLength = _description?.Length ?? 0;
                var footerLength = Footer?.Length ?? 0;
                var authorLength = Author?.Length ?? 0;
                var fieldsLength = _fields?.Sum(x => x.Length) ?? 0;
                return titleLength + descriptionLength + footerLength + authorLength + fieldsLength;
            }
        }

        public LocalEmbed()
        {
            _fields = new List<LocalEmbedField>();
        }

        private LocalEmbed(LocalEmbed other)
        {
            _title = other.Title;
            _description = other.Description;
            Url = other.Url;
            ImageUrl = other.ImageUrl;
            ThumbnailUrl = other.ThumbnailUrl;
            Timestamp = other.Timestamp;
            Color = other.Color;
            Footer = other.Footer?.Clone();
            Author = other.Author?.Clone();
            _fields = other.Fields.Select(x => x.Clone()).ToList();
        }

        public LocalEmbed WithTitle(string title)
        {
            Title = title;
            return this;
        }

        public LocalEmbed WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public LocalEmbed WithUrl(string url)
        {
            Url = url;
            return this;
        }

        public LocalEmbed WithImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
            return this;
        }

        public LocalEmbed WithThumbnailUrl(string thumbnailUrl)
        {
            ThumbnailUrl = thumbnailUrl;
            return this;
        }

        public LocalEmbed WithTimestamp(DateTimeOffset? timestamp)
        {
            Timestamp = timestamp;
            return this;
        }

        public LocalEmbed WithColor(Color? color)
        {
            Color = color;
            return this;
        }

        public LocalEmbed WithFooter(string text, string iconUrl = null)
        {
            Footer = new LocalEmbedFooter
            {
                Text = text,
                IconUrl = iconUrl
            };
            return this;
        }

        public LocalEmbed WithFooter(LocalEmbedFooter footer)
        {
            Footer = footer;
            return this;
        }

        public LocalEmbed WithAuthor(string name, string iconUrl = null, string url = null)
        {
            Author = new LocalEmbedAuthor
            {
                Name = name,
                IconUrl = iconUrl,
                Url = url
            };
            return this;
        }

        public LocalEmbed WithAuthor(LocalEmbedAuthor author)
        {
            Author = author;
            return this;
        }

        public LocalEmbed WithAuthor(IUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            Author = new LocalEmbedAuthor
            {
                Name = user.Tag,
                IconUrl = user.GetAvatarUrl(size: 128)
            };
            return this;
        }

        public LocalEmbed WithFields(IEnumerable<LocalEmbedField> fields)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));

            _fields.Clear();
            foreach (var field in fields)
                _fields.Add(field);

            return this;
        }

        public LocalEmbed AddField(string name, string value, bool isInline = false)
        {
            Fields.Add(new LocalEmbedField
            {
                Name = name,
                Value = value,
                IsInline = isInline
            });
            return this;
        }

        public LocalEmbed AddField(string name, object value, bool isInline = false)
        {
            Fields.Add(new LocalEmbedField
            {
                Name = name,
                Value = value?.ToString(),
                IsInline = isInline
            });
            return this;
        }

        public LocalEmbed AddField(LocalEmbedField field)
        {
            Fields.Add(field);
            return this;
        }

        public LocalEmbed AddBlankField(bool isInline = false)
            => AddField("\u200b", "\u200b", isInline);

        public LocalEmbed Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();

        public static LocalEmbed FromEmbed(Embed embed)
        {
            var builder = new LocalEmbed
            {
                Title = embed.Title,
                Description = embed.Description,
                Url = embed.Url,
                ImageUrl = embed.Image?.Url,
                ThumbnailUrl = embed.Thumbnail?.Url,
                Timestamp = embed.Timestamp,
                Color = embed.Color,
            };

            if (embed.Footer != null)
                builder.WithFooter(embed.Footer.Text, embed.Footer.IconUrl);

            if (embed.Author != null)
                builder.WithAuthor(embed.Author.Name, embed.Author.IconUrl, embed.Author.Url);

            for (var i = 0; i < embed.Fields.Count; i++)
            {
                var field = embed.Fields[i];
                builder.AddField(field.Name, field.Value, field.IsInline);
            }

            return builder;
        }

        public void Validate()
        {
            if (_fields.Count > MAX_FIELDS_AMOUNT)
                throw new InvalidOperationException($"The embed builder must not contain more than {MAX_FIELDS_AMOUNT} fields.");

            if (Length > MAX_LENGTH)
                throw new InvalidOperationException($"The length of an embed must not exceed {MAX_LENGTH} characters.");

            Footer?.Validate();
            Author?.Validate();

            for (var i = 0; i < _fields.Count; i++)
                _fields[i].Validate();
        }
    }
}
