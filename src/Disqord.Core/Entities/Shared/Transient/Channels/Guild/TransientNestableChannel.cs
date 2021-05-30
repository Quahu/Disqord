using Disqord.Models;

namespace Disqord
{
    public abstract class TransientNestableChannel : TransientGuildChannel, INestableChannel
    {
        public Snowflake? CategoryId => Model.ParentId.Value;

        protected TransientNestableChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
