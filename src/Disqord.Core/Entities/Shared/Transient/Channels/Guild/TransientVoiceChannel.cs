using Disqord.Models;

namespace Disqord
{
    public class TransientVoiceChannel : TransientVocalGuildChannel, IVoiceChannel
    {
        public int MemberLimit => Model.UserLimit.Value;

        public TransientVoiceChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
