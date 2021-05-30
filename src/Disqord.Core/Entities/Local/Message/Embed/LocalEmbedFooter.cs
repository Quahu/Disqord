using System;

namespace Disqord
{
    public class LocalEmbedFooter : ILocalConstruct
    {
        public const int MAX_TEXT_LENGTH = 2048;

        public string Text
        {
            get => _text;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value), "The embed footer's text must not be null or whitespace.");

                if (value.Length > MAX_TEXT_LENGTH)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The embed footer's text must not be longer than {MAX_TEXT_LENGTH} characters.");

                _text = value;
            }
        }
        private string _text;

        public string IconUrl { get; set; }

        public int Length => _text?.Length ?? 0;

        public LocalEmbedFooter()
        { }

        private LocalEmbedFooter(LocalEmbedFooter other)
        {
            _text = other.Text;
            IconUrl = other.IconUrl;
        }

        public LocalEmbedFooter WithText(string text)
        {
            Text = text;
            return this;
        }

        public LocalEmbedFooter WithIconUrl(string iconUrl)
        {
            IconUrl = iconUrl;
            return this;
        }

        public LocalEmbedFooter Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();

        public void Validate()
        {
            if (_text == null)
                throw new InvalidOperationException("The embed footer's text must be set.");
        }
    }
}
