using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Disqord.Extensions.Interactivity.Menus;

namespace Disqord.Bot
{
    public class DiscordMenuCommandResult : DiscordCommandResult
    {
        private readonly MenuBase _menu;

        public DiscordMenuCommandResult(DiscordCommandContext context, MenuBase menu)
            : base(context)
        {
            _menu = menu;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override TaskAwaiter GetAwaiter()
            => Context.Bot.RunMenuAsync(Context.ChannelId, _menu).GetAwaiter();

        public override Task ExecuteAsync()
            => Context.Bot.StartMenuAsync(Context.ChannelId, _menu);
    }
}
