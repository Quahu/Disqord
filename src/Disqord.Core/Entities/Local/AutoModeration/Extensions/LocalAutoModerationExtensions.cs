using System.ComponentModel;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalAutoModerationExtensions
{
    public static TAction WithType<TAction>(this TAction action, AutoModerationActionType type)
        where TAction : LocalAutoModerationAction
    {
        action.Type = type;
        return action;
    }

    public static TAction WithMetadata<TAction>(this TAction action, LocalAutoModerationActionMetadata metadata)
        where TAction : LocalAutoModerationAction
    {
        action.Metadata = metadata;
        return action;
    }
}
