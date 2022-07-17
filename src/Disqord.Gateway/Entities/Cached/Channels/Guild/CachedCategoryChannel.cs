using Disqord.Models;

namespace Disqord.Gateway;

/// <inheritdoc cref="ICategoryChannel"/>
public class CachedCategoryChannel : CachedGuildChannel, ICategoryChannel
{
    public CachedCategoryChannel(IGatewayClient client, ChannelJsonModel model)
        : base(client, model)
    { }
}