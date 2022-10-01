using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Extensions.Interactivity.Menus.Paged;

public abstract class PagedViewBase : ViewBase
{
    public bool HasPageChanges { get; private set; }

    /// <summary>
    ///     Gets the <see cref="PageProvider"/> of this paged menu.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown if <paramref name="value"/> is <see langword="null"/>. </exception>
    public PageProvider PageProvider
    {
        get => _pageProvider;
        protected set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "The page provider must not be null.");

            ReportPageChanges();
            _pageProvider = value;
        }
    }
    private PageProvider _pageProvider;

    /// <summary>
    ///     Gets or sets the current page index of this menu.
    ///     Defaults to <c>0</c> at startup.
    ///     By default this property will modify the view's current page and ignores out of range indices.
    /// </summary>
    public int CurrentPageIndex
    {
        get => _currentPageIndex;
        set
        {
            if (value < 0 || value > PageProvider.PageCount - 1)
                return;

            ReportPageChanges();
            _currentPageIndex = value;
        }
    }
    private int _currentPageIndex;

    public Page? CurrentPage { get; protected set; }

    protected PagedViewBase(PageProvider pageProvider, Action<LocalMessageBase>? messageTemplate = null)
        : base(messageTemplate)
    {
        _pageProvider = pageProvider;
    }

    protected void ReportPageChanges()
    {
        if (_menu != null)
        {
            HasPageChanges = true;
            ReportChanges();
        }
    }

    public override async ValueTask UpdateAsync()
    {
        if (HasPageChanges || CurrentPage == null)
        {
            HasPageChanges = false;
            CurrentPage = await PageProvider.GetPageAsync(this).ConfigureAwait(false);
        }
    }

    public override void FormatLocalMessage(LocalMessageBase message)
    {
        base.FormatLocalMessage(message);

        var currentPage = CurrentPage;
        if (currentPage != null)
        {
            message.Content = currentPage.Content.GetValueOrDefault();
            message.Embeds = currentPage.Embeds.GetValueOrDefault() is IList<LocalEmbed> embeds ? embeds.ToArray() : Array.Empty<LocalEmbed>();
        }
    }
}
