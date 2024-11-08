﻿using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;
// If you update any members of this class, make sure to do the same for the gateway equivalent.

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
    public IReadOnlyList<IAttachment> Attachments => _attachments ??= Model.Attachments.ToReadOnlyList(model => new TransientAttachment(model));

    private IReadOnlyList<IAttachment>? _attachments;

    /// <inheritdoc/>
    public IReadOnlyList<IEmbed> Embeds => _embeds ??= Model.Embeds.ToReadOnlyList(model => new TransientEmbed(model));

    private IReadOnlyList<IEmbed>? _embeds;

    /// <inheritdoc/>
    public IMessageActivity? Activity => Optional.ConvertOrDefault(Model.Activity, model => new TransientMessageActivity(model));

    /// <inheritdoc/>
    public IMessageApplication? Application => Optional.ConvertOrDefault(Model.Application, model => new TransientMessageApplication(model));

    /// <inheritdoc/>
    public Snowflake? ApplicationId => Model.ApplicationId.GetValueOrNullable();

    /// <inheritdoc/>
    public IMessageReference? Reference => Optional.ConvertOrDefault(Model.MessageReference, model => new TransientMessageReference(model));

    /// <inheritdoc/>
    public virtual Optional<IUserMessage?> ReferencedMessage
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
    private Optional<IUserMessage?>? _referencedMessage;

    public IMessageInteraction? Interaction
    {
        get
        {
            if (!Model.Interaction.HasValue)
                return null;

            if (_interaction == null)
            {
                var interactionModel = Model.Interaction.Value;
                IUser author;

                // TODO: test interaction
                // if (interactionModel.Member.TryGetValue(out var memberModel))
                // {
                //     memberModel.User = interactionModel.User;
                // }
                // else
                {
                    author = new TransientUser(Client, interactionModel.User);
                }

                _interaction = new TransientMessageInteraction(author, interactionModel);
            }

            return _interaction;
        }
    }
    private IMessageInteraction? _interaction;

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
    private IReadOnlyList<IRowComponent>? _components;

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
    private IReadOnlyList<IMessageSticker>? _stickers;

    /// <inheritdoc/>
    public IPoll? Poll => _poll ??= Optional.ConvertOrDefault(Model.Poll, poll => new TransientPoll(poll));

    private IPoll? _poll;

    public IReadOnlyList<IMessageSnapshot> MessageSnapshots
    {
        get
        {
            if (!Model.MessageSnapshots.HasValue)
                return Array.Empty<IMessageSnapshot>();

            return _messageSnapshots ??= Model.MessageSnapshots.Value.ToReadOnlyList(Client, (model, client) => new TransientMessageSnapshot(client, model));
        }
    }
    private IReadOnlyList<IMessageSnapshot>? _messageSnapshots;

    public TransientUserMessage(IClient client, MessageJsonModel model)
        : base(client, model)
    { }
}
