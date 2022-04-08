using System.ComponentModel;
using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedVoiceChannel : CachedVocalGuildChannel, IVoiceChannel
    {
        public int MemberLimit { get; private set; }

        public VideoQualityMode VideoQualityMode { get; private set; }

        public CachedVoiceChannel(IGatewayClient client, ChannelJsonModel model)
            : base(client, model)
        { }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Update(ChannelJsonModel model)
        {
            base.Update(model);

            if (model.UserLimit.HasValue)
                MemberLimit = model.UserLimit.Value;
        }
    }
}
