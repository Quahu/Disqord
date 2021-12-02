namespace Disqord.Models
{
    public static partial class LocalEntityExtensions
    {
        public static OverwriteJsonModel ToModel(this LocalOverwrite overwrite)
        {
            var overwritePermission = overwrite.Permissions;
            return new()
            {
                Id = overwrite.TargetId,
                Type = overwrite.TargetType,
                Allow = overwritePermission.Allowed.RawValue,
                Deny = overwritePermission.Denied.RawValue
            };
        }
    }
}
