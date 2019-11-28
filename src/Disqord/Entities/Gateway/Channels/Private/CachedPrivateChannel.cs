using System;
using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    public abstract partial class CachedPrivateChannel : CachedChannel, IPrivateChannel, ICachedMessageChannel
    {
        public Snowflake? LastMessageId { get; internal set; }

        public DateTimeOffset? LastPinTimestamp { get; internal set; }

        internal CachedPrivateChannel(DiscordClientBase client, ChannelModel model) : base(client, model)
        { }

        internal override void Update(ChannelModel model)
        {
            if (model.LastMessageId.HasValue)
                LastMessageId = model.LastMessageId.Value;

            if (model.LastPinTimestamp.HasValue)
                LastPinTimestamp = model.LastPinTimestamp.Value;

            base.Update(model);
        }

        internal static CachedPrivateChannel Create(DiscordClientBase client, ChannelModel model)
        {
            return model.Type.Value switch
            {
                ChannelType.Dm => new CachedDmChannel(client, model),
                ChannelType.Group => new CachedGroupChannel(client, model),
                _ => null,
            };
        }

        public CachedUserMessage GetMessage(Snowflake id)
            => Client.GetMessage(Id, id);

        public IReadOnlyList<CachedUserMessage> GetMessages()
            => Client.GetMessages(Id);
    }
}
