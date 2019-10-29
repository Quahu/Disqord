using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public sealed class LocalEmbedBuilder
    {
        public const int MAX_FIELDS_AMOUNT = 25;

        public const int MAX_TITLE_LENGTH = 256;

        public const int MAX_DESCRIPTION_LENGTH = 2048;

        public const int MAX_TOTAL_LENGTH = 6000;

        public string Title
        {
            get => _title;
            set
            {
                if (value != null && value.Length > MAX_TITLE_LENGTH)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The title of the embed must not be longer than {MAX_TITLE_LENGTH} characters.");

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
                    throw new ArgumentOutOfRangeException(nameof(value), $"The description of the embed must not be longer than {MAX_DESCRIPTION_LENGTH} characters.");

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

        public List<LocalEmbedFieldBuilder> Fields { get; }

        public LocalEmbedBuilder()
        {
            Fields = new List<LocalEmbedFieldBuilder>();
        }

        public LocalEmbedBuilder(LocalEmbedBuilder builder)
        {
            Title = builder.Title;
            Description = builder.Description;
            Url = builder.Url;
            ImageUrl = builder.ImageUrl;
            ThumbnailUrl = builder.ThumbnailUrl;
            Timestamp = builder.Timestamp;
            Color = builder.Color;

            if (builder.Footer != null)
                Footer = new LocalEmbedFooterBuilder(builder.Footer);

            if (builder.Author != null)
                Author = new LocalEmbedAuthorBuilder(builder.Author);

            Fields = builder.Fields.Select(x => new LocalEmbedFieldBuilder(x)).ToList();
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

        public LocalEmbedBuilder WithFooter(LocalEmbedFooterBuilder footer)
        {
            Footer = footer;
            return this;
        }

        public LocalEmbedBuilder WithFooter(Action<LocalEmbedFooterBuilder> action)
        {
            var footer = new LocalEmbedFooterBuilder();
            action(footer);
            Footer = footer;
            return this;
        }

        public LocalEmbedBuilder WithFooter(string text = null, string iconUrl = null)
        {
            Footer = new LocalEmbedFooterBuilder
            {
                Text = text,
                IconUrl = iconUrl
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
            var author = new LocalEmbedAuthorBuilder();
            action(author);
            Author = author;
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

        public static LocalEmbedBuilder FromEmbed(Embed embed)
        {
            var embedBuilder = new LocalEmbedBuilder
            {
                Title = embed.Title,
                Description = embed.Description,
                Url = embed.Url,
                ImageUrl = embed.Image?.Url,
                ThumbnailUrl = embed.Thumbnail?.Url,
                Timestamp = embed.Timestamp,
                Color = embed.Color,
                Footer = embed.Footer != null
                    ? new LocalEmbedFooterBuilder
                    {
                        Text = embed.Footer.Text,
                        IconUrl = embed.Footer.IconUrl
                    }
                    : null,
                Author = embed.Author != null
                    ? new LocalEmbedAuthorBuilder
                    {
                        Name = embed.Author.Name,
                        Url = embed.Author.Url,
                        IconUrl = embed.Author.IconUrl
                    }
                    : null
            };

            for (var i = 0; i < embed.Fields.Count; i++)
            {
                var field = embed.Fields[i];
                embedBuilder.AddField(field.Name, field.Value, field.IsInline);
            }

            return embedBuilder;
        }

        public LocalEmbed Build()
        {
            // TODO
            //if (TotalLength > MAX_TOTAL_LENGTH)
            //    throw new InvalidOperationException($"The total length of an embed must not exceed {MAX_TOTAL_LENGTH} characters.");

            return new LocalEmbed(this);
        }
    }
}
