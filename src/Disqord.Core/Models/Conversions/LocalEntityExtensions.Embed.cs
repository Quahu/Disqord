using System.Linq;
using Qommon;

namespace Disqord.Models
{
    public static partial class LocalEntityExtensions
    {
        public static EmbedFieldJsonModel ToModel(this LocalEmbedField field)
            => field == null ? null : new EmbedFieldJsonModel
            {
                Name = field.Name,
                Value = field.Value,
                Inline = field.IsInline
            };

        public static EmbedAuthorJsonModel ToModel(this LocalEmbedAuthor author)
            => author == null ? null : new EmbedAuthorJsonModel
            {
                Name = author.Name,
                Url = author.Url,
                IconUrl = author.IconUrl
            };

        public static EmbedFooterJsonModel ToModel(this LocalEmbedFooter footer)
            => footer == null ? null : new EmbedFooterJsonModel
            {
                Text = footer.Text,
                IconUrl = footer.IconUrl
            };

        public static EmbedJsonModel ToModel(this LocalEmbed embed)
            => embed == null ? null : new EmbedJsonModel
            {
                Title = embed.Title,
                Type = "rich",
                Description = embed.Description,
                Url = embed.Url,
                Timestamp = Optional.FromNullable(embed.Timestamp),
                Color = Optional.FromNullable(embed.Color?.RawValue),
                Image = embed.ImageUrl == null ? null
                    : new EmbedImageJsonModel
                    {
                        Url = embed.ImageUrl
                    },
                Thumbnail = embed.ThumbnailUrl == null ? null
                    : new EmbedThumbnailJsonModel
                    {
                        Url = embed.ThumbnailUrl
                    },
                Footer = embed.Footer == null ? null
                    : new EmbedFooterJsonModel
                    {
                        Text = embed.Footer.Text,
                        IconUrl = embed.Footer.IconUrl
                    },
                Author = embed.Author == null ? null
                    : new EmbedAuthorJsonModel
                    {
                        Name = embed.Author.Name,
                        Url = embed.Author.Url,
                        IconUrl = embed.Author.IconUrl
                    },
                Fields = embed.Fields.Select(x => new EmbedFieldJsonModel
                {
                    Name = x.Name,
                    Value = x.Value,
                    Inline = x.IsInline
                }).ToArray()
            };
    }
}
