namespace Disqord.Events
{
    public sealed class MemberBannedEventArgs : DiscordEventArgs
    {
        public CachedGuild Guild { get; }

        public CachedUser User { get; }

        internal MemberBannedEventArgs(
            CachedGuild guild,
            CachedUser user) : base(guild.Client)
        {
            Guild = guild;
            User = user;
        }
    }
}
