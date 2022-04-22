using Disqord.Models;
using Qommon;

namespace Disqord
{
    public class TransientEmbedProvider : TransientEntity<EmbedProviderJsonModel>, IEmbedProvider
    {
        /// <inheritdoc/>
        public string Name => Model.Name.GetValueOrDefault();

        /// <inheritdoc/>
        public string Url => Model.Url.GetValueOrDefault();

        public TransientEmbedProvider(EmbedProviderJsonModel model)
            : base(model)
        { }
    }
}
