using Disqord.Models;

namespace Disqord
{
    /// <inheritdoc cref="IButtonComponent"/>
    public class TransientButtonComponent : TransientComponent, IButtonComponent
    {
        /// <inheritdoc/>
        public ButtonComponentStyle Style => Model.Style.Value;

        /// <inheritdoc/>
        public string Label => Model.Label.Value;

        /// <inheritdoc/>
        public IEmoji Emoji => Optional.ConvertOrDefault(Model.Emoji, Disqord.Emoji.Create);

        /// <inheritdoc/>
        public string CustomId => Model.CustomId.Value;

        /// <inheritdoc/>
        public string Url => Model.Url.GetValueOrDefault();

        /// <inheritdoc/>
        public bool IsDisabled => Model.Disabled.GetValueOrDefault();

        public TransientButtonComponent(IClient client, ComponentJsonModel model)
            : base(client, model)
        { }
    }
}
