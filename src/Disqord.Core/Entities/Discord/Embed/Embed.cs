using System;
using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public class Embed
    {
        public string? Title { get; }

        public string? Type { get; }

        public string? Description { get; }

        public string? Url { get; }

        public DateTimeOffset? Timestamp { get; }

        public Color? Color { get; }

        public EmbedImage? Image { get; }

        public EmbedThumbnail? Thumbnail { get; }

        public EmbedVideo? Video { get; }

        public EmbedProvider? Provider { get; }

        public EmbedFooter? Footer { get; }

        public EmbedAuthor? Author { get; }

        public IReadOnlyList<EmbedField>? Fields { get; }

        /// <summary>
        ///     Checks if <see cref="Type"/> is <c>rich</c>.
        /// </summary>
        public bool IsRich => Type == "rich";

        public Embed(EmbedJsonModel model)
        {
            Title = model.Title.GetValueOrDefault();
            Type = model.Type.GetValueOrDefault();
            Description = model.Description.GetValueOrDefault();
            Url = model.Url.GetValueOrDefault();
            Timestamp = model.Timestamp.GetValueOrNullable();
            Color = model.Color.GetValueOrNullable();
            Image = Optional.ConvertOrDefault(model.Image, x => new EmbedImage(x));
            Video = Optional.ConvertOrDefault(model.Video, x => new EmbedVideo(x));
            Provider = Optional.ConvertOrDefault(model.Provider, x => new EmbedProvider(x));
            Footer = Optional.ConvertOrDefault(model.Footer, x => new EmbedFooter(x));
            Author = Optional.ConvertOrDefault(model.Author, x => new EmbedAuthor(x));
            Fields = Optional.ConvertOrDefault(model.Fields, x => x.ToReadOnlyList(y => new EmbedField(y)));
        }
    }
}
