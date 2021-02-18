using Disqord.Models;

namespace Disqord
{
    public class TransientVoiceChannel : TransientNestableChannel, IVoiceChannel
    {
        public int MemberLimit => Model.UserLimit.Value;

        public int Bitrate => Model.Bitrate.Value;

        public TransientVoiceChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
