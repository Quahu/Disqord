namespace Disqord.Extensions.Interactivity.Menus.Paged
{
    /// <summary>
    ///     Represents what essentially is a <see cref="(T1, T2)"/> of <see cref="string"/> and <see cref="LocalEmbed"/>
    ///     which by default map to <see cref="LocalMessage.Content"/> and <see cref="LocalMessage.Embed"/> respectively.
    /// </summary>
    public class Page
    {
        /// <summary>
        ///     Gets or sets the content of this page.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     Gets or sets the embed of this page.
        /// </summary>
        public LocalEmbed Embed { get; set; }

        /// <summary>
        ///     Instantiates a new <see cref="Page"/> without any properties.
        /// </summary>
        protected Page()
        { }

        /// <summary>
        ///     Instantiates a new <see cref="Page"/> with the specified content.
        /// </summary>
        /// <param name="content"> The content of this page. </param>
        public Page(string content)
            : this(content, null)
        { }

        /// <summary>
        ///     Instantiates a new <see cref="Page"/> with the specified embed.
        /// </summary>
        /// <param name="embed"> The embed of this page. </param>
        public Page(LocalEmbed embed)
            : this(null, embed)
        { }

        /// <summary>
        ///     Instantiates a new <see cref="Page"/> with the specified message content and embed.
        /// </summary>
        /// <param name="content"> The content of this page. </param>
        /// <param name="embed"> The embed of this page. </param>
        public Page(string content, LocalEmbed embed)
        {
            Content = content;
            Embed = embed;
        }

        /// <summary>
        ///     Implicitly wraps the specified content in a <see cref="Page"/>.
        /// </summary>
        /// <param name="value"> The content to wrap. </param>
        public static implicit operator Page(string value)
            => new(value);

        /// <summary>
        ///     Implicitly wraps the specified embed in a <see cref="Page"/>.
        /// </summary>
        /// <param name="value"> The embed to wrap. </param>
        public static implicit operator Page(LocalEmbed value)
            => new(value);

        /// <summary>
        ///     Implicitly wraps the specified tuple of content and embed in a <see cref="Page"/>.
        /// </summary>
        /// <param name="value"> The tuple to wrap. </param>
        public static implicit operator Page((string Content, LocalEmbed Embed) value)
            => new(value.Content, value.Embed);
    }
}
