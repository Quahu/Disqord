using System.ComponentModel;
using Disqord.Models;

namespace Disqord.Gateway
{
    public abstract class CachedVocalGuildChannel : CachedCategorizableGuildChannel, IVocalGuildChannel
    {
        public int Bitrate { get; private set; }

        public string Region { get; private set; }

        protected CachedVocalGuildChannel(IGatewayClient client, ChannelJsonModel model)
            : base(client, model)
        { }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Update(ChannelJsonModel model)
        {
            base.Update(model);

            if (model.Bitrate.HasValue)
                Bitrate = model.Bitrate.Value;

            if (model.RtcRegion.HasValue)
                Region = model.RtcRegion.Value;
        }
    }
}
