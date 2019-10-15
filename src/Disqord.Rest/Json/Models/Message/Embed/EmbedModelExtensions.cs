using System.Collections.Immutable;
using System.Linq;

namespace Disqord
{
    internal static partial class ModelExtensions
    {
        public static EmbedProvider ToProvider(this Models.EmbedProviderModel model)
            => model == null ? null : new EmbedProvider
            {
                Name = model.Name,
                Url = model.Url
            };

        public static EmbedField ToField(this Models.EmbedFieldModel model)
            => model == null ? null : new EmbedField
            {
                Name = model.Name,
                Value = model.Value,
                IsInline = model.Inline
            };

        public static Models.EmbedFieldModel ToModel(this EmbedField field)
            => field == null ? null : new Models.EmbedFieldModel
            {
                Name = field.Name,
                Value = field.Value,
                Inline = field.IsInline
            };

        public static EmbedVideo ToVideo(this Models.EmbedVideoModel model)
            => model == null ? null : new EmbedVideo
            {
                Url = model.Url,
                Height = model.Height,
                Width = model.Width
            };

        public static Models.EmbedAuthorModel ToModel(this EmbedAuthor author)
            => author == null ? null : new Models.EmbedAuthorModel
            {
                Name = author.Name,
                Url = author.Url,
                IconUrl = author.IconUrl
            };

        public static EmbedAuthor ToAuthor(this Models.EmbedAuthorModel model)
            => model == null ? null : new EmbedAuthor
            {
                Name = model.Name,
                Url = model.Url,
                IconUrl = model.IconUrl,
                ProxyIconUrl = model.ProxyIconUrl
            };

        public static Models.EmbedFooterModel ToModel(this EmbedFooter footer)
            => footer == null ? null : new Models.EmbedFooterModel
            {
                Text = footer.Text,
                IconUrl = footer.IconUrl
            };

        public static EmbedFooter ToFooter(this Models.EmbedFooterModel model)
            => model == null ? null : new EmbedFooter
            {
                Text = model.Text,
                IconUrl = model.IconUrl,
                ProxyIconUrl = model.ProxyIconUrl,
            };

        public static Models.EmbedThumbnailModel ToModel(this EmbedThumbnail thumbnail)
            => thumbnail == null ? null : new Models.EmbedThumbnailModel
            {
                Url = thumbnail.Url
            };

        public static EmbedThumbnail ToThumbnail(this Models.EmbedThumbnailModel model)
            => model == null ? null : new EmbedThumbnail
            {
                Url = model.Url,
                ProxyUrl = model.ProxyUrl,
                Height = model.Height,
                Width = model.Width
            };

        public static Models.EmbedImageModel ToModel(this EmbedImage image)
            => image == null ? null : new Models.EmbedImageModel
            {
                Url = image.Url
            };

        public static EmbedImage ToImage(this Models.EmbedImageModel model)
            => model == null ? null : new EmbedImage
            {
                Url = model.Url,
                ProxyUrl = model.ProxyUrl,
                Height = model.Height,
                Width = model.Width
            };

        public static Models.EmbedModel ToModel(this Embed embed)
            => embed == null ? null : new Models.EmbedModel
            {
                Title = embed.Title,
                Type = embed.Type,
                Description = embed.Description,
                Url = embed.Url,
                Timestamp = embed.Timestamp,
                Color = embed.Color,
                Image = embed.Image.ToModel(),
                Thumbnail = embed.Thumbnail.ToModel(),
                Footer = embed.Footer.ToModel(),
                Author = embed.Author.ToModel(),
                Fields = embed.Fields.Select(x => x.ToModel()).ToArray()
            };

        public static Embed ToEmbed(this Models.EmbedModel model)
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
                Fields = model.Fields.Select(ToField).ToImmutableArray(),
                Provider = model.Provider.ToProvider(),
            };
    }
}
