using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Gateway;

namespace Disqord.Extensions.Interactivity.Menus
{
    /// <summary>
    ///     Represents a method that executes when the given <see cref="SelectionViewComponent"/> is triggered.
    /// </summary>
    /// <param name="e"> The reaction event data. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the callback work.
    /// </returns>
    public delegate ValueTask SelectionViewComponentCallback(SelectionEventArgs e);

    public class SelectionViewComponent : InteractableViewComponent
    {
        public string Placeholder
        {
            get => _placeholder;
            set
            {
                ReportChanges();
                _placeholder = value;
            }
        }
        private string _placeholder;

        public int? MinimumSelectedOptions
        {
            get => _minimumSelectedOptions;
            set
            {
                ReportChanges();
                _minimumSelectedOptions = value;
            }
        }
        private int? _minimumSelectedOptions;

        public int? MaximumSelectedOptions
        {
            get => _maximumSelectedOptions;
            set
            {
                ReportChanges();
                _maximumSelectedOptions = value;
            }
        }
        private int? _maximumSelectedOptions;

        public IEnumerable<LocalSelectionComponentOption> Options
        {
            get => _options;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                ReportChanges();
                _options = value;
            }
        }
        private IEnumerable<LocalSelectionComponentOption> _options = Array.Empty<LocalSelectionComponentOption>();

        public bool IsDisabled
        {
            get => _isDisabled;
            set
            {
                ReportChanges();
                _isDisabled = value;
            }
        }
        private bool _isDisabled;

        private readonly SelectionViewComponentCallback _callback;

        public override int Width => 1;

        public SelectionViewComponent(SelectionViewComponentCallback callback)
        {
            _callback = callback;
        }

        internal SelectionViewComponent(SelectionAttribute attribute, SelectionOptionAttribute[] optionAttributes, SelectionViewComponentCallback callback)
            : base(attribute)
        {
            _callback = callback;
            _placeholder = attribute.Placeholder;
            _minimumSelectedOptions = attribute.MinimumSelectedOptions != -1
                ? attribute.MinimumSelectedOptions
                : null;
            _maximumSelectedOptions = attribute.MaximumSelectedOptions != -1
                ? attribute.MaximumSelectedOptions
                : null;
            _isDisabled = attribute.IsDisabled;
            _options = Array.ConvertAll(optionAttributes, optionAttribute => new LocalSelectionComponentOption
            {
                Label = optionAttribute.Label,
                Value = optionAttribute.Value ?? optionAttribute.Label,
                Description = optionAttribute.Description,
                Emoji = optionAttribute.Emoji is string emojiString
                    ? LocalEmoji.FromString(emojiString)
                    : optionAttribute.Emoji != null
                        ? LocalEmoji.Custom(Convert.ToUInt64(optionAttribute.Emoji))
                        : null,
                IsDefault = optionAttribute.IsDefault
            });
        }

        protected internal override ValueTask ExecuteAsync(InteractionReceivedEventArgs e)
        {
            var data = new SelectionEventArgs(this, e);
            return _callback(data);
        }

        protected internal override LocalNestedComponent ToLocalComponent()
            => LocalComponent.Selection(CustomId)
                .WithPlaceholder(_placeholder)
                .WithMinimumSelectedOptions(_minimumSelectedOptions)
                .WithMaximumSelectedOptions(_maximumSelectedOptions)
                .WithOptions(_options)
                .WithIsDisabled(_isDisabled);
    }
}
