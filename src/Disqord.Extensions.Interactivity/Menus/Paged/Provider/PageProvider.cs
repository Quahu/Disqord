using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Disqord.Extensions.Interactivity.Menus.Paged
{
    /// <summary>
    ///     Provides <see cref="Page"/>s from a collection.
    /// </summary>
    public sealed class PageProvider : IPageProvider
    {
        /// <summary>
        ///     Gets the collection of <see cref="Page"/>s.
        /// </summary>
        public ImmutableArray<Page> Pages { get; }

        /// <inheritdoc/>
        public int PageCount => Pages.Length;

        /// <summary>
        ///     Instantiates a new <see cref="PageProvider"/> with the specified collection of <see cref="Page"/>s.
        /// </summary>
        /// <param name="pages"> The collection of <see cref="Page"/>s. </param>
        public PageProvider(IEnumerable<Page> pages)
        {
            if (pages == null)
                throw new ArgumentNullException(nameof(pages));

            Pages = pages.ToImmutableArray();
        }

        /// <inheritdoc/>
        public ValueTask<Page> GetPageAsync(PagedMenu menu)
            => new ValueTask<Page>(Pages[menu.CurrentPageNumber]);
    }
}
