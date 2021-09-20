using System;
using System.Collections.Generic;
using Qommon.Collections;
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

        public IReadOnlyList<Snowflake> MentionedRoleIds => Model.MentionRoles;

        public IReadOnlyList<Attachment> Attachments => _attachments ??= Model.Attachments.ToReadOnlyList(x => new Attachment(x));
        private IReadOnlyList<Attachment> _attachments;

        public IReadOnlyList<Embed> Embeds => _embeds ??= Model.Embeds.ToReadOnlyList(x => new Embed(x));

        private IReadOnlyList<Embed> _embeds;

        public MessageActivity Activity => Optional.ConvertOrDefault(Model.Activity, x => new MessageActivity(x));

        public MessageApplication Application => Optional.ConvertOrDefault(Model.Application, x => new MessageApplication(x));

        public Snowflake? ApplicationId => Model.ApplicationId.GetValueOrNullable();

        public MessageReference Reference => Optional.ConvertOrDefault(Model.MessageReference, x => new MessageReference(x));

        public MessageFlag Flags => Model.Flags.GetValueOrDefault();

        public virtual Optional<IUserMessage> ReferencedMessage
        {
            get
            {
                if (!Model.ReferencedMessage.HasValue)
                    return default;

                if (Model.ReferencedMessage.Value == null)
                    return null;

                return _referencedMessage ??= new TransientUserMessage(Client, Model.ReferencedMessage.Value);
            }
        }
        private Optional<IUserMessage>? _referencedMessage;

        public IReadOnlyList<IRowComponent> Components
        {
            get
            {
                if (!Model.Components.HasValue)
                    return Array.Empty<IRowComponent>();

                return _components ??= Model.Components.Value.ToReadOnlyList(Client, (model, client) => new TransientRowComponent(client, model));
            }
        }
        private IReadOnlyList<IRowComponent> _components;

        public IReadOnlyList<MessageSticker> Stickers
        {
            get
            {
                if (!Model.StickerItems.HasValue)
                    return Array.Empty<MessageSticker>();

                return _stickers ??= Model.StickerItems.Value.ToReadOnlyList(x => new MessageSticker(x));
            }
        }
        private IReadOnlyList<MessageSticker> _stickers;

        public TransientUserMessage(IClient client, MessageJsonModel model)
            : base(client, model)
        { }
    }
}
