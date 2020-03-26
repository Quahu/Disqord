using System;
using System.Collections.Generic;
using System.Diagnostics;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public sealed partial class CachedUserMessage : CachedMessage, IUserMessage
    {
        public override string Content => _content;

        public DateTimeOffset? EditedAt { get; private set; }

        public bool IsTextToSpeech { get; }

        public bool MentionsEveryone { get; private set; }

        public IReadOnlyList<Snowflake> MentionedRoleIds { get; private set; }

        public IReadOnlyList<Attachment> Attachments { get; private set; }

        public IReadOnlyList<Embed> Embeds { get; private set; }

        public string Nonce { get; }

        public bool IsPinned { get; private set; }

        public Snowflake? WebhookId { get; }

        public MessageFlags Flags { get; private set; }

        public MessageActivity Activity { get; private set; }

        public MessageApplication Application { get; private set; }

        public MessageReference Reference { get; private set; }

        private string _content;

        internal CachedUserMessage(ICachedMessageChannel channel, CachedUser author, MessageModel model) : base(channel, author, model)
        {
            IsTextToSpeech = model.Tts.GetValueOrDefault();
            Nonce = model.Nonce.GetValueOrDefault();
            WebhookId = model.WebhookId.GetValueOrDefault();

            Update(model);
        }

        internal override void Update(MessageModel model)
        {
            if (model.EditedTimestamp.HasValue)
                EditedAt = model.EditedTimestamp.Value;

            if (model.Content.HasValue)
                _content = model.Content.Value;

            if (model.MentionEveryone.HasValue)
                MentionsEveryone = model.MentionEveryone.Value;

            if (model.RoleMentions.HasValue)
                MentionedRoleIds = model.RoleMentions.Value.ToSnowflakeList();

            if (model.Attachments.HasValue)
                Attachments = model.Attachments.Value.ToReadOnlyList(x => x.ToAttachment());

            if (model.Embeds.HasValue)
                Embeds = model.Embeds.Value.ToReadOnlyList(x => x.ToEmbed());

            if (model.Pinned.HasValue)
                IsPinned = model.Pinned.Value;

            if (model.Flags.HasValue)
                Flags = model.Flags.Value;

            if (model.Activity.HasValue)
                Activity = model.Activity.Value.ToActivity();

            if (model.Application.HasValue)
                Application = model.Application.Value.ToApplication();

            if (model.MessageReference.HasValue)
                Reference = model.MessageReference.Value.ToReference();

            base.Update(model);
        }

        internal CachedUserMessage Clone()
            => MemberwiseClone() as CachedUserMessage;

        public override string ToString()
            => $"{Author}: {Content}";

        private string DebuggerDisplay
            => $"{Id} sent by {Author}";
    }
}
