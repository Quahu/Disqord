using System.ComponentModel;
using Disqord.Models;
using Qommon;

namespace Disqord.Gateway
{
    public class CachedVoiceChannel : CachedVocalGuildChannel, IVoiceChannel
    {
        public int MemberLimit { get; private set; }

        public VideoQualityMode VideoQualityMode { get; private set; }

        public CachedVoiceChannel(IGatewayClient client, ChannelJsonModel model)
            : base(client, model)
        {
            VideoQualityMode = model.VideoQualityMode.GetValueOrDefault(VideoQualityMode.Automatic);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Update(ChannelJsonModel model)
        {
            base.Update(model);

            if (model.UserLimit.HasValue)
                MemberLimit = model.UserLimit.Value;

            if (model.VideoQualityMode.HasValue)
                VideoQualityMode = model.VideoQualityMode.Value;
        }
    }
}
