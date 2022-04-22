using System.Collections.Generic;
using System.Linq;
using Qommon;

namespace Disqord
{
    public class LocalSelectionComponent : LocalComponent, ILocalCustomIdentifiableEntity
    {
        /// <inheritdoc/>
        public Optional<string> CustomId { get; set; }

        /// <summary>
        ///     Gets or sets the placeholder of this selection.
        /// </summary>
        public Optional<string> Placeholder { get; set; }

        /// <summary>
        ///     Gets or sets the minimum amount of options of this selection.
        /// </summary>
        public Optional<int> MinimumSelectedOptions { get; set; }

        /// <summary>
        ///     Gets or sets the maximum amount of options of this selection.
        /// </summary>
        public Optional<int> MaximumSelectedOptions { get; set; }

        /// <summary>
        ///     Gets or sets whether this selection is disabled.
        /// </summary>
        public Optional<bool> IsDisabled { get; set; }

        /// <summary>
        ///     Gets or sets the options of this selection.
        /// </summary>
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
