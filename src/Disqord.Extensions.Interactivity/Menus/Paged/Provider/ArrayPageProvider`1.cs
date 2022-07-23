using System;
using System.Linq;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Extensions.Interactivity.Menus.Paged;

/// <summary>
///     Represents a method that formats a given item into a page.
/// </summary>
/// <typeparam name="T"> The type of the item. </typeparam>
/// <param name="view"> The view to format the item for. </param>
/// <param name="items"> The <typeparamref name="T"/> items to format. </param>
/// <returns>
///     The formatted item as a page.
/// </returns>
public delegate Page ArrayPageFormatter<T>(PagedViewBase view, ArraySegment<T> items);

/// <summary>
///     Creates pages automatically from an array of items.
/// </summary>
/// <typeparam name="T"> The type of items in the array. </typeparam>
public class ArrayPageProvider<T> : PageProvider
    where T : notnull
{
    /// <summary>
    ///     Gets the array of items of this provider.
    /// </summary>
    public T[] Array { get; }

    /// <summary>
    ///     Gets the amount of items per-page of this provider.
    /// </summary>
    public int ItemsPerPage { get; }

    /// <summary>
    ///     Gets the <see cref="ArrayPageFormatter{T}"/> of this provider.
    /// </summary>
    public ArrayPageFormatter<T> Formatter { get; }

    /// <inheritdoc/>
    public override int PageCount => (int) Math.Ceiling(Array.Length / (double) ItemsPerPage);

    /// <summary>
    ///     Instantiates a new <see cref="ArrayPageProvider{T}"/> with the specified array of items
    ///     and optionally the <see cref="ArrayPageFormatter{T}"/> and a number of items per <see cref="Page"/>.
    /// </summary>
    /// <param name="array"> The array of items. </param>
    /// <param name="formatter"> The <see cref="ArrayPageFormatter{T}"/>. If <see langword="null"/>, defaults to <see cref="DefaultFormatter"/>. </param>
    /// <param name="itemsPerPage"> The number of items per-<see cref="Page"/>. </param>
    /// <exception cref="ArgumentNullException"> Thrown if <paramref name="array"/> is <see langword="null"/>. </exception>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown if <paramref name="itemsPerPage"/> is not positive. </exception>
    public ArrayPageProvider(T[] array, ArrayPageFormatter<T>? formatter = null, int itemsPerPage = 10)
    {
        Guard.IsNotNull(array);

        if (itemsPerPage <= 0)
            throw new ArgumentOutOfRangeException(nameof(itemsPerPage));

        Array = array;
        ItemsPerPage = itemsPerPage;
        Formatter = formatter ?? DefaultFormatter;
    }

    /// <inheritdoc/>
    public override ValueTask<Page?> GetPageAsync(PagedViewBase view)
    {
        var offset = view.CurrentPageIndex * ItemsPerPage;
        var remainder = Array.Length - offset;
        var segment = new ArraySegment<T>(Array, offset, ItemsPerPage > Array.Length
            ? Array.Length
            : ItemsPerPage > remainder
                ? remainder
                : ItemsPerPage);

        var page = Formatter(view, segment);
        return new(page);
    }

    public static readonly ArrayPageFormatter<T> DefaultFormatter = static (view, segment) =>
    {
        var description = string.Join('\n', segment.Select((x, i) =>
        {
            var pageProvider = (view.PageProvider as ArrayPageProvider<T>)!;
            var itemPrefix = $"{i + segment.Offset + 1}. ";
            var maxItemLength = (int) Math.Floor((double) Discord.Limits.Message.Embed.MaxDescriptionLength / pageProvider.ItemsPerPage) - itemPrefix.Length - 2;
            if (maxItemLength <= 0)
                throw new InvalidOperationException("There are too many items to fit on the page. Set a lower amount or provide a custom page formatter.");

            var item = x.ToString();
            if (item == null)
                return null;

            if (item.Length > maxItemLength)
                item = $"{item[..maxItemLength]}…";

            return $"{itemPrefix}{item}";
        }));

        return new Page()
            .WithEmbeds(new LocalEmbed()
                .WithDescription(description));
    };
}
