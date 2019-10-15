namespace Disqord
{
    public sealed class EmbedAuthor
    {
        public string Name { get; internal set; }

        public string Url { get; internal set; }

        public string IconUrl { get; internal set; }

        public string ProxyIconUrl { get; internal set; }

        internal EmbedAuthor()
        { }

        internal EmbedAuthor(EmbedAuthorBuilder builder)
        {
            Name = builder.Name;
            Url = builder.Url;
            IconUrl = builder.IconUrl;
        }
    }
}
