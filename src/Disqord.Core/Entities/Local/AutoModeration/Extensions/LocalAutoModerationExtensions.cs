using Disqord.Models;
using Qommon;

namespace Disqord
{
    public static class LocalAutoModerationExtensions
    {
        public static LocalAutoModerationAction WithType(this LocalAutoModerationAction action, AutoModerationActionType type)
        {
            action.Type = type;
            return action;
        }

        public static LocalAutoModerationAction WithMetadata(this LocalAutoModerationAction action, LocalAutoModerationActionMetadata metadata)
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
}
