using Disqord.Models;

namespace Disqord.Rest
{
    public abstract class RestChannel : RestSnowflakeEntity, IChannel
    {
        public string Name { get; private set; }

        internal RestChannel(RestDiscordClient client, ChannelModel model) : base(client, model.Id)
        { }

        internal virtual void Update(ChannelModel model)
        {
            if (model.Name.HasValue)
                Name = model.Name.Value;
        }

        internal static RestChannel Create(RestDiscordClient client, ChannelModel model)
        {
            switch (model.Type.Value)
            {
                case ChannelType.Text:
                case ChannelType.Voice:
                case ChannelType.Category:
                case ChannelType.News:
                    return RestGuildChannel.Create(client, model);

                case ChannelType.Dm:
                case ChannelType.Group:
                    return RestPrivateChannel.Create(client, model);

                default:
                    return null;
            }
        }

        public override string ToString()
            => Name;
    }
}
