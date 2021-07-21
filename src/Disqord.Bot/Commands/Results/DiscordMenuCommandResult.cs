using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Disqord.Extensions.Interactivity.Menus;

namespace Disqord.Bot
{
    public class DiscordMenuCommandResult : DiscordCommandResult
    {
        public MenuBase Menu { get; protected set; }

        public TimeSpan Timeout { get; protected set; }

        public DiscordMenuCommandResult(DiscordCommandContext context, MenuBase menu, TimeSpan timeout)
            : base(context)
        {
            Menu = menu;
            Timeout = timeout;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override TaskAwaiter GetAwaiter()
            => Context.Bot.RunMenuAsync(Context.ChannelId, Menu, Timeout).GetAwaiter();

        public override Task ExecuteAsync()
            => Context.Bot.StartMenuAsync(Context.ChannelId, Menu, Timeout);
    }
}
