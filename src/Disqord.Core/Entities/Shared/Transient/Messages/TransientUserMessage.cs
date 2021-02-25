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

        public IReadOnlyList<Snowflake> MentionedRoleIds => Model.MentionRoles;

        public IReadOnlyList<Attachment> Attachments
        {
            get
            {
                if (_attachments == null)
                    _attachments = Model.Attachments.ToReadOnlyList(x => new Attachment(x));

                return _attachments;
            }
        }
        private IReadOnlyList<Attachment>? _attachments;

        public IReadOnlyList<Embed> Embeds
        {
            get
            {
                if (_embeds == null)
                    _embeds = Model.Embeds.ToReadOnlyList(x => new Embed(x));

                return _embeds;
            }
        }
        private IReadOnlyList<Embed>? _embeds;

        public MessageActivity? Activity => Optional.ConvertOrDefault(Model.Activity, x => new MessageActivity(x));

        public MessageApplication? Application => Optional.ConvertOrDefault(Model.Application, x => new MessageApplication(x));

        public MessageReference? Reference => Optional.ConvertOrDefault(Model.MessageReference, x => new MessageReference(x));

        public MessageFlag Flags => Model.Flags.GetValueOrDefault();

        public IReadOnlyList<Sticker> Stickers
        {
            get
            {
                if (_stickers == null)
                    _stickers = Model.Stickers.Value.ToReadOnlyList(x => new Sticker(x));

                return _stickers;
            }
        }
        private IReadOnlyList<Sticker>? _stickers;

        public Optional<IUserMessage> ReferencedMessage
        {
            get
            {
                if (_referencedMessage == null)
                    _referencedMessage = Optional.Convert(Model.ReferencedMessage, x => new TransientUserMessage(Client, x) as IUserMessage);

                return _referencedMessage.Value;
            }
        }
        private Optional<IUserMessage>? _referencedMessage;

        public TransientUserMessage(IClient client, MessageJsonModel model)
            : base(client, model)
        { }
    }
}
