using System.ComponentModel;
using Disqord.Models;

namespace Disqord.Gateway
{
    public abstract class CachedChannel : CachedSnowflakeEntity, IChannel
    {
        public virtual string Name { get; private set; }

        public CachedChannel(IGatewayClient client, ChannelJsonModel model)
            : base(client, model.Id)
        {
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

        public static CachedChannel Create(IGatewayClient client, ChannelJsonModel model)
        {
            switch (model.Type)
            {
                case ChannelType.Direct:
                case ChannelType.Group:
                    // TODO: cached private channels?
                    break;

                case ChannelType.Text:
                case ChannelType.Voice:
                case ChannelType.Category:
                case ChannelType.News:
                case ChannelType.Store:
                case ChannelType.Thread:
                    return CachedGuildChannel.Create(client, model);
            }

            return null/*TransientUnknownChannel(client, model)*/;
        }
    }
}
