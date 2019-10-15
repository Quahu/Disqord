using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public sealed class EmbedBuilder
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
                if (value?.Length > MAX_TITLE_LENGTH)
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
                if (value?.Length > MAX_DESCRIPTION_LENGTH)
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

        public EmbedFooterBuilder Footer { get; set; }

        public EmbedAuthorBuilder Author { get; set; }

        public List<EmbedFieldBuilder> Fields { get; }

        public EmbedBuilder()
        {
            Fields = new List<EmbedFieldBuilder>();
        }

        public EmbedBuilder(EmbedBuilder builder)
        {
            Title = builder.Title;
            Description = builder.Description;
            Url = builder.Url;
            ImageUrl = builder.ImageUrl;
            ThumbnailUrl = builder.ThumbnailUrl;
            Timestamp = builder.Timestamp;
            Color = builder.Color;
            Footer = new EmbedFooterBuilder(builder.Footer);
            Author = new EmbedAuthorBuilder(builder.Author);
            Fields = builder.Fields.Select(x => new EmbedFieldBuilder(x)).ToList();
        }

        public EmbedBuilder WithTitle(string title)
        {
            Title = title;
            return this;
        }

        public EmbedBuilder WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public EmbedBuilder WithUrl(string url)
        {
            Url = url;
            return this;
        }

        public EmbedBuilder WithImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
            return this;
        }

        public EmbedBuilder WithThumbnailUrl(string thumbnailUrl)
        {
            ThumbnailUrl = thumbnailUrl;
            return this;
        }

        public EmbedBuilder WithTimestamp(DateTimeOffset? timestamp)
        {
            Timestamp = timestamp;
            return this;
        }

        public EmbedBuilder WithColor(Color? color)
        {
            Color = color;
            return this;
        }

        public EmbedBuilder WithFooter(EmbedFooterBuilder footer)
        {
            Footer = footer;
            return this;
        }

        public EmbedBuilder WithFooter(Action<EmbedFooterBuilder> action)
        {
            var footer = new EmbedFooterBuilder();
            action(footer);
            Footer = footer;
            return this;
        }

        public EmbedBuilder WithFooter(string text = null, string iconUrl = null)
        {
            Footer = new EmbedFooterBuilder
            {
                Text = text,
                IconUrl = iconUrl
            };
            return this;
        }

        public EmbedBuilder WithAuthor(EmbedAuthorBuilder author)
        {
            Author = author;
            return this;
        }

        public EmbedBuilder WithAuthor(Action<EmbedAuthorBuilder> action)
        {
            var author = new EmbedAuthorBuilder();
            action(author);
            Author = author;
            return this;
        }

        public EmbedBuilder WithAuthor(string name, string iconUrl = null, string url = null)
        {
            Author = new EmbedAuthorBuilder
            {
                Name = name,
                IconUrl = iconUrl,
                Url = url
            };
            return this;
        }

        public EmbedBuilder WithAuthor(IUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            Author = new EmbedAuthorBuilder
            {
                Name = user.Tag,
                IconUrl = user.GetAvatarUrl(size: 128)
            };
            return this;
        }

        public EmbedBuilder AddField(string name, object value, bool isInline = false)
        {
            Fields.Add(new EmbedFieldBuilder
            {
                Name = name,
                Value = value?.ToString(),
                IsInline = isInline
            });
            return this;
        }

        public static EmbedBuilder FromEmbed(Embed embed)
        {
            var embedBuilder = new EmbedBuilder
            {
                Title = embed.Title,
                Description = embed.Description,
                Url = embed.Url,
                ImageUrl = embed.Image?.Url,
                ThumbnailUrl = embed.Thumbnail?.Url,
                Timestamp = embed.Timestamp,
                Color = embed.Color,
                Footer = embed.Footer != null
                    ? new EmbedFooterBuilder
                    {
                        Text = embed.Footer.Text,
                        IconUrl = embed.Footer.IconUrl
                    }
                    : null,
                Author = embed.Author != null
                    ? new EmbedAuthorBuilder
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

        public Embed Build()
        {
            // TODO
            //if (TotalLength > MAX_TOTAL_LENGTH)
            //    throw new InvalidOperationException($"The total length of an embed must not exceed {MAX_TOTAL_LENGTH} characters.");

            return new Embed(this);
        }
    }
}
