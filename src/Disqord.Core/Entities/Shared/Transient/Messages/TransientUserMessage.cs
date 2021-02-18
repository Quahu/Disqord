using System;
using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientUserMessage : TransientMessage, IUserMessage
    {
        public DateTimeOffset? EditedAt => Model.EditedTimestamp;

        public Snowflake? WebhookId => Model.WebhookId.GetValueOrNullable();

        public bool IsTextToSpeech => Model.Tts;

        public Optional<string> Nonce => Model.Nonce;

        public bool IsPinned => Model.Pinned;

        public bool MentionsEveryone => Model.MentionEveryone;

        public IReadOnlyList<Snowflake> MentionedRoleIds => Model.MentionRoles.ReadOnly();

        public MessageFlag Flags => Model.Flags.GetValueOrDefault();

        public TransientUserMessage(IClient client, MessageJsonModel model)
            : base(client, model)
        { }
    }
}
