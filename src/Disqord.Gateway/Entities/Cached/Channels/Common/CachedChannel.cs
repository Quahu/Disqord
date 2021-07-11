using System.ComponentModel;
using Disqord.Models;

namespace Disqord.Gateway
{
    public abstract class CachedChannel : CachedSnowflakeEntity, IChannel
    {
        public virtual string Name { get; private set; }

        public ChannelType Type { get; }

        protected CachedChannel(IGatewayClient client, ChannelJsonModel model)
            : base(client, model.Id)
        {
            Type = model.Type;

            Update(model);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void Update(ChannelJsonModel model)
        {
            if (model.Name.HasValue)
                Name = model.Name.Value;
        }

        public override string ToString()
            => Name;
    }
}
