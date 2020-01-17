using System;
using System.Linq;
using System.Threading.Tasks;

namespace Disqord.Extensions.Interactivity.Menus.Paged
{
    /// <summary>
    ///     Represents the method that formats given data into a <see cref="Page"/>.
    /// </summary>
    /// <typeparam name="T"> The type of the data. </typeparam>
    /// <param name="menu"> The <see cref="PagedMenu"/> to format for. </param>
    /// <param name="data"> The <typeparamref name="T"/> data. </param>
    /// <returns> The formatted <see cref="Page"/>. </returns>
    public delegate Page PageFormatter<T>(PagedMenu menu, T data);

    /// <summary>
    ///     Creates pages automatically from an <see cref="System.Array"/> of data.
    /// </summary>
    /// <typeparam name="T"> The <see cref="Type"/> of elements in the <see cref="System.Array"/>. </typeparam>
    public sealed class ArrayPageProvider<T> : IPageProvider
    {
        /// <summary>
        ///     Gets the <see cref="System.Array"/> of data.
        /// </summary>
        public T[] Array { get; }

        /// <summary>
        ///     Gets the amount of items per <see cref="Page"/>.
        /// </summary>
        public int ItemsPerPage { get; }

        /// <summary>
        ///     Gets the <see cref="PageFormatter{T}"/>.
        /// </summary>
        public PageFormatter<ArraySegment<T>> Formatter { get; }

        /// <inheritdoc/>
        public int PageCount => (int) Math.Ceiling(Array.Length / (double) ItemsPerPage);

        /// <summary>
        ///     Instantiates a new <see cref="ArrayPageProvider{T}"/> wtih the given <see cref="System.Array"/> of data
        ///     and optionally the <see cref="PageFormatter{T}"/> and a number of items per <see cref="Page"/>.
        /// </summary>
        /// <param name="array"> The <see cref="System.Array"/> of data. </param>
        /// <param name="formatter"> The <see cref="PageFormatter{T}"/>. </param>
        /// <param name="itemsPerPage"> The number of items per <see cref="Page"/>. </param>
        public ArrayPageProvider(T[] array, PageFormatter<ArraySegment<T>> formatter = null, int itemsPerPage = 10)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (itemsPerPage <= 0 || itemsPerPage > array.Length)
                throw new ArgumentOutOfRangeException(nameof(itemsPerPage));

            Array = array;
            ItemsPerPage = itemsPerPage;
            Formatter = formatter
                ?? ((menu, segment) => new LocalEmbedBuilder()
                .WithDescription(string.Join('\n', segment.Select((x, i) =>
                {
                    var item = x.ToString();
                    if (item.Length > 30)
                        item = $"{item[0..30]}…";

                    return $"{i + segment.Offset + 1}. {item}";
                })))
                .WithFooter($"Page {menu.CurrentPageIndex + 1}/{menu.PageProvider.PageCount}")
                .Build());
        }

        /// <inheritdoc/>
        public ValueTask<Page> GetPageAsync(PagedMenu menu)
        {
            var offset = menu.CurrentPageIndex * ItemsPerPage;
            var remainder = Array.Length - offset;
            var segment = new ArraySegment<T>(Array, offset, ItemsPerPage > remainder
                ? remainder
                : ItemsPerPage);
            var page = Formatter(menu, segment);
            return new ValueTask<Page>(page);
        }
    }
}
