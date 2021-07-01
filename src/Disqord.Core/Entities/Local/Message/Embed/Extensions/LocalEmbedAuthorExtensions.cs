namespace Disqord
{
    public static class LocalEmbedAuthorExtensions
    {
        public static LocalEmbedAuthor WithName(this LocalEmbedAuthor author, string name)
        {
            author.Name = name;
            return author;
        }

        public static LocalEmbedAuthor WithUrl(this LocalEmbedAuthor author, string url)
        {
            author.Url = url;
            return author;
        }

        public static LocalEmbedAuthor WithIconUrl(this LocalEmbedAuthor author, string iconUrl)
        {
            author.IconUrl = iconUrl;
            return author;
        }
    }
}
