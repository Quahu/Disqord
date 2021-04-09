using Disqord.Models;

namespace Disqord
{
    public class TransientVoiceChannel : TransientNestableChannel, IVoiceChannel
    {
        public int Bitrate => Model.Bitrate.Value;

        public int MemberLimit => Model.UserLimit.Value;

        public string Region => Model.RtcRegion.Value;

        public TransientVoiceChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
