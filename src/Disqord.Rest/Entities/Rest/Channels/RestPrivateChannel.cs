using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest
{
    public abstract class RestPrivateChannel : RestChannel, IRestMessageChannel, IPrivateChannel
    {
        public Snowflake? LastMessageId { get; private set; }

        public DateTimeOffset? LastPinTimestamp { get; private set; }

        public int Bitrate { get; private set; }

        internal RestPrivateChannel(RestDiscordClient client, ChannelModel model) : base(client, model)
        {
            Update(model);
        }

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
                    return new RestGroupDmChannel(client, model);

                default:
                    return null;
            }
        }

        public Task TriggerTypingIndicatorAsync(RestRequestOptions options = null)
            => Client.TriggerTypingIndicatorAsync(Id, options);

        public Task MarkAsReadAsync(RestRequestOptions options = null)
            => RestImplementation.MarkAsReadAsync(this, options);

        public IDisposable Typing()
            => new TypingRepeater(Client, this);

        public Task<RestUserMessage> SendMessageAsync(string content = null, bool isTTS = false, Embed embed = null, RestRequestOptions options = null)
            => Client.SendMessageAsync(Id, content, isTTS, embed, options);

        public Task<RestUserMessage> SendMessageAsync(LocalAttachment attachment, string content = null, bool isTTS = false, Embed embed = null, RestRequestOptions options = null)
            => Client.SendMessageAsync(Id, attachment, content, isTTS, embed, options);

        public Task<RestUserMessage> SendMessageAsync(IEnumerable<LocalAttachment> attachments, string content = null, bool isTTS = false, Embed embed = null, RestRequestOptions options = null)
            => Client.SendMessageAsync(Id, attachments, content, isTTS, embed, options);
    }
}
