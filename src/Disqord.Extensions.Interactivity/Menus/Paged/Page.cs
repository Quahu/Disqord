using System;

namespace Disqord.Extensions.Interactivity.Menus.Paged
{
    public sealed class Page
    {
        public string Content { get; }

        public LocalEmbed Embed { get; }

        public Page(string content)
            : this(content, null)
        { }

        public Page(LocalEmbed embed)
            : this(null, embed)
        { }

        public Page(string content, LocalEmbed embed)
        {
            if (string.IsNullOrWhiteSpace(content) && embed == null)
                throw new ArgumentException("At least one of content and embed must be specified.");

            Content = content;
            Embed = embed;
        }

        public static implicit operator Page(string value)
            => new Page(value);

        public static implicit operator Page(LocalEmbed value)
            => new Page(value);

        public static implicit operator Page((string Content, LocalEmbed Embed) value)
            => new Page(value.Content, value.Embed);
    }
}
