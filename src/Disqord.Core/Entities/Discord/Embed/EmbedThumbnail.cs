using System.Drawing;
using Disqord.Models;

namespace Disqord
{
    public sealed class EmbedThumbnail
    {
        public string? Url { get; }

        public string? ProxyUrl { get; }

        public Size Size { get; }

        public EmbedThumbnail(EmbedThumbnailJsonModel model)
        {
            Url = model.Url.GetValueOrDefault();
            ProxyUrl = model.ProxyUrl.GetValueOrDefault();
            Size = new Size(model.Width.GetValueOrDefault(), model.Height.GetValueOrDefault());
        }
    }
}
