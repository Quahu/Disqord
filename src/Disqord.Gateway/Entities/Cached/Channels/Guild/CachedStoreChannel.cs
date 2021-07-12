using System.ComponentModel;
using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedStoreChannel : CachedCategorizableGuildChannel, IStoreChannel
    {
        public bool IsNsfw { get; private set; }

        public CachedStoreChannel(IGatewayClient client, ChannelJsonModel model)
            : base(client, model)
        { }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Update(ChannelJsonModel model)
        {
            base.Update(model);

            if (model.Nsfw.HasValue)
                IsNsfw = model.Nsfw.Value;
        }
    }
}
