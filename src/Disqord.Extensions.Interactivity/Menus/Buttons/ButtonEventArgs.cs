using Disqord.Events;
using Disqord.Rest;

namespace Disqord.Extensions.Interactivity.Menus
{
    public sealed class ButtonEventArgs : DiscordEventArgs
    {
        public FetchableSnowflakeOptional<IMessage> Message { get; }

        public FetchableSnowflakeOptional<IUser> User { get; }

        public Optional<ReactionData> Reaction { get; }

        public IEmoji Emoji { get; }

        public bool WasAdded { get; }

        internal ButtonEventArgs(ReactionAddedEventArgs e) : base(e.Client)
        {
            Message = e.Message;
            User = e.User;
            Reaction = e.Reaction;
            Emoji = e.Emoji;
            WasAdded = true;
        }

        internal ButtonEventArgs(ReactionRemovedEventArgs e) : base(e.Client)
        {
            Message = e.Message;
            User = e.User;
            Reaction = e.Reaction;
            Emoji = e.Emoji;
            WasAdded = false;
        }
    }
}
