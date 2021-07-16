using Disqord.Models;

namespace Disqord
{
    public interface IApplicationCommandPermissions : ISnowflakeEntity,
        IJsonUpdatable<ApplicationCommandPermissionsJsonModel>
    {
        ApplicationCommandPermissionType Type { get; }

        bool IsAllowed { get; }
    }
}