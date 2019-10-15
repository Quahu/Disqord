namespace Disqord.Events
{
    public sealed class GuildUnavailableEventArgs : DiscordEventArgs
    {
        public CachedGuild Guild { get; }

        internal GuildUnavailableEventArgs(CachedGuild guild) : base(guild.Client)
        {
            Guild = guild;
        }
    }
}
