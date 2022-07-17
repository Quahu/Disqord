using System.ComponentModel;
using Disqord.Models;

namespace Disqord.Gateway;

public abstract class CachedUser : CachedSnowflakeEntity, IUser
{
    /// <inheritdoc cref="INamableEntity.Name"/>
    public abstract string Name { get; }

    /// <inheritdoc/>
    public abstract string Discriminator { get; }

    /// <inheritdoc/>
    public abstract string? AvatarHash { get; }

    /// <inheritdoc/>
    public abstract bool IsBot { get; }

    /// <inheritdoc/>
    public abstract UserFlags PublicFlags { get; }

    /// <inheritdoc/>
    public string Mention => Disqord.Mention.User(this);

    /// <inheritdoc/>
    public string Tag => $"{Name}#{Discriminator}";

    /// <summary>
    ///     Instantiates a new user from the provided client and model.
    /// </summary>
    /// <remarks>
    ///     This constructor should be used exclusively by sharee user implementations.
    /// </remarks>
    /// <param name="client"> The client that created this user. </param>
    /// <param name="id"> The ID of the user. </param>
    protected CachedUser(IGatewayClient client, Snowflake id)
        : base(client, id)
    { }

    /// <summary>
    ///     Instantiates a new user from the provided client and model.
    /// </summary>
    /// <remarks>
    ///     This constructor should be used exclusively by shared user implementations.
    /// </remarks>
    /// <param name="client"> The client that created this user. </param>
    /// <param name="model"> The model to create the user from. </param>
    protected CachedUser(IGatewayClient client, UserJsonModel model)
        : base(client, model.Id)
    { }

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract void Update(UserJsonModel model);

    /// <inheritdoc/>
    public override string ToString()
    {
        return Tag;
    }
}
