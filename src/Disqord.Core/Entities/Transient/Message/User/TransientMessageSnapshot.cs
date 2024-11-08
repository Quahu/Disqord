﻿using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientMessageSnapshot : TransientClientEntity<MessageSnapshotJsonModel>, IMessageSnapshot
{
    /// <inheritdoc/>
    public UserMessageType Type => Model.Message.Type;

    /// <inheritdoc/>
    public string Content => Model.Message.Content;
    
    /// <inheritdoc/>
    public IReadOnlyList<IUser> MentionedUsers => _mentionedUsers ??= Model.Message.Mentions.ToReadOnlyList(Client, (model, client) => new TransientUser(client, model));

    private IReadOnlyList<IUser>? _mentionedUsers;

    /// <inheritdoc/>
    public IReadOnlyList<Snowflake> MentionedRoleIds => Model.Message.MentionRoles;
    
    /// <inheritdoc/>
    public IReadOnlyList<IAttachment> Attachments => _attachments ??= Model.Message.Attachments.ToReadOnlyList(model => new TransientAttachment(model));

    private IReadOnlyList<IAttachment>? _attachments;
    
    /// <inheritdoc/>
    public IReadOnlyList<IEmbed> Embeds => _embeds ??= Model.Message.Embeds.ToReadOnlyList(model => new TransientEmbed(model));

    private IReadOnlyList<IEmbed>? _embeds;
    
    /// <inheritdoc/>
    public DateTimeOffset Timestamp { get; }
    
    /// <inheritdoc/>
    public DateTimeOffset? EditedAt { get; }
    
    /// <inheritdoc/>
    public MessageFlags Flags { get; }
    
    /// <inheritdoc/>
    public IReadOnlyList<IMessageSticker> Stickers
    {
        get
        {
            if (!Model.Message.StickerItems.HasValue)
                return Array.Empty<IMessageSticker>();

            return _stickers ??= Model.Message.StickerItems.Value.ToReadOnlyList(model => new TransientMessageSticker(model));
        }
    }

    private IReadOnlyList<IMessageSticker>? _stickers;
    
    public IReadOnlyList<IRowComponent> Components
    {
        get
        {
            if (!Model.Message.Components.HasValue)
                return Array.Empty<IRowComponent>();

            return _components ??= Model.Message.Components.Value.ToReadOnlyList(Client, (model, client) => new TransientRowComponent(client, model));
        }
    }
    private IReadOnlyList<IRowComponent>? _components;
    
    public TransientMessageSnapshot(IClient client, MessageSnapshotJsonModel model) 
        : base(client, model)
    { }
}