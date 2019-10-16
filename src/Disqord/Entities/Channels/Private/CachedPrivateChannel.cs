using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Models;
using Disqord.Rest;
using Qommon.Collections;

namespace Disqord
{
    public abstract class CachedPrivateChannel : CachedChannel, IPrivateChannel, ICachedMessageChannel
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

        public Task TriggerTypingIndicatorAsync(RestRequestOptions options = null)
            => Client.RestClient.TriggerTypingIndicatorAsync(Id, options);

        public Task MarkAsReadAsync(RestRequestOptions options = null)
            => RestImplementation.MarkAsReadAsync(this, options);

        public IDisposable Typing()
            => new TypingRepeater(Client.RestClient, this);

        public Task CloseAsync(RestRequestOptions options = null)
            => Client.RestClient.DeleteOrCloseChannelAsync(Id, options);

        public Task<RestUserMessage> SendMessageAsync(string content = null, bool isTTS = false, Embed embed = null, RestRequestOptions options = null)
            => Client.RestClient.SendMessageAsync(Id, content, isTTS, embed, options);

        public Task<RestUserMessage> SendMessageAsync(LocalAttachment attachment, string content = null, bool isTTS = false, Embed embed = null, RestRequestOptions options = null)
            => Client.RestClient.SendMessageAsync(Id, attachment, content, isTTS, embed, options);

        public Task<RestUserMessage> SendMessageAsync(IEnumerable<LocalAttachment> attachments, string content = null, bool isTTS = false, Embed embed = null, RestRequestOptions options = null)
            => Client.RestClient.SendMessageAsync(Id, attachments, content, isTTS, embed, options);
    }
}
