using System.Collections.Generic;
using System.ComponentModel;
using Qommon;

namespace Disqord.Extensions.Interactivity.Menus.Paged;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class PageExtensions
{
    public static Page WithContent(this Page page, string content)
    {
        page.Content = content;
        return page;
    }

    public static Page WithEmbeds(this Page page, params LocalEmbed[] embeds)
    {
        return page.WithEmbeds(embeds as IEnumerable<LocalEmbed>);
    }

    public static Page WithEmbeds(this Page page, IEnumerable<LocalEmbed> embeds)
    {
        Guard.IsNotNull(embeds);

        if (page.Embeds.With(embeds, out var list))
            page.Embeds = new(list);

        return page;
    }

    public static Page AddEmbed(this Page page, LocalEmbed embed)
    {
        Guard.IsNotNull(embed);

        if (page.Embeds.Add(embed, out var list))
            page.Embeds = new(list);

        return page;
    }
}
