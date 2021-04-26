using System;
using System.Collections.Generic;
using System.ComponentModel;
using Disqord.Api;
using Disqord.Collections;
using Disqord.Gateway.Api.Models;
using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedUserMessage : CachedMessage, IGatewayUserMessage, IJsonUpdatable<MessageUpdateJsonModel>
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

        public CachedUserMessage(IGatewayClient client, CachedMember author, MessageJsonModel model)
            : base(client, author, model)
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

            if (model.Type == MessageType.Reply || model.ReferencedMessage.GetValueOrDefault() != null)
            {
                // Fix for Discord always sending an empty property.
                ReferencedMessage = Optional.Convert(model.ReferencedMessage, x => new TransientUserMessage(Client, x) as IUserMessage);
            }
        }

        public void Update(MessageUpdateJsonModel model)
        {
            if (model.Author.HasValue)
            {
                if (_transientAuthor != null)
                {
                    if (model.Member.HasValue)
                    {
                        model.Member.Value.User = model.Author;
                        _transientAuthor = new TransientMember(Client, GuildId.Value, model.Member.Value);
                    }
                    else
                    {
                        _transientAuthor = new TransientUser(Client, model.Author.Value);
                    }
                }
            }

            if (model.Content.HasValue)
                Content = model.Content.Value;

            if (model.Mentions.HasValue)
                MentionedUsers = model.Mentions.Value.ToReadOnlyList(Client, (x, client) =>
                {
                    var user = client.GetUser(x.Id);
                    if (user != null)
                        return user;

                    return new TransientUser(client, x) as IUser;
                });

            if (model.Reactions.HasValue)
                Reactions = Optional.Convert(model.Reactions, x => x.ToReadOnlyDictionary(x => Emoji.Create(x.Emoji), x => new Reaction(x)));

            if (model.EditedTimestamp.HasValue)
                EditedAt = model.EditedTimestamp.Value;

            if (model.Pinned.HasValue)
                IsPinned = model.Pinned.Value;

            if (model.MentionEveryone.HasValue)
                MentionsEveryone = model.MentionEveryone.Value;

            if (model.MentionRoles.HasValue)
                MentionedRoleIds = model.MentionRoles.Value.ReadOnly();

            if (model.Attachments.HasValue)
                Attachments = model.Attachments.Value.ToReadOnlyList(x => new Attachment(x));

            if (model.Embeds.HasValue)
                Embeds = model.Embeds.Value.ToReadOnlyList(x => new Embed(x));

            if (model.Activity.HasValue)
                Activity = Optional.ConvertOrDefault(model.Activity, x => new MessageActivity(x));

            if (model.Application.HasValue)
                Application = Optional.ConvertOrDefault(model.Application, x => new MessageApplication(x));

            if (model.MessageReference.HasValue)
                Reference = Optional.ConvertOrDefault(model.MessageReference, x => new MessageReference(x));

            if (model.Flags.HasValue)
                Flags = model.Flags.Value;

            if (model.Stickers.HasValue)
                Stickers = Optional.ConvertOrDefault(model.Stickers, x => x.ToReadOnlyList(y => new Sticker(y)), Array.Empty<Sticker>());

            if (model.ReferencedMessage.HasValue)
                ReferencedMessage = Optional.Convert(model.ReferencedMessage, x => new TransientUserMessage(Client, x) as IUserMessage);
        }
    }
}
