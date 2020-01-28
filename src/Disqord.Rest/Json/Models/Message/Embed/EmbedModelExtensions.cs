using System.Linq;
using Disqord.Collections;

namespace Disqord.Models
{
    internal static partial class ModelExtensions
    {
        public static EmbedProvider ToProvider(this EmbedProviderModel model)
            => model == null ? null : new EmbedProvider
            {
                Name = model.Name,
                Url = model.Url
            };

        public static EmbedField ToField(this EmbedFieldModel model)
            => model == null ? null : new EmbedField
            {
                Name = model.Name,
                Value = model.Value,
                IsInline = model.Inline
            };

        public static EmbedFieldModel ToModel(this EmbedField field)
            => field == null ? null : new EmbedFieldModel
            {
                Name = field.Name,
                Value = field.Value,
                Inline = field.IsInline
            };

        public static EmbedVideo ToVideo(this EmbedVideoModel model)
            => model == null ? null : new EmbedVideo
            {
                Url = model.Url,
                Height = model.Height,
                Width = model.Width
            };

        public static EmbedAuthorModel ToModel(this EmbedAuthor author)
            => author == null ? null : new EmbedAuthorModel
            {
                Name = author.Name,
                Url = author.Url,
                IconUrl = author.IconUrl
            };

        public static EmbedAuthor ToAuthor(this EmbedAuthorModel model)
            => model == null ? null : new EmbedAuthor
            {
                Name = model.Name,
                Url = model.Url,
                IconUrl = model.IconUrl,
                ProxyIconUrl = model.ProxyIconUrl
            };

        public static EmbedFooterModel ToModel(this EmbedFooter footer)
            => footer == null ? null : new EmbedFooterModel
            {
                Text = footer.Text,
                IconUrl = footer.IconUrl
            };

        public static EmbedFooter ToFooter(this EmbedFooterModel model)
            => model == null ? null : new EmbedFooter
            {
                Text = model.Text,
                IconUrl = model.IconUrl,
                ProxyIconUrl = model.ProxyIconUrl,
            };

        public static EmbedThumbnailModel ToModel(this EmbedThumbnail thumbnail)
            => thumbnail == null ? null : new EmbedThumbnailModel
            {
                Url = thumbnail.Url
            };

        public static EmbedThumbnail ToThumbnail(this EmbedThumbnailModel model)
            => model == null ? null : new EmbedThumbnail
            {
                Url = model.Url,
                ProxyUrl = model.ProxyUrl,
                Height = model.Height,
                Width = model.Width
            };

        public static EmbedImageModel ToModel(this EmbedImage image)
            => image == null ? null : new EmbedImageModel
            {
                Url = image.Url
            };

        public static EmbedImage ToImage(this EmbedImageModel model)
            => model == null ? null : new EmbedImage
            {
                Url = model.Url,
                ProxyUrl = model.ProxyUrl,
                Height = model.Height,
                Width = model.Width
            };

        public static EmbedModel ToModel(this LocalEmbed embed)
            => embed == null ? null : new EmbedModel
            {
                Title = embed.Title,
                Type = "rich",
                Description = embed.Description,
                Url = embed.Url,
                Timestamp = embed.Timestamp,
                Color = embed.Color,
                Image = embed.ImageUrl == null ? null
                    : new EmbedImageModel
                    {
                        Url = embed.ImageUrl
                    },
                Thumbnail = embed.ThumbnailUrl == null ? null
                    : new EmbedThumbnailModel
                    {
                        Url = embed.ThumbnailUrl
                    },
                Footer = embed.Footer == null ? null
                    : new EmbedFooterModel
                    {
                        Text = embed.Footer.Text,
                        IconUrl = embed.Footer.IconUrl
                    },
                Author = embed.Author == null ? null
                    : new EmbedAuthorModel
                    {
                        Name = embed.Author.Name,
                        Url = embed.Author.Url,
                        IconUrl = embed.Author.IconUrl
                    },
                Fields = embed.Fields.Select(x => new EmbedFieldModel
                {
                    Name = x.Name,
                    Value = x.Value,
                    Inline = x.IsInline
                }).ToArray()
            };

        public static Embed ToEmbed(this EmbedModel model)
            => model == null ? null : new Embed
            {
                Title = model.Title,
                Type = model.Type,
                Description = model.Description,
                Url = model.Url,
                Timestamp = model.Timestamp,
                Color = model.Color,
                Image = model.Image.ToImage(),
                Thumbnail = model.Thumbnail.ToThumbnail(),
                Footer = model.Footer.ToFooter(),
                Author = model.Author.ToAuthor(),
                Video = model.Video.ToVideo(),
                Fields = model.Fields.ToReadOnlyList(ToField),
                Provider = model.Provider.ToProvider(),
            };
    }
}
