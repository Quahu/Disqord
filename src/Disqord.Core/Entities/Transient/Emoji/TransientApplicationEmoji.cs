using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientApplicationEmoji : TransientClientEntity<EmojiJsonModel>, IApplicationEmoji
{
    /// <inheritdoc />
    public Snowflake Id => Model.Id!.Value;

    /// <inheritdoc />
    public Snowflake ApplicationId { get; }

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name!;

    /// <inheritdoc/>
    public bool IsAnimated => Model.Animated.Value;

    /// <inheritdoc/>
    public IUser? Creator => _creator ??= Optional.ConvertOrDefault(Model.User, static (user, client) => new TransientUser(client, user), Client);

    private IUser? _creator;

    /// <inheritdoc/>
    public bool RequiresColons => Model.RequireColons.Value;

    /// <inheritdoc/>
    public bool IsManaged => Model.Managed.Value;

    /// <inheritdoc/>
    public string Tag => ToString();

    public TransientApplicationEmoji(IClient client, Snowflake applicationId, EmojiJsonModel model)
        : base(client, model)
    {
        ApplicationId = applicationId;
    }

    public bool Equals(IEmoji? other)
    {
        return Comparers.Emoji.Equals(this, other);
    }

    public bool Equals(ICustomEmoji? other)
    {
        return Comparers.Emoji.Equals(this, other);
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