using Disqord.Models;
using Qommon;

namespace Disqord;

public static class LocalApplicationCommandPermissionExtensions
{
    public static TPermission WithTargetId<TPermission>(this TPermission permission, Snowflake targetId)
        where TPermission : LocalApplicationCommandPermission
    {
        permission.TargetId = targetId;
        return permission;
    }

    public static TPermission WithType<TPermission>(this TPermission permission, ApplicationCommandPermissionTargetType targetType)
        where TPermission : LocalApplicationCommandPermission
    {
        permission.TargetType = targetType;
        return permission;
    }

    public static TPermission WithHasPermission<TPermission>(this TPermission permission, bool hasPermission = true)
        where TPermission : LocalApplicationCommandPermission
    {
        permission.HasPermission = hasPermission;
        return permission;
    }

    public static ApplicationCommandPermissionsJsonModel ToModel(this LocalApplicationCommandPermission permission)
    {
        return new ApplicationCommandPermissionsJsonModel
        {
            Id = permission.TargetId.Value,
            Type = permission.TargetType.Value,
            Permission = permission.HasPermission.GetValueOrDefault()
        };
    }
}
