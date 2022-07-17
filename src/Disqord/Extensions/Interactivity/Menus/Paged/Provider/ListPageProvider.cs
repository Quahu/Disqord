using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Extensions.Interactivity.Menus.Paged;

/// <summary>
///     Represents a <see cref="PageProvider"/> that provides pages from a list.
/// </summary>
public class ListPageProvider : PageProvider
{
    /// <summary>
    ///     Gets the list of pages of this provider.
    /// </summary>
    public List<Page> Pages { get; protected set; }

    /// <inheritdoc/>
    public override int PageCount => Pages.Count;

    /// <summary>
    ///     Instantiates a new <see cref="ListPageProvider"/> with the specified collection of pages.
    /// </summary>
    /// <param name="pages"> The collection of pages. </param>
    /// <exception cref="ArgumentNullException"> Thrown if <paramref name="pages"/> is <see langword="null"/>. </exception>
    public ListPageProvider(IEnumerable<Page> pages)
        : this(pages.ToList())
    { }

    /// <summary>
    ///     Instantiates a new <see cref="ListPageProvider"/> with the specified list of pages.
    /// </summary>
    /// <remarks>
    ///     The list is not copied.
    /// </remarks>
    /// <param name="pages"> The list of pages. </param>\
    /// <exception cref="ArgumentNullException"> Thrown if <paramref name="pages"/> is <see langword="null"/>. </exception>
    public ListPageProvider(List<Page> pages)
    {
        Guard.IsNotNull(pages);

        Pages = pages;
    }

    /// <inheritdoc/>
    public override ValueTask<Page?> GetPageAsync(PagedViewBase view)
    {
        var page = Pages.ElementAtOrDefault(view.CurrentPageIndex);
        return new(page);
    }
}