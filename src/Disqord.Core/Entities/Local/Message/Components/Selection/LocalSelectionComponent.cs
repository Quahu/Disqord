using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public class LocalSelectionComponent : LocalNestedComponent, ILocalInteractiveComponent
    {
        public const int MaxPlaceholderLength = 100;

        public const int MaxOptionsAmount = 25;

        public string CustomId
        {
            get => _customId;
            set
            {
                if (value != null && value.Length > ILocalInteractiveComponent.MaxCustomIdLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The selection's custom ID must not be longer than {ILocalInteractiveComponent.MaxCustomIdLength} characters.");

                _customId = value;
            }
        }
        private string _customId;

        public string Placeholder
        {
            get => _placeholder;
            set
            {
                if (value != null && value.Length > MaxPlaceholderLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The selection's placeholder must not be longer than {MaxPlaceholderLength} characters.");

                _placeholder = value;
            }
        }
        private string _placeholder;

        public int? MinimumSelectedOptions { get; set; }

        public int? MaximumSelectedOptions { get; set; }

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

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(CustomId))
                throw new InvalidOperationException("The selection's custom ID must be set.");

            if (_options.Count > MaxOptionsAmount)
                throw new InvalidOperationException($"The selection must not have more than {MaxOptionsAmount} options.");
        }
    }
}
