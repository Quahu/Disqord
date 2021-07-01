using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public class LocalEmbed : ILocalConstruct
    {
        public const int MaxFieldsAmount = 25;

        public const int MaxTitleLength = 256;

        public const int MaxDescriptionLength = 4096;

        public string Title
        {
            get => _title;
            set
            {
                if (value != null && value.Length > MaxTitleLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The embed's title must not be longer than {MaxTitleLength} characters.");

                _title = value;
            }
        }
        private string _title;

        public string Description
        {
            get => _description;
            set
            {
                if (value != null && value.Length > MaxDescriptionLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The embed's description must not be longer than {MaxDescriptionLength} characters.");

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
            set => this.WithFields(value);
        }
        internal readonly List<LocalEmbedField> _fields;

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

        public virtual LocalEmbed Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();

        public void Validate()
        {
            if (_fields.Count > MaxFieldsAmount)
                throw new InvalidOperationException($"The embed builder must not contain more than {MaxFieldsAmount} fields.");

            Footer?.Validate();
            Author?.Validate();

            for (var i = 0; i < _fields.Count; i++)
                _fields[i].Validate();
        }

        public static LocalEmbed FromEmbed(Embed embed)
        {
            var builder = new LocalEmbed
            {
                _title = embed.Title,
                _description = embed.Description,
                Url = embed.Url,
                ImageUrl = embed.Image?.Url,
                ThumbnailUrl = embed.Thumbnail?.Url,
                Timestamp = embed.Timestamp,
                Color = embed.Color
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
    }
}
