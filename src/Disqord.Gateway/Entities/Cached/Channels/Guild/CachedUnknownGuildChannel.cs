using Disqord.Models;

namespace Disqord.Gateway;

/// <inheritdoc cref="IUnknownGuildChannel"/>
public class CachedUnknownGuildChannel : CachedCategorizableGuildChannel, IUnknownGuildChannel
{
    public CachedUnknownGuildChannel(IGatewayClient client, ChannelJsonModel model)
        : base(client, model)
    { }
}