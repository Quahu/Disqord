using Disqord.Models;

namespace Disqord
{
    public static class LocalApplicationCommandPermissionExtensions
    {
        public static LocalApplicationCommandPermission WithTargetId(this LocalApplicationCommandPermission permission, Snowflake targetId)
        {
            permission.TargetId = targetId;
            return permission;
        }

        public static LocalApplicationCommandPermission WithType(this LocalApplicationCommandPermission permission, ApplicationCommandPermissionType type)
        {
            permission.Type = type;
            return permission;
        }

        public static LocalApplicationCommandPermission WithHasPermission(this LocalApplicationCommandPermission permission, bool hasPermission)
        {
            permission.HasPermission = hasPermission;
            return permission;
        }

        public static ApplicationCommandPermissionsJsonModel ToModel(this LocalApplicationCommandPermission permission)
        {
            return new ApplicationCommandPermissionsJsonModel
            {
                Id = permission.TargetId.Value,
                Type = permission.Type.Value,
                Permission = permission.HasPermission.Value
            };
        }
    }
}
