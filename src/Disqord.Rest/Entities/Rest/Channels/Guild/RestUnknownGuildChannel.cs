using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestUnknownGuildChannel : RestGuildChannel, IUnknownGuildChannel
    {
        public byte Type { get; }

        internal RestUnknownGuildChannel(RestDiscordClient client, ChannelModel model) : base(client, model)
        {
            Type = (byte) model.Type.Value;
        }
    }
}
