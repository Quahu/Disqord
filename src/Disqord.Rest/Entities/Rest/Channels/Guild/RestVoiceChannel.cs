using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestVoiceChannel : RestGuildChannel, IVoiceChannel
    {
        public int Bitrate { get; private set; }

        public int UserLimit { get; private set; }

        internal RestVoiceChannel(RestDiscordClient client, ChannelModel model, RestGuild guild = null) : base(client, model, guild)
        {
            Update(model);
        }

        internal override void Update(ChannelModel model)
        {
            Bitrate = model.Bitrate.Value;
            UserLimit = model.UserLimit.Value;

            base.Update(model);
        }
    }
}
