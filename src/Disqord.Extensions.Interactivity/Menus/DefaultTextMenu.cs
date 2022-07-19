using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Extensions.Interactivity.Menus;

/// <inheritdoc/>
public class DefaultTextMenu : DefaultMenuBase
{
    /// <inheritdoc/>
    public DefaultTextMenu(ViewBase view)
        : base(view)
    { }

    /// <inheritdoc/>
    public DefaultTextMenu(ViewBase view, Snowflake messageId)
        : base(view, messageId)
    { }

    /// <inheritdoc/>
    public override LocalMessageBase CreateLocalMessage()
    {
        return new LocalMessage();
    }

    /// <inheritdoc/>
    protected override Task<IUserMessage> SendLocalMessageAsync(LocalMessageBase message, CancellationToken cancellationToken)
    {
        return Client.SendMessageAsync(ChannelId, (message as LocalMessage)!, cancellationToken: cancellationToken);
    }
}