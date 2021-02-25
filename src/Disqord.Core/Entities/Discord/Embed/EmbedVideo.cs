using System.Drawing;
using Disqord.Models;

namespace Disqord
{
    public sealed class EmbedVideo
    {
        public string? Url { get; }

        public Size Size { get; }

        public EmbedVideo(EmbedVideoJsonModel model)
        {
            Url = model.Url.GetValueOrDefault();
            Size = new Size(model.Width.GetValueOrDefault(), model.Height.GetValueOrDefault());
        }
    }
}
