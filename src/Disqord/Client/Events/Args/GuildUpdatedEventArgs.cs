namespace Disqord.Events
{
    public sealed class GuildUpdatedEventArgs : DiscordEventArgs
    {
        public CachedGuild OldGuild { get; }

        public CachedGuild NewGuild { get; }

        internal GuildUpdatedEventArgs(CachedGuild oldGuild, CachedGuild newGuild) : base(newGuild.Client)
        {
            OldGuild = oldGuild;
            NewGuild = newGuild;
        }
    }
}
