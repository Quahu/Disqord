using System;
using System.Threading.Tasks;
using Disqord.Gateway;
using Qommon;

namespace Disqord.Extensions.Interactivity.Menus
{
    /// <summary>
    ///     Represents a method that executes when the given <see cref="ButtonViewComponent"/> is triggered.
    /// </summary>
    /// <param name="e"> The reaction event data. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the callback work.
    /// </returns>
    public delegate ValueTask ButtonViewComponentCallback(ButtonEventArgs e);

    public class ButtonViewComponent : InteractableViewComponent
    {
        public string Label
        {
            get => _label;
            set
            {
                _label = value;
                ReportChanges();
            }
        }
        private string _label;

        public LocalButtonComponentStyle Style
        {
            get => _style;
            set
            {
                _style = value;
                ReportChanges();
            }
        }
        private LocalButtonComponentStyle _style = LocalButtonComponentStyle.Primary;

        public LocalEmoji Emoji
        {
            get => _emoji;
            set
            {
                _emoji = value;
                ReportChanges();
            }
        }
        private LocalEmoji _emoji;

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

        private readonly ButtonViewComponentCallback _callback;

        public override int Width => 1;

        public ButtonViewComponent(ButtonViewComponentCallback callback)
        {
            _callback = callback;
        }

        internal ButtonViewComponent(ButtonAttribute attribute, ButtonViewComponentCallback callback)
            : base(attribute)
        {
            _callback = callback;
            _label = attribute.Label;
            _style = attribute.Style;
            _emoji = attribute.Emoji is string emojiString
                ? LocalEmoji.FromString(emojiString)
                : attribute.Emoji != null
                    ? LocalEmoji.Custom(Convert.ToUInt64(attribute.Emoji))
                    : null;

            _isDisabled = attribute.IsDisabled;
        }

        protected internal override ValueTask ExecuteAsync(InteractionReceivedEventArgs e)
        {
            var data = new ButtonEventArgs(this, e);
            return _callback(data);
        }

        protected internal override LocalComponent ToLocalComponent()
        {
            var button = new LocalButtonComponent()
                .WithCustomId(CustomId)
                .WithStyle(_style)
                .WithIsDisabled(_isDisabled);

            button.Label = Optional.FromNullable(_label);
            button.Emoji = Optional.FromNullable(_emoji);

            return button;
        }
    }
}
