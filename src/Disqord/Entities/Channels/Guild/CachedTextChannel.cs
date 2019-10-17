using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord
{
    public sealed partial class CachedTextChannel : CachedNestedChannel, ITextChannel, ICachedMessageChannel
    {
        public string Topic { get; private set; }

        public bool IsNsfw { get; private set; }

        public int Slowmode { get; private set; }

        public bool IsNews { get; private set; }

        public Snowflake? LastMessageId { get; internal set; }

        public DateTimeOffset? LastPinTimestamp { get; private set; }

        public string Mention => Discord.MentionChannel(this);

        public string Tag => $"#{Name}";

        public IReadOnlyList<CachedUserMessage> CachedMessages
        {
            get
            {
                var cache = _cache;
                if (cache != null)
                    return cache;

                if (!Client.CachedMessages.TryGetValue(Id, out var result))
                    return ImmutableArray<CachedUserMessage>.Empty;

                var wrapper = new ReadOnlyList<CachedUserMessage>(result);
                _cache = wrapper;
                return wrapper;
            }
        }
        private IReadOnlyList<CachedUserMessage> _cache;

        internal CachedTextChannel(DiscordClient client, ChannelModel model, CachedGuild guild) : base(client, model, guild)
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

        public CachedUserMessage GetMessage(Snowflake id)
            => CachedMessages.FirstOrDefault(x => x.Id == id);

        public override string ToString()
            => Tag;
    }
}
