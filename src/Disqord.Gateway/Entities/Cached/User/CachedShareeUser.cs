using System.ComponentModel;
using Disqord.Models;

namespace Disqord.Gateway;

/// <summary>
///     Represents a user that references a shared user instance.
/// </summary>
public abstract class CachedShareeUser : CachedUser
{
    /// <inheritdoc/>
    public override string Name => SharedUser.Name;

    /// <inheritdoc/>
    public override string Discriminator => SharedUser.Discriminator;

    /// <inheritdoc/>
    public override string? AvatarHash => SharedUser.AvatarHash;

    /// <inheritdoc/>
    public override bool IsBot => SharedUser.IsBot;

    /// <inheritdoc/>
    public override UserFlags PublicFlags => SharedUser.PublicFlags;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public CachedSharedUser SharedUser { get; private set; }

    /// <summary>
    ///     Instantiates a new sharee user from the provided <see cref="ICachedSharedUser"/>.
    /// </summary>
    /// <param name="sharedUser"></param>
    protected CachedShareeUser(CachedSharedUser sharedUser)
        : base(sharedUser.Client, sharedUser.Id)
    {
        SharedUser = sharedUser;
        sharedUser.AddReference(this);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void SetSharedUser(CachedSharedUser sharedUser)
    {
        SharedUser = sharedUser;
    }

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override void Update(UserJsonModel model)
    {
        SharedUser.Update(model);
    }
}
