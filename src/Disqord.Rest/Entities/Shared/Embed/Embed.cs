using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Disqord
{
    public sealed class Embed
    {
        public string Title { get; internal set; }

        public string Type { get; internal set; }

        public string Description { get; internal set; }

        public string Url { get; internal set; }

        public DateTimeOffset? Timestamp { get; internal set; }

        public Color? Color { get; internal set; }

        public EmbedImage Image { get; internal set; }

        public EmbedThumbnail Thumbnail { get; internal set; }

        public EmbedVideo Video { get; internal set; }

        public EmbedProvider Provider { get; internal set; }

        public EmbedFooter Footer { get; internal set; }

        public EmbedAuthor Author { get; internal set; }

        public IReadOnlyList<EmbedField> Fields { get; internal set; }

        internal Embed()
        { }

        internal Embed(EmbedBuilder builder)
        {
            Title = builder.Title;
            Type = "rich";
            Description = builder.Description;
            Url = builder.Url;
            Image = builder.ImageUrl == null ? null : new EmbedImage { Url = builder.ImageUrl };
            Thumbnail = builder.ThumbnailUrl == null ? null : new EmbedThumbnail { Url = builder.ThumbnailUrl };
            Timestamp = builder.Timestamp;
            Color = builder.Color;
            Footer = builder.Footer?.Build();
            Author = builder.Author?.Build();
            Fields = builder.Fields?.Select(x => x.Build()).ToImmutableArray() ?? ImmutableArray<EmbedField>.Empty;
        }
    }
}
