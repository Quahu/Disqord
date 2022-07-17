using Disqord.Models;
using Qommon;

namespace Disqord;

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

    public static AutoModerationActionJsonModel ToModel(this LocalAutoModerationAction action)
    {
        return new AutoModerationActionJsonModel
        {
            Type = action.Type.Value,
            Metadata = Optional.Convert(action.Metadata, x => x.ToModel())
        };
    }
}
