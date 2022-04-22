using Disqord.Models;
using Qommon;

namespace Disqord
{
    public class TransientEmbedField : TransientEntity<EmbedFieldJsonModel>, IEmbedField
    {
        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public string Value => Model.Value;

        /// <inheritdoc/>
        public bool IsInline => Model.Inline.GetValueOrDefault();

        public TransientEmbedField(EmbedFieldJsonModel model)
            : base(model)
        { }
    }
}
