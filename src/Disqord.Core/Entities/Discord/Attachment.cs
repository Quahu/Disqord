using Disqord.Models;

namespace Disqord
{
    public sealed class Attachment : IIdentifiable
    {
        public Snowflake Id { get; }

        public string FileName { get; }

        /// <summary>
        ///     Gets the media type of this attachment.
        ///     Returns <see langword="null"/> if unknown.
        /// </summary>
        public string ContentType { get; }

        public int FileSize { get; }

        public string Url { get; }

        public string ProxyUrl { get; }

        public int? Width { get; }

        public int? Height { get; }

        public bool IsEphemeral { get; }

        public Attachment(AttachmentJsonModel model)
        {
            Id = model.Id;
            FileName = model.FileName;
            ContentType = model.ContentType.GetValueOrDefault();
            FileSize = model.Size;
            Url = model.Url;
            ProxyUrl = model.ProxyUrl;
            Width = model.Width.GetValueOrNullable();
            Height = model.Height.GetValueOrNullable();
            IsEphemeral = model.Ephemeral.GetValueOrDefault();
        }

        public override string ToString()
            => $"Attachment {FileName} ({Id})";
    }
}
