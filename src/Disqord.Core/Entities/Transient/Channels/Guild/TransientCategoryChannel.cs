using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="ICategoryChannel"/>
public class TransientCategoryChannel : TransientGuildChannel, ICategoryChannel
{
    public TransientCategoryChannel(IClient client, ChannelJsonModel model)
        : base(client, model)
    { }
}