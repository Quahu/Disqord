using Disqord.Models;
using Qommon;

namespace Disqord
{
    public class TransientEmbedImage : TransientEntity<EmbedImageJsonModel>, IEmbedImage
    {
        /// <inheritdoc/>
        public string Url => Model.Url.GetValueOrDefault();

        /// <inheritdoc/>
        public string ProxyUrl => Model.ProxyUrl.GetValueOrDefault();

        /// <inheritdoc/>
        public int? Width => Model.Width.GetValueOrNullable();

        /// <inheritdoc/>
        public int? Height => Model.Height.GetValueOrNullable();

        public TransientEmbedImage(EmbedImageJsonModel model)
            : base(model)
        { }
    }
}
