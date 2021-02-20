using System;

namespace Disqord
{
    public sealed class LocalEmbedFooterBuilder : ICloneable
    {
        public const int MAX_FOOTER_LENGTH = 2048;

        public string Text
        {
            get => _text;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value), "The embed footer's text must not be null or whitespace.");

                if (value.Length > MAX_FOOTER_LENGTH)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The embed footer's text must not be longer than {MAX_FOOTER_LENGTH} characters.");

                _text = value;
            }
        }
        private string _text;

        public string IconUrl { get; set; }

        public int Length => _text?.Length ?? 0;

        public LocalEmbedFooterBuilder()
        { }

        internal LocalEmbedFooterBuilder(LocalEmbedFooterBuilder builder)
        {
            _text = builder.Text;
            IconUrl = builder.IconUrl;
        }

        public LocalEmbedFooterBuilder WithText(string text)
        {
            Text = text;
            return this;
        }

        public LocalEmbedFooterBuilder WithIconUrl(string iconUrl)
        {
            IconUrl = iconUrl;
            return this;
        }

        public LocalEmbedFooterBuilder Clone()
            => new LocalEmbedFooterBuilder(this);

        object ICloneable.Clone()
            => Clone();

        internal LocalEmbedFooter Build()
        {
            if (_text == null)
                throw new InvalidOperationException("The embed footer's text must be set.");

            return new LocalEmbedFooter(this);
        }
    }
}
