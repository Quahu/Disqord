using System;

namespace Disqord
{
    public class LocalOverwrite : ILocalConstruct
    {
        public static LocalOverwrite Member(Snowflake memberId, OverwritePermissions permissions)
            => new(memberId, OverwriteTargetType.Member, permissions);

        public static LocalOverwrite Role(Snowflake roleId, OverwritePermissions permissions)
            => new(roleId, OverwriteTargetType.Role, permissions);

        public Snowflake TargetId { get; init; }

        public OverwritePermissions Permissions { get; init; }

        public OverwriteTargetType TargetType { get; init; }

        public LocalOverwrite()
        { }

        public LocalOverwrite(Snowflake targetId, OverwriteTargetType targetType, OverwritePermissions permissions)
        {
            TargetId = targetId;
            TargetType = targetType;
            Permissions = permissions;
        }

        public LocalOverwrite(IUser target, OverwritePermissions permissions)
            : this(target?.Id ?? throw new ArgumentNullException(nameof(target)), OverwriteTargetType.Member, permissions)
        { }

        public LocalOverwrite(IRole target, OverwritePermissions permissions)
            : this(target?.Id ?? throw new ArgumentNullException(nameof(target)), OverwriteTargetType.Role, permissions)
        { }

        public virtual LocalOverwrite Clone()
            => MemberwiseClone() as LocalOverwrite;

        object ICloneable.Clone()
            => Clone();

        public virtual void Validate()
        {
            if (TargetId == 0)
                throw new InvalidOperationException("The target ID of the overwrite must be set.");
        }
    }
}
