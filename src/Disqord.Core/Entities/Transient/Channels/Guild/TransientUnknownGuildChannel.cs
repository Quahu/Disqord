using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="IUnknownGuildChannel"/>
public class TransientUnknownGuildChannel : TransientCategorizableGuildChannel, IUnknownGuildChannel
{
    public TransientUnknownGuildChannel(IClient client, ChannelJsonModel model)
        : base(client, model)
    { }
}