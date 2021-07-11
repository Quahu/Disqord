using Disqord.Models;

namespace Disqord
{
    public class TransientUnknownGuildChannel : TransientCategorizableGuildChannel, IUnknownGuildChannel
    {
        public TransientUnknownGuildChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
