using Disqord.Models;

namespace Disqord
{
    public class TransientStageChannel : TransientVocalGuildChannel, IStageChannel
    {
        public TransientStageChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
