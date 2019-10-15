namespace Disqord.Events
{
    public sealed class JoinedGuildEventArgs : DiscordEventArgs
    {
        public CachedGuild Guild { get; }

        internal JoinedGuildEventArgs(CachedGuild guild) : base(guild.Client)
        {
            Guild = guild;
        }
    }
}
