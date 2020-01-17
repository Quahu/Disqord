using System;
using System.Threading.Tasks;

namespace Disqord.Extensions.Interactivity.Menus.Paged
{
    /// <summary>
    ///     Represents a <see cref="Page"/> provider.
    /// </summary>
    public interface IPageProvider : IAsyncDisposable
    {
        /// <summary>
        ///     Gets the total amount of pages.
        /// </summary>
        int PageCount { get; }

        /// <summary>
        ///     Gets a <see cref="Page"/> for the <see cref="PagedMenu"/>'s current state.
        /// </summary>
        /// <param name="menu"> The <see cref="PagedMenu"/>. </param>
        ValueTask<Page> GetPageAsync(PagedMenu menu);

        ValueTask IAsyncDisposable.DisposeAsync()
            => default;
    }
}