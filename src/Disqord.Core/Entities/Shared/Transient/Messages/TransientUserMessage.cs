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

        public IReadOnlyList<Attachment> Attachments => _attachments ??= Model.Attachments.ToReadOnlyList(x => new Attachment(x));
        private IReadOnlyList<Attachment> _attachments;

        public IReadOnlyList<Embed> Embeds => _embeds ??= Model.Embeds.ToReadOnlyList(x => new Embed(x));
        private IReadOnlyList<Embed> _embeds;

        public MessageActivity Activity => Optional.ConvertOrDefault(Model.Activity, x => new MessageActivity(x));

        public MessageApplication Application => Optional.ConvertOrDefault(Model.Application, x => new MessageApplication(x));

        public MessageReference Reference => Optional.ConvertOrDefault(Model.MessageReference, x => new MessageReference(x));

        public MessageFlag Flags => Model.Flags.GetValueOrDefault();

        public IReadOnlyList<Sticker> Stickers => _stickers ??= Optional.ConvertOrDefault(Model.Stickers, x => x.ToReadOnlyList(x => new Sticker(x)), Array.Empty<Sticker>());
        private IReadOnlyList<Sticker> _stickers;

        public virtual Optional<IUserMessage> ReferencedMessage => _referencedMessage ??= Optional.Convert(Model.ReferencedMessage, x => new TransientUserMessage(Client, x) as IUserMessage);
        private Optional<IUserMessage>? _referencedMessage;

        public IReadOnlyList<IComponent> Components => _components ??= Optional.ConvertOrDefault(Model.Components, (models, client) => models.ToReadOnlyList(client, (model, client) => TransientComponent.Create(client, model)), Client) ?? Array.Empty<IComponent>();
        private IReadOnlyList<IComponent> _components;

        public TransientUserMessage(IClient client, MessageJsonModel model)
            : base(client, model)
        { }
    }
}
