using System;
using System.Collections.Generic;
using System.ComponentModel;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedUserMessage : CachedMessage, IGatewayUserMessage
    {
        public DateTimeOffset? EditedAt { get; private set; }

        public Snowflake? WebhookId { get; }

        public bool IsTextToSpeech { get; }

        public Optional<string> Nonce { get; }

        public bool IsPinned { get; private set; }

        public bool MentionsEveryone { get; private set; }

        public IReadOnlyList<Snowflake> MentionedRoleIds { get; private set; }

        public IReadOnlyList<Attachment> Attachments { get; private set; }

        public IReadOnlyList<Embed> Embeds { get; private set; }

        public MessageActivity Activity { get; private set; }

        public MessageApplication Application { get; private set; }

        public MessageReference Reference { get; private set; }

        public MessageFlag Flags { get; private set; }

        public IReadOnlyList<Sticker> Stickers { get; private set; }

        public Optional<IUserMessage> ReferencedMessage { get; private set; }

        public CachedUserMessage(IGatewayClient client, MessageJsonModel model)
            : base(client, model)
        {
            WebhookId = model.WebhookId.GetValueOrNullable();
            IsTextToSpeech = model.Tts;
            Nonce = model.Nonce;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Update(MessageJsonModel model)
        {
            base.Update(model);

            EditedAt = model.EditedTimestamp;
            IsPinned = model.Pinned;
            MentionsEveryone = model.MentionEveryone;
            MentionedRoleIds = model.MentionRoles.ReadOnly();
            Attachments = model.Attachments.ToReadOnlyList(x => new Attachment(x));
            Embeds = model.Embeds.ToReadOnlyList(x => new Embed(x));
            Activity = Optional.ConvertOrDefault(model.Activity, x => new MessageActivity(x));
            Application = Optional.ConvertOrDefault(model.Application, x => new MessageApplication(x));
            Reference = Optional.ConvertOrDefault(model.MessageReference, x => new MessageReference(x));
            Flags = model.Flags.GetValueOrDefault();
            Stickers = Optional.ConvertOrDefault(model.Stickers, x => x.ToReadOnlyList(y => new Sticker(y)), Array.Empty<Sticker>());
            ReferencedMessage = Optional.Convert(model.ReferencedMessage, x => new TransientUserMessage(Client, x) as IUserMessage);
        }
    }
}
