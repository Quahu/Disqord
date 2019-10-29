using System;

namespace Disqord
{
    public sealed class LocalEmbedAuthorBuilder
    {
        public const int MAX_AUTHOR_NAME_LENGTH = 256;

        public string Name
        {
            get => _name;
            set
            {
                if (value != null && value.Length > MAX_AUTHOR_NAME_LENGTH)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The name of the embed author must not be longer than {MAX_AUTHOR_NAME_LENGTH} characters.");

                _name = value;
            }
        }
        private string _name;

        public string Url { get; set; }

        public string IconUrl { get; set; }

        public LocalEmbedAuthorBuilder()
        { }

        public LocalEmbedAuthorBuilder(LocalEmbedAuthorBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            Name = builder.Name;
            Url = builder.Url;
            IconUrl = builder.IconUrl;
        }

        public LocalEmbedAuthorBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public LocalEmbedAuthorBuilder WithUrl(string url)
        {
            Url = url;
            return this;
        }

        public LocalEmbedAuthorBuilder WithIconUrl(string iconUrl)
        {
            IconUrl = iconUrl;
            return this;
        }

        internal LocalEmbedAuthor Build()
            => new LocalEmbedAuthor(this);
    }
}
