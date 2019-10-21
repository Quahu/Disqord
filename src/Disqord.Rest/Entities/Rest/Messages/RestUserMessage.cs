﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed partial class RestUserMessage : RestMessage, IUserMessage
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

        IUser IMessage.Author => Author;

        internal RestUserMessage(RestDiscordClient client, MessageModel model) : base(client, model)
        {
            IsTextToSpeech = model.Tts.Value;
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

            if (model.Pinned.HasValue)
                IsPinned = model.Pinned.Value;

            base.Update(model);
        }

        public override string ToString()
            => $"{Author}: {Content}";
    }
}
