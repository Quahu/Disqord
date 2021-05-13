using System.Drawing;
using Disqord.Models;

namespace Disqord
{
    public sealed class Attachment : IIdentifiable
    {
        public Snowflake Id { get; }

        public string Filename { get; }

        public int Filesize { get; }

        public string Url { get; }

        public string ProxyUrl { get; }

        public Size? Size { get; }

        public Attachment(AttachmentJsonModel model)
        {
            Id = model.Id;
            Filename = model.Filename;
            Filesize = model.Size;
            Url = model.Url;
            ProxyUrl = model.ProxyUrl;
            Size = model.Width.HasValue && model.Height.HasValue
                ? new Size(model.Width.Value.GetValueOrDefault(), model.Height.Value.GetValueOrDefault())
                : null;
        }

        public override string ToString()
            => $"Attachment {Filename} ({Id})";
    }
}
