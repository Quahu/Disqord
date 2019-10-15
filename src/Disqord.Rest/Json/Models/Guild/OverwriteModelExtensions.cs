using Disqord.Models;

namespace Disqord
{
    internal static partial class ModelExtensions
    {
        public static OverwriteModel ToModel(this LocalOverwrite overwrite)
            => new OverwriteModel
            {
                Id = overwrite.TargetId,
                Type = overwrite.TargetType,
                Allow = overwrite.Permissions.Allowed,
                Deny = overwrite.Permissions.Denied
            };
    }
}
