using Disqord.Models;

namespace Disqord.Rest
{
    public sealed partial class RestVoiceChannel : RestNestedChannel, IVoiceChannel
    {
        public int Bitrate { get; private set; }

        public int MemberLimit { get; private set; }

        internal RestVoiceChannel(RestDiscordClient client, ChannelModel model) : base(client, model)
        {
            Update(model);
        }

        internal override void Update(ChannelModel model)
        {
            if (model.Bitrate.HasValue)
                Bitrate = model.Bitrate.Value;

            if (model.UserLimit.HasValue)
                MemberLimit = model.UserLimit.Value;

            base.Update(model);
        }
    }
}
