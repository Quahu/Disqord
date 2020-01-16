using System;
using System.Linq;

namespace Disqord
{
    /// <summary>
    ///     Allows for building of <see cref="LocalEmbed"/>s.
    ///     This class is not thread-safe.
    /// </summary>
    public sealed class LocalEmbedBuilder : ICloneable
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

        public LocalEmbedFooterBuilder Footer { get; set; }

        public LocalEmbedAuthorBuilder Author { get; set; }

        public LocalEmbedFieldBuilderCollection Fields { get; }

        public int Length
        {
            get
            {
                var titleLength = _title?.Length ?? 0;
                var descriptionLength = _description?.Length ?? 0;
                var footerLength = Footer?.Length ?? 0;
                var authorLength = Author?.Length ?? 0;
                var fieldsLength = Fields.Sum(x => x.Length);
                return titleLength + descriptionLength + footerLength + authorLength + fieldsLength;
            }
        }

        public LocalEmbedBuilder()
        {
            Fields = new LocalEmbedFieldBuilderCollection();
        }

        internal LocalEmbedBuilder(LocalEmbedBuilder builder)
        {
            _title = builder.Title;
            _description = builder.Description;
            Url = builder.Url;
            ImageUrl = builder.ImageUrl;
            ThumbnailUrl = builder.ThumbnailUrl;
            Timestamp = builder.Timestamp;
            Color = builder.Color;
            Footer = builder.Footer?.Clone();
            Author = builder.Author?.Clone();

            Fields = new LocalEmbedFieldBuilderCollection(builder.Fields.Select(x => x.Clone()));
        }

        public LocalEmbedBuilder WithTitle(string title)
        {
            Title = title;
            return this;
        }

        public LocalEmbedBuilder WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public LocalEmbedBuilder WithUrl(string url)
        {
            Url = url;
            return this;
        }

        public LocalEmbedBuilder WithImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
            return this;
        }

        public LocalEmbedBuilder WithThumbnailUrl(string thumbnailUrl)
        {
            ThumbnailUrl = thumbnailUrl;
            return this;
        }

        public LocalEmbedBuilder WithTimestamp(DateTimeOffset? timestamp)
        {
            Timestamp = timestamp;
            return this;
        }

        public LocalEmbedBuilder WithColor(Color? color)
        {
            Color = color;
            return this;
        }

        public LocalEmbedBuilder WithFooter(string text, string iconUrl = null)
        {
            Footer = new LocalEmbedFooterBuilder
            {
                Text = text,
                IconUrl = iconUrl
            };
            return this;
        }

        public LocalEmbedBuilder WithFooter(LocalEmbedFooterBuilder footer)
        {
            Footer = footer;
            return this;
        }

        public LocalEmbedBuilder WithFooter(Action<LocalEmbedFooterBuilder> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var footer = new LocalEmbedFooterBuilder();
            action(footer);
            Footer = footer;
            return this;
        }

        public LocalEmbedBuilder WithAuthor(string name, string iconUrl = null, string url = null)
        {
            Author = new LocalEmbedAuthorBuilder
            {
                Name = name,
                IconUrl = iconUrl,
                Url = url
            };
            return this;
        }

        public LocalEmbedBuilder WithAuthor(LocalEmbedAuthorBuilder author)
        {
            Author = author;
            return this;
        }

        public LocalEmbedBuilder WithAuthor(Action<LocalEmbedAuthorBuilder> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var author = new LocalEmbedAuthorBuilder();
            action(author);
            Author = author;
            return this;
        }

        public LocalEmbedBuilder WithAuthor(IUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            Author = new LocalEmbedAuthorBuilder
            {
                Name = user.Tag,
                IconUrl = user.GetAvatarUrl(size: 128)
            };
            return this;
        }

        public LocalEmbedBuilder AddField(string name, string value, bool isInline = false)
        {
            Fields.Add(new LocalEmbedFieldBuilder
            {
                Name = name,
                Value = value,
                IsInline = isInline
            });
            return this;
        }

        public LocalEmbedBuilder AddField(string name, object value, bool isInline = false)
        {
            Fields.Add(new LocalEmbedFieldBuilder
            {
                Name = name,
                Value = value?.ToString(),
                IsInline = isInline
            });
            return this;
        }

        public LocalEmbedBuilder AddField(LocalEmbedFieldBuilder field)
        {
            Fields.Add(field);
            return this;
        }

        public LocalEmbedBuilder AddField(Action<LocalEmbedFieldBuilder> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var field = new LocalEmbedFieldBuilder();
            action(field);
            Fields.Add(field);
            return this;
        }

        public LocalEmbedBuilder AddBlankField(bool isInline = false)
            => AddField("\u200b", "\u200b", isInline);

        /// <summary>
        ///     Creates a deep copy of this <see cref="LocalEmbedBuilder"/>.
        /// </summary>
        /// <returns>
        ///     A deep copy of this <see cref="LocalEmbedBuilder"/>.
        /// </returns>
        public LocalEmbedBuilder Clone()
            => new LocalEmbedBuilder(this);

        object ICloneable.Clone()
            => Clone();

        public static LocalEmbedBuilder FromEmbed(Embed embed)
        {
            var builder = new LocalEmbedBuilder
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

        public LocalEmbed Build()
        {
            if (Length > MAX_LENGTH)
                throw new InvalidOperationException($"The length of an embed must not exceed {MAX_LENGTH} characters.");

            return new LocalEmbed(this);
        }
    }
}
