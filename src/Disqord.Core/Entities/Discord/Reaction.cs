using Disqord.Models;

namespace Disqord
{
    public class Reaction
    {
        public IEmoji Emoji { get; }

        public int Count { get; }

        public bool HasCurrentUserReacted { get; }

        public Reaction(ReactionJsonModel model)
        {
            Emoji = Disqord.Emoji.Create(model.Emoji);
            Count = model.Count;
            HasCurrentUserReacted = model.Me;
        }

        public override string ToString()
            => $"{Count} of {Emoji} (bot reacted: {HasCurrentUserReacted})";
    }
}
