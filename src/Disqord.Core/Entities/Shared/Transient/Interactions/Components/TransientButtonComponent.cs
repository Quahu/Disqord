using Disqord.Models;
using Qommon;

namespace Disqord
{
    /// <inheritdoc cref="IButtonComponent"/>
    public class TransientButtonComponent : TransientComponent, IButtonComponent
    {
        /// <inheritdoc/>
        public string CustomId => Model.CustomId.Value;

        /// <inheritdoc/>
        public ButtonComponentStyle Style => (ButtonComponentStyle) Model.Style.Value;

        /// <inheritdoc/>
        public string Label => Model.Label.GetValueOrDefault();

        /// <inheritdoc/>
        public IEmoji Emoji => Optional.ConvertOrDefault(Model.Emoji, TransientEmoji.Create);

        /// <inheritdoc/>
        public string Url => Model.Url.GetValueOrDefault();

        /// <inheritdoc/>
        public bool IsDisabled => Model.Disabled.GetValueOrDefault();

        public TransientButtonComponent(IClient client, ComponentJsonModel model)
            : base(client, model)
        { }
    }
}
