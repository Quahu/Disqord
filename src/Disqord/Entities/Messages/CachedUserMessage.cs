﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public sealed class CachedUserMessage : CachedMessage, IUserMessage
    {
        public override string Content => _content;

        public DateTimeOffset? EditedTimestamp { get; private set; }

        public bool IsTextToSpeech { get; }

        public bool MentionsEveryone { get; private set; }

        public IReadOnlyList<Snowflake> RoleIdsMentioned { get; private set; }

        public IReadOnlyList<Attachment> Attachments { get; private set; }

        public IReadOnlyList<Embed> Embeds { get; private set; }

        public Snowflake? Nonce { get; }

        public bool IsPinned { get; private set; }

        public Snowflake? WebhookId { get; }

        private string _content;

        internal CachedUserMessage(DiscordClient client, MessageModel model, ICachedMessageChannel channel, CachedUser author) : base(client, model, channel, author)
        {
            IsTextToSpeech = model.Tts.GetValueOrDefault();
            Nonce = model.Nonce.GetValueOrDefault();
            WebhookId = model.WebhookId.GetValueOrDefault();
            Update(model);
        }

        internal override void Update(MessageModel model)
        {
            if (model.EditedTimestamp.HasValue)
                EditedTimestamp = model.EditedTimestamp.Value;

            if (model.Content.HasValue)
                _content = model.Content.Value;

            if (model.MentionEveryone.HasValue)
                MentionsEveryone = model.MentionEveryone.Value;

            if (model.RoleMentions.HasValue)
                RoleIdsMentioned = model.RoleMentions.Value.Select(x => new Snowflake(x)).ToImmutableArray();

            if (model.Attachments.HasValue)
                Attachments = model.Attachments.Value.Select(x => x.ToAttachment()).ToImmutableArray();

            if (model.Embeds.HasValue)
                Embeds = model.Embeds.Value.Select(x => x.ToEmbed()).ToImmutableArray();

            // TODO: Are reactions ever available cached?
            //if (model.Reactions.HasValue)
            //    Reactions = model.Reactions.Value?.Select(x => CachedReaction.Create(Client, x)).ToImmutableArray() ?? ImmutableArray<CachedReaction>.Empty;

            if (model.Pinned.HasValue)
                IsPinned = model.Pinned.Value;

            base.Update(model);
        }

        internal CachedUserMessage Clone()
            => MemberwiseClone() as CachedUserMessage;

        public Task ModifyAsync(Action<ModifyMessageProperties> action, RestRequestOptions options = null)
            => Client.ModifyMessageAsync(Channel.Id, Id, action, options);

        public override string ToString()
            => $"{Author}: {Content}";

        private string DebuggerDisplay
            => $"{Id} sent by {Author}";
    }
}
