﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Gateway;
using Qommon;

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
                _placeholder = value;
                ReportChanges();
            }
        }
        private string _placeholder;

        public int? MinimumSelectedOptions
        {
            get => _minimumSelectedOptions;
            set
            {
                _minimumSelectedOptions = value;
                ReportChanges();
            }
        }
        private int? _minimumSelectedOptions;

        public int? MaximumSelectedOptions
        {
            get => _maximumSelectedOptions;
            set
            {
                _maximumSelectedOptions = value;
                ReportChanges();
            }
        }
        private int? _maximumSelectedOptions;

        /// <summary>
        ///     Gets or sets the options.
        /// </summary>
        /// <remarks>
        ///     If the collection is updated directly, ensure <see cref="ViewBase.ReportChanges"/> is called if an update is expected.
        /// </remarks>
        /// <exception cref="ArgumentNullException"> Thrown when setting a null value. </exception>
        public IList<LocalSelectionComponentOption> Options
        {
            get => _options;
            set
            {
                Guard.IsNotNull(value);

                _options.Clear();
                _options.AddRange(value);
                ReportChanges();
            }
        }
        private readonly List<LocalSelectionComponentOption> _options;

        public bool IsDisabled
        {
            get => _isDisabled;
            set
            {
                _isDisabled = value;
                ReportChanges();
            }
        }
        private bool _isDisabled;

        private readonly SelectionViewComponentCallback _callback;

        public override int Width => 5;

        public SelectionViewComponent(SelectionViewComponentCallback callback)
        {
            _callback = callback;
            _options = new List<LocalSelectionComponentOption>();
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
            _options = new List<LocalSelectionComponentOption>(optionAttributes.Length);
            foreach (var optionAttribute in optionAttributes)
            {
                _options.Add(new LocalSelectionComponentOption
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
        }

        protected internal override ValueTask ExecuteAsync(InteractionReceivedEventArgs e)
        {
            var data = new SelectionEventArgs(this, e);
            return _callback(data);
        }

        protected internal override LocalComponent ToLocalComponent()
        {
            var selection = LocalComponent.Selection(CustomId)
                .WithOptions(_options)
                .WithIsDisabled(_isDisabled);

            selection.Placeholder = Optional.FromNullable(_placeholder);
            selection.MinimumSelectedOptions = Optional.FromNullable(_minimumSelectedOptions);
            selection.MaximumSelectedOptions = Optional.FromNullable(_maximumSelectedOptions);

            return selection;
        }
    }
}
