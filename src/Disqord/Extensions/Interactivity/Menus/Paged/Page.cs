using System.Collections.Generic;
using Qommon;

namespace Disqord.Extensions.Interactivity.Menus.Paged;

/// <summary>
///     Represents what essentially is a tuple of <see cref="LocalMessage.Content"/> and <see cref="LocalMessage.Embeds"/> respectively.
/// </summary>
public class Page : ILocalConstruct<Page>
{
    /// <summary>
    ///     Gets or sets the message content of this page.
    /// </summary>
    public Optional<string?> Content { get; set; }

    /// <summary>
    ///     Gets or sets the embeds of this page.
    /// </summary>
    public Optional<IList<LocalEmbed>> Embeds { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="Page"/>.
    /// </summary>
    public Page()
    { }

    protected Page(Page other)
    {
        Content = other.Content;
        Embeds = other.Embeds.DeepClone();
    }

    public virtual Page Clone()
    {
        return new(this);
    }
}
