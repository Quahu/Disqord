using Disqord.Models;

namespace Disqord
{
    public class TransientReaction : TransientEntity<ReactionJsonModel>, IReaction
    {
        public int Count { get; }

        public bool HasCurrentUserReacted { get; }

        public IEmoji Emoji
        {
            get
            {
                if (_emoji == null)
                    _emoji = new TransientEmoji(Client, Model.Emoji);

                return _emoji;
            }
        }
        private IEmoji _emoji;

        public TransientReaction(IClient client, ReactionJsonModel model)
            : base(client, model)
        { }
    }
}
