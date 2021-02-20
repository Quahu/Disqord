namespace Disqord
{
    public sealed class LocalEmbedAuthor
    {
        public string Name { get; }

        public string Url { get; }

        public string IconUrl { get; }

        internal LocalEmbedAuthor(LocalEmbedAuthorBuilder builder)
        {
            Name = builder.Name;
            Url = builder.Url;
            IconUrl = builder.IconUrl;
        }
    }
}
