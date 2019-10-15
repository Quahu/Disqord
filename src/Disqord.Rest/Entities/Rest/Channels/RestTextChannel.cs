using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestTextChannel : RestGuildChannel, IRestMessageChannel, ITextChannel
    {
        public string Topic { get; private set; }

        public bool IsNsfw { get; private set; }

        public int Slowmode { get; private set; }

        public Snowflake? LastMessageId { get; private set; }

        public DateTimeOffset? LastPinTimestamp { get; private set; }

        public string Mention => Discord.MentionChannel(this);

        public string Tag => $"#{Name}";

        public bool IsNews { get; private set; }

        internal RestTextChannel(RestDiscordClient client, ChannelModel model, RestGuild guild = null) : base(client, model, guild)
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

        public Task TriggerTypingIndicatorAsync(RestRequestOptions options = null)
            => Client.TriggerTypingIndicatorAsync(Id, options);

        public Task MarkAsReadAsync(RestRequestOptions options = null)
        {
            var lastMessageId = LastMessageId;
            if (!lastMessageId.HasValue)
                throw new InvalidOperationException("Channel has no last message id.");

            return Client.MarkMessageAsReadAsync(Id, lastMessageId.Value, options);
        }

        public IDisposable Typing()
            => new TypingRepeater(Client, this);

        public Task<RestUserMessage> SendMessageAsync(string content = null, bool isTTS = false, Embed embed = null, RestRequestOptions options = null)
            => Client.SendMessageAsync(Id, content, isTTS, embed, options);

        public Task<RestUserMessage> SendMessageAsync(LocalAttachment attachment, string content = null, bool isTTS = false, Embed embed = null, RestRequestOptions options = null)
            => Client.SendMessageAsync(Id, attachment, content, isTTS, embed, options);

        public Task<RestUserMessage> SendMessageAsync(IEnumerable<LocalAttachment> attachments, string content = null, bool isTTS = false, Embed embed = null, RestRequestOptions options = null)
            => Client.SendMessageAsync(Id, attachments, content, isTTS, embed, options);

        public override string ToString()
            => Tag;
    }
}
