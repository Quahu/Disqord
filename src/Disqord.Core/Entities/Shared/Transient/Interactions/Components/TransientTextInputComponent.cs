using Disqord.Models;

namespace Disqord
{
    /// <inheritdoc cref="ITextInputComponent"/>
    public class TransientTextInputComponent : TransientComponent, ITextInputComponent
    {
        /// <inheritdoc/>
        public string CustomId => Model.CustomId.Value;

        /// <inheritdoc/>
        public TextInputComponentStyle ComponentStyle => (TextInputComponentStyle) Model.Style.Value;

        /// <inheritdoc/>
        public string Label => Model.Label.Value;

        /// <inheritdoc/>
        public int? MinimumLength => Model.MinLength.GetValueOrNullable();

        /// <inheritdoc/>
        public int? MaximumLength => Model.MaxLength.GetValueOrNullable();

        /// <inheritdoc/>
        public bool IsRequired => Model.Required.GetValueOrDefault(true);

        /// <inheritdoc/>
        public string Value => Model.Value.GetValueOrDefault();

        /// <inheritdoc/>
        public string Placeholder => Model.Placeholder.GetValueOrDefault();

        public TransientTextInputComponent(IClient client, ComponentJsonModel model)
            : base(client, model)
        { }
    }
}
