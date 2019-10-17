using System;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed partial class RestTextChannel : RestNestedChannel, IRestMessageChannel, ITextChannel
    {
        public string Topic { get; private set; }

        public bool IsNsfw { get; private set; }

        public int Slowmode { get; private set; }

        public Snowflake? LastMessageId { get; private set; }

        public DateTimeOffset? LastPinTimestamp { get; private set; }

        public string Mention => Discord.MentionChannel(this);

        public string Tag => $"#{Name}";

        public bool IsNews { get; private set; }

        internal RestTextChannel(RestDiscordClient client, ChannelModel model) : base(client, model)
        {
            Update(model);
        }

        internal override void Update(ChannelModel model)
        {
            if (model.Topic.HasValue)
                Topic = model.Topic.Value;

            if (model.Nsfw.HasValue)
                IsNsfw = model.Nsfw.Value;

            if (model.RateLimitPerUser.HasValue)
                Slowmode = model.RateLimitPerUser.Value;

            if (model.Type.HasValue && model.Type == (IsNews ? ChannelType.Text : ChannelType.News))
                IsNews = !IsNews;

            if (model.LastMessageId.HasValue)
                LastMessageId = model.LastMessageId.Value;

            if (model.LastPinTimestamp.HasValue)
                LastPinTimestamp = model.LastPinTimestamp.Value;

            base.Update(model);
        }

        public override string ToString()
            => Tag;
    }
}
