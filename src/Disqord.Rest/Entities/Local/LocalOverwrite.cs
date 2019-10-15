using System;

namespace Disqord
{
    public sealed class LocalOverwrite
    {
        public Snowflake TargetId { get; }

        public OverwritePermissions Permissions { get; }

        public OverwriteTargetType TargetType { get; }

        public LocalOverwrite(Snowflake targetId, OverwriteTargetType targetType, OverwritePermissions permissions)
        {
            TargetId = targetId;
            TargetType = targetType;
            Permissions = permissions;
        }

        public LocalOverwrite(IUser target, OverwritePermissions permissions)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            TargetId = target.Id;
            TargetType = OverwriteTargetType.Member;
            Permissions = permissions;
        }

        public LocalOverwrite(IRole target, OverwritePermissions permissions)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            TargetId = target.Id;
            TargetType = OverwriteTargetType.Role;
            Permissions = permissions;
        }
    }
}
