using System;

namespace Disqord
{
    public class LocalEmbedFooter : ILocalConstruct
    {
        public const int MaxTextLength = 2048;

        public string Text
        {
            get => _text;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value), "The embed footer's text must not be null or whitespace.");

                if (value.Length > MaxTextLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The embed footer's text must not be longer than {MaxTextLength} characters.");

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

        public virtual LocalEmbedFooter Clone()
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
