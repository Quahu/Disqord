using System;
using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    public sealed partial class CachedTextChannel : CachedNestedChannel, ITextChannel, ICachedMessageChannel
    {
        public string Topic { get; private set; }

        public bool IsNsfw { get; private set; }

        public int Slowmode { get; private set; }

        public bool IsNews { get; private set; }

        public bool IsStore { get; private set; }

        public Snowflake? LastMessageId { get; internal set; }

        public DateTimeOffset? LastPinTimestamp { get; internal set; }

        public string Mention => Discord.MentionChannel(this);

        public string Tag => $"#{Name}";

        internal CachedTextChannel(CachedGuild guild, ChannelModel model) : base(guild, model)
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

            if (model.Type.HasValue)
            {
                if (model.Type == (IsNews ? ChannelType.Text : ChannelType.News))
                    IsNews = !IsNews;

                else if (model.Type == (IsStore ? ChannelType.Text : ChannelType.Store))
                    IsStore = !IsStore;
            }

            if (model.LastMessageId.HasValue)
                LastMessageId = model.LastMessageId.Value;

            if (model.LastPinTimestamp.HasValue)
                LastPinTimestamp = model.LastPinTimestamp.Value;

            base.Update(model);
        }

        public CachedUserMessage GetMessage(Snowflake id)
            => Client.GetMessage(Id, id);

        public IReadOnlyList<CachedUserMessage> GetMessages()
            => Client.GetMessages(Id);

        public override string ToString()
            => Tag;
    }
}
