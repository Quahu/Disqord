using System;
using Disqord.Models;

namespace Disqord
{
    public class TransientTextChannel : TransientNestableChannel, ITextChannel
    {
        public string Topic => Model.Topic.Value;

        public bool IsNsfw => Model.Nsfw.Value;

        public int Slowmode => Model.RateLimitPerUser.Value;

        public bool IsNews => Model.Type == ChannelType.News;

        public bool IsStore => Model.Type == ChannelType.Store;

        public bool IsThread => Model.Type == ChannelType.Thread;

        public Snowflake? LastMessageId => Model.LastMessageId.Value;

        public DateTimeOffset? LastPinTimestamp => Model.LastPinTimestamp.Value;

        public string Mention => Disqord.Mention.TextChannel(this);

        public string Tag => $"#{Name}";

        public TransientTextChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
