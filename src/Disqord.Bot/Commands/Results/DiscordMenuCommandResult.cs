using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Extensions.Interactivity.Menus;

namespace Disqord.Bot.Commands;

public class DiscordMenuCommandResult : DiscordCommandResult<IDiscordCommandContext>
{
    public MenuBase Menu { get; protected set; }

    public TimeSpan Timeout { get; protected set; }

    public DiscordMenuCommandResult(IDiscordCommandContext context, MenuBase menu, TimeSpan timeout)
        : base(context)
    {
        Menu = menu;
        Timeout = timeout;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override TaskAwaiter GetAwaiter()
    {
        return Context.Bot.RunMenuAsync(Context.ChannelId, Menu, Timeout).GetAwaiter();
    }

    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return Context.Bot.StartMenuAsync(Context.ChannelId, Menu, Timeout, cancellationToken: cancellationToken);
    }
}
