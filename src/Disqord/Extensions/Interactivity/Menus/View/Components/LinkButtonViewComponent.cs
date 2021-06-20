using System;

namespace Disqord.Extensions.Interactivity.Menus
{
    public class LinkButtonViewComponent : ViewComponent
    {
        public string Url
        {
            get => _url;
            set
            {
                ReportChanges();
                _url = value;
            }
        }
        private string _url;

        public string Label
        {
            get => _label;
            set
            {
                ReportChanges();
                _label = value;
            }
        }
        private string _label;

        public LocalEmoji Emoji
        {
            get => _emoji;
            set
            {
                ReportChanges();
                _emoji = value;
            }
        }
        private LocalEmoji _emoji;

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

        public override int Width => 1;

        public LinkButtonViewComponent(string url)
        {
            _url = url;
        }

        internal LinkButtonViewComponent(LinkButtonAttribute attribute, string url)
            : base(attribute)
        {
            _url = url;
            _label = attribute.Label;
            _emoji = attribute.Emoji is string emojiString
                ? LocalEmoji.FromString(emojiString)
                : attribute.Emoji != null
                    ? LocalEmoji.Custom(Convert.ToUInt64(attribute.Emoji))
                    : null;
            _isDisabled = attribute.IsDisabled;
        }

        protected internal override LocalComponent ToLocalComponent()
            => LocalComponent.LinkButton(Url, Label, Emoji, IsDisabled);
    }
}
