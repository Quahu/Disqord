using Disqord.Models;

namespace Disqord
{
    public abstract class TransientCategorizableGuildChannel : TransientGuildChannel, ICategorizableGuildChannel
    {
        public virtual Snowflake? CategoryId => Model.ParentId.Value;

        protected TransientCategorizableGuildChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
