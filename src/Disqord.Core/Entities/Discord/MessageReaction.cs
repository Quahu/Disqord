using Disqord.Models;

namespace Disqord
{
    public class MessageReaction
    {
        public IEmoji Emoji { get; }

        public int Count { get; }

        public bool HasOwnReaction { get; }

        public MessageReaction(ReactionJsonModel model)
        {
            Emoji = Disqord.Emoji.Create(model.Emoji);
            Count = model.Count;
            HasOwnReaction = model.Me;
        }

        public MessageReaction(IEmoji emoji, int count, bool hasOwnReaction)
        {
            Emoji = emoji;
            Count = count;
            HasOwnReaction = hasOwnReaction;
        }

        public override string ToString()
            => $"{Count} of {Emoji} (own reaction: {HasOwnReaction})";
    }
}
