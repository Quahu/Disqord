using Disqord.Models;

namespace Disqord
{
    public class TransientVoiceChannel : TransientVocalGuildChannel, IVoiceChannel
    {
        public int MemberLimit => Model.UserLimit.Value;

        public VideoQualityMode VideoQualityMode => Model.VideoQualityMode.GetValueOrDefault(VideoQualityMode.Auto);

        public TransientVoiceChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
