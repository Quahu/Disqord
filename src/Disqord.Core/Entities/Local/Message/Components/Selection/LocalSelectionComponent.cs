using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public class LocalSelectionComponent : LocalComponent, ILocalInteractiveComponent
    {
        /// <inheritdoc/>
        public string CustomId { get; set; }

        public Optional<string> Placeholder { get; set; }

        public Optional<int> MinimumSelectedOptions { get; set; }

        public Optional<int> MaximumSelectedOptions { get; set; }

        /// <summary>
        ///     Gets or sets whether this interactive component is disabled.
        /// </summary>
        public Optional<bool> IsDisabled { get; set; }

        public IList<LocalSelectionComponentOption> Options
        {
            get => _options;
            set => this.WithOptions(value);
        }
        internal readonly List<LocalSelectionComponentOption> _options;

        public LocalSelectionComponent()
        {
            _options = new List<LocalSelectionComponentOption>();
        }

        protected LocalSelectionComponent(LocalSelectionComponent other)
        {
            CustomId = other.CustomId;
            Placeholder = other.Placeholder;
            MinimumSelectedOptions = other.MinimumSelectedOptions;
            MaximumSelectedOptions = other.MaximumSelectedOptions;
            _options = other._options.Select(x => x.Clone()).ToList();
        }

        public override LocalSelectionComponent Clone()
            => new(this);
    }
}
