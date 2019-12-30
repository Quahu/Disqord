namespace Disqord.Events
{
    public sealed class PresenceUpdatedEventArgs : DiscordEventArgs
    {
        public CachedUser User { get; }

        public Presence OldPresence { get; }

        public Presence NewPresence { get; }

        internal PresenceUpdatedEventArgs(CachedUser user, Presence oldPresence) : base(user.Client)
        {
            User = user;
            OldPresence = oldPresence;
            NewPresence = user.Presence;
        }
    }
}