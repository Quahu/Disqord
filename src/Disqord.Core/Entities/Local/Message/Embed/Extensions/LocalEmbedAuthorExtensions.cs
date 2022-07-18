using System.ComponentModel;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalEmbedAuthorExtensions
{
    public static TEmbedAuthor WithName<TEmbedAuthor>(this TEmbedAuthor author, string name)
        where TEmbedAuthor : LocalEmbedAuthor
    {
        author.Name = name;
        return author;
    }

    public static TEmbedAuthor WithUrl<TEmbedAuthor>(this TEmbedAuthor author, string url)
        where TEmbedAuthor : LocalEmbedAuthor
    {
        author.Url = url;
        return author;
    }

    public static TEmbedAuthor WithIconUrl<TEmbedAuthor>(this TEmbedAuthor author, string iconUrl)
        where TEmbedAuthor : LocalEmbedAuthor
    {
        author.IconUrl = iconUrl;
        return author;
    }
}
