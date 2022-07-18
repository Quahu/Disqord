using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalApplicationCommandPermission : ILocalConstruct<LocalApplicationCommandPermission>, IJsonConvertible<ApplicationCommandPermissionsJsonModel>
{
    /// <summary>
    ///     Gets or sets the ID of the targeted entity.
    /// </summary>
    public Optional<Snowflake> TargetId { get; set; }

    /// <summary>
    ///     Gets or sets the type of the targeted entity.
    /// </summary>
    public Optional<ApplicationCommandPermissionTargetType> TargetType { get; set; }

    /// <summary>
    ///     Gets or sets whether the targeted entity is allowed through.
    /// </summary>
    public Optional<bool> HasPermission { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalApplicationCommandPermission"/>.
    /// </summary>
    public LocalApplicationCommandPermission()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalApplicationCommandPermission"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalApplicationCommandPermission(LocalApplicationCommandPermission other)
    {
        TargetId = other.TargetId;
        TargetType = other.TargetType;
        HasPermission = other.HasPermission;
    }

    /// <summary>
    ///     Instantiates a new <see cref="LocalApplicationCommandPermission"/>.
    /// </summary>
    /// <param name="targetId"> The ID of the targeted entity. </param>
    /// <param name="targetType"> The type of the targeted entity. </param>
    /// <param name="hasPermission"> Whether the targeted entity is allowed through. </param>
    public LocalApplicationCommandPermission(Snowflake targetId, ApplicationCommandPermissionTargetType targetType, bool hasPermission)
    {
        TargetId = targetId;
        TargetType = targetType;
        HasPermission = hasPermission;
    }

    /// <inheritdoc/>
    public virtual LocalApplicationCommandPermission Clone()
    {
        return new(this);
    }

    /// <inheritdoc />
    public ApplicationCommandPermissionsJsonModel ToModel()
    {
        OptionalGuard.HasValue(TargetId);
        OptionalGuard.HasValue(TargetType);
        OptionalGuard.HasValue(HasPermission);

        return new ApplicationCommandPermissionsJsonModel
        {
            Id = TargetId.Value,
            Type = TargetType.Value,
            Permission = HasPermission.Value
        };
    }
}
