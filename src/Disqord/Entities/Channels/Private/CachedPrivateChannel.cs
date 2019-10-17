using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord
{
    public abstract partial class CachedPrivateChannel : CachedChannel, IPrivateChannel, ICachedMessageChannel
    {
        public Snowflake? LastMessageId { get; internal set; }

        public DateTimeOffset? LastPinTimestamp { get; private set; }

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

        internal CachedPrivateChannel(DiscordClient client, ChannelModel model) : base(client, model)
        { }

        internal override void Update(ChannelModel model)
        {
            if (model.LastMessageId.HasValue)
                LastMessageId = model.LastMessageId.Value;

            if (model.LastPinTimestamp.HasValue)
                LastPinTimestamp = model.LastPinTimestamp.Value;

            base.Update(model);
        }

        internal static CachedPrivateChannel Create(DiscordClient client, ChannelModel model)
        {
            switch (model.Type.Value)
            {
                case ChannelType.Dm:
                    return new CachedDmChannel(client, model);

                case ChannelType.Group:
                    return new CachedGroupChannel(client, model);

                default:
                    return null;
            }
        }

        public CachedUserMessage GetMessage(Snowflake id)
            => CachedMessages.FirstOrDefault(x => x.Id == id);
    }
}
