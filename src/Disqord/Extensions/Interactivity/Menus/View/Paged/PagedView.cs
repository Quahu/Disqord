using System;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Extensions.Interactivity.Menus.Paged
{
    public class PagedView : PagedViewBase
    {
        /// <summary>
        ///     Gets the first page button.
        /// </summary>
        public ButtonViewComponent FirstPageButton { get; }

        /// <summary>
        ///     Gets the previous page button.
        /// </summary>
        public ButtonViewComponent PreviousPageButton { get; }

        /// <summary>
        ///     Gets the next page button.
        /// </summary>
        public ButtonViewComponent NextPageButton { get; }

        /// <summary>
        ///     Gets the last page button.
        /// </summary>
        public ButtonViewComponent LastPageButton { get; }

        /// <summary>
        ///     Gets the stop button.
        /// </summary>
        public ButtonViewComponent StopButton { get; }

        public PagedView(PageProvider pageProvider, Action<LocalMessageBase> messageTemplate = null)
            : base(pageProvider, messageTemplate)
        {
            FirstPageButton = new ButtonViewComponent(OnFirstPageButtonAsync)
            {
                Emoji = new LocalEmoji("⏮️"),
                Style = LocalButtonComponentStyle.Secondary
            };

            PreviousPageButton = new ButtonViewComponent(OnPreviousPageButtonAsync)
            {
                Emoji = new LocalEmoji("◀️"),
                Style = LocalButtonComponentStyle.Secondary
            };

            NextPageButton = new ButtonViewComponent(OnNextPageButtonAsync)
            {
                Emoji = new LocalEmoji("▶️"),
                Style = LocalButtonComponentStyle.Secondary
            };

            LastPageButton = new ButtonViewComponent(OnLastPageButtonAsync)
            {
                Emoji = new LocalEmoji("⏭️"),
                Style = LocalButtonComponentStyle.Secondary
            };

            StopButton = new ButtonViewComponent(OnStopButtonAsync)
            {
                Emoji = new LocalEmoji("⏹️"),
                Style = LocalButtonComponentStyle.Secondary
            };

            AddComponent(FirstPageButton);
            AddComponent(PreviousPageButton);
            AddComponent(NextPageButton);
            AddComponent(LastPageButton);
            AddComponent(StopButton);
        }

        protected virtual Action<LocalMessageBase> GetPagelessMessageTemplate()
        {
            return message => message.WithContent("No pages to view.");
        }

        protected virtual void ApplyPageIndex(Page page)
        {
            var indexText = $"Page {CurrentPageIndex + 1}/{PageProvider.PageCount}";
            var embed = page.Embeds.LastOrDefault();
            if (embed != null)
            {
                if (embed.Footer != null)
                {
                    if (embed.Footer.Text == null)
                    {
                        embed.Footer.Text = indexText;
                    }
                    else if (embed.Footer.Text.Length + indexText.Length + 3 <= LocalEmbedFooter.MaxTextLength)
                    {
                        embed.Footer.Text += $" | {indexText}";
                    }
                }
                else
                {
                    embed.WithFooter(indexText);
                }
            }
            else
            {
                if (page.Content == null)
                {
                    page.Content = indexText;
                }
                else if (page.Content.Length + indexText.Length + 1 <= LocalMessageBase.MaxContentLength)
                {
                    page.Content += $"\n{indexText}";
                }
            }
        }

        public override async ValueTask UpdateAsync()
        {
            var previousPage = CurrentPage;
            await base.UpdateAsync().ConfigureAwait(false);

            var currentPage = CurrentPage;
            if (currentPage != null)
            {
                var currentPageIndex = CurrentPageIndex;
                var pageCount = PageProvider.PageCount;
                FirstPageButton.IsDisabled = currentPageIndex == 0;
                PreviousPageButton.IsDisabled = currentPageIndex == 0;
                NextPageButton.IsDisabled = currentPageIndex == pageCount - 1;
                LastPageButton.IsDisabled = currentPageIndex == pageCount - 1;

                if (previousPage != currentPage)
                {
                    currentPage = currentPage.Clone();
                    ApplyPageIndex(currentPage);
                    CurrentPage = currentPage;
                }
            }
            else
            {
                MessageTemplate ??= GetPagelessMessageTemplate();

                FirstPageButton.IsDisabled = true;
                PreviousPageButton.IsDisabled = true;
                NextPageButton.IsDisabled = true;
                LastPageButton.IsDisabled = true;
            }
        }

        /// <summary>
        ///     Triggered when the <see cref="FirstPageButton"/> is triggered.
        /// </summary>
        /// <param name="e"> The event data. </param>
        /// <returns>
        ///     A <see cref="ValueTask"/> representing the callback work.
        /// </returns>
        protected virtual ValueTask OnFirstPageButtonAsync(ButtonEventArgs e)
        {
            CurrentPageIndex = 0;
            return default;
        }

        /// <summary>
        ///     Triggered when the <see cref="PreviousPageButton"/> is triggered.
        /// </summary>
        /// <param name="e"> The event data. </param>
        /// <returns>
        ///     A <see cref="ValueTask"/> representing the callback work.
        /// </returns>
        protected virtual ValueTask OnPreviousPageButtonAsync(ButtonEventArgs e)
        {
            CurrentPageIndex--;
            return default;
        }

        /// <summary>
        ///     Triggered when the <see cref="NextPageButton"/> is triggered.
        /// </summary>
        /// <param name="e"> The event data. </param>
        /// <returns>
        ///     A <see cref="ValueTask"/> representing the callback work.
        /// </returns>
        protected virtual ValueTask OnNextPageButtonAsync(ButtonEventArgs e)
        {
            CurrentPageIndex++;
            return default;
        }

        /// <summary>
        ///     Triggered when the <see cref="LastPageButton"/> is triggered.
        /// </summary>
        /// <param name="e"> The event data. </param>
        /// <returns>
        ///     A <see cref="ValueTask"/> representing the callback work.
        /// </returns>
        protected virtual ValueTask OnLastPageButtonAsync(ButtonEventArgs e)
        {
            CurrentPageIndex = PageProvider.PageCount - 1;
            return default;
        }

        /// <summary>
        ///     Triggered when the <see cref="StopButton"/> is triggered.
        /// </summary>
        /// <param name="e"> The event data. </param>
        /// <returns>
        ///     A <see cref="ValueTask"/> representing the callback work.
        /// </returns>
        protected virtual ValueTask OnStopButtonAsync(ButtonEventArgs e)
        {
            if (Menu is DefaultMenuBase defaultMenu)
            {
                var message = defaultMenu.Message;
                if (message != null)
                    _ = message.DeleteAsync();
            }

            Menu.Stop();
            return default;
        }
    }
}
