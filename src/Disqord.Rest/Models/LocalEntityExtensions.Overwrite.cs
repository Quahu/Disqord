using Disqord.Models;

namespace Disqord.Rest.Models
{
    public static partial class LocalEntityExtensions
    {
        public static OverwriteJsonModel ToModel(this LocalOverwrite overwrite)
            => new OverwriteJsonModel
            {
                Id = overwrite.TargetId,
                Type = overwrite.TargetType,
                Allow = overwrite.Permissions.Allowed,
                Deny = overwrite.Permissions.Denied
            };
    }
}
