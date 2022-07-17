using System.Collections.Generic;
using Disqord.Models;

namespace Disqord;

public class TransientGuildEmoji : TransientClientEntity<EmojiJsonModel>, IGuildEmoji
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id!.Value;

    /// <inheritdoc/>
    public Snowflake GuildId { get; }

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name!;

    /// <inheritdoc/>
    public bool IsAnimated => Model.Animated.Value;

    /// <inheritdoc/>
    public IReadOnlyList<Snowflake> RoleIds => Model.Roles.Value;

    /// <inheritdoc/>
    public IUser Creator => _creator ??= new TransientUser(Client, Model.User.Value);

    private IUser? _creator;

    /// <inheritdoc/>
    public bool RequiresColons => Model.RequireColons.Value;

    /// <inheritdoc/>
    public bool IsManaged => Model.Managed.Value;

    /// <inheritdoc/>
    public bool IsAvailable => Model.Available.Value;

    /// <inheritdoc/>
    public string Tag => ToString();

    public TransientGuildEmoji(IClient client, Snowflake guildId, EmojiJsonModel model)
        : base(client, model)
    {
        GuildId = guildId;
    }

    public bool Equals(IEmoji? other)
    {
        return Comparers.Emoji.Equals(this, other);
    }

    public bool Equals(ICustomEmoji? other)
    {
        return Comparers.Emoji.Equals(this, other);
    }

    public override bool Equals(object? obj)
    {
        return obj is IEmoji other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Comparers.Emoji.GetHashCode(this);
    }

    public override string ToString()
    {
        return this.GetString();
    }
}
