using System.Threading.Tasks;

namespace Disqord.Extensions.Interactivity.Menus.Paged
{
    /// <summary>
    ///     Represents a <see cref="Page"/> provider.
    /// </summary>
    public interface IPageProvider
    {
        /// <summary>
        ///     Gets the total amount of <see cref="Page"/>s of this provider.
        /// </summary>
        int PageCount { get; }

        /// <summary>
        ///     Gets a <see cref="Page"/> for the <see cref="PagedMenu"/>'s current state.
        /// </summary>
        /// <param name="menu"> The <see cref="PagedMenu"/> to get the page for. </param>
        /// <returns>
        ///     A <see cref="ValueTask{TResult}"/> with a <see cref="Page"/> result representing the work.
        /// </returns>
        ValueTask<Page> GetPageAsync(PagedMenu menu);
    }
}