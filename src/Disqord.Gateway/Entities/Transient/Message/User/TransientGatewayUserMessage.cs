using System;
using System.Collections.Generic;
using Disqord.Gateway;
using Disqord.Models;
using Qommon;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord
{
    public class TransientGatewayUserMessage : TransientGatewayMessage, IGatewayUserMessage
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
        public IReadOnlyList<IAttachment> Attachments => _attachments ??= Model.Attachments.ToReadOnlyList(model => new TransientAttachment(model));
        private IReadOnlyList<IAttachment> _attachments;

        /// <inheritdoc/>
        public IReadOnlyList<IEmbed> Embeds => _embeds ??= Model.Embeds.ToReadOnlyList(model => new TransientEmbed(model));
        private IReadOnlyList<IEmbed> _embeds;

        /// <inheritdoc/>
        public IMessageActivity Activity => Optional.ConvertOrDefault(Model.Activity, model => new TransientMessageActivity(model));

        /// <inheritdoc/>
        public IMessageApplication Application => Optional.ConvertOrDefault(Model.Application, model => new TransientMessageApplication(model));

        /// <inheritdoc/>
        public Snowflake? ApplicationId => Model.ApplicationId.GetValueOrNullable();

        /// <inheritdoc/>
        public IMessageReference Reference => Optional.ConvertOrDefault(Model.MessageReference, model => new TransientMessageReference(model));

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
        public IReadOnlyList<IMessageSticker> Stickers
        {
            get
            {
                if (!Model.StickerItems.HasValue)
                    return Array.Empty<IMessageSticker>();

                return _stickers ??= Model.StickerItems.Value.ToReadOnlyList(model => new TransientMessageSticker(model));
            }
        }
        private IReadOnlyList<IMessageSticker> _stickers;

        public TransientGatewayUserMessage(IClient client, MessageJsonModel model)
            : base(client, model)
        { }
    }
}
