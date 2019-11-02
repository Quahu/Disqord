using System;
using System.Collections.Generic;

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

        /// <summary>
        ///     Checks if <see cref="Type"/> equals 'rich'.
        /// </summary>
        public bool IsRich => Type == "rich";

        internal Embed()
        { }
    }
}
