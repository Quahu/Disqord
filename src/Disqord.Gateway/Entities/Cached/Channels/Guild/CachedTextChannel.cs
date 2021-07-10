using System;
using System.ComponentModel;
using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedTextChannel : CachedNestableChannel, ITextChannel
    {
        public string Topic { get; private set; }

        public bool IsNsfw { get; private set; }

        public int Slowmode { get; private set; }

        public bool IsNews => _type == ChannelType.News;

        public bool IsStore => _type == ChannelType.Store;

        private ChannelType _type;

        public Snowflake? LastMessageId { get; set; }

        public DateTimeOffset? LastPinTimestamp { get; set; }

        public string Mention => Disqord.Mention.TextChannel(this);

        public string Tag => $"#{Name}";

        public CachedTextChannel(IGatewayClient client, Snowflake guildId, ChannelJsonModel model)
            : base(client, guildId, model)
        { }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Update(ChannelJsonModel model)
        {
            base.Update(model);

            if (model.Topic.HasValue)
                Topic = model.Topic.Value;

            if (model.Nsfw.HasValue)
                IsNsfw = model.Nsfw.Value;

            if (model.RateLimitPerUser.HasValue)
                Slowmode = model.RateLimitPerUser.Value;

            _type = model.Type;

            if (model.LastMessageId.HasValue)
                LastMessageId = model.LastMessageId.Value;

            if (model.LastPinTimestamp.HasValue)
                LastPinTimestamp = model.LastPinTimestamp.Value;
        }
    }
}
