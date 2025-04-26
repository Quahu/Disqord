using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon;
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
    public DateTimeOffset Timestamp => Model.Message.Timestamp;

    /// <inheritdoc/>
    public DateTimeOffset? EditedAt => Model.Message.EditedTimestamp;

    /// <inheritdoc/>
    public MessageFlags Flags => Model.Message.Flags.GetValueOrDefault();

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

    public IReadOnlyList<IComponent> Components
    {
        get
        {
            if (!Model.Message.Components.HasValue)
                return Array.Empty<IComponent>();

            return _components ??= Model.Message.Components.Value.ToReadOnlyList(Client, static (model, client) => TransientComponent.Create(client, model));
        }
    }
    private IReadOnlyList<IComponent>? _components;

    public TransientMessageSnapshot(IClient client, MessageSnapshotJsonModel model)
        : base(client, model)
    { }
}
