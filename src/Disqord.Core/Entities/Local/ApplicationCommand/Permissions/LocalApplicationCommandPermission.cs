using Qommon;

namespace Disqord;

public class LocalApplicationCommandPermission : ILocalConstruct<LocalApplicationCommandPermission>
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
    ///     Instantiates a new <see cref="LocalApplicationCommandPermission"/>.
    /// </summary>
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

    public virtual LocalApplicationCommandPermission Clone()
    {
        return new(this);
    }
}
