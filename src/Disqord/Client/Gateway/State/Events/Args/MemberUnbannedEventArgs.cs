namespace Disqord.Events
{
    public sealed class MemberUnbannedEventArgs : DiscordEventArgs
    {
        public CachedGuild Guild { get; }

        public CachedUser User { get; }

        internal MemberUnbannedEventArgs(
            CachedGuild guild,
            CachedUser user) : base(guild.Client)
        {
            Guild = guild;
            User = user;
        }
    }
}
