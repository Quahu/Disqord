using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Disqord
{
    public sealed class LocalEmbed
    {
        public string Title { get; }

        public string Description { get; }

        public string Url { get; }

        public string ImageUrl { get; }

        public string ThumbnailUrl { get; }

        public DateTimeOffset? Timestamp { get; }

        public Color? Color { get; }

        public LocalEmbedFooter Footer { get; }

        public LocalEmbedAuthor Author { get; }

        public IReadOnlyList<LocalEmbedField> Fields { get; }

        internal LocalEmbed(LocalEmbedBuilder builder)
        {
            Title = builder.Title;
            Description = builder.Description;
            Url = builder.Url;
            ImageUrl = builder.ImageUrl;
            ThumbnailUrl = builder.ThumbnailUrl;
            Timestamp = builder.Timestamp;
            Color = builder.Color;
            Footer = builder.Footer?.Build();
            Author = builder.Author?.Build();
            Fields = builder.Fields.Select(x => x.Build()).ToImmutableArray();
        }
    }
}
