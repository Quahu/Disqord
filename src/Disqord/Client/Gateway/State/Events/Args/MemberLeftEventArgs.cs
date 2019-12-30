namespace Disqord.Events
{
    public sealed class MemberLeftEventArgs : DiscordEventArgs
    {
        public CachedGuild Guild { get; }

        public CachedUser User { get; }

        internal MemberLeftEventArgs(
            CachedGuild guild,
            CachedUser user) : base(guild.Client)
        {
            Guild = guild;
            User = user;
        }
    }
}
