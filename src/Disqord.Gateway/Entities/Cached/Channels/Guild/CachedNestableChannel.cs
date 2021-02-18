using Disqord.Models;

namespace Disqord.Gateway
{
    public abstract class CachedNestableChannel : CachedGuildChannel, INestableChannel
    {
        public Snowflake? CategoryId { get; private set; }

        public CachedNestableChannel(IGatewayClient client, ChannelJsonModel model)
            : base(client, model)
        { }

        public override void Update(ChannelJsonModel model)
        {
            base.Update(model);

            if (model.ParentId.HasValue)
                CategoryId = model.ParentId.Value;
        }
    }
}
