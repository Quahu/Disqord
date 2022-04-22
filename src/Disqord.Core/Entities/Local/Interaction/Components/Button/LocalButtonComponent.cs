using Qommon;

namespace Disqord
{
    public class LocalButtonComponent : LocalButtonComponentBase, ILocalCustomIdentifiableEntity
    {
        /// <summary>
        ///     Gets or sets the style of this button.
        /// </summary>
        /// <remarks>
        ///     This property is required.
        /// </remarks>
        public Optional<LocalButtonComponentStyle> Style { get; set; } = LocalButtonComponentStyle.Primary;

        /// <inheritdoc/>
        public Optional<string> CustomId { get; set; }

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
    }
}
