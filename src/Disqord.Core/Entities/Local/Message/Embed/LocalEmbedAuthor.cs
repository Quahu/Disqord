using System;

namespace Disqord
{
    public class LocalEmbedAuthor : ILocalConstruct
    {
        public const int MAX_AUTHOR_NAME_LENGTH = 256;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value), "The embed author's name must not be null or whitespace.");

                if (value.Length > MAX_AUTHOR_NAME_LENGTH)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The name of the embed author must not be longer than {MAX_AUTHOR_NAME_LENGTH} characters.");

                _name = value;
            }
        }
        private string _name;

        public string Url { get; set; }

        public string IconUrl { get; set; }

        public int Length => _name?.Length ?? 0;

        public LocalEmbedAuthor()
        { }

        private LocalEmbedAuthor(LocalEmbedAuthor other)
        {
            _name = other.Name;
            Url = other.Url;
            IconUrl = other.IconUrl;
        }

        public LocalEmbedAuthor WithName(string name)
        {
            Name = name;
            return this;
        }

        public LocalEmbedAuthor WithUrl(string url)
        {
            Url = url;
            return this;
        }

        public LocalEmbedAuthor WithIconUrl(string iconUrl)
        {
            IconUrl = iconUrl;
            return this;
        }

        public LocalEmbedAuthor Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();

        public void Validate()
        {
            if (_name == null)
                throw new InvalidOperationException("The embed author's name must be set.");
        }
    }
}
