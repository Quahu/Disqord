using System;
using System.Collections.Generic;
using System.ComponentModel;
using Disqord.Gateway.Api.Models;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord.Gateway;

public class CachedUserMessage : CachedMessage, IGatewayUserMessage, IJsonUpdatable<MessageUpdateJsonModel>
{
    /// <inheritdoc/>
    public UserMessageType Type { get; }

    /// <inheritdoc/>
    public DateTimeOffset? EditedAt { get; private set; }

    /// <inheritdoc/>
    public Snowflake? WebhookId { get; }

    /// <inheritdoc/>
    public bool IsTextToSpeech { get; }

    /// <inheritdoc/>
    public Optional<string> Nonce { get; }

    /// <inheritdoc/>
    public bool IsPinned { get; private set; }

    /// <inheritdoc/>
    public bool MentionsEveryone { get; private set; }

    /// <inheritdoc/>
    public IReadOnlyList<Snowflake> MentionedRoleIds { get; private set; } = null!;

    /// <inheritdoc/>
    public IReadOnlyList<IAttachment> Attachments { get; private set; } = null!;

    /// <inheritdoc/>
    public IReadOnlyList<IEmbed> Embeds { get; private set; } = null!;

    /// <inheritdoc/>
    public IMessageActivity? Activity { get; private set; }

    /// <inheritdoc/>
    public IMessageApplication? Application { get; private set; }

    /// <inheritdoc/>
    public Snowflake? ApplicationId { get; }

    /// <inheritdoc/>
    public IMessageReference? Reference { get; private set; }

    /// <inheritdoc/>
    public Optional<IUserMessage?> ReferencedMessage { get; private set; }

    /// <inheritdoc/>
    public IMessageInteraction? Interaction { get; private set; }

    /// <inheritdoc/>
    public IReadOnlyList<IRowComponent> Components { get; private set; } = null!;

    /// <inheritdoc/>
    public IReadOnlyList<IMessageSticker> Stickers { get; private set; } = null!;

    public CachedUserMessage(IGatewayClient client, CachedMember? author, MessageJsonModel model)
        : base(client, author, model)
    {
        Type = model.Type;
        WebhookId = model.WebhookId.GetValueOrNullable();
        ApplicationId = model.ApplicationId.GetValueOrNullable();
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
        Attachments = model.Attachments.ToReadOnlyList(model => new TransientAttachment(model));
        Embeds = model.Embeds.ToReadOnlyList(model => new TransientEmbed(model));
        Activity = Optional.ConvertOrDefault(model.Activity, model => new TransientMessageActivity(model));
        Application = Optional.ConvertOrDefault(model.Application, model => new TransientMessageApplication(model));
        Reference = Optional.ConvertOrDefault(model.MessageReference, model => new TransientMessageReference(model));
        ReferencedMessage = Optional.Convert(model.ReferencedMessage, model => new TransientUserMessage(Client, model!) as IUserMessage)!;
        Interaction = Optional.ConvertOrDefault(model.Interaction, (model, client) => new TransientMessageInteraction(new TransientUser(client, model.User), model), Client);
        Components = Optional.ConvertOrDefault(model.Components, (models, client) => models.ToReadOnlyList(client, (model, client) => new TransientRowComponent(client, model) as IRowComponent), Client) ?? Array.Empty<IRowComponent>();
        Stickers = Optional.ConvertOrDefault(model.StickerItems, models => models.ToReadOnlyList(model => new TransientMessageSticker(model) as IMessageSticker), Array.Empty<IMessageSticker>());
    }

    public void Update(MessageUpdateJsonModel model)
    {
        if (model.Author.HasValue)
        {
            if (_author is TransientUser)
            {
                if (model.Member.HasValue)
                {
                    model.Member.Value.User = model.Author;
                    _author = new TransientMember(Client, GuildId!.Value, model.Member.Value);
                }
                else
                {
                    _author = new TransientUser(Client, model.Author.Value);
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
            Reactions = Optional.Convert(model.Reactions, models => models.ToReadOnlyDictionary(model => TransientEmoji.Create(model.Emoji), model => new TransientMessageReaction(model) as IMessageReaction));

        if (model.EditedTimestamp.HasValue)
            EditedAt = model.EditedTimestamp.Value;

        if (model.Pinned.HasValue)
            IsPinned = model.Pinned.Value;

        if (model.MentionEveryone.HasValue)
            MentionsEveryone = model.MentionEveryone.Value;

        if (model.MentionRoles.HasValue)
            MentionedRoleIds = model.MentionRoles.Value.ReadOnly();

        if (model.Attachments.HasValue)
            Attachments = model.Attachments.Value.ToReadOnlyList(model => new TransientAttachment(model));

        if (model.Embeds.HasValue)
            Embeds = model.Embeds.Value.ToReadOnlyList(model => new TransientEmbed(model));

        if (model.Activity.HasValue)
            Activity = Optional.ConvertOrDefault(model.Activity, model => new TransientMessageActivity(model));

        if (model.Application.HasValue)
            Application = Optional.ConvertOrDefault(model.Application, model => new TransientMessageApplication(model));

        if (model.MessageReference.HasValue)
            Reference = Optional.ConvertOrDefault(model.MessageReference, model => new TransientMessageReference(model));

        if (model.Flags.HasValue)
            Flags = model.Flags.Value;

        if (model.ReferencedMessage.HasValue)
            ReferencedMessage = Optional.Convert(model.ReferencedMessage, x => new TransientUserMessage(Client, x) as IUserMessage)!;

        if (model.Components.HasValue)
            Components = Optional.ConvertOrDefault(model.Components, (models, client) => models.ToReadOnlyList(client, (model, client) => new TransientRowComponent(client, model) as IRowComponent), Client) ?? Array.Empty<IRowComponent>();

        if (model.StickerItems.HasValue)
            Stickers = Optional.ConvertOrDefault(model.StickerItems, models => models.ToReadOnlyList(model => new TransientMessageSticker(model) as IMessageSticker), Array.Empty<IMessageSticker>());
    }
}
