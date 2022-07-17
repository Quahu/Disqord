using System.Threading.Tasks;

namespace Disqord.Extensions.Interactivity.Menus.Paged;

/// <summary>
///     Represents a provider for pages.
/// </summary>
public abstract class PageProvider
{
    /// <summary>
    ///     Gets the total amount of pages in this provider.
    /// </summary>
    public abstract int PageCount { get; }

    /// <summary>
    ///     Gets a page for the paged menu's current state.
    /// </summary>
    /// <param name="view"> The view to get the page for. </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> with a page result representing the work.
    /// </returns>
    public abstract ValueTask<Page?> GetPageAsync(PagedViewBase view);
}