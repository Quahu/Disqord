﻿using Disqord.Models;
using Qommon;

namespace Disqord.Gateway;

public class CachedRole : CachedSnowflakeEntity, IRole
{
    /// <inheritdoc/>
    public Snowflake GuildId { get; }

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name { get; private set; } = null!;

    /// <inheritdoc/>
    public Color? Color { get; private set; }

    /// <inheritdoc/>
    public bool IsHoisted { get; private set; }

    /// <inheritdoc/>
    public string? IconHash { get; private set; }

    /// <inheritdoc/>
    public int Position { get; private set; }

    /// <inheritdoc/>
    public Permissions Permissions { get; private set; }

    /// <inheritdoc/>
    public bool IsManaged { get; private set; }

    /// <inheritdoc/>
    public bool IsMentionable { get; private set; }

    /// <inheritdoc/>
    public IEmoji? UnicodeEmoji { get; private set; }

    /// <inheritdoc/>
    public IRoleTags Tags { get; private set; } = null!;

    public string Mention => Disqord.Mention.Role(this);

    public CachedRole(IGatewayClient client, Snowflake guildId, RoleJsonModel model)
        : base(client, model.Id)
    {
        GuildId = guildId;

        Update(model);
    }

    public void Update(RoleJsonModel model)
    {
        Name = model.Name;
        Color = model.Color != 0 ? model.Color : null;
        IsHoisted = model.Hoist;
        IconHash = model.Icon.GetValueOrDefault();
        Position = model.Position;
        Permissions = model.Permissions;
        IsManaged = model.Managed;
        IsMentionable = model.Mentionable;
        UnicodeEmoji = Optional.ConvertOrDefault(model.UnicodeEmoji, emojiName => emojiName != null ? new TransientEmoji(null, emojiName) : null);
        Tags = Optional.ConvertOrDefault(model.Tags, model => new TransientRoleTags(model), IRoleTags.Empty);
    }
}
