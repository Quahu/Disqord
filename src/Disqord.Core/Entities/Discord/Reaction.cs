using Disqord.Models;

namespace Disqord
{
    public class Reaction
    {
        public IEmoji Emoji { get; }

        public int Count { get; }

        public bool HasOwnReaction { get; }

        public Reaction(ReactionJsonModel model)
        {
            Emoji = Disqord.Emoji.Create(model.Emoji);
            Count = model.Count;
            HasOwnReaction = model.Me;
        }

        public Reaction(EmojiJsonModel emoji, bool hasOwnReaction)
        {
            Emoji = Disqord.Emoji.Create(emoji);
            HasOwnReaction = hasOwnReaction;
        }

        public override string ToString()
            => $"{Count} of {Emoji} (own reaction: {HasOwnReaction})";
    }
}
