using Disqord.Models;

namespace Disqord
{
    public interface IReaction : IJsonUpdatable<ReactionJsonModel>
    {
        IEmoji Emoji { get; }

        int Count { get; }

        bool HasCurrentUserReacted { get; }
    }
}
