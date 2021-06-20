using System.Threading.Tasks;

namespace Disqord.Extensions.Interactivity.Menus.Paged
{
    /// <summary>
    ///     Represents a provider for pages.
    /// </summary>
    public interface IPageProvider
    {
        /// <summary>
        ///     Gets the total amount of pages in this provider.
        /// </summary>
        int PageCount { get; }

        /// <summary>
        ///     Gets a page for the paged menu's current state.
        /// </summary>
        /// <param name="view"> The view to get the page for. </param>
        /// <returns>
        ///     A <see cref="ValueTask{TResult}"/> with a page result representing the work.
        /// </returns>
        ValueTask<Page> GetPageAsync(PagedViewBase view);
    }
}
