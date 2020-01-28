using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Collections;

namespace Disqord.Extensions.Interactivity.Menus.Paged
{
    /// <summary>
    ///     Provides <see cref="Page"/>s from a collection.
    /// </summary>
    public sealed class DefaultPageProvider : IPageProvider
    {
        /// <summary>
        ///     Gets the collection of <see cref="Page"/>s.
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
        public ValueTask<Page> GetPageAsync(PagedMenu menu)
            => new ValueTask<Page>(Pages[menu.CurrentPageIndex]);
    }
}
