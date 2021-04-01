using System.ComponentModel;
using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedVoiceChannel : CachedNestableChannel, IVoiceChannel
    {
        public int Bitrate { get; private set; }

        public int MemberLimit { get; private set; }

        public string Region { get; private set; }

        public CachedVoiceChannel(IGatewayClient client, Snowflake guildId, ChannelJsonModel model)
            : base(client, guildId, model)
        { }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Update(ChannelJsonModel model)
        {
            base.Update(model);

            if (model.Bitrate.HasValue)
                Bitrate = model.Bitrate.Value;

            if (model.UserLimit.HasValue)
                MemberLimit = model.UserLimit.Value;

            if (model.RtcRegion.HasValue)
                Region = model.RtcRegion.Value;
        }
    }
}
