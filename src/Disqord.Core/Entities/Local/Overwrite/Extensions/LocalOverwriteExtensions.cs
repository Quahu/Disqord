using System.ComponentModel;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalOverwriteExtensions
{
    public static TOverwrite WithTargetId<TOverwrite>(this TOverwrite overwrite, Snowflake targetId)
        where TOverwrite : LocalOverwrite
    {
        overwrite.TargetId = targetId;
        return overwrite;
    }

    public static TOverwrite WithTargetType<TOverwrite>(this TOverwrite overwrite, OverwriteTargetType targetType)
        where TOverwrite : LocalOverwrite
    {
        overwrite.TargetType = targetType;
        return overwrite;
    }

    public static TOverwrite WithPermissions<TOverwrite>(this TOverwrite overwrite, OverwritePermissions permissions)
        where TOverwrite : LocalOverwrite
    {
        overwrite.Permissions = permissions;
        return overwrite;
    }
}
