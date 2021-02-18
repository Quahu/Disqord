using System.ComponentModel;
using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedUnknownGuildChannel : CachedGuildChannel, IUnknownGuildChannel
    {
        public Snowflake? CategoryId { get; }

        public ChannelType Type { get; }

        public CachedUnknownGuildChannel(IGatewayClient client, ChannelJsonModel model)
            : base(client, model)
        { }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
