using System;

namespace Disqord.Interactivity.Pagination
{
    public class Page
    {
        public string Content { get; }

        public LocalEmbed Embed { get; }

        public Page(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException(nameof(content));

            Content = content;
        }

        public Page(LocalEmbed embed)
        {
            if (embed == null)
                throw new ArgumentNullException(nameof(embed));

            Embed = embed;
        }

        public Page(string content, LocalEmbed embed)
        {
            if (string.IsNullOrWhiteSpace(content) && embed == null)
                throw new ArgumentNullException();

            Content = content;
            Embed = embed;
        }

        public static implicit operator Page(string content)
            => new Page(content);

        public static implicit operator Page(LocalEmbed embed)
            => new Page(embed);

        public static implicit operator Page((string Content, LocalEmbed Embed) tuple)
            => new Page(tuple.Content, tuple.Embed);
    }
}
