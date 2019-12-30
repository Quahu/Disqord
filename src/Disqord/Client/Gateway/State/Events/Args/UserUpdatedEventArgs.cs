namespace Disqord.Events
{
    public sealed class UserUpdatedEventArgs : DiscordEventArgs
    {
        public CachedUser OldUser { get; }

        public CachedUser NewUser { get; }

        internal UserUpdatedEventArgs(CachedUser oldUser, CachedUser newUser) : base(newUser.Client)
        {
            OldUser = oldUser;
            NewUser = newUser;
        }
    }
}
