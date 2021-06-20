using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Disqord.Extensions.Interactivity.Menus.Paged
{
    /// <summary>
    ///     Represents an <see cref="IPageProvider"/> that provides pages from a pre-defined collection.
    /// </summary>
    public class PageProvider : IPageProvider
    {
        /// <summary>
        ///     Gets the list of pages of this provider.
        /// </summary>
        public IReadOnlyList<Page> Pages { get; }

        /// <inheritdoc/>
        public int PageCount => Pages.Count;

        /// <summary>
        ///     Instantiates a new <see cref="PageProvider"/> with the specified collection of pages.
        /// </summary>
        /// <param name="pages"> The collection of pages. </param>
        public PageProvider(IEnumerable<Page> pages)
        {
            if (pages == null)
                throw new ArgumentNullException(nameof(pages));

            Pages = pages.ToArray();

            if (Pages.Count == 0)
                throw new ArgumentException("The collection of pages must contain at least 1 page.", nameof(pages));
        }

        /// <inheritdoc/>
        public virtual ValueTask<Page> GetPageAsync(PagedViewBase view)
        {
            var page = Pages[view.CurrentPageIndex];
            return new(page);
        }
    }
}
