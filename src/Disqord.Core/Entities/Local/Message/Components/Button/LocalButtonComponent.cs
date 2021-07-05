using System;

namespace Disqord
{
    public class LocalButtonComponent : LocalButtonComponentBase, ILocalInteractiveComponent
    {
        /// <summary>
        ///     Gets or sets the style of this button.
        /// </summary>
        public LocalButtonComponentStyle Style { get; set; } = LocalButtonComponentStyle.Primary;

        /// <inheritdoc/>
        public string CustomId { get; set; }

        public LocalButtonComponent()
        { }

        private LocalButtonComponent(LocalButtonComponent other)
            : base(other)
        {
            Style = other.Style;
            CustomId = other.CustomId;
        }

        public override LocalButtonComponent Clone()
            => new(this);

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(CustomId))
                throw new InvalidOperationException("The button's custom ID must be set.");

            base.Validate();
        }
    }
}
