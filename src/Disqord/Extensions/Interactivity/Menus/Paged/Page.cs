using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord.Extensions.Interactivity.Menus.Paged
{
    /// <summary>
    ///     Represents what essentially is a tuple of <see cref="LocalMessage.Content"/> and <see cref="LocalMessage.Embeds"/> respectively.
    /// </summary>
    public class Page : ILocalConstruct
    {
        /// <summary>
        ///     Gets or sets the content of this page.
        /// </summary>
        public string Content { get; set; }

        public IList<LocalEmbed> Embeds
        {
            get => _embeds;
            set => WithEmbeds(value);
        }
        private readonly List<LocalEmbed> _embeds;

        /// <summary>
        ///     Instantiates a new <see cref="Page"/> without any properties.
        /// </summary>
        public Page()
        {
            _embeds = new List<LocalEmbed>();
        }

        private Page(Page other)
        {
            Content = other.Content;
            _embeds = other._embeds.Select(x => x.Clone()).ToList();
        }

        public Page WithContent(string content)
        {
            Content = content;
            return this;
        }

        public Page WithEmbeds(params LocalEmbed[] embeds)
            => WithEmbeds(embeds as IEnumerable<LocalEmbed>);

        public Page WithEmbeds(IEnumerable<LocalEmbed> embeds)
        {
            if (embeds == null)
                throw new ArgumentNullException(nameof(embeds));

            _embeds.Clear();
            _embeds.AddRange(embeds);
            return this;
        }

        public Page AddEmbed(LocalEmbed embed)
        {
            if (embed == null)
                throw new ArgumentNullException(nameof(embed));

            _embeds.Add(embed);
            return this;
        }

        public Page Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();

        public void Validate()
        { }
    }
}
