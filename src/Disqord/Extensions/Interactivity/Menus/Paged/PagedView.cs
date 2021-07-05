using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Extensions.Interactivity.Menus.Paged
{
    public class PagedView : PagedViewBase
    {
        public PagedView(IPageProvider pageProvider)
            : base(pageProvider)
        { }

        [Button(Emoji = "⏮️", Style = LocalButtonComponentStyle.Secondary)]
        public ValueTask First(ButtonEventArgs e)
        {
            CurrentPageIndex = 0;
            return default;
        }

        [Button(Emoji = "◀️", Style = LocalButtonComponentStyle.Secondary)]
        public ValueTask Previous(ButtonEventArgs e)
        {
            CurrentPageIndex--;
            return default;
        }

        [Button(Emoji = "▶️", Style = LocalButtonComponentStyle.Secondary)]
        public ValueTask Next(ButtonEventArgs e)
        {
            CurrentPageIndex++;
            return default;
        }

        [Button(Emoji = "⏭️", Style = LocalButtonComponentStyle.Secondary)]
        public ValueTask Last(ButtonEventArgs e)
        {
            CurrentPageIndex = PageProvider.PageCount - 1;
            return default;
        }

        [Button(Emoji = "⏹️", Style = LocalButtonComponentStyle.Secondary)]
        public ValueTask Stop(ButtonEventArgs e)
        {
            if (Menu is InteractiveMenu interactiveMenu)
                _ = interactiveMenu.Message.DeleteAsync();

            return Menu.StopAsync();
        }
    }

}
