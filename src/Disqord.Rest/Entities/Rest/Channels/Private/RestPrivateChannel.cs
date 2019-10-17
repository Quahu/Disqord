using System;
using Disqord.Models;

namespace Disqord.Rest
{
    public abstract partial class RestPrivateChannel : RestChannel, IRestMessageChannel, IPrivateChannel
    {
        public Snowflake? LastMessageId { get; private set; }

        public DateTimeOffset? LastPinTimestamp { get; private set; }

        public int Bitrate { get; private set; }

        internal RestPrivateChannel(RestDiscordClient client, ChannelModel model) : base(client, model)
        { }

        internal override void Update(ChannelModel model)
        {
            if (model.LastMessageId.HasValue)
                LastMessageId = model.LastMessageId.Value;

            if (model.LastPinTimestamp.HasValue)
                LastPinTimestamp = model.LastPinTimestamp.Value;

            base.Update(model);
        }

        internal new static RestPrivateChannel Create(RestDiscordClient client, ChannelModel model)
        {
            switch (model.Type.Value)
            {
                case ChannelType.Dm:
                    return new RestDmChannel(client, model);

                case ChannelType.Group:
                    return new RestGroupChannel(client, model);

                default:
                    return null;
            }
        }
    }
}
