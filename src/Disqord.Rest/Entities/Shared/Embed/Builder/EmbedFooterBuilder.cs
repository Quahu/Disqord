using System;

namespace Disqord
{
    public sealed class EmbedFooterBuilder
    {
        public const int MAX_FOOTER_LENGTH = 2048;

        public string Text
        {
            get => _text;
            set
            {
                if (value?.Length > MAX_FOOTER_LENGTH)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The text of the embed footer must not be longer than {MAX_FOOTER_LENGTH} characters.");

                _text = value;
            }
        }
        private string _text;

        public string IconUrl { get; set; }

        public EmbedFooterBuilder()
        { }

        public EmbedFooterBuilder(EmbedFooterBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            Text = builder.Text;
            IconUrl = builder.IconUrl;
        }

        public EmbedFooterBuilder WithText(string text)
        {
            Text = text;
            return this;
        }

        public EmbedFooterBuilder WithIconUrl(string iconUrl)
        {
            IconUrl = iconUrl;
            return this;
        }

        public EmbedFooter Build()
            => new EmbedFooter(this);
    }
}
