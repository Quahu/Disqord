using System.Threading.Tasks;

namespace Disqord.Extensions.Interactivity.Menus.Paged
{
    public class PagedMenu : MenuBase
    {
        public Snowflake UserId { get; }

        public IPageProvider PageProvider { get; }

        public int CurrentPageIndex { get; private set; }

        public PagedMenu(Snowflake userId, IPageProvider pageProvider, bool addDefaultButtons = true) : base(true, true)
        {
            UserId = userId;
            PageProvider = pageProvider;

            if (addDefaultButtons)
            {
                // these contain the variation selectors
                AddButtonAsync(new Button(new LocalEmoji("⏮️"), e => ChangePageAsync(0), 0));
                AddButtonAsync(new Button(new LocalEmoji("◀️"), e => ChangePageAsync(CurrentPageIndex - 1), 1));
                AddButtonAsync(new Button(new LocalEmoji("▶️"), e => ChangePageAsync(CurrentPageIndex + 1), 2));
                AddButtonAsync(new Button(new LocalEmoji("⏭️"), e => ChangePageAsync(PageProvider.PageCount - 1), 3));
                AddButtonAsync(new Button(new LocalEmoji("⏹️"), async e =>
                {
                    try
                    {
                        await Message.DeleteAsync().ConfigureAwait(false);
                    }
                    catch { }
                    await StopAsync().ConfigureAwait(false);
                }, 4));
            }
        }

        protected override ValueTask<bool> CheckReactionAsync(ButtonEventArgs e)
            => new ValueTask<bool>(e.User.Id == UserId);

        protected internal override sealed async Task<IUserMessage> InitialiseAsync()
        {
            var page = await PageProvider.GetPageAsync(this).ConfigureAwait(false);
            var message = await Channel.SendMessageAsync(page.Content, embed: page.Embed).ConfigureAwait(false);
            return message;
        }

        protected virtual ValueTask PageChangedAsync()
            => default;

        public async Task ChangePageAsync(int pageIndex)
        {
            if (pageIndex < 0 || pageIndex > PageProvider.PageCount - 1)
                return;

            CurrentPageIndex = pageIndex;
            var page = await PageProvider.GetPageAsync(this).ConfigureAwait(false);
            await Message.ModifyAsync(x =>
            {
                x.Content = page.Content;
                x.Embed = page.Embed;
            }).ConfigureAwait(false);
            await PageChangedAsync().ConfigureAwait(false);
        }
    }
}
