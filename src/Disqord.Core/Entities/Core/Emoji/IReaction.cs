using Disqord.Models;

namespace Disqord
{
    public interface IReaction : IJsonUpdatable<ReactionJsonModel>
    {
        int Count { get; }

        bool HasCurrentUserReacted { get; }

        IEmoji Emoji { get; }
    }
}
