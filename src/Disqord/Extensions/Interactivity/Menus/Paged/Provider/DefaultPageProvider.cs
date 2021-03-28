using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Collections;

namespace Disqord.Extensions.Interactivity.Menus.Paged
{
    /// <summary>
    ///     Represents an <see cref="IPageProvider"/> that provides <see cref="Page"/>s from a collection.
    /// </summary>
    public class DefaultPageProvider : IPageProvider
    {
        /// <summary>
        ///     Gets the list of <see cref="Page"/>s of this provider.
        /// </summary>
        public IReadOnlyList<Page> Pages { get; }

        /// <inheritdoc/>
        public int PageCount => Pages.Count;

        /// <summary>
        ///     Instantiates a new <see cref="DefaultPageProvider"/> with the specified collection of <see cref="Page"/>s.
        /// </summary>
        /// <param name="pages"> The collection of <see cref="Page"/>s. </param>
        public DefaultPageProvider(IEnumerable<Page> pages)
        {
            if (pages == null)
                throw new ArgumentNullException(nameof(pages));

            Pages = pages.ToReadOnlyList();
        }

        /// <inheritdoc/>
        public virtual ValueTask<Page> GetPageAsync(PagedMenu menu)
        {
            var page = Pages[menu.CurrentPageIndex];
            return new(page);
        }
    }
}
