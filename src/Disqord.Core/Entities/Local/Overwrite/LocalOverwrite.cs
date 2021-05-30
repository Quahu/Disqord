using System;

namespace Disqord
{
    public sealed class LocalOverwrite : ILocalConstruct
    {
        public Snowflake TargetId { get; set; }

        public OverwritePermissions Permissions { get; set; }

        public OverwriteTargetType TargetType { get; set; }

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

        public LocalOverwrite(LocalOverwrite other)
        {
            TargetId = other.TargetId;
            Permissions = other.Permissions;
            TargetType = other.TargetType;
        }

        public LocalOverwrite Clone()
            => new(this);

        object ICloneable.Clone()
            => throw new NotImplementedException();

        public void Validate()
        { }
    }
}
