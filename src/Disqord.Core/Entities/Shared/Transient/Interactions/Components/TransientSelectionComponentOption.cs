using Disqord.Models;

namespace Disqord
{
    /// <inheritdoc cref="ISelectionComponentOption"/>
    public class TransientSelectionComponentOption : TransientEntity<SelectOptionJsonModel>, ISelectionComponentOption
    {
        /// <inheritdoc/>
        public string Label => Model.Label;

        /// <inheritdoc/>
        public string Value => Model.Value;

        /// <inheritdoc/>
        public string Description => Model.Description.GetValueOrDefault();

        /// <inheritdoc/>
        public IEmoji Emoji => Optional.ConvertOrDefault(Model.Emoji, Disqord.Emoji.Create);

        /// <inheritdoc/>
        public bool IsDefault => Model.Default.GetValueOrDefault();

        public TransientSelectionComponentOption(IClient client, SelectOptionJsonModel model)
            : base(client, model)
        { }
    }
}
