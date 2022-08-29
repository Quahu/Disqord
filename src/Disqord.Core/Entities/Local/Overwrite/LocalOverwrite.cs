using Disqord.Models;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents a local permission overwrite that can be applied to a channel.
/// </summary>
public class LocalOverwrite : ILocalConstruct<LocalOverwrite>, IJsonConvertible<OverwriteJsonModel>
{
    /// <summary>
    ///     Creates a <see cref="LocalOverwrite"/> with the target being the member with the specified ID.
    /// </summary>
    /// <param name="memberId"> The ID of the member. </param>
    /// <param name="permissions"> The permissions of the overwrite. </param>
    /// <returns>
    ///     A <see cref="LocalOverwrite"/> with the specified values set.
    /// </returns>
    public static LocalOverwrite Member(Snowflake memberId, OverwritePermissions permissions)
    {
        return new(memberId, OverwriteTargetType.Member, permissions);
    }

    /// <summary>
    ///     Creates a <see cref="LocalOverwrite"/> with the target being the role with the specified ID.
    /// </summary>
    /// <param name="roleId"> The ID of the role. </param>
    /// <param name="permissions"> The permissions of the overwrite. </param>
    /// <returns>
    ///     A <see cref="LocalOverwrite"/> with the specified values set.
    /// </returns>
    public static LocalOverwrite Role(Snowflake roleId, OverwritePermissions permissions)
    {
        return new(roleId, OverwriteTargetType.Role, permissions);
    }

    /// <summary>
    ///     Gets or sets the ID of the entity this overwrite targets.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<Snowflake> TargetId { get; set; }

    /// <summary>
    ///     Gets or sets the type of the entity this overwrite targets.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<OverwriteTargetType> TargetType { get; set; }

    /// <summary>
    ///     Gets or sets the overwritten permissions of this overwrite.
    /// </summary>
    public Optional<OverwritePermissions> Permissions { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalOverwrite"/>.
    /// </summary>
    public LocalOverwrite()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalOverwrite"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalOverwrite(LocalOverwrite other)
    {
        TargetId = other.TargetId;
        TargetType = other.TargetType;
        Permissions = other.Permissions;
    }

    /// <summary>
    ///     Instantiates a new <see cref="LocalOverwrite"/>.
    /// </summary>
    /// <param name="targetId"> The ID of the target entity. </param>
    /// <param name="targetType"> The type of the target entity. </param>
    /// <param name="permissions"> The overwritten permissions. </param>
    public LocalOverwrite(Snowflake targetId, OverwriteTargetType targetType, OverwritePermissions permissions)
    {
        TargetId = targetId;
        TargetType = targetType;
        Permissions = permissions;
    }

    /// <summary>
    ///     Clones this instance.
    /// </summary>
    /// <returns>
    ///     A deep copy of this instance.
    /// </returns>
    public virtual LocalOverwrite Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public virtual OverwriteJsonModel ToModel()
    {
        OptionalGuard.HasValue(TargetId);
        OptionalGuard.HasValue(TargetType);

        var overwritePermission = Permissions.GetValueOrDefault();
        return new OverwriteJsonModel
        {
            Id = TargetId.Value,
            Type = TargetType.Value,
            Allow = overwritePermission.Allowed,
            Deny = overwritePermission.Denied
        };
    }

    /// <summary>
    ///     Converts the specified overwrite to a <see cref="LocalOverwrite"/>.
    /// </summary>
    /// <param name="overwrite"> The overwrite to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalOverwrite"/>.
    /// </returns>
    public static LocalOverwrite CreateFrom(IOverwrite overwrite)
    {
        return new LocalOverwrite
        {
            TargetId = overwrite.TargetId,
            TargetType = overwrite.TargetType,
            Permissions = overwrite.Permissions
        };
    }
}
