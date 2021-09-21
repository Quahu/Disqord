using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord
{
    public class TransientUserMessage : TransientMessage, IUserMessage
    {
        /// <inheritdoc/>
        public UserMessageType Type => Model.Type;

        /// <inheritdoc/>
        public DateTimeOffset? EditedAt => Model.EditedTimestamp;

        /// <inheritdoc/>
        public Snowflake? WebhookId => Model.WebhookId.GetValueOrNullable();

        /// <inheritdoc/>
        public bool IsTextToSpeech => Model.Tts;

        /// <inheritdoc/>
        public Optional<string> Nonce => Model.Nonce;

        /// <inheritdoc/>
        public bool IsPinned => Model.Pinned;

        /// <inheritdoc/>
        public bool MentionsEveryone => Model.MentionEveryone;

        /// <inheritdoc/>
        public IReadOnlyList<Snowflake> MentionedRoleIds => Model.MentionRoles;

        /// <inheritdoc/>
        public IReadOnlyList<Attachment> Attachments => _attachments ??= Model.Attachments.ToReadOnlyList(x => new Attachment(x));
        private IReadOnlyList<Attachment> _attachments;

        /// <inheritdoc/>
        public IReadOnlyList<Embed> Embeds => _embeds ??= Model.Embeds.ToReadOnlyList(x => new Embed(x));
        private IReadOnlyList<Embed> _embeds;

        /// <inheritdoc/>
        public MessageActivity Activity => Optional.ConvertOrDefault(Model.Activity, x => new MessageActivity(x));

        /// <inheritdoc/>
        public MessageApplication Application => Optional.ConvertOrDefault(Model.Application, x => new MessageApplication(x));

        /// <inheritdoc/>
        public Snowflake? ApplicationId => Model.ApplicationId.GetValueOrNullable();

        /// <inheritdoc/>
        public MessageReference Reference => Optional.ConvertOrDefault(Model.MessageReference, x => new MessageReference(x));

        /// <inheritdoc/>
        public MessageFlag Flags => Model.Flags.GetValueOrDefault();

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
