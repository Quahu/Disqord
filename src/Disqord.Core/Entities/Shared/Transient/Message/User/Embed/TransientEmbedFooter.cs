using Disqord.Models;
using Qommon;

namespace Disqord
{
    public class TransientEmbedFooter : TransientEntity<EmbedFooterJsonModel>, IEmbedFooter
    {
        /// <inheritdoc/>
        public string Text => Model.Text;

        /// <inheritdoc/>
        public string IconUrl => Model.IconUrl.GetValueOrDefault();

        /// <inheritdoc/>
        public string ProxyIconUrl => Model.ProxyIconUrl.GetValueOrDefault();

        public TransientEmbedFooter(EmbedFooterJsonModel model)
            : base(model)
        { }
    }
}
