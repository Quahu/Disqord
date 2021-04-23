using System;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Extensions.Interactivity.Menus.Paged
{
    /// <summary>
    ///     Represents an indexed menu that displays pages to a user accordingly.
    /// </summary>
    public class PagedMenu : MenuBase
    {
        /// <summary>
        ///     Gets the ID of the user this menu is bound to.
        /// </summary>
        public Snowflake UserId { get; }

        /// <summary>
        ///     Gets the <see cref="IPageProvider"/> of this menu.
        /// </summary>
        public IPageProvider PageProvider { get; }

        /// <summary>
        ///     Gets the message of this menu.
        /// </summary>
        public IUserMessage Message { get; protected set; }

        /// <summary>
        ///     Gets or sets the current page index of this menu.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The current page index must be a positive value lower than the page count.
        /// </exception>
        public int CurrentPageIndex
        {
            get => _currentPageIndex;
            set
            {
                if (value < 0 || value > PageProvider.PageCount - 1)
                    throw new ArgumentOutOfRangeException(nameof(value), "The current page index must be a positive value lower than the page count.");

                _currentPageIndex = value;
            }
        }
        private int _currentPageIndex;

        /// <summary>
        ///     Gets or sets the behavior used by the stop button of this menu.
        ///     Defaults to <see cref="Disqord.Extensions.Interactivity.Menus.Paged.StopBehavior.DeleteMessage"/>.
        /// </summary>
        public StopBehavior StopBehavior { get; set; } = StopBehavior.DeleteMessage;

        public PagedMenu(Snowflake userId, IPageProvider pageProvider, bool addDefaultButtons = true)
        {
            if (pageProvider == null)
                throw new ArgumentNullException(nameof(pageProvider));

            if (pageProvider.PageCount == 0)
                throw new ArgumentException("The page provider must contain at least a single page.", nameof(pageProvider));

            UserId = userId;
            PageProvider = pageProvider;

            if (addDefaultButtons)
            {
                // Note: the Unicode strings contain the variation selectors.
                AddButtonAsync(new Button(new LocalEmoji("⏮️"), e => ChangePageAsync(0), 0));
                AddButtonAsync(new Button(new LocalEmoji("◀️"), e => ChangePageAsync(CurrentPageIndex - 1), 1));
                AddButtonAsync(new Button(new LocalEmoji("▶️"), e => ChangePageAsync(CurrentPageIndex + 1), 2));
                AddButtonAsync(new Button(new LocalEmoji("⏭️"), e => ChangePageAsync(PageProvider.PageCount - 1), 3));
                AddButtonAsync(new Button(new LocalEmoji("⏹️"), async e =>
                {
                    try
                    {
                        switch (StopBehavior)
                        {
                            case StopBehavior.ClearReactions:
                                await Message.ClearReactionsAsync().ConfigureAwait(false); // TODO: check permissions
                                break;

                            case StopBehavior.DeleteMessage:
                                await Message.DeleteAsync().ConfigureAwait(false);
                                break;
                        }
                    }
                    finally
                    {
                        await StopAsync().ConfigureAwait(false);
                    }
                }, 4));
            }
        }

        /// <summary>
        ///     By default checks if the ID of the user who reacted is equal to <see cref="UserId"/>.
        /// </summary>
        /// <param name="e"> <inheritdoc/> </param>
        /// <returns> <inheritdoc/> </returns>
        protected override ValueTask<bool> CheckReactionAsync(ButtonEventArgs e)
            => new(e.UserId == UserId);

        /// <inheritdoc/>
        protected internal override sealed async ValueTask<Snowflake> InitializeAsync()
        {
            var page = await PageProvider.GetPageAsync(this).ConfigureAwait(false);
            Message = await InitializeAsync(page).ConfigureAwait(false);
            return Message.Id;
        }

        /// <summary>
        ///     Initializes this menu by sending a message with the page retrieved from the <see cref="PageProvider"/>.
        ///     By default this will be the page with the index <c>0</c> as that is the default value of <see cref="CurrentPageIndex"/>.
        /// </summary>
        /// <param name="page"> The page to send. </param>
        /// <returns>
        ///     A <see cref="Task"/> representing the initialization work.
        /// </returns>
        protected virtual async ValueTask<IUserMessage> InitializeAsync(Page page)
        {
            var message = new LocalMessageBuilder
            {
                Content = page.Content,
                Embed = page.Embed,
                Mentions = LocalMentionsBuilder.None
            }.Build();
            return await Client.SendMessageAsync(ChannelId, message).ConfigureAwait(false);
        }

        /// <summary>
        ///     Changes the current page index to the specified value.
        ///     By default this method modifies the <see cref="Message"/>
        ///     to the new page and ignores out of range values.
        /// </summary>
        /// <param name="pageIndex"> The index to change the page to. </param>
        /// <returns>
        ///     A <see cref="Task"/> representing the page change work.
        /// </returns>
        public virtual async ValueTask ChangePageAsync(int pageIndex)
        {
            if (pageIndex < 0 || pageIndex > PageProvider.PageCount - 1)
                return;

            CurrentPageIndex = pageIndex;
            var page = await PageProvider.GetPageAsync(this).ConfigureAwait(false);
            await Client.ModifyMessageAsync(ChannelId, MessageId, x =>
            {
                x.Content = page.Content;
                x.Embed = page.Embed?.Build();
            }).ConfigureAwait(false);
        }
    }
}
