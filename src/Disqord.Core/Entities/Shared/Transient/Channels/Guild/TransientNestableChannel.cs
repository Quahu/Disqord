using Disqord.Models;

namespace Disqord
{
    public abstract class TransientNestableChannel : TransientGuildChannel, INestableChannel
    {
        public Snowflake? CategoryId => Model.ParentId.Value;

        public TransientNestableChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
